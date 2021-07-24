using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
/**
* GraphData class.
* 
* Given a list of filtering criteria, produce a List of Tuples, containing patients passing the filtering criteria, paired with their
* average radiation values for the specified teeth.
*/
public class GraphData : MonoBehaviour
{
    private List<Patient> allPatients;
    /*
     * Fields for filtering:
     * Where strings are required, enter exactly as done so on the data spreadsheet 'Contour-for-analysis.xlsx';
     * The items specified in the Lists 'ToKeep' are the only things that will be kept - everything not in them will be filtered out (i.e. and empty list means everything will be filtered out);
     * If "All" is specified as the first value of the lists, everything will be kept (and nothing filtered out);
     * Min and Max values are inclusive: i.e. to specify radiation or age of just 30, set both min and max to 30;
     * DataType is which radiation (min/mean/max) values will be considered: 0 = min, 1 = mean, 2 = max 
    */
    private List<int> toothNumbers;
    private List<string> tumoursToKeep;
    private List<string> nodalsToKeep;
    private List<string> gendersToKeep;
    private int ageMin;
    private int ageMax;
    private List<string> sitesToKeep;
    private List<string> sidesToKeep;
    private List<string> treatmentsToKeep;
    private List<string> totalRTsToKeep;
    private int dataType;

    /**
     * Start the GraphData class, with filters set such that everything is filtered out - these need to be set before using other methods.
     */
    void Start()
    {
        //Set List equal to the current data
        UpdateData();

        //Initialise filters such that nothing is left in - everything is filtered out
        toothNumbers = new List<int>();
        tumoursToKeep = new List<string>();
        nodalsToKeep = new List<string>();
        gendersToKeep = new List<string>();
        ageMin = 0;
        ageMax = 0;
        sitesToKeep = new List<string>();
        sidesToKeep = new List<string>();
        treatmentsToKeep = new List<string>();
        totalRTsToKeep = new List<string>();

        //Unless otherwise set, default data type will be mean.
        dataType = 1;
    }

    /**
     * Set the data (list of patients) to its most recent form.
     */
    private void UpdateData()
    {
        allPatients = new List<Patient>(DataLoader.GetAllPatients());
    }

    /**
     * Returns the List of Tuples <Patients, doubles> of the filtered patients and their radiation values, for the teeth specified in toothNumbers.
     * If new filters are set, this method needs to be called again.
     * @return the List of Tuples <Patients, doubles> of the filtered patients and their radiation values.
     */
    public List<Tuple<Patient, double>> GetPatientsRadiationPairsList()
    {
        UpdateData();
        //make a copy of the data as to not change the original.
        List<Patient> patientsToFilter = new List<Patient>(allPatients);
        GraphDataFilter filterer = new GraphDataFilter(patientsToFilter);

        List<Patient> filteredPatients = RunAllFilters(filterer);
        List<Tuple<Patient, double>> patientRadiationPairs = filterer.GetRadiationList(filteredPatients, dataType, toothNumbers);

        return patientRadiationPairs;
    }

    /**
     * Update the list of patients,
     * then run all the filtering methods on that full list of patients - filtering depends on the set fields.
     * @return a filtered list of Patients.
     */
    public List<Patient> RunAllFilters(GraphDataFilter filterer)
    {
        //perform all the filters
        filterer.FilterByTumour(tumoursToKeep);
        filterer.FilterByNodal(nodalsToKeep);
        filterer.FilterByGender(gendersToKeep);
        filterer.FilterByAge(ageMin, ageMax);
        filterer.FilterBySite(sitesToKeep);
        filterer.FilterBySide(sidesToKeep);
        filterer.FilterByTreatment(treatmentsToKeep);
        filterer.FilterByTotalRT(totalRTsToKeep);

        List<Patient> filteredPatients = filterer.GetPatients();
        return filteredPatients;
    }


    //Setter methods for filters

    public void SetToothNumbers(List<string> newToothNumbers) { toothNumbers = ConvertToothNameToNumbers(newToothNumbers); }

    public void SetTumoursToKeep(List<string> newTumoursToKeep) { tumoursToKeep = newTumoursToKeep; }

    public void SetNodalsToKeep(List<string> newNodalsToKeep) { nodalsToKeep = newNodalsToKeep; }

    public void SetGendersToKeep(List<string> newGendersToKeep) { gendersToKeep = newGendersToKeep; }

    public void SetAgeMin(int newMin) { ageMin = newMin; }

    public void SetAgeMax(int newMax) { ageMax = newMax; }

    public void SetSitesToKeep(List<string> newSitesToKeep) { sitesToKeep = newSitesToKeep; }

    public void SetSidesToKeep(List<string> newSidesToKeep) { sidesToKeep = newSidesToKeep; }

    public void SetTreatmentsToKeep(List<string> newTreatmentsToKeep) { treatmentsToKeep = newTreatmentsToKeep; }

    public void SetTotalRTsToKeep(List<string> newTotalRTsToKeep) { totalRTsToKeep = newTotalRTsToKeep; }

    public void SetDataType(int newDataType) { dataType = newDataType; }


    /*
     * Given a list of strings, specifying teeth, return the converted list of ints of these.
     * Converts each tooth from Region & Value to an absolute value.
     * Tooth number values will correspond as follows: 0-7 LL; 8-15 LR; 16-23 UL; 24-31 UR.
     * If All is specified, then all the teeth will be selected
     * @param selectedTeeth - A list of strings of teeth selected 
     * @return List of ints each mapping to a specific tooth
     */
    private List<int> ConvertToothNameToNumbers(List<string> selectedTeeth)
    {
        List<int> numberList = new List<int>();
        foreach (string tooth in selectedTeeth)
        {
            string firstTwoLetters = tooth.Substring(0, 2);
            int digit = (int)Char.GetNumericValue(tooth[2]);
            switch (firstTwoLetters)
            {
                case "Al":
                    for (int i = 0; i < 32; i++)
                    {
                        numberList.Add(i);
                    }
                    break;
                case "LL":
                    numberList.Add(digit - 1);
                    break;

                case "LR":
                    numberList.Add(digit + 7);
                    break;

                case "UL":
                    numberList.Add(digit + 15);
                    break;

                case "UR":
                    numberList.Add(digit + 23);
                    break;
            }
        }
        return numberList;
    }

    //Getter methods
    public List<int> GetTeethSelected() { return toothNumbers; }

    public int GetDataType() { return dataType; }

    public List<Patient> GetPatients() { return allPatients; }
}