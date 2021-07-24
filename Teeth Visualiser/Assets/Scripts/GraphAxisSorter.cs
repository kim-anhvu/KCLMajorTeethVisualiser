using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
/**
* GraphAxisSorter class.
* 
* Sort the X-axis of a given Patients-RadiationValue pair list, into a String-RadiationValue pair list, depending on the set x-axis attribute.
* This essentially aggregates all the patients according to their attribute value.
* For example, sorting into Nodals would return a List of N0, N1, N2, N3, each paired with the aggregate of all of the patients falling into that category.
* If there are no Patients with N1, then no tuple with String N1 is added to the list.
*/
public class GraphAxisSorter
{
    //An attribute from the Data File, exactly as written as in the SortIntoXVariables method.
    private string xVariable;

    //If the SortIntoAges method is called, this will be used to increment the grouping, starting from 0.
    private int ageRange;

    /**
    * Constructs a GraphAxisSorter, with default values set for both the fields. 
    */
    public GraphAxisSorter()
    {
        xVariable = "Patient";
        ageRange = 1;
    }

    /**
    * Calculates and returns a List of String-Radiation Value pairs, depending on the xVariable set.
    * @param patientsRadiationPairs - A list of Patient-Radiation Value pairs
    * @return List of String-Radiation Value pairs, sorted into groups depending on the xVariable set
    */
    public List<Tuple<string, double>> SortIntoXVariables(List<Tuple<Patient, double>> patientsRadiationPairs)
    {
        List<Tuple<string, double>> result = new List<Tuple<string, double>>();

        switch (xVariable)
        {
            case "Patient":
                result = SortIntoPatients(patientsRadiationPairs);
                break;
             
            case "Gender":
                result = SortIntoGenders(patientsRadiationPairs);
                break;
            
             case "Site":
                result = SortIntoSites(patientsRadiationPairs);
                break;

             case "Treatment":
                result = SortIntoTreatments(patientsRadiationPairs);
                break;
            
             case "Tumour":
                result = SortIntoTumours(patientsRadiationPairs);
                break;

             case "Jaw Side":
                result = SortIntoJawSides(patientsRadiationPairs);
                break;

             case "Total RT":
                result = SortIntoTotalRTs(patientsRadiationPairs);
                break;

            case "Nodal":
                result = SortIntoNodals(patientsRadiationPairs);
                break;
            
             case "Age":
                result = SortIntoAges(patientsRadiationPairs);
                break;
        }
        return result;
    }

    /**
     * Sorts filtered list of tuples <Patient, double> into numbers: 0 - number of Tuples.
     * @param patientsRadiationPairs - Already filtered List of Tuples <Patient, double>
     * @return sorted list of tuples <String, double>
     */
    private List<Tuple<string, double>> SortIntoPatients(List<Tuple<Patient, double>> patientsRadiationPairs)
    {
        int counter = 0;
        List<Tuple<string, double>> result = new List<Tuple<String, double>>();

        foreach(Tuple<Patient, double> x in patientsRadiationPairs)
        {
            result.Add(new Tuple<string, double>(counter.ToString(), x.Item2));
            counter++;
        }
        return result;
    }

    /**
     * Sorts filtered list of tuples <Patient, double> into the 2 Gender groups.
     * @param patientsRadiationPairs - Already filtered List of Tuples <Patient, double>
     * @return sorted list of tuples <String, double>
     */
    private List<Tuple<string, double>> SortIntoGenders(List<Tuple<Patient, double>> patientsRadiationPairs)
    {
        double totalMaleRadiation = 0;
        double totalFemaleRadiation = 0;

        int maleCounter = 0;
        int femaleCounter = 0;

        List<Tuple<string, double>> result = new List<Tuple<String, double>>();

        foreach (Tuple<Patient,double> x in patientsRadiationPairs)
        {
            if(x.Item1.GetGender().Equals("M")){
                totalMaleRadiation += x.Item2;
                maleCounter++;
            }
            else{
                totalFemaleRadiation += x.Item2;
                femaleCounter++;
            }
        }

        double maleAverage = totalMaleRadiation / maleCounter;
        double femaleAverage = totalFemaleRadiation / femaleCounter;

        if(maleAverage > 0) 
        { 
            result.Add(new Tuple<string, double>( "M", maleAverage));
        }
        if(femaleAverage > 0) 
        {
            result.Add(new Tuple<string, double>( "F", femaleAverage));
        }
        return result;
    }

