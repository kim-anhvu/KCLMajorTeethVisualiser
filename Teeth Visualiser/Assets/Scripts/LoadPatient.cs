using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/**
 * Displays loaded patients' data on patient display panel
 */
public class LoadPatient : MonoBehaviour
{
    // Patient detail textboxes
    public TextMeshProUGUI genderText;
    public TextMeshProUGUI ageText;
    public TextMeshProUGUI tumourText;
    public TextMeshProUGUI nodalText;
    public TextMeshProUGUI siteText;
    public TextMeshProUGUI jawSideText;
    public TextMeshProUGUI treatmentText;
    public TextMeshProUGUI totalRTText;
    public List<Patient> recordList;

    /*
     * Find average gender data for patients loaded
     */
    public string CalculateAverageGender()
    {
        string gender = null;
        if(recordList.Count > 1)
        {
            if (recordList[0].GetGender() == "M")
            {
                gender = "M";
                for (int i = 1; i < recordList.Count; i++)
                {
                    if(recordList[i].GetGender() == "F")
                    {
                        gender = "M,F";
                    }
                }
            }
            else
            {
                gender = "F";
                for (int i = 1; i < recordList.Count; i++)
                {
                    if (recordList[i].GetGender() == "M")
                    {
                        gender = "M,F";
                    }
                }
            }
            return gender;
        }
        else
        {
            return recordList[0].GetGender();
        }
    }

    /*
     * Find average age data for patients loaded
     */
    public string CalculateAverageAge()
    {
        string age = null;
        if (recordList.Count > 1)
        {
            int averageAge = 0;
            for(int i = 0; i < recordList.Count; i++)
            {
                averageAge += recordList[i].GetCancerAge();
            }
            age = "" + (averageAge / recordList.Count);
            return age;
        }
        else
        {
            return "" + recordList[0].GetCancerAge();
        }
    }

    /*
     * Find average tumour type data for patients loaded
     */
    public string CalculateAverageTumour()
    {
        string tumour = null;
        SortedSet<string> tumours = new SortedSet<string>();
        if (recordList.Count > 1)
        {
            for (int i = 0; i < recordList.Count; i++)
            {
                tumours.Add(recordList[i].GetTumourName());
            }
        }
        else
        {
            tumours.Add(recordList[0].GetTumourName());
        }
        foreach (string tumourName in tumours)
        {
            if (tumour == null)
            {
                tumour = tumourName;
            }
            else
            {
                tumour += "," + tumourName;
            }
        }
        return tumour;
    }

    /*
     * Find average nodal data for patients loaded
     */
    public string CalculateAverageNodal()
    {
        string nodal = null;
        SortedSet<string> nodals = new SortedSet<string>();
        if (recordList.Count > 1)
        {
            for (int i = 0; i < recordList.Count; i++)
            {
                nodals.Add(recordList[i].GetNodalPosition());
            }
        }
        else
        {
            nodals.Add(recordList[0].GetNodalPosition());
        }
        foreach (string nodalPos in nodals)
        {
            if (nodal == null)
            {
                nodal = nodalPos;
            }
            else
            {
                nodal += "," + nodalPos;
            }
        }
        return nodal;
    }

    /*
     * Find average site data for patients loaded
     */
    public string CalculateAverageSite()
    {
        string site = null;
        SortedSet<string> sites = new SortedSet<string>();
        if (recordList.Count > 1)
        {
            for (int i = 0; i < recordList.Count; i++)
            {
                sites.Add(recordList[i].GetSite());
            }
        }
        else
        {
            sites.Add(recordList[0].GetSite());
        }
        foreach (string siteAvg in sites)
        {
            if (site == null)
            {
                site = siteAvg;
            }
            else
            {
                site += "," + siteAvg;
            }
        }
        return site;
    }

    /*
     * Find average jaw side data for patients loaded
     */
    public string CalculateAverageJawSide()
    {
        string jawSide = null;
        SortedSet<string> jawSides = new SortedSet<string>();
        if (recordList.Count > 1)
        {
            for (int i = 0; i < recordList.Count; i++)
            {
                jawSides.Add(recordList[i].GetTumourJawSide());
            }
        }
        else
        {
            jawSides.Add(recordList[0].GetTumourJawSide());
        }
        foreach (string jawSideAvg in jawSides)
        {
            if (jawSide == null)
            {
                jawSide = jawSideAvg;
            }
            else
            {
                jawSide += "," + jawSideAvg;
            }
        }
        return jawSide;
    }

    /*
     * Find average treatment data for patients loaded
     */
    public string CalculateAverageTreatment()
    {
        string treatment = null;
        SortedSet<string> treatments = new SortedSet<string>();
        if (recordList.Count > 1)
        {
            for (int i = 0; i < recordList.Count; i++)
            {
                treatments.Add(recordList[i].GetTreatment());
            }
        }
        else
        {
            treatments.Add(recordList[0].GetTreatment());
        }
        foreach (string treatmentAvg in treatments)
        {
            if (treatment == null)
            {
                treatment = treatmentAvg;
            }
            else
            {
                treatment += "," + treatmentAvg;
            }
        }
        return treatment;
    }

    /*
     * Find average total RT data for patients loaded
     */
    public string CalculateAverageTotalRT()
    {
        string totalRT = null;
        SortedSet<string> totalRTs = new SortedSet<string>();
        if (recordList.Count > 1)
        {
            for (int i = 0; i < recordList.Count; i++)
            {
                totalRTs.Add(recordList[i].GetTotalRT());
            }
        }
        else
        {
            totalRTs.Add(recordList[0].GetTotalRT());
        }
        foreach (string totalRTAvg in totalRTs)
        {
            if (totalRT == null)
            {
                totalRT = totalRTAvg;
            }
            else
            {
                totalRT += "," + totalRTAvg;
            }
        }
        return totalRT;
    }

    /*
     * Get list of loaded patients
     * @param List<Patient> records
     *   list of loaded patients
     */
    public void LoadPatients(List<Patient> records)
    {
        recordList = records;

    }

    /*
     * Set text of textboxes to average data
     */
    public void DisplayPatients()
    {
        if (genderText.text == "Gender")
        {
            genderText.text = CalculateAverageGender();
            ageText.text = CalculateAverageAge();
            tumourText.text = CalculateAverageTumour();
            nodalText.text = CalculateAverageNodal();
            siteText.text = CalculateAverageSite();
            jawSideText.text = CalculateAverageJawSide();
            treatmentText.text = CalculateAverageTreatment();
            totalRTText.text = CalculateAverageTotalRT();
        }
        else
        {
            genderText.text = "Gender";
            ageText.text = "Age";
            tumourText.text = "Tumour";
            nodalText.text = "Nodal";
            siteText.text = "Site";
            jawSideText.text = "Jaw Side";
            treatmentText.text = "Treatment";
            totalRTText.text = "Total RT";
        }
    }

}
