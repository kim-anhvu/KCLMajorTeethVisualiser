using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Calculations to preform for Boxplot Functionality
 */

public static class BoxPlotBackend 
{
    /**
     * Perfoms calculations on a list to get the Boxplot Data
     * @param input - list of doubles which will be used to find the values needed
     * @return List<double> in the BoxPlot Order (min,q1,q2,q3,max,...outliers)
     */
    public static List<double> BoxPlot(List<double> input)
    {
        List<double> res = new List<double>();
        //Order input list in ascending order
        input.Sort();
        if (input.Count < 5){
            return null;
        }

        int len = input.Count;
        double q1, q2, q3;

        if (len %2 == 0)
        {
            q2 = (input[len / 2] + input[(len / 2) - 1]) / 2;   
            if((len/2) % 2 == 0)
            {
                q1 = (input[len / 4] + input[(len / 4) - 1]) / 2;
                q3 = (input[(len / 4) + (len / 2)] + input[((len / 4) + (len / 2)) - 1]) / 2;
            } else
            {
                q1 = input[len / 4];
                q3 = input[(len / 4) + (len / 2)];
            }
        } else
        {
            q2 = input[len / 2];
            if(((len-1)/2) % 2 == 0)
            {
                q1 = (input[len / 4] + input[(len / 4) - 1]) / 2;
                q3 = (input[(len / 4) + (len / 2)] + input[((len / 4) + (len / 2)) - 1]) / 2;
            } else
            {
                q1 = input[len / 4];
                q3 = input[(len / 4) + (len / 2)];
            }
        }
        List<int> outlierPosistions = CalculateOutliersPosistions(q1,q3,len,input);
        List<double> outlierValues = new List<double>();
        double outlier;
        double min = -1;
        double max = -1;
        int lastLowOutlier = -1;
        int firstAboveOutlier = -1;

        for(int i = 0 ; i< outlierPosistions.Count; i++)
        {
            outlier = input[outlierPosistions[i]];
            outlierValues.Add(outlier);
            if(outlier < q1){
                lastLowOutlier = outlierPosistions[i];
            }
            if(outlier > q3 && (firstAboveOutlier == -1)){
                firstAboveOutlier = outlierPosistions[i];
            }
        }

        //min = CalculateMin(lastLowOutlier, input[lastLowOutlier+1], input[0]);
        if(lastLowOutlier != -1){
            min = input[lastLowOutlier+1];
        }else{
            min = input[0];
        }

        if(firstAboveOutlier != -1){
            max = input[firstAboveOutlier -1];
        }else {
            max = input[len-1];
        }

        res.Add(min);
        res.Add(q1);
        res.Add(q2);
        res.Add(q3);
        res.Add(max);
        res.AddRange(outlierValues);
        return res;
    }

    /**
     * @param firstAboveOutlier - index within the array of the last outlier with postion < q1
     * @param valBeforeOutlier - value of next element preeceding value at firstAboveOutlier
     * @param last - last radiation value in the list
     * @return valBeforeOutlier iff firstAboveOutlier != -1 else last
     */
    public static double CalculateMax(int firstAboveOutlier, double valBeforeOutlier, double last)
    {
        if(firstAboveOutlier != -1){
            return  valBeforeOutlier;
        }else{
            return last;
        }
    }

    /**
     * @param lastLowOutlierPosistion - index within the array of the last outlier with postion < q1
     * @param valAfterOutluer - value of next element preeceding value at lastLowOutlierPosistion
     * @param first - first radiation value in the list
     * @return first if lastLowOutlierPosistion == -1 else valAfterOutluer
     */
    public static double CalculateMin(int lastLowOutlierPosistion, double valAfterOutluer, double first)
    {
        if(lastLowOutlierPosistion != -1){ 
            return valAfterOutluer;
        }else {
            return first;
        }
    }
   
    /**
     * Outlier formula
     * IQR = q3-q1
     * < first quartile – 1.5·IQR
     * > third quartile + 1.5·IQR  
     * @param q1,q3 - value of q1 and q3
     * @param len - length of radiations list
     * @param radiations - list of radiation values
     * @return ascending order of the posistion of every outlier using the formula above.
     */
    public static List<int> CalculateOutliersPosistions(double q1, double q3, int len, List<double> radiations)
    {
        List<int> outliers = new List<int>();
        if(len < 5){
            return outliers;
        }
        double IQR = q3-q1;
        double lowerBound = q1 - 1.5f*IQR;
        double upperBound = q3 + 1.5f*IQR;
        for (int i = 0; i < len/4; i++)
        {
            if(radiations[i] < lowerBound){
                outliers.Add(i);
            }
        }
        for (int i = len-1; i > len*3/4; i--)
        {
            if(radiations[i] > upperBound){
                if(radiations[i] != 0f){
                    outliers.Add(i);
                }
            }
        }
        outliers.Sort();
        return outliers; 
    }