    /**
     * Sorts filtered list of tuples <Patient, double> into the 3 Site groups.
     * @param patientsRadiationPairs - Already filtered List of Tuples <Patient, double>
     * @return sorted list of tuples <String, double>
     */
    private List<Tuple<string, double>> SortIntoSites(List<Tuple<Patient, double>> patientsRadiationPairs)
    {
        double totalBotRadiation = 0;
        double totalTonsilRadiation = 0;
        double totalOtherRadiation = 0;

        int botCounter = 0;
        int tonsilCounter = 0;
        int otherCounter = 0;

        List<Tuple<string, double>> result = new List<Tuple<String, double>>();

        foreach (Tuple<Patient,double> x in patientsRadiationPairs)
        {
            if(x.Item1.GetSite().Equals("BOT"))
            {
                totalBotRadiation += x.Item2;
                botCounter++;
            }
            else if(x.Item1.GetSite().Equals("TONSIL"))
            {
                totalTonsilRadiation += x.Item2;
                tonsilCounter++;
            }
            else 
            {
                totalOtherRadiation += x.Item2;
                otherCounter++;
            }
        }

        double botAverage = totalBotRadiation / botCounter;
        double tonsilAverage = totalTonsilRadiation / tonsilCounter;
        double otherAverage = totalOtherRadiation / otherCounter;

        if(botAverage > 0) 
        { 
            result.Add(new Tuple<string, double>( "BOT", botAverage));
        }
        if(tonsilAverage > 0) 
        {
            result.Add(new Tuple<string, double>( "TONSIL", tonsilAverage));
        }
        if(otherAverage > 0)
        {
            result.Add(new Tuple<string, double>( "OP-OTHER", otherAverage));
        }
        return result;
    }

    /**
     * Sorts filtered list of tuples <Patient, double> into the 2 Treatment groups.
     * @param patientsRadiationPairs - Already filtered List of Tuples <Patient, double>
     * @return sorted list of tuples <String, double>
     */
    private List<Tuple<string, double>> SortIntoTreatments(List<Tuple<Patient, double>> patientsRadiationPairs)
    {
        double totalCRTRadiation = 0;
        double totalRTRadiation = 0;

        int CRTCounter = 0;
        int RTCounter = 0;

        List<Tuple<string, double>> result = new List<Tuple<String, double>>();

        foreach (Tuple<Patient,double> x in patientsRadiationPairs)
        {
            if(x.Item1.GetTreatment().Equals("CRT")){
                totalCRTRadiation += x.Item2;
                CRTCounter++;
            }
            else{
                totalRTRadiation += x.Item2;
                RTCounter++;
            }
        }

        double CRTAverage = totalCRTRadiation / CRTCounter;
        double RTAverage = totalRTRadiation / RTCounter;

        if(CRTAverage > 0) 
        { 
            result.Add(new Tuple<string, double>( "CRT", CRTAverage));
        }
        if(RTAverage > 0) 
        {
            result.Add(new Tuple<string, double>( "RT", RTAverage));
        }
        return result;
    }

    /**
     * Sorts filtered list of tuples <Patient, double> into the 4 Tumour groups.
     * @param patientsRadiationPairs - Already filtered List of Tuples <Patient, double>
     * @return sorted list of tuples <String, double>
     */
    private List<Tuple<string, double>> SortIntoTumours(List<Tuple<Patient, double>> patientsRadiationPairs)
    {
        double totalT1Radiation = 0;
        double totalT2Radiation = 0;
        double totalT3Radiation = 0;
        double totalT4Radiation = 0;

        int T1Counter = 0;
        int T2Counter = 0;
        int T3Counter = 0;
        int T4Counter = 0;

        List<Tuple<string, double>> result = new List<Tuple<String, double>>();

        foreach (Tuple<Patient,double> x in patientsRadiationPairs)
        {
            if(x.Item1.GetTumourName().Equals("T1"))
            {
                totalT1Radiation += x.Item2;
                T1Counter++;
            }
            else if(x.Item1.GetTumourName().Equals("T2"))
            {
                totalT2Radiation += x.Item2;
                T2Counter++;
            }
            else if(x.Item1.GetTumourName().Equals("T3"))
            {
                totalT3Radiation += x.Item2;
                T3Counter++;
            }
            else 
            {
                totalT4Radiation += x.Item2;
                T4Counter++;
            }
        }

        double T1Average = totalT1Radiation / T1Counter;
        double T2Average = totalT2Radiation / T2Counter;
        double T3Average = totalT3Radiation / T3Counter;
        double T4Average = totalT4Radiation / T4Counter;

        if(T1Average > 0) 
        { 
            result.Add(new Tuple<string, double>( "T1", T1Average));
        }
        if(T2Average > 0) 
        {
            result.Add(new Tuple<string, double>( "T2", T2Average));
        }
        if(T3Average > 0)
        {
            result.Add(new Tuple<string, double>( "T3", T3Average));
        }
        if(T4Average > 0)
        {
            result.Add(new Tuple<string, double>( "T4", T4Average));
        }
        return result;
    }

