using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

/**
 * Resets teeth model and patient data to empty
 */
public class ResetTeeth : MonoBehaviour
{
    public GameObject teethA;
    public GameObject teethB;
    public ChangeTeethColour teethAScript;
    public ChangeTeethColour teethBScript;
    public RecordScrollList deselect;
    public ComparePatients deselectCompare;
    public GameObject patientDisplay;
    public GameObject patientDisplayComp;
    public ColourPickerUI colourPickerUI;

    void Start(){
        teethAScript = teethA.GetComponent<ChangeTeethColour>();
        teethBScript = teethB.GetComponent<ChangeTeethColour>();
        colourPickerUI = this.transform.root.GetComponentInChildren<ColourPickerUI>();
        deselect = this.transform.root.GetComponentInChildren<RecordScrollList>();
        deselectCompare = this.transform.root.GetComponentInChildren<ComparePatients>();
    }

    public void Reset() {
      ResetColour();
      ResetHover();
      ClearPatients();
        ResetMinMax();
    }

    public void ResetMinMax()
    {
        colourPickerUI.resetMinMAx();
    }

    public void ResetColour()
    {
       double[] empty = new double[16];
        DataModel.setAverageMeanRadiation(empty, empty,"firstSet");
        DataModel.setAverageMeanRadiation(empty, empty, "secondSet");
       
        colourPickerUI.resetColourMapA();
        colourPickerUI.resetColourMapB();

        teethAScript.resetColour();
        teethBScript.resetColour();
              
    }

    public void ResetHover() {

      GameObject bottomSetA;
      GameObject topSetA;
      GameObject bottomSetB;
      GameObject topSetB;

      topSetA = GameObject.Find("TopTeethSet");
      bottomSetA = GameObject.Find("BottomTeethSet");

      topSetB = GameObject.Find("teethTopSetB");
      bottomSetB = GameObject.Find("teethBottomSetB");

      ResetComponents(topSetA, bottomSetA);
      ResetComponents(topSetB, bottomSetB);
    }

    private void ResetComponents(GameObject topSet, GameObject bottomSet) {
      foreach(Transform t in bottomSet.transform)
      {
        try {
          MeshCollider meshCollider = t.gameObject.GetComponent<MeshCollider>();
          Destroy(meshCollider);
        } catch(Exception e) {
        }

        try {
          TeethDisplayPanel tdp = t.gameObject.GetComponent<TeethDisplayPanel>();
          Destroy(tdp);
        } catch(Exception e) {
        }
      }

      foreach(Transform t in topSet.transform)
      {
        try {
          MeshCollider meshCollider = t.gameObject.GetComponent<MeshCollider>();
          Destroy(meshCollider);
        } catch(Exception e) {
        }

        try {
          TeethDisplayPanel tdp = t.gameObject.GetComponent<TeethDisplayPanel>();
          Destroy(tdp);
        } catch(Exception e) {
        }
      }
    }

    private void ClearPatients()
    {
        deselect.ClearSelectedRecords();
        deselect.DeSelectAllButtons();
        deselectCompare.Clear();
        if(patientDisplay.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text != "gender")
        {
            patientDisplay.GetComponent<LoadPatient>().DisplayPatients();
        }
        patientDisplay.SetActive(false);
        patientDisplayComp.SetActive(false);
    }
}
