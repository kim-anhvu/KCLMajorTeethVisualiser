using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
 * Script attached to the pateint display button prefab
 */

public class DisplayButton : MonoBehaviour {
    
    public Button buttonComponent;
    public Text indexText;
    public Text genderText;
    public Text ageText;
    public Text tumourText;
    public Text nodalText;
    public Text siteText;
    public Text jawSideText;
    public Text treatmentText;
    public Text totalRTText;
    
    //Keep track of when button is clicked and the colour.
    private bool clicked;  
    private ColorBlock theColor;
    
    private Patient record;
    
    // Use this for initialization
    void Start () 
    {
        //add listener for this button
        buttonComponent.onClick.AddListener (HandleClick);
        clicked = false; 
        theColor = GetComponent<Button>().colors;
        displayOptions();

    }
    /**
     * Used to create a new instance of the button. 
     */
    public void Setup(Patient currentRecord)
    {
        record = currentRecord;
        genderText.text = ""+ record.GetGender();
        ageText.text = "" + record.GetCancerAge();
        tumourText.text = "" + record.GetTumourName();
        nodalText.text = "" + record.GetNodalPosition();
        siteText.text = "" + record.GetSite();
        jawSideText.text = "" + record.GetTumourJawSide();
        treatmentText.text = "" + record.GetTreatment();
        totalRTText.text = "" + record.GetTotalRT();
    }

    private void HandleClick()
    {
        clicked = !clicked; 
        displayOptions();
        record.SetSelectedForVisual(clicked);

    }
    public void Select()
    {
        clicked = true;
        displayOptions();
        record.SetSelectedForVisual(clicked);
    }
    public void DeSelect()
    {
        clicked = false;
        displayOptions();
        record.SetSelectedForVisual(clicked);
    }

    private void displayOptions()
    {
        // Updates colour of entry. 
        if(clicked){
            theColor.normalColor = Color.grey;
            buttonComponent.colors = theColor;
        }
        else
        {
            theColor.normalColor = Color.black;
            buttonComponent.colors = theColor;
        }
    }

    public bool getClicked()
    {
        return clicked;
    }

    public Patient getPatient()
    {
        return record;
    }
    
}