using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

/**
    Class to display the results (patient records)
    Class is updated on user input.
 */

public class RecordScrollList : MonoBehaviour
{

    // Where results are displayed
    public Transform contentPanel;
    // Data values to display
    public List<Patient> records;
    // Data values selected for loading
    private List<Patient> selectedRecords;
    // Prefab of how the record will be displayed
    public GameObject prefab;
    public GameObject patientDisplayPanel;
    public GameObject patientDisplayPanelComp;
    //Search query parameters
    private SearchQuery search;
    public GameObject teethA;
    public GameObject teethB;
    private ChangeTeethColour teethAScript;
    private ChangeTeethColour teethBScript;

    // Select All
    public Toggle selectAll;

    //Values for number of records displayed
    private int maxRecoredsDisplayed = 10;
    private int recordsToAdd = 5;
    private int viewMoreClickCount = 0;

    public Button viewMore;

    public List<Patient> getSelectedRecords()
    {
        return selectedRecords;
    }

    public void ClearSelectedRecords()
    {
        selectedRecords.Clear();
    }
    // Use this for initialization
    void Start()
    {
        ComparePatients.rcl = this;
        records = DataModel.GetAllPatients();
        search = new SearchQuery();
        selectedRecords = new List<Patient>();
        updateResults();
        patientDisplayPanel.SetActive(false);
        patientDisplayPanelComp.SetActive(false);
        teethAScript = teethA.GetComponent<ChangeTeethColour>();
        teethBScript = teethB.GetComponent<ChangeTeethColour>();

        // Action Listener for toggle
        selectAll.onValueChanged.AddListener((bool on) =>
        {
            //Select All Records as selected else De-select all records
            if (on) {  SelectAllDisplayedRecords(); }
            else{ DeSelectAllDisplayedRecords();}
        }
        );
    }

    // Shows more records if the user clicks to view more.
    private void ViewMoreRecords()
    {
        // TODO - Functionality of if selectAll was on.
        bool allSelected = selectAll.isOn;
        if(allSelected){
            DeSelectAllDisplayedRecords();
        }
        viewMoreClickCount ++;
        updateResults();

    }

    // Select all dispalyed records
    private void SelectAllDisplayedRecords()
    {
        SelectAllButtons();
    }

    // Deselect all dispalyed records
    private void DeSelectAllDisplayedRecords()
    {
        DeSelectAllButtons();
    }

    // Called when Pateints data has changed.
    public void ReloadData()
    {
        //re-get pateient data.
        updateResults();
    }

    private void updateResults()
    {
        RemoveButtons();
        placeButtons();

    }

    // Places the display button for each record
    private void placeButtons()
    {
        int dispalyed = 0;
        int pateintsToDisplay = 0;
        foreach (Patient record in records)
        {
            // Record must meet filter criteria
            if (search.filter(record))
            {
                GameObject newButton = (GameObject)GameObject.Instantiate(prefab);
                newButton.transform.SetParent(null);
                newButton.transform.SetParent(contentPanel);    //place button inside contentPanel
                newButton.transform.localScale = new Vector3(1, 1, 1);  // ensure scaling is set to 1
                newButton.SetActive(true);  //make visible.

                DisplayButton displayButton = newButton.GetComponent<DisplayButton>();
                displayButton.Setup(record);    // sets data of the display button.

                if(maxRecoredsDisplayed + (viewMoreClickCount * recordsToAdd) <= dispalyed){
                    newButton.gameObject.SetActive(false);
                }
                else{
                    dispalyed = dispalyed + 1;
                }
                pateintsToDisplay = pateintsToDisplay + 1;

            }
        }

        //If there are more records to add display the View More button.
        if(pateintsToDisplay > dispalyed){
            Button viewMoreButton = (Button)GameObject.Instantiate(viewMore);
            viewMoreButton.transform.SetParent(contentPanel);
            viewMoreButton.transform.localScale = new Vector3(1, 1, 1);
            viewMoreButton.onClick.AddListener (ViewMoreRecords);
            viewMoreButton.gameObject.SetActive(true);
        }
    }
    // Remove all the buttons from the list.
    private void RemoveButtons()
    {
        //buttonObjectPool.GetObject()
        foreach (Transform child in contentPanel)
        {
            var button = child.GetComponent<DisplayButton>();
            GameObject.Destroy(child.gameObject);
            
        }
    }

