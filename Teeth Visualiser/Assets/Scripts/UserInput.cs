using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;

/**
    Keeps track of user inputs.
    Passes updated values to the content panel. 
 */
public class UserInput : MonoBehaviour
{
    //User Input fields
    public TextMeshProUGUI from;
    public TextMeshProUGUI to;
    public GameObject genders;
    public GameObject tumourType;
    public GameObject toothType;
    public GameObject nodal;
    public GameObject site;
    public GameObject jawSide;
    public GameObject treatment;
    public GameObject totalRT;
    public TMP_Dropdown dataType;
    public TMP_Dropdown xVariable;
    public TMP_InputField ageRange;

    public ErrorMessage errorMessage;
    // Where results are displayed 
    public Transform contentPanel;

    // Start is called before the first frame update
    void Start()
    {
        updateFilter();
    }


    public void updateFilter()
    {
        FromAgeValueChangeCheck();
        ToAgeValueChangeCheck();
        GenderChangeCheck();
        TumourChangeCheck();
        NodalChangeCheck();
        SiteChangeCheck();
        JawSideChangeCheck();
        TreatmentChangeCheck();
        TotalRTChangeCheck();
        if (dataType != null && xVariable != null && toothType != null) 
        {
            DataTypeCheck();
            XVariableCheck();
            ToothTypeCheck();

            if(ageRange.text != string.Empty) 
            {
                AgeRangeCheck();
            }
        }
        updateResults();
    }

    //METHODS FOR UPDATING FILTERS

    // Checks Gender toggle changes
    public void GenderChangeCheck()
    {
        List<string> selected = genders.GetComponent<Dropdown>().GetSelected();
        //update content panel. 
        for (int i = 0; i < selected.Count; i++) {
            if (selected[i] == "Male") {
                selected[i] = "M";
            }
            else if (selected[i] == "Female") {
                selected[i] = "F";
            }
        }
        if (contentPanel != null)
        {
            contentPanel.GetComponent<RecordScrollList>().setGender(selected);
        }

        else {
            GraphInput.SetGender(selected);
        }
    
    }

    // Checks the value of the text field of From Age.
    private void FromAgeValueChangeCheck()
    {   
        int input = 0;
        Int32.TryParse(from.text, out input);
        int fromAge = (input>=0) ? input : -1;
        //update content panel. 

        if(contentPanel != null) {
            contentPanel.GetComponent<RecordScrollList>().setFromAge(fromAge);
        } 
        else {
            GraphInput.SetFromAge(fromAge);
        }
    }
    // Checks the value of the text field of To Age.
    private void ToAgeValueChangeCheck()
    {
        int input = 0;
        Int32.TryParse(to.text, out input);
        int toAge = (input>0) ? input : -1;

        if(contentPanel != null) {
            contentPanel.GetComponent<RecordScrollList>().setToAge(toAge);
        }
        else {
            GraphInput.SetToAge(toAge);
        }
    }
    //Called to update content panel. 
    private void updateResults()
    {
        if (contentPanel != null)
        {
            contentPanel.GetComponent<RecordScrollList>().filter();
        }

    }

    private void TumourChangeCheck()
    {
        List<string> selected = tumourType.GetComponent<Dropdown>().GetSelected();
        if(contentPanel != null) {
            contentPanel.GetComponent<RecordScrollList>().setTumour(selected);
        }
        else {
            GraphInput.SetTumour(selected);
        }
    }

    private void NodalChangeCheck()
    {
        List<string> selected = nodal.GetComponent<Dropdown>().GetSelected();
        if(contentPanel != null) {
            contentPanel.GetComponent<RecordScrollList>().setNodal(selected);
        }
        else {
            GraphInput.SetNodal(selected);
        }
    }

    private void SiteChangeCheck()
    {
        List<string> selected = site.GetComponent<Dropdown>().GetSelected();
        if(contentPanel != null) {
            contentPanel.GetComponent<RecordScrollList>().setSite(selected);
        }
        else {
            GraphInput.SetSite(selected);
        }
    }

    private void JawSideChangeCheck()
    {
        List<string> selected = jawSide.GetComponent<Dropdown>().GetSelected();
        
        for (int i = 0; i < selected.Count; i++)
        {
            if (selected[i] == "Left")
            {
                selected[i] = "L";
            }
            else if (selected[i] == "Right")
            {
                selected[i] = "R";
            }
        }
        if (contentPanel != null)
        {
            contentPanel.GetComponent<RecordScrollList>().setJawSide(selected);
        }
        else {
            GraphInput.SetJawSide(selected);
        }
    }

    private void TreatmentChangeCheck()
    {
        List<string> selected = treatment.GetComponent<Dropdown>().GetSelected();
            if(contentPanel != null) {
            contentPanel.GetComponent<RecordScrollList>().setTreatment(selected);
        }
        else {
            GraphInput.SetTreatment(selected);
        }
    }

    private void TotalRTChangeCheck()
    {
        List<string> selected = totalRT.GetComponent<Dropdown>().GetSelected();
        if(contentPanel != null) {
            contentPanel.GetComponent<RecordScrollList>().setTotalRT(selected);
        }
        else {
            GraphInput.SetTotalRT(selected);
        }
    }

    private void DataTypeCheck() 
    {
        GraphInput.SetDataType(dataType.value);

    }

    private void XVariableCheck() 
    {
        GraphInput.SetXVariable(xVariable.options[xVariable.value].text);
    }

    private void ToothTypeCheck() 
    {
        List<string> selected = toothType.GetComponent<Dropdown>().GetSelected();
        GraphInput.SetToothType(selected);
    }

    private void AgeRangeCheck() 
    {
        int value = int.Parse(ageRange.text);
        GraphInput.SetAgeRange(value);
        if (value <= 0)
        {
            errorMessage.SetErrorMessage("Invalid age range!");
        }
    }
}
