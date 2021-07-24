using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/**
 * This Class is responsible for Handeling the GUI components of the colourPickerUI
 * component
 */

public class ColourPickerUI : MonoBehaviour
{
    public List<Button> colourButtons;
    public InputField from;
    public InputField to;
    public GameObject teethA;
    public GameObject teethB;
    private ChangeTeethColour teethBScript;
    private ChangeTeethColour teethAScript;
    public ErrorMessage errorMessageText;

    public Text SetALabel;
    public Text SetALabel1;
    public GameObject[] gradient;
    public Text range;

    public Text SetBLabel;
    public Text SetBLabel1;
    public GameObject[] gradientB;
    public Text rangeB;


    public Text[] teethAMaxMin;
    public Text[] teethBMaxMin;

    public Transform transformPanel;
    public Transform transformPanelB;

    public GameObject gradiantButtonPrefab;
    private int counter = 1;
    private int counterB = 1;

    private ColourPickerUIController teethAController;
    private ColourPickerUIController teethBController;

    void Start()
    {
        teethAController = new ColourPickerUIController();
        teethBController = new ColourPickerUIController();
        teethAScript = teethA.GetComponent<ChangeTeethColour>();
        teethBScript = teethB.GetComponent<ChangeTeethColour>();
    }


    /*
     * Apply first set Button Handler,apply colour to the first set of 
     * Teeth.
     */    
    public void ApplyFirst()
    {
        teethAController.setAverageMeanRadiation(DataModel.MeanRadiationBottomTeethA, DataModel.MeanRadiationTopTeethA);
        Apply("first", ref teethAController, ref teethAScript);
        resetUI();

    }


    /*
     * Apply second set Button Handler,apply colour to the second set of 
     * Teeth.
     */
    public void ApplySecond()
    {
        ThirdPersonCamera t = GameObject.Find("Main Camera").GetComponent<ThirdPersonCamera>();
        if (t.splitScreen)
        {
            teethAController.setAverageMeanRadiation(DataModel.MeanRadiationBottomTeethA, DataModel.MeanRadiationTopTeethA);
            teethBController.setAverageMeanRadiation(DataModel.MeanRadiationBottomTeethB, DataModel.MeanRadiationTopTeethB);
            Apply("second", ref teethBController, ref teethBScript);
            resetUI();
        }
        else {
            errorMessageText.SetErrorMessage("Second set not selected");
        }
    }


    /*
     *Based on the button pressed, apply generate and apply the correct map.
     *@param set, teethController, teeth
     */   
    public void Apply(string set, ref ColourPickerUIController teethController, ref ChangeTeethColour teeth)
    {
        errorMessageText.ResetErrorMessage();
        try
        {
            if (!(float.Parse(from.text) < 0) && !(float.Parse(to.text) < float.Parse(from.text)))
            {
                SetUpUIController(ref teethController);
                if (teethController.getSelectedColours().Count > 0)
                {
                   PaintTheTeeth(set, ref teethController, ref teeth);
                }
                else
                {
                    errorMessageText.SetErrorMessage("No colour selected!");
                }
            }
            else
            {
                errorMessageText.SetErrorMessage("Invalid range");
            }
        }
        catch (FormatException ex)
        {
            errorMessageText.SetErrorMessage("Invalid input!");
        }
        catch (Exception ex)
        {
            throw;
        }

    }

    /*Paint the teeth with 
     *the correct map
     *@param set, teethController, teeth
     */
    private void PaintTheTeeth(string set, ref ColourPickerUIController teethController, ref ChangeTeethColour teeth)
    {
        int oldColourMapSize = teethController.getRadColourMapSize();
        teethController.CreateColourMaps();
        int newColourMapSize = teethController.getRadColourMapSize();
        List<Tuple<int, Color, string>> topPaintList = teethController.paintTopTeeth();
        List<Tuple<int, Color, string>> bottomPaintList = teethController.paintBottomTeeth();
        if (topPaintList != null)
        {
            if (oldColourMapSize != newColourMapSize)
            {
                GenerateColourPickerUIUserFeedBack(set);

                for (int i = 0; i < topPaintList.Count; ++i)
                {
                    teethController.clearSelectedColours();
                    teeth.setTeethColour(topPaintList[i].Item1, topPaintList[i].Item2, topPaintList[i].Item3);
                    teeth.setTeethColour(bottomPaintList[i].Item1, bottomPaintList[i].Item2, bottomPaintList[i].Item3);
                }
            }
            else
            {
                teethController.clearSelectedColours();

                Debug.Log("select a Patient none selected");
                errorMessageText.SetErrorMessage("No patients selected!");   
               }
        }
    }

    /*
     * Generates a bar of the colours the teeth will be painted
     * with buttons to switch between selected gradients.    
     * @param set
    */
    private void GenerateColourPickerUIUserFeedBack(string set)
    {
        if (set == "first")
        {
            createButton("firstSet");
            setGradientsA();
        }
        else
        {
            createButton("secondSet");
            setGradientsB();
        }
    }

    /*
     * Setup the ControllerFor the UI
     * @param teethController    
    */
    private void SetUpUIController(ref ColourPickerUIController teethController)
    {
        teethController.setInputAndOutputRadiation(float.Parse(from.text), float.Parse(to.text));
        teethController.createListOfSelectedColours(colourButtons);
    }