    /**
     * Sorts Patient List into A list of GraphDataPoints (Boxplots) based on the following params
     * @param xVals - list of x axis varaibles which will be displayed on the graph
     * @param patientList - the list of patients who the data is going to reflect
     * @param dependantVaraible - What the X axis label is
     * @param dataTypeIndex - index indicating whether the data is to calculate for min, max or mean
     * @param teethSelected - a list of ints which correspond to the teeth selected by the user in filtering
     * @return a list of boxplots
     */
    public static List<GraphDataPoint> SortIntoXVariables(List<string> xVals, List<Patient> patientsList, string dependantVaraible, int dataTypeIndex, List<int> teethSelected)
    {
        List<GraphDataPoint> result = new List<GraphDataPoint>();
        switch (dependantVaraible)
        {
            case "Gender":
                result = SortIntoGenders(patientsList,xVals, dataTypeIndex, teethSelected);
                break;
            case "Site":
                result = SortIntoSites(patientsList,xVals, dataTypeIndex, teethSelected);
                break;
            case "Treatment":
                result = SortIntoTreatments(patientsList,xVals, dataTypeIndex, teethSelected);
                break;
            case "Tumour":
                result = SortIntoTumours(patientsList,xVals, dataTypeIndex, teethSelected);
                break;
            case "Jaw Side":
                result = SortIntoJawSides(patientsList,xVals, dataTypeIndex, teethSelected);
                break;
            case "Total RT":
                result = SortIntoTotalRTs(patientsList,xVals, dataTypeIndex, teethSelected);
                break;
            case "Nodal":
                result = SortIntoNodals(patientsList,xVals, dataTypeIndex, teethSelected);
                break;
        }
        return result;
    }

    /**
     * Returns a list of radiation values for every tooth specified
     * @param p - Patient 
     * @param index - whether to check the min, max or mean tooth radiation
     * @param teeth - which teeth numbers to check
     * @return list of all the radiation values of the teeth to check, not including 0 radiation values
     */
    public static List<double> GetAllTeethRadiations(Patient p, int index, List<int> teeth)
    {
        List<double> radiationValues = new List<double>();
        foreach (int tooth in teeth)  
        {   
            double radiation = DataModel.GetToothRadiation(p, tooth, index);
            if(radiation != 0) {
                radiationValues.Add(radiation);
            }
        }
        return radiationValues;
    }

    /** 
     * Sorts Patient List into A list of GraphDataPoints (Boxplots) for genders
     * @param xVals - list of x axis varaibles which will be displayed on the graph
     * @param patientList - the list of patients who the data is going to reflect
     * @param dataTypeIndex - index indicating whether the data is to calculate for min, max or mean
     * @param teethSelected - a list of ints which correspond to the teeth selected by the user in filtering
     * @return a list of boxplots
     */
    public static List<GraphDataPoint> SortIntoGenders(List<Patient> patientsList, List<string> xVals, int index, List<int> teeth)
    {
        List<GraphDataPoint> result = new List<GraphDataPoint>(xVals.Count);
        List<double> maleRadiations = new List<double>(patientsList.Count * teeth.Count);
        List<double> femaleRadiations = new List<double>(patientsList.Count * teeth.Count);
        foreach (Patient p in patientsList)
        {
            if(p.GetGender() == "M"){
                maleRadiations.AddRange(GetAllTeethRadiations(p, index, teeth));
            }
            else{
                femaleRadiations.AddRange(GetAllTeethRadiations(p, index, teeth));
            }
        }

        GraphDataPoint males = new GraphDataPoint();
        males.AddDataPointRange(BoxPlot(maleRadiations));
        GraphDataPoint females = new GraphDataPoint();
        females.AddDataPointRange(BoxPlot(femaleRadiations));

        //Add Data points in correct order
        foreach (string label in xVals)
        {
            if(label == "M"){
                result.Add(males);
            }else{
                result.Add(females);
            }
        }
        return result;
    }