    public void load()
    {
        FindObjectOfType<ResetTeeth>().ResetColour();
        FindObjectOfType<ResetTeeth>().ResetHover();
        selectedRecords = getClickedRecords();
        if (selectedRecords.Count > 0)
        {
            patientDisplayPanel.SetActive(true);
            LoadPatient patientDisplay = patientDisplayPanel.GetComponent<LoadPatient>();
            patientDisplay.LoadPatients(selectedRecords);

            TeethScriptAssigner assigner = ScriptableObject.CreateInstance<TeethScriptAssigner>();
            assigner.Begin(selectedRecords, true);

            teethAScript.resetColour();
            teethBScript.resetColour();

            DataModel.setAverageMeanRadiation(
            DataModel.getBottomAverageMeanRadiation(selectedRecords, "bottom"),
            DataModel.getTopAverageMeanRadiation(selectedRecords, "top"), "firstSet");

            transform.root.GetComponentInChildren<ColourPickerUI>().setMAxMin(
            DataModel.getMinMAx(selectedRecords).Item1,
            DataModel.getMinMAx(selectedRecords).Item2, "firstSet");
        }
        else
        {
            double[] empty = new double[16];
            patientDisplayPanel.SetActive(false);
            DataModel.setAverageMeanRadiation(empty,empty,"firstSet");
            //ColourModel.applyMap(empty,"bottom");
            //ColourModel.applyMap(empty,"top");

        }

        ThirdPersonCamera t = GameObject.Find("Main Camera").GetComponent<ThirdPersonCamera>();
        t.setSplitScreen(false);
        ThirdPersonCamera t2 = GameObject.Find("Main Camera-B").GetComponent<ThirdPersonCamera>();
        t2.setSplitScreen(false);
    }

    // If a record has been clicked add to the selected list.
    public List<Patient> getClickedRecords()
    {
        List<Patient> sRecords = new List<Patient>();
        //sRecords.Clear();
        foreach (Transform child in contentPanel)
        {
            var button = child.GetComponent<DisplayButton>();
            if(button != null){
                if (button.getClicked())
                {
                    sRecords.Add(button.getPatient());
                }
            }
        }
        return sRecords;
    }


    // Click All Buttons
    public void SelectAllButtons()
    {
        foreach (Transform child in contentPanel)
        {
            var button = child.GetComponent<DisplayButton>();
            if(button != null){
                button.Select();
            }
        }
    }

    // Un-Click All Buttons
    public void DeSelectAllButtons()
    {
        foreach (Transform child in contentPanel)
        {
            var button = child.GetComponent<DisplayButton>();
            if(button != null){
                button.DeSelect();
            }
        }
    }
    // Methods for keeping track of search query from the user

    public void setGender(List<string> gender)
    {
        search.SetGender(gender);
    }
    public void setFromAge(int age)
    {
        search.SetFromAge(age);
    }
    public void setToAge(int age)
    {
        search.SetToAge(age);
    }
    public void setTumour(List<string> tumourType)
    {
        search.SetTumour(tumourType);
    }
    public void setNodal(List<string> nodal)
    {
        search.SetNodal(nodal);
    }
    public void setSite(List<string> site)
    {
        search.SetSite(site);
    }
    public void setJawSide(List<string> jawSide)
    {
        search.SetJawSide(jawSide);
    }
    public void setTreatment(List<string> treatment)
    {
        search.SetTreatment(treatment);
    }
    public void setTotalRT(List<string> totalRT)
    {
        search.SetTotalRT(totalRT);
    }
    public void filter()
    {
        this.viewMoreClickCount = 0;
        updateResults();
    }
}
