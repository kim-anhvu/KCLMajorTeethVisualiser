using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 *Class assigned to each Gradient button that allows the user
 *to switch between the gradient display UI to see what gradients they have to a single 
 *or both sets of teeth.
 */
public class GradientButton : MonoBehaviour
{

    public GameObject[] gradientUI = new GameObject[13];
    private List<KeyValuePair<float, Color>> gradientColours;
    public Text range;
    private Tuple<string, string> fromTo;


    /*
     * Displays the chosen gradient on the Gradient Display UI.   
     */   
    public void displayChosenGradient()
    {

        for (int i = 0; i < gradientColours.Count; ++i)
        {
            gradientUI[i].GetComponent<Image>().color = gradientColours[i].Value;
        }
        range.text = fromTo.Item2 + "-" + fromTo.Item1;
        range.fontSize = 40;
        range.color = Color.white;

    }

    /*
     * Setup the gradients panels in the ColourPickerUI.
     * @param i, panel
     */   
    public void setUpGradients(int i, GameObject panel)
    {
        gradientUI[i] = panel;
    }

    /*
     * Setup the gradient range text in the ColourPickerUI.
     * @param range
     */   
    public void setUpGradientText(Text range)
    {
        this.range = range;
    }

    /*
     * Setup the the gradient display element with gradient colours.
     * @param from, to,colours.
     */   
    public void setUpButton(string from, string to, List<Color> colours)
    {
        fromTo = new Tuple<string, string>(from, to);
        gradientColours = ColourCalculator.CalculateColourScale(float.Parse(to), float.Parse(from), colours);

    }


}