    /** 
     * Sorts Patient List into A list of GraphDataPoints (Boxplots) for Sites
     * @param xVals - list of x axis varaibles which will be displayed on the graph
     * @param patientList - the list of patients who the data is going to reflect
     * @param dataTypeIndex - index indicating whether the data is to calculate for min, max or mean
     * @param teethSelected - a list of ints which correspond to the teeth selected by the user in filtering
     * @return a list of boxplots
     */
    public static List<GraphDataPoint> SortIntoSites(List<Patient> patientsList, List<string> xVals, int index, List<int> teeth)
    {
        List<GraphDataPoint> result = new List<GraphDataPoint>(xVals.Count);
        List<double> botRadiations = new List<double>(patientsList.Count * teeth.Count);
        List<double> tonsilRadiations = new List<double>(patientsList.Count * teeth.Count);
        List<double> otherRadiations = new List<double>(patientsList.Count * teeth.Count);
        string site; 
        foreach (Patient p in patientsList)
        {
            site = p.GetSite();
            if(site == "BOT"){
                botRadiations.AddRange(GetAllTeethRadiations(p, index, teeth));
            }
            else if (site == "TONSIL"){
                tonsilRadiations.AddRange(GetAllTeethRadiations(p, index, teeth));
            }else {
                otherRadiations.AddRange(GetAllTeethRadiations(p, index, teeth));
            }
        }

        GraphDataPoint bots = new GraphDataPoint();
        bots.AddDataPointRange(BoxPlot(botRadiations));
        GraphDataPoint tonsils = new GraphDataPoint();
        tonsils.AddDataPointRange(BoxPlot(tonsilRadiations));
        GraphDataPoint others = new GraphDataPoint();
        others.AddDataPointRange(BoxPlot(otherRadiations));

        //Add Data points in correct order
        foreach (string label in xVals)
        {
            if(label == "BOT"){
                result.Add(bots);
            }else if(label == "TONSIL"){
                result.Add(tonsils);
            }else{
                result.Add(others);
            }
        }
        return result;
    }

    /** 
     * Sorts Patient List into A list of GraphDataPoints (Boxplots) for Treatment Types
     * @param xVals - list of x axis varaibles which will be displayed on the graph
     * @param patientList - the list of patients who the data is going to reflect
     * @param dataTypeIndex - index indicating whether the data is to calculate for min, max or mean
     * @param teethSelected - a list of ints which correspond to the teeth selected by the user in filtering
     * @return a list of boxplots
     */
     public static List<GraphDataPoint> SortIntoTreatments(List<Patient> patientsList, List<string> xVals, int index, List<int> teeth)
    {
        List<GraphDataPoint> result = new List<GraphDataPoint>(xVals.Count);
        List<double> crtRadiations = new List<double>(patientsList.Count * teeth.Count);
        List<double> rtRadiations = new List<double>(patientsList.Count * teeth.Count);
        string treatment; 
        foreach (Patient p in patientsList)
        {
            treatment = p.GetTreatment();
            if(treatment == "CRT"){
                crtRadiations.AddRange(GetAllTeethRadiations(p, index, teeth));
            }
            else if (treatment == "RT"){
                rtRadiations.AddRange(GetAllTeethRadiations(p, index, teeth));
            }
        }

        GraphDataPoint crts = new GraphDataPoint();
        crts.AddDataPointRange(BoxPlot(crtRadiations));
        GraphDataPoint rts = new GraphDataPoint();
        rts.AddDataPointRange(BoxPlot(rtRadiations));

        //Add Data points in correct order
        foreach (string label in xVals)
        {
            if(label == "CRT"){
                result.Add(crts);
            }else if(label == "RT"){
                result.Add(rts);
            }
        }
        return result;
    }