    /**
     * Sorts filtered list of tuples <Patient, double> into the 3 Jaw Side groups.
     * @param patientsRadiationPairs - Already filtered List of Tuples <Patient, double>
     * @return sorted list of tuples <String, double>
     */
    private List<Tuple<string, double>> SortIntoJawSides(List<Tuple<Patient, double>> patientsRadiationPairs)
    {
        double totalLeftRadiation = 0;
        double totalRightRadiation = 0;
        double totalNARadiation = 0;

        int leftCounter = 0;
        int rightCounter = 0;
        int NACounter = 0;

        List<Tuple<string, double>> result = new List<Tuple<String, double>>();

        foreach (Tuple<Patient,double> x in patientsRadiationPairs)
        {
            if(x.Item1.GetTumourJawSide().Equals("L"))
            {
                totalLeftRadiation += x.Item2;
                leftCounter++;
            }
            else if(x.Item1.GetTumourJawSide().Equals("R"))
            {
                totalRightRadiation += x.Item2;
                rightCounter++;
            }
            else 
            {
                totalNARadiation += x.Item2;
                NACounter++;
            }
        }

        double leftAverage = totalLeftRadiation / leftCounter;
        double rightAverage = totalRightRadiation / rightCounter;
        double NAAverage = totalNARadiation / NACounter;

        if(leftAverage > 0) 
        { 
            result.Add(new Tuple<string, double>( "LEFT", leftAverage));
        }
        if(rightAverage > 0) 
        {
            result.Add(new Tuple<string, double>( "RIGHT", rightAverage));
        }
        if(NAAverage > 0)
        {
            result.Add(new Tuple<string, double>( "N/A", NAAverage));
        }
        return result;
    }

    /**
     * Sorts filtered list of tuples <Patient, double> into the 4 Total LRT groups.
     * @param patientsRadiationPairs - Already filtered List of Tuples <Patient, double>
     * @return sorted list of tuples <String, double>
     */
    private List<Tuple<string, double>> SortIntoTotalRTs(List<Tuple<Patient, double>> patientsRadiationPairs)
    {
        double total6530Radiation = 0;
        double total5520Radiation = 0;
        double total5430Radiation = 0;
        double total7035Radiation = 0;

        int counter6530 = 0;
        int counter5520 = 0;
        int counter5430 = 0;
        int counter7035 = 0;

        List<Tuple<string, double>> result = new List<Tuple<String, double>>();

        foreach (Tuple<Patient,double> x in patientsRadiationPairs)
        {
            if(x.Item1.GetTotalRT().Equals("65/30"))
            {
                total6530Radiation += x.Item2;
                counter6530++;
            }
            else if(x.Item1.GetTotalRT().Equals("55/20"))
            {
                total5520Radiation += x.Item2;
                counter5520++;
            }
            else if(x.Item1.GetTotalRT().Equals("54/30"))
            {
                total5430Radiation += x.Item2;
                counter5430++;
            }
            else 
            {
                total7035Radiation += x.Item2;
                counter7035++;
            }
        }

        double average6530 = total6530Radiation / counter6530;
        double average5520= total5520Radiation / counter5520;
        double average5430 = total5430Radiation / counter5430;
        double average7035 = total7035Radiation / counter7035;

        if(average6530 > 0) 
        { 
            result.Add(new Tuple<string, double>( "65/30", average6530));
        }
        if(average5520 > 0) 
        {
            result.Add(new Tuple<string, double>( "55/20", average5520));
        }
        if(average5430 > 0)
        {
            result.Add(new Tuple<string, double>( "54/30", average5430));
        }
        if(average7035 > 0)
        {
            result.Add(new Tuple<string, double>( "70/35", average7035));
        }
        return result;
    }

