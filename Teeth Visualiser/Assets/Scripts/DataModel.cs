using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * Class to store manipulation of the radiation values from the teeth.
 */
public static class DataModel 
{
    private static List<Patient> patients;

    private static double[] meanRadiationTopTeethA;
    private static double[] meanRadiationBottomTeethA;
    private static double[] meanRadiationTopTeethB;
    private static double[] meanRadiationBottomTeethB;

    static DataModel()
    {
        patients = DataLoader.GetAllPatients();
    }

    /*
     * Getter for the patients.   
     */   
    public static List<Patient> GetAllPatients()
    {
        return patients;
    }

    //Calculations on the data
     
    
     /*Combine the left and right radiation values of the teeth into one list.
      *@param records, position
      *@return List<double[]>     
      */      
     public static List<double[]> CombineRightLeftTeethRadiation(List<Patient> records, string position)
    {
        List<double[]> patientRadiation = new List<double[]>();
        foreach (Patient record in records)
        {
            if (position == "bottom")
            {
                patientRadiation.Add(FilterMeanTeethRadiation(record.GetLowerLeft(), record.GetLowerRight()));
            }
            else if(position == "top"){
                patientRadiation.Add(FilterMeanTeethRadiation(record.GetUpperLeft(), record.GetUpperRight()));
            }
            else
            {
                throw new Exception("Incorrect string parsed");
            }
        }
        return patientRadiation;
    }

    /*Calclulate the average radiation for a collection of patient teeth.
     * @param patientRadiation
     * @return double[]    
     */
    public static double[] CalculateAverageMeanRadiation(List<double[]> patientRadiation){
       double[] patientTeeth = new double[16];
       double totalRadiation = 0;
       
       for(int i=0; i < 16; ++i) {
            for (int j = 0; j < patientRadiation.Count; ++j)
            {
                double[] patientRad = patientRadiation[j];
                totalRadiation += patientRad[i];
            }    
            patientTeeth[i] = totalRadiation / patientRadiation.Count;
            totalRadiation = 0;
        }
        return patientTeeth;
    }

    /*Filter out just the mean values from the array of radiation values.
     * @param left, right
     * @return double[]    
     */
    public static double[] FilterMeanTeethRadiation(double[,] left, double[,] right)
    {
        double[] teeth = new double[16];
        for (int i = 0; i < teeth.Length / 2; ++i)
        {
            teeth[i] = left[i, 1];

        }
        int k = 0;
        for (int i = 8; i < teeth.Length; ++i)
        {
            teeth[i] = right[k, 1];
            ++k;
        }
        return teeth;
    }

   /*
    * Returns the average of the bottom teeth for 1 or more patients
    * @param selectedRecords, bot
    * @return double[]
    */
    public static double[] getBottomAverageMeanRadiation(List<Patient> selectedRecords, string bot)
    {
        return
        DataModel.CalculateAverageMeanRadiation(
        DataModel.CombineRightLeftTeethRadiation(selectedRecords, bot)
            );
    }

    /*
     * Returns the average of the top teeth for 1 or more patients
     * @param selectedRecords, top
     * @return double[]
     */
    public static double[] getTopAverageMeanRadiation(List<Patient> selectedRecords, string top)
    {
        return
        DataModel.CalculateAverageMeanRadiation(
        DataModel.CombineRightLeftTeethRadiation(selectedRecords, top)
            );
    }

    /*
     *Find the minimum and maximum radiation values for the data set and return it.
     *@param SelectedRecords
     *@return Tuple<double, double>     
     */
    public static Tuple<double, double> getMinMAx(List<Patient> selectedRecords)
    {
        double[] top = getTopAverageMeanRadiation(selectedRecords, "top");
        double[] bottom = getBottomAverageMeanRadiation(selectedRecords, "bottom");
        double[] teethCombo = new double[top.Length + bottom.Length];
        top.CopyTo(teethCombo, 0);
        bottom.CopyTo(teethCombo, top.Length);

        Tuple<double, double> minMax = new Tuple<double, double>(Math.Truncate(findMinimum(teethCombo) * 1000) / 1000, Math.Truncate(teethCombo.Max() * 1000) / 1000);
        return minMax;
    }

    /*
     * Helper function that allows you to find the minimum values of the teeth that isnt Zero.
     * @param teeth
     * @return double    
     */   
    private static double findMinimum(double[] teeth) {
        double minimum = 0;
        for (int i=0; i<teeth.Length; ++i) { 
            if (teeth[i]!=0 && (teeth[i] < minimum || minimum == 0)) {
                minimum = teeth[i];
            }
        }
        return minimum;
    }

    /*
     *Set the average teeth teeth radiation for the corresponding sets.
     *@param bottom, top, set
     */   
    public static void setAverageMeanRadiation(double[] bottom, double[] top, string set)
    {
        if (set == "firstSet")
        {
            meanRadiationBottomTeethA = bottom;
            meanRadiationTopTeethA = top;
        }
        else if (set == "secondSet")
        {
            meanRadiationBottomTeethB = bottom;
            meanRadiationTopTeethB = top;
        }
    }

    /*
     * Getter for the mean radiation of the bottom set of teeth B.
     * return double[]
     */   
    public static double[] MeanRadiationBottomTeethB
    {
        get
        {
            return meanRadiationBottomTeethB;
        }
    }

    /*
     * Getter for the mean radiation of the top set of teeth B.
     * return double[]
     */
    public static double[] MeanRadiationTopTeethB
    {
        get
        {
            return meanRadiationTopTeethB;
        }
    }

    /*
     * Getter for the mean radiation of the bottom set of teeth A.
     * return double[]
     */
    public static double[] MeanRadiationBottomTeethA
    {
        get
        {
            return meanRadiationBottomTeethA;
        }
    }

    /*
     * Getter for the mean radiation of the top set of teeth A.
     * return double[]
     */
    public static double[] MeanRadiationTopTeethA
    {
        get
        {
            return meanRadiationTopTeethA;
        }
    }


    /*
       Returns the radiations of a specified tooth number for a given patient.
       @param secondIndex refers to either the min, average or max radation value.
    */
    public static double GetToothRadiation(Patient patient, int toothNumber, int secondIndex)
    {
        if(toothNumber < 0 || toothNumber > 31)
        {
            //We have an error
            return 0;
        }

        double [,] teethArray;
        if(toothNumber >= 0 && toothNumber <= 7)
        {
            teethArray = patient.GetLowerLeft();
        }
        else if(toothNumber >= 8 && toothNumber <= 15)
        {
            teethArray = patient.GetLowerRight();
            toothNumber = toothNumber - 8;
        }
        else if(toothNumber >= 16 && toothNumber <= 23)
        {
            teethArray = patient.GetUpperLeft();
            toothNumber = toothNumber - 16;
        }
        else
        {
            teethArray = patient.GetUpperRight();
            toothNumber = toothNumber - 24;
        }
        double radiationValueToReturn = teethArray[toothNumber, secondIndex];
        return radiationValueToReturn;  
    }



}