    /** 
     * Sorts Patient List into A list of GraphDataPoints (Boxplots) for Tumpur Types
     * @param xVals - list of x axis varaibles which will be displayed on the graph
     * @param patientList - the list of patients who the data is going to reflect
     * @param dataTypeIndex - index indicating whether the data is to calculate for min, max or mean
     * @param teethSelected - a list of ints which correspond to the teeth selected by the user in filtering
     * @return a list of boxplots
     */
    public static List<GraphDataPoint> SortIntoTumours(List<Patient> patientsList, List<string> xVals, int index, List<int> teeth)
    {
        List<GraphDataPoint> result = new List<GraphDataPoint>(xVals.Count);
        List<double> T1sRadiations = new List<double>(patientsList.Count * teeth.Count);
        List<double> T2sRadiations = new List<double>(patientsList.Count * teeth.Count);
        List<double> T3sRadiations = new List<double>(patientsList.Count * teeth.Count);
        List<double> T4sRadiations = new List<double>(patientsList.Count * teeth.Count);
        string tumour; 
        foreach (Patient p in patientsList)
        {
            tumour = p.GetTumourName();
            if(tumour == "T1"){
                T1sRadiations.AddRange(GetAllTeethRadiations(p, index, teeth));
            }
            else if (tumour == "T2"){
                T2sRadiations.AddRange(GetAllTeethRadiations(p, index, teeth));
            }
            else if (tumour == "T3"){
                T3sRadiations.AddRange(GetAllTeethRadiations(p, index, teeth));
            }
            else if (tumour == "T4"){
                T4sRadiations.AddRange(GetAllTeethRadiations(p, index, teeth));
            }
        }

        GraphDataPoint t1 = new GraphDataPoint();
        t1.AddDataPointRange(BoxPlot(T1sRadiations));
        GraphDataPoint t2 = new GraphDataPoint();
        t2.AddDataPointRange(BoxPlot(T2sRadiations));
        GraphDataPoint t3 = new GraphDataPoint();
        t3.AddDataPointRange(BoxPlot(T3sRadiations));
        GraphDataPoint t4 = new GraphDataPoint();
        t4.AddDataPointRange(BoxPlot(T4sRadiations));

        //Add Data points in correct order
        foreach (string label in xVals)
        {
            if(label == "T1"){
                result.Add(t1);
            }else if(label == "T2"){
                result.Add(t2);
            }else if(label == "T3"){
                result.Add(t3);
            }else if(label == "T4"){
                result.Add(t4);
            }
        }
        return result;
    }

    /** 
     * Sorts Patient List into A list of GraphDataPoints (Boxplots) for JawSides
     * @param xVals - list of x axis varaibles which will be displayed on the graph
     * @param patientList - the list of patients who the data is going to reflect
     * @param dataTypeIndex - index indicating whether the data is to calculate for min, max or mean
     * @param teethSelected - a list of ints which correspond to the teeth selected by the user in filtering
     * @return a list of boxplots
     */
    public static List<GraphDataPoint> SortIntoJawSides(List<Patient> patientsList, List<string> xVals, int index, List<int> teeth)
    {
        List<GraphDataPoint> result = new List<GraphDataPoint>(xVals.Count);
        List<double> leftRadiations = new List<double>(patientsList.Count * teeth.Count);
        List<double> rightRadiations = new List<double>(patientsList.Count * teeth.Count);
        List<double> naRadiations = new List<double>(patientsList.Count * teeth.Count);
        string side; 
        foreach (Patient p in patientsList)
        {
            side = p.GetTumourJawSide();
            if(side == "L"){
                leftRadiations.AddRange(GetAllTeethRadiations(p, index, teeth));
            }
            else if (side == "R"){
                rightRadiations.AddRange(GetAllTeethRadiations(p, index, teeth));
            }else {
                naRadiations.AddRange(GetAllTeethRadiations(p, index, teeth));
            }
        }

        GraphDataPoint left = new GraphDataPoint();
        left.AddDataPointRange(BoxPlot(leftRadiations));
        GraphDataPoint right = new GraphDataPoint();
        right.AddDataPointRange(BoxPlot(rightRadiations));
        GraphDataPoint NA = new GraphDataPoint();
        NA.AddDataPointRange(BoxPlot(naRadiations));

        //Add Data points in correct order
        foreach (string label in xVals)
        {
            if(label == "LEFT"){
                result.Add(left);
            }else if(label == "RIGHT"){
                result.Add(right);
            }else{
                result.Add(NA);
            }
        }
        return result;
    }