    /*
     * Reset the UI of the Colour Picker
     */
    public void resetUI()
    {
        from.text = "";
        to.text = "";
        foreach (Button button in colourButtons)
        {
            if (button.GetComponent<ColourSelect>().getClicked())
            {
                button.GetComponent<ColourSelect>().HandleClick(button.animator);
            }
        }
    }

    /*
     * Reset the colourMap for set A
     */    
    public void resetColourMapA()
    {
        teethAController.resetColourMapToFromList();
        teethAController.clearSelectedColours();
        teethAScript.resetColour();
        resetGradientColoursA();
        range.text = "";
        SetALabel.text = "";
        SetALabel1.text = "";
        RemoveButtonsA();
        counter = 1;
    }

    /*
     * Reset colourMapB for set B
     */
    public void resetColourMapB()
    {
        teethBController.resetColourMapToFromList();
        teethBController.clearSelectedColours();
        teethBScript.resetColour();
        resetGradientColoursB();
        rangeB.text = "";
        SetBLabel.text = "";
        SetBLabel1.text = "";
        RemoveButtonsB();
        counterB = 1;
    }

    /*
     * Create buttons that allow user to switch between gradients for the assigned
     * teeth.
     * @param set
     */
    public void createButton(string set)
    {
        if (set == "firstSet")
        {
            createNewButton(from.text, to.text, teethAController.getSelectedColours(), transformPanel, ref counter, range, gradient);
        }
        else
        {
            createNewButton(from.text, to.text, teethBController.getSelectedColours(), transformPanelB, ref counterB, rangeB, gradientB);
        }
    }

    /*
    * Create buttons that allow user to switch between gradients for the assigned
    * teeth.
    * @param set
    */
    public void createNewButton(string from, string to, List<Color> colors, Transform panel, ref int counter, Text range, GameObject[] gradients)
    {
        GameObject newButton = (GameObject)GameObject.Instantiate(gradiantButtonPrefab);
        newButton.transform.SetParent(null);
        newButton.transform.SetParent(panel);    //place button inside contentPanel
        newButton.transform.localScale = new Vector3(1, 1, 1);  // ensure scaling is set to 1
        newButton.GetComponentInChildren<Text>().text = "Gradient" + counter.ToString();
        newButton.SetActive(true);  //make visible.
        counter++;
        GradientButton gradientButton = newButton.GetComponent<GradientButton>();
        gradientButton.setUpGradientText(range);
        for (int i = 0; i < gradients.Length; ++i)
        {
            gradientButton.setUpGradients(i, gradients[i]);
        }
        gradientButton.setUpButton(to, from, colors);
    }

    /*
     * Reset colour gradients for teeth set A   
     */   
    public void resetGradientColoursA()
    {
        foreach (GameObject segment in gradient)
        {
            segment.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }
    }

    /*
     * Reset colour gradient buttons for teeth set B
     */
    public void resetGradientColoursB()
    {
        foreach (GameObject segment in gradientB)
        {
            segment.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }
    }

    /*
     * Remove all the buttons for creating the gradients for set A.
     */    
    private void RemoveButtonsA()
    {
        foreach (Transform child in transformPanel)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    /*
     * Remove all the buttons for creating the gradients for set B.
     */
    private void RemoveButtonsB()
    {
        foreach (Transform child in transformPanelB)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

   
    /*
     * Set gradients for the colour picker gradient set A element.
     */
    public void setGradientsA()
    {
        List<KeyValuePair<float, Color>> colours = ColourCalculator.CalculateColourScale(float.Parse(from.text), float.Parse(to.text), teethAController.getSelectedColours());
        range.text = from.text + "-" + to.text;
        range.fontSize = 40;
        range.color = Color.white;

        SetALabel.text = "Set 1";
        SetALabel1.text = "Set 1";
        for (int i = 0; i < colours.Count; ++i)
        {
            gradient[i].GetComponent<Image>().color = colours[i].Value;
        }
    }

    /*
    * Set gradients for the colour picker gradient set B element.
    */
    public void setGradientsB()
    {
        List<KeyValuePair<float, Color>> colours = ColourCalculator.CalculateColourScale(float.Parse(from.text), float.Parse(to.text), teethBController.getSelectedColours());
        rangeB.text = from.text + "-" + to.text;
        rangeB.fontSize = 40;
        rangeB.color = Color.white;

        SetBLabel.text = "Set 2";
        SetBLabel1.text = "Set 2";
        for (int i = 0; i < colours.Count; ++i)
        {
            gradientB[i].GetComponent<Image>().color = colours[i].Value;
        }
    }

    /*
     * Set min and max radiation values for the user to see in colour picker.   
     */
    public void setMAxMin(double min, double max, string set)
    {
        if (set == "firstSet")
        {

            setMAxMinSet(teethAMaxMin, min, max);

        }
        else
        {

            setMAxMinSet(teethBMaxMin, min, max);
        }
    }

    /*
     * Set min and max radiation values for the user to see in colour picker.   
     */
    public void setMAxMinSet(Text[] teethMaxMin, double min, double max)
    {

        teethMaxMin[1].text = "Min: " + min;
        teethMaxMin[2].text = "Max: " + max;
    }

    public void resetMinMAx(){
        teethAMaxMin[0].text = "";
        teethAMaxMin[1].text = "";
        teethAMaxMin[2].text = "";

        teethBMaxMin[0].text = "";
        teethBMaxMin[1].text = "";
        teethBMaxMin[2].text = "";
    }
}