    /**
     * Sorts filtered list of tuples <Patient, double> into the 4 Nodal groups.
     * @param patientsRadiationPairs - Already filtered List of Tuples <Patient, double>
     * @return sorted list of tuples <String, double>
     */
    private List<Tuple<string, double>> SortIntoNodals(List<Tuple<Patient, double>> patientsRadiationPairs)
    {
        double totalN0Radiation = 0;
        double totalN1Radiation = 0;
        double totalN2Radiation = 0;
        double totalN3Radiation = 0;

        int N0Counter = 0;
        int N1Counter = 0;
        int N2Counter = 0;
        int N3Counter = 0;

        List<Tuple<string, double>> result = new List<Tuple<String, double>>();

        foreach (Tuple<Patient,double> x in patientsRadiationPairs)
        {
            if(x.Item1.GetNodalPosition().Equals("N0"))
            {
                totalN0Radiation += x.Item2;
                N0Counter++;
            }
            else if(x.Item1.GetNodalPosition().Equals("N1"))
            {
                totalN1Radiation += x.Item2;
                N1Counter++;
            }
            else if(x.Item1.GetNodalPosition().Equals("N2"))
            {
                totalN2Radiation += x.Item2;
                N2Counter++;
            }
            else 
            {
                totalN3Radiation += x.Item2;
                N3Counter++;
            }
        }

        double N0Average = totalN0Radiation / N0Counter;
        double N1Average = totalN1Radiation / N1Counter;
        double N2Average = totalN2Radiation / N2Counter;
        double N3Average = totalN3Radiation / N3Counter;

        if(N0Average > 0) 
        { 
            result.Add(new Tuple<string, double>( "N0", N0Average));
        }
        if(N1Average > 0) 
        {
            result.Add(new Tuple<string, double>( "N1", N1Average));
        }
        if(N2Average > 0)
        {
            result.Add(new Tuple<string, double>( "N2", N2Average));
        }
        if(N3Average > 0)
        {
            result.Add(new Tuple<string, double>( "N3", N3Average));
        }
        return result;
    }

    /**
     * Sorts filtered list of tuples <Patient, double> into age groups.
     * Grouped such that, starting from 0, go to the max age rounded up to the next age range, incrementing by ageRanges field.
     * E.g. if the ageRange is 10, ranges would be: 0-9, 10-19,...
     * @param patientsRadiationPairs - Already filtered List of Tuples <Patient, double>
     * @return sorted list of tuples <String, double>
     */
    private List<Tuple<string, double>> SortIntoAges(List<Tuple<Patient, double>> patients)
    {
        List<Tuple<String, double>> ageRanges = new List<Tuple<string, double>>();
        int maxAge = GetMaxAge(patients);
        for(int min = 0; min < maxAge; min = min + ageRange)
        {
            int max = min + ageRange - 1;
            double rangeTotal = 0;
            int rangeNumber = 0;
            foreach(Tuple<Patient, double> pair in patients)
            {
                Patient patient = pair.Item1;
                if(patient.GetCancerAge() >= min && patient.GetCancerAge() <= max)
                {
                    rangeTotal = rangeTotal + pair.Item2;
                    rangeNumber++;
                }
            }
            if(rangeTotal > 0)
            {
                double rangeMean = rangeTotal / rangeNumber;
                ageRanges.Add(new Tuple<string, double>(min + " - " + max, rangeMean));
            }

        }
        return ageRanges;
    }

    /**
    * Given a list of tuples <Patient, double>, find the maximum age of all of the patients.
    * @param patientsRadiationPairs - filtered List of Tuples <Patient, double>
    * @return the max age of all of the patients.
    */
    private int GetMaxAge(List<Tuple<Patient, double>> patients)
    {
        int max = patients[0].Item1.GetCancerAge();
        foreach(Tuple<Patient, double> pair in patients)
        {
            int age = pair.Item1.GetCancerAge();
            if(age > max){max = age;}
        }
        return max;
    }

    //Setter & Getter Methods
    public void SetXVariable(string newXVariable){xVariable = newXVariable;}

    public void SetAgeRange(int newAgeRange){ageRange = newAgeRange;}

    public string GetXvariable(){return xVariable;}

    public int GetAgeRange(){return ageRange;}
}