    /** 
     * Sorts Patient List into A list of GraphDataPoints (Boxplots) for RTS
     * @param xVals - list of x axis varaibles which will be displayed on the graph
     * @param patientList - the list of patients who the data is going to reflect
     * @param dataTypeIndex - index indicating whether the data is to calculate for min, max or mean
     * @param teethSelected - a list of ints which correspond to the teeth selected by the user in filtering
     * @return a list of boxplots
     */
    public static List<GraphDataPoint> SortIntoTotalRTs(List<Patient> patientsList, List<string> xVals, int index, List<int> teeth)
    {
        List<GraphDataPoint> result = new List<GraphDataPoint>(xVals.Count);
        List<double> Radiations6530 = new List<double>(patientsList.Count * teeth.Count);
        List<double> Radiations5520 = new List<double>(patientsList.Count * teeth.Count);
        List<double> Radiations5430 = new List<double>(patientsList.Count * teeth.Count);
        List<double> Radiations7035 = new List<double>(patientsList.Count * teeth.Count);
        string rt; 
        foreach (Patient p in patientsList)
        {
            rt = p.GetTotalRT();
            if(rt == "65/30"){
                Radiations6530.AddRange(GetAllTeethRadiations(p, index, teeth));
            }
            else if (rt == "55/20"){
                Radiations5520.AddRange(GetAllTeethRadiations(p, index, teeth));
            }
            else if (rt == "54/30"){
                Radiations5430.AddRange(GetAllTeethRadiations(p, index, teeth));
            }
            else if (rt == "70/35"){
                Radiations7035.AddRange(GetAllTeethRadiations(p, index, teeth));
            }
        }

        GraphDataPoint rt6530 = new GraphDataPoint();
        rt6530.AddDataPointRange(BoxPlot(Radiations6530));
        GraphDataPoint rt5520 = new GraphDataPoint();
        rt5520.AddDataPointRange(BoxPlot(Radiations5520));
        GraphDataPoint rt5430 = new GraphDataPoint();
        rt5430.AddDataPointRange(BoxPlot(Radiations5430));
        GraphDataPoint rt7035 = new GraphDataPoint();
        rt7035.AddDataPointRange(BoxPlot(Radiations7035));

        //Add Data points in correct order
        foreach (string label in xVals)
        {
            if(label == "65/30"){
                result.Add(rt6530);
            }else if(label == "55/20"){
                result.Add(rt5520);
            }else if(label == "54/30"){
                result.Add(rt5430);
            }else if(label == "70/35"){
                result.Add(rt7035);
            }
        }
        return result;
    }

    /** 
     * Sorts Patient List into A list of GraphDataPoints (Boxplots) for Nodals
     * @param xVals - list of x axis varaibles which will be displayed on the graph
     * @param patientList - the list of patients who the data is going to reflect
     * @param dataTypeIndex - index indicating whether the data is to calculate for min, max or mean
     * @param teethSelected - a list of ints which correspond to the teeth selected by the user in filtering
     * @return a list of boxplots
     */
    public static List<GraphDataPoint> SortIntoNodals(List<Patient> patientsList, List<string> xVals, int index, List<int> teeth)
    {   
        List<GraphDataPoint> result = new List<GraphDataPoint>(xVals.Count);
        List<double> RadiationsN0 = new List<double>(patientsList.Count * teeth.Count);
        List<double> RadiationsN1 = new List<double>(patientsList.Count * teeth.Count);
        List<double> RadiationsN2 = new List<double>(patientsList.Count * teeth.Count);
        List<double> RadiationsN3 = new List<double>(patientsList.Count * teeth.Count);
        string nodal; 
        foreach (Patient p in patientsList)
        {
            nodal = p.GetNodalPosition();
            if(nodal == "N0"){
                RadiationsN0.AddRange(GetAllTeethRadiations(p, index, teeth));
            }
            else if (nodal == "N1"){
                RadiationsN1.AddRange(GetAllTeethRadiations(p, index, teeth));
            }
            else if (nodal == "N2"){
                RadiationsN2.AddRange(GetAllTeethRadiations(p, index, teeth));
            }
            else if (nodal == "N3"){
                RadiationsN3.AddRange(GetAllTeethRadiations(p, index, teeth));
            }
        }

        GraphDataPoint N0 = new GraphDataPoint();
        N0.AddDataPointRange(BoxPlot(RadiationsN0));
        GraphDataPoint N1 = new GraphDataPoint();
        N1.AddDataPointRange(BoxPlot(RadiationsN1));
        GraphDataPoint N2 = new GraphDataPoint();
        N2.AddDataPointRange(BoxPlot(RadiationsN2));
        GraphDataPoint N3 = new GraphDataPoint();
        N3.AddDataPointRange(BoxPlot(RadiationsN3));

        //Add Data points in correct order
        foreach (string label in xVals)
        {
            if(label == "N0"){
                result.Add(N0);
            }else if(label == "N1"){
                result.Add(N1);
            }else if(label == "N2"){
                result.Add(N2);
            }else if(label == "N3"){
                result.Add(N3);
            }
        }
        return result;
    }


    
}
