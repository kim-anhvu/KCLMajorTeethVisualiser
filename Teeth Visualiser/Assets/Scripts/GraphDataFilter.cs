using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/**
* GraphDataFilter class.
* 
* Provide methods for filtering out patients based on given attributes.
*/
public class GraphDataFilter
{
    List<Patient> patients;
    public GraphDataFilter(List<Patient> patients)
    {
        this.patients = patients;
    }

    /**
     * Filtering methods:
     * using the fields, any Patients which do not pass the filters will be removed from the list.
     * If All is specified as the first entry in any of the List fields, no patients will be removed by that one filter.
     * If an empty string list is given, or age where the max < min, then return an empty patients list (essentially everything removed)
     * Order of patients remains unchanged
     */
    public void FilterByTumour(List<string> tumoursToKeep)
    {
        if(tumoursToKeep.Count == 0)
        {
            patients = new List<Patient>();
            return;
        }
        else if (!(tumoursToKeep[0] == "All"))
        {
            for (int i = patients.Count - 1; i >= 0; i--)
            {
                string patientTumour = patients[i].GetTumourName();
                if (!tumoursToKeep.Contains(patientTumour))
                {
                    patients.RemoveAt(i);
                }
            }
        }
    }

    public void FilterByNodal(List<string> nodalsToKeep)
    {
        if(nodalsToKeep.Count == 0)
        {
            patients = new List<Patient>();
            return;
        }
        else if (!(nodalsToKeep[0] == "All"))
        {
            for (int i = patients.Count - 1; i >= 0; i--)
            {
                string patientNodal = patients[i].GetNodalPosition();
                if (!nodalsToKeep.Contains(patientNodal))
                {
                    patients.RemoveAt(i);
                }
            }
        }
    }

    public void FilterByGender(List<string> gendersToKeep)
    {
        if(gendersToKeep.Count == 0)
        {
            patients = new List<Patient>();
            return;
        }
        else if (!(gendersToKeep[0] == "All"))
        {
            for (int i = patients.Count - 1; i >= 0; i--)
            {
                string patientGender = patients[i].GetGender();
                if (!gendersToKeep.Contains(patientGender))
                {
                    patients.RemoveAt(i);
                }
            }
        }
    }

    public void FilterByAge(int ageMin, int ageMax)
    {
        if(ageMax < ageMin)
        {
            patients = new List<Patient>();
            return;
        }
        for(int i = patients.Count - 1; i >= 0; i--)
        {
            int patientCancerAge = patients[i].GetCancerAge();
            if(patientCancerAge < ageMin || patientCancerAge > ageMax) //Age is inclusive, meaning those less than the min or more more than max the specified will be removed
            {
                patients.RemoveAt(i);
            }
        }
    }

    public void FilterBySite(List<string> sitesToKeep)
    {
        if(sitesToKeep.Count == 0)
        {
            patients = new List<Patient>();
            return;
        }
        else if (!(sitesToKeep[0] == "All"))
        {
            for(int i = 0; i < sitesToKeep.Count; i++)
            {
                sitesToKeep[i] = sitesToKeep[i].ToLower();
            }
            for (int i = patients.Count - 1; i >= 0; i--)
            {
                string patientSite = patients[i].GetSite();
                if (!sitesToKeep.Contains(patientSite.ToLower()))
                {
                    patients.RemoveAt(i);
                }
            }
        }
    }

    public void FilterBySide(List<string> sidesToKeep)
    {
        if(sidesToKeep.Count == 0)
        {
            patients = new List<Patient>();
            return;
        }
        else if (!(sidesToKeep[0] == "All"))
        {
            for (int i = patients.Count - 1; i >= 0; i--)
            {
                string patientSide = patients[i].GetTumourJawSide();
                if (!sidesToKeep.Contains(patientSide))
                {
                    patients.RemoveAt(i);
                }
            }
        }
    }

    public void FilterByTreatment(List<string> treatmentsToKeep)
    {
        if(treatmentsToKeep.Count == 0)
        {
            patients = new List<Patient>();
            return;
        }
        else if (!(treatmentsToKeep[0] == "All"))
        {
            for (int i = patients.Count - 1; i >= 0; i--)
            {
                string patientTreatment = patients[i].GetTreatment();
                if (!treatmentsToKeep.Contains(patientTreatment))
                {
                    patients.RemoveAt(i);
                }
            }
        }
    }

    public void FilterByTotalRT(List<string> totalRTsToKeep)
    {
        if(totalRTsToKeep.Count == 0)
        {
            patients = new List<Patient>();
            return;
        }
        else if (!(totalRTsToKeep[0] == "All"))
        {
            for (int i = patients.Count - 1; i >= 0; i--)
            {
                string patientTotalRT = patients[i].GetTotalRT();
                if (!totalRTsToKeep.Contains(patientTotalRT))
                {
                    patients.RemoveAt(i);
                }
            }
        }
    }


    /**
     * Given a list of filtered patients, return a List of Tuples <Patient, double> of the patients and their radiation values, for the teeth specified in toothNumbers.
     * @param filteredPatients - a List of filtered Patients
     * @param dataType -  which data type to access: 0=min, 1=mean, 2=max
     * @param toothNumbers - The teeth from which to get radiation from (if more than 1 provided, then mean-avg the total radiation values)
     * @return a List of Tuples <Patient, double> of the patients and their radiation values.
     */
    public List<Tuple<Patient, double>> GetRadiationList(List<Patient> filteredPatients, int dataType, List<int> toothNumbers)
    {
        List<Tuple<Patient, double>> radiationValues = new List<Tuple<Patient, double>>();

        foreach(Patient patient in filteredPatients)
        {
            double patientTotalRadiation = 0.0;
            foreach(int toothNumber in toothNumbers)
            {
                patientTotalRadiation = patientTotalRadiation + GetToothRadiation(patient, toothNumber, dataType);
            }
            double meanOfRadiations = patientTotalRadiation / toothNumbers.Count;
            Tuple<Patient, double> tupleToAdd = new Tuple<Patient, double>(patient, meanOfRadiations);
            radiationValues.Add(tupleToAdd);
        }
        return radiationValues;
    }

    /**
     * Given a patient and a tooth number, get the radiation value of that patient's tooth
     * @param patient - the patients whose radiation to get
     * @param toothNumber - the tooth number of which to get radiation for
     * @return the radiation value of that specified patient's specified tooth
     */
    private double GetToothRadiation(Patient patient, int toothNumber, int dataType)
    {
        if(toothNumber < 0 || toothNumber > 31)
        {
            //This is an invalid tooth number, and thus a value of 0 will be removed.
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

        double radiationValueToReturn = teethArray[toothNumber, dataType];
        return radiationValueToReturn;  
    }

    //Getter Method(s)
    public List<Patient> GetPatients(){return patients;}
}