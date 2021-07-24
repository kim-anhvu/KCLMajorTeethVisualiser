using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Compare Patients Class which provides the abilty to keep track of the user selections.
 * Monitors the progess of the selection process.
 * Abilty to cancel the comparison process.
 * Invokes the methods to comparison data.
 */

public class ComparePatients : MonoBehaviour
{
    Text compareButtonText;
    Button loadButton;
    Button compareButton;
    Button cancelButton;

    public Transform contentPanel;
    public GameObject patientDisplayPanel;
    public GameObject patientDisplayPanelB;

    private string compareText = "Compare";
    private string selectionOne = "Select First Set";
    private string selectionTwo = "Select Second Set";
    private double cancelXValue;

    public static RecordScrollList rcl;

    // Used to store selected patient records
    private List<Patient> firstSet;
    private List<Patient> secondSet;

    private int stage = 0;

    //sliderButton
    public Button openSlider;
    public Button closeSlider;

    /**
    * This method is assigned to the compare button which is convenient
    * as it can carry out different methods depending on the stage the
    * compare record selection is at.
    */
    public void ClickCompare()
    {
        switch(stage)
        {
            case 0:
                BeginCompare();
            break;
            case 1:
               SelectFirstSet();
            break;
            case 2:
                SelectSecondSet();
            break;
        }
    }

    public void BeginCompare()
    {

        loadButton = GameObject.Find("LoadButton").GetComponent<Button>();
        compareButton = GameObject.Find("CompareButton").GetComponent<Button>();
        cancelButton = GameObject.Find("CancelButton").GetComponent<Button>();
        loadButton.interactable = false;
        cancelButton.interactable = true;

        compareButtonText = compareButton.GetComponentInChildren<Text>();
        compareButtonText.text = selectionOne;
        compareButtonText.fontSize = 24;

        stage++;
        rcl.DeSelectAllButtons();
    }

    public void SelectFirstSet()
    {
        contentPanel = GameObject.Find("Content").GetComponent<Transform>();
        firstSet = contentPanel.GetComponent<RecordScrollList>().getClickedRecords();
        if(firstSet.Count > 0)
          SelectedFirstSet();
    }

    private void SelectedFirstSet()
    {
        compareButtonText = compareButton.GetComponentInChildren<Text>();
        compareButtonText.text = selectionTwo;
        stage++;
        rcl.DeSelectAllButtons();
    }

    public void SelectSecondSet()
    {
        contentPanel = GameObject.Find("Content").GetComponent<Transform>();
        secondSet = contentPanel.GetComponent<RecordScrollList>().getClickedRecords();
        if(secondSet.Count > 0)
          SelectedSecondSet();
    }

    private void SelectedSecondSet()
    {
        rcl.DeSelectAllButtons();
        Submit();
    }

    /**
    * Activates the split screen to display two sets of teeth and
    * passes the data sets so the values for radiation per tooth
    * can be calculated.
    */
    private void Submit()
    {

        ThirdPersonCamera t = GameObject.Find("Main Camera").GetComponent<ThirdPersonCamera>();
        t.setSplitScreen(true);
        ThirdPersonCamera t2 = GameObject.Find("Main Camera-B").GetComponent<ThirdPersonCamera>();
        t2.setSplitScreen(true);
        patientDisplayPanel.SetActive(true);
        patientDisplayPanelB.SetActive(true);
        LoadPatient patientDisplay = patientDisplayPanel.GetComponent<LoadPatient>();
        LoadPatient patientDisplayComp = patientDisplayPanelB.GetComponent<LoadPatient>();
        patientDisplay.LoadPatients(firstSet);
        patientDisplayComp.LoadPatients(secondSet);

        // Reset any previous colour or hover values
        FindObjectOfType<ResetTeeth>().ResetColour();
        FindObjectOfType<ResetTeeth>().ResetHover();

        DataModel.setAverageMeanRadiation(
        DataModel.getBottomAverageMeanRadiation(firstSet, "bottom"),
        DataModel.getTopAverageMeanRadiation(firstSet, "top"), "firstSet");

        transform.root.GetComponentInChildren<ColourPickerUI>().setMAxMin(
        DataModel.getMinMAx(firstSet).Item1,
        DataModel.getMinMAx(firstSet).Item2, "firstSet");


        DataModel.setAverageMeanRadiation(
        DataModel.getBottomAverageMeanRadiation(secondSet, "bottom"),
        DataModel.getTopAverageMeanRadiation(secondSet, "top"), "secondSet");

        transform.root.GetComponentInChildren<ColourPickerUI>().setMAxMin(
        DataModel.getMinMAx(secondSet).Item1,
        DataModel.getMinMAx(secondSet).Item2, "secondSet");

        // Assign hover values to both sets of teeth
        TeethScriptAssigner assignerA = ScriptableObject.CreateInstance<TeethScriptAssigner>();
        assignerA.Begin(firstSet, true);

        TeethScriptAssigner assignerB = ScriptableObject.CreateInstance<TeethScriptAssigner>();
        assignerB.Begin(secondSet, false);

        //slider animation
        closeSlider.onClick.Invoke();
        openSlider.onClick.Invoke();

        Reset();
    }

    /**
    * Set the buttons involved in the compare function back to default
    */
    private void Reset()
    {
        loadButton = GameObject.Find("LoadButton").GetComponent<Button>();
        compareButton = GameObject.Find("CompareButton").GetComponent<Button>();
        cancelButton = GameObject.Find("CancelButton").GetComponent<Button>();
        cancelButton.interactable = false;
        loadButton.interactable = true;
        compareButtonText = compareButton.GetComponentInChildren<Text>();
        compareButtonText.text = compareText;
        compareButtonText.fontSize = 30;
        this.stage = 0;

    }

    public void CancelCompare()
    {
        Reset();
    }

    public void Clear()
    {
      if(firstSet != null)
          firstSet.Clear();
      if(secondSet != null)
          secondSet.Clear();
      ThirdPersonCamera t = GameObject.Find("Main Camera").GetComponent<ThirdPersonCamera>();
      t.setSplitScreen(false);
      ThirdPersonCamera t2 = GameObject.Find("Main Camera-B").GetComponent<ThirdPersonCamera>();
      t2.setSplitScreen(false);
    }
}
