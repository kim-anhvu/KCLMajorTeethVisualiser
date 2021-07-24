
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


/**
 * This is the main class for the logic of the Colour Picker UI where colour maps and
 * paint values and multiple gradients are manipulated.
 */
public class ColourPickerUIController
{
    private List<Color> selectedColoursTeeth = new List<Color>();


    private float fromInput;
    private float toInput;

    private double[] meanRadiationTopTeeth;
    private double[] meanRadiationBottomTeeth;

    private List<KeyValuePair<float, Color>> RadColourMap = new List<KeyValuePair<float, Color>>();
    private List<Tuple<float, float, List<Color>>> fromToRadiationAndColourList = new List<Tuple<float, float, List<Color>>>();

    public ColourPickerUIController()
    {
        fromInput = -1;
        toInput = -1;
        meanRadiationTopTeeth = null;
        meanRadiationBottomTeeth = null;
    }

    /* 
    * Add selected colours to a List in the order they are clicked.   
    * @param colourButtons, set    
    */
    public void createListOfSelectedColours(List<Button> colourButtons)
    {
        List<Button> clickedButtonList = getClickedButtons(colourButtons);
        List<Color> clickedColourList = ExtractColoursFromButton(clickedButtonList);
        setSelectedTeethColours(clickedColourList);

    }

    /*
     * Return a ordered List of the clicked buttons. 
     * @param colourButtons
     */   
    private List<Button> getClickedButtons(List<Button> colourButtons)
    {
        List<Button> unOrderedButtonList = new List<Button>();

        foreach (Button button in colourButtons)
        {
            if (button.GetComponent<ColourSelect>().getClicked())
            {
                unOrderedButtonList.Add(button);
            }
        }

        List<Button> orderedButtonList = unOrderedButtonList
        .OrderBy(aTime => aTime
        .GetComponent<ColourSelect>()
        .getTime())
        .ToList<Button>();
        return orderedButtonList;
    }

    /*
     * Extract the colours from the button and place in List.
     * @param tempButtonList    
     */   
    private List<Color> ExtractColoursFromButton(List<Button> tempButtonList)
    {
        List<Color> colourList = new List<Color>();
        foreach (Button button in tempButtonList)
        {

            colourList.Add(
            button
                .GetComponent<Image>()
                .color);
        }
        return colourList;
    }

    /*
     * Setter for the from and to input values for the radiation gradient.
     * @param from, to
     */
    public void setInputAndOutputRadiation(float from, float to)
    {
        fromInput = from;
        toInput = to;
    }

    /*
     * Create map for each set of teeth.
     * @param map, toFromList, sColours
     */
    public void CreateColourMaps()
    {
        if (fromInput == -1 || toInput == -1)
        {
            throw new Exception("From or To value not set");
        }
        if (selectedColoursTeeth.Count == 0)
        {
            throw new Exception("No colours have been selected");
        }
        else if (RadColourMap.Count != 0)
        {
            addToFromToInputColourList(fromInput, toInput, ref selectedColoursTeeth, ref fromToRadiationAndColourList);
            RadColourMap = checkFromToInputValueDoesntAreadyExist(RadColourMap, fromToRadiationAndColourList);
        }
        else
        {

            addToFromToInputColourList(fromInput, toInput, ref selectedColoursTeeth, ref fromToRadiationAndColourList);
            RadColourMap = ColourCalculator.CalculateColourScale(fromInput, toInput, selectedColoursTeeth);
        }
    }

    /*
     * Check the from and to input doesnt already exist in our List of ranges.
     * @param map, tofromList
     * @return List<KeyValuePair<float, Color>>    
     */
    private List<KeyValuePair<float, Color>> checkFromToInputValueDoesntAreadyExist(List<KeyValuePair<float, Color>> map, List<Tuple<float, float, List<Color>>> tofromList)
    {
        List<float> tempList = new List<float>();
        for (int i = 0; i < map.Count; ++i) { tempList.Add(map[i].Key); }
        for (int i = 0; i < tofromList.Count; ++i)
        {
            if (!(tempList.Contains(tofromList[i].Item1)))
            {
                map.AddRange(ColourCalculator.CalculateColourScale(tofromList[i].Item1, tofromList[i].Item2, tofromList[i].Item3));
            }
        }
        map = map.OrderBy(rad => rad.Key).ToList<KeyValuePair<float, Color>>();
        return map;
    }

    /* 
     * Add the radiation range for the colours to be mapped to.
     * @param from, to, sColours, toFromList 
    */
    private void addToFromToInputColourList(float from, float to, ref List<Color> sColour, ref List<Tuple<float, float, List<Color>>> toFromList)
    {
        if (toFromList.Count == 0)
        {
            toFromList.Add(new Tuple<float, float, List<Color>>(from, to, sColour));
        }
        else
        {
            bool insideRange = checkNewFromToInputInsideRangeExistingFromTo(from, to, ref toFromList);
            if (insideRange == false)
            {
                toFromList.Add(new Tuple<float, float, List<Color>>(from, to, sColour));
            }
        }
    }

    /*
     * Check the user input values aren't within an existing range.
     * @param from, to, toFromList
     * @return bool   
     */
    private bool checkNewFromToInputInsideRangeExistingFromTo(float from, float to, ref List<Tuple<float, float, List<Color>>> toFromList)
    {
        bool insideRange = false;
        for (int i = 0; i < toFromList.Count; ++i)
        {
            if (from >= toFromList[i].Item1 && from <= toFromList[i].Item2)
            {
                insideRange = true;
            }
            if (to >= toFromList[i].Item1 && to <= toFromList[i].Item2)
            {
                insideRange = true;
            }
        }
        return insideRange;
    }

    /*
 * Getter for the position of the tooth for the colour its going to be painted 
 * on bottom set.
 * @param set    
 * @return List<Tuple<int, Color, string>>
 */
    public List<Tuple<int, Color, string>> paintBottomTeeth()
    {
        if (meanRadiationBottomTeeth != null)
        {
            if (RadColourMap.Count == 0)
            {
                throw new Exception("Can't paint teeth with empty map");
            }
            return getToothPositionColourToPaint(ref meanRadiationBottomTeeth, "bottom", ref RadColourMap, ref fromToRadiationAndColourList);
        }
        else
        {
            return null;
        }
    }

    /*
    * Getter for the position of the tooth for the colour its going to be painted 
    * on Top set.
    * @param set   
    * @return List<Tuple<int, Color, string>>
    */
    public List<Tuple<int, Color, string>> paintTopTeeth()
    {
        if (meanRadiationTopTeeth != null)
        {
            if (RadColourMap.Count == 0)
            {
                throw new Exception("Can't paint teeth with empty map");
            }
            return getToothPositionColourToPaint(ref meanRadiationTopTeeth, "top", ref RadColourMap, ref fromToRadiationAndColourList);
        }
        else
        {
            return null;
        }

    }

    /* 
     * Paint the teeth based on the radiation value and the colour associated
     * with it.
     * @param meanRadiation, position, map
     */
    public List<Tuple<int, Color, string>> getToothPositionColourToPaint(ref double[] meanRadiation, string position, ref List<KeyValuePair<float, Color>> map, ref List<Tuple<float, float, List<Color>>> fromToList)
    {
        List<Tuple<int, Color, string>> coloursForTeeth = new List<Tuple<int, Color, string>>();
        Color currentColour = Color.white;
        for (int i = 0; i < meanRadiation.Length; ++i)
        {
            currentColour = getColourAssociatedToRadiation(ref meanRadiation, i, ref map, ref fromToList);
            coloursForTeeth.Add(new Tuple<int, Color, string>(i, currentColour, position));
        }
        return coloursForTeeth;
    }

    /*
     * Assign a colour based on the radiation.
     * @param meanRadiation, position, map
     * @return Color
     */
    private Color getColourAssociatedToRadiation(ref double[] meanRadiation, int position, ref List<KeyValuePair<float, Color>> map, ref List<Tuple<float, float, List<Color>>> fromToList)
    {

        Color currentColour;
        int count = 0;
        currentColour = Color.white;

        bool withinRange = isWihinRange(meanRadiation, position, fromToList);
        try
        {
            while ((count < map.Count
                &&
                    meanRadiation[position] <= map[map.Count - 1].Key)
                &&
                    withinRange
                &&
                    meanRadiation[position] >= map[count].Key)
            {

                currentColour = map[count].Value;
                count++;

            }
        }
        catch (Exception)
        {

            throw new Exception("Value causing error" + position.ToString() + " " + count.ToString() + " fromTo " + fromToList.Count + "map " + map.Count);
        }
        return currentColour;


    }

    /*
     *Check the radiation is within the radiation range to be painted
     *@param meanRadiation, position, fromToList
     *@return bool
     */
    private bool isWihinRange(double[] meanRadiation, int position, List<Tuple<float, float, List<Color>>> fromToList)
    {
        bool withinRange = false;
        foreach (Tuple<float, float, List<Color>> fromTo in fromToList)
        {
            if (meanRadiation[position] >= fromTo.Item1 && meanRadiation[position] <= fromTo.Item2)
            {
                withinRange = true;
            }
        }
        return withinRange;
    }

    /*
    * Accessor method to set radiation values of the top set and bottom set of the teeth.
    * @param bottom,top, teethSet
   */
    public void setAverageMeanRadiation(double[] bottom, double[] top)
    {
        meanRadiationBottomTeeth = bottom;
        meanRadiationTopTeeth = top;
    }


    /*
     * Get the tsize of the radiation colour map.
     * @return int
     */   
    public int getRadColourMapSize() { return RadColourMap.Count; }

    /*
     * Clear the colour map   
     */   
    public void clearRadColourMap() { RadColourMap.Clear(); }

    /*
     * Clear the List of from to values with the colours.
     */   
    public void clearFromToColourList() { fromToRadiationAndColourList.Clear(); }

    /*
     * Getter for the the List of from to values with the colours. 
     * @return List<Tuple<float, float, List<Color>>>
     */
    public List<Tuple<float, float, List<Color>>> getFromToRadiationAndColourList() { return fromToRadiationAndColourList; }

    /*
     * Reset the radiation colour map and List of from to values with the colours
     */
    public void resetColourMapToFromList() { clearRadColourMap(); clearFromToColourList(); }

    /*
     * Get the values of the radiaation colour map.
     * @return List<KeyValuePair<float, Color>>
     */
    public List<KeyValuePair<float, Color>> getRadColourMap() { return RadColourMap; }

    /*
     * Getter for the List of colours selected.
     * @ List<Color>
     */
    public List<Color> getSelectedColours() { return selectedColoursTeeth; }

    /*
     * Clear the List of colours.   
     */   
    public void clearSelectedColours()
    {
        selectedColoursTeeth.Clear();
    }

    /*
     * Setter for the list of colours created.   
     */   
    public void setSelectedTeethColours(List<Color> colours)
    {
        selectedColoursTeeth = colours;
    }

    /*
     * Setter for the List of from to values with the colours.   
     */
    public void setToFromColourList(List<Tuple<float, float, List<Color>>> toFromColourList)
    {
        fromToRadiationAndColourList = toFromColourList;
    }

    /*
     * Setter for radiation colour map.   
     */   
    public void setRadiationMap(List<KeyValuePair<float, Color>> map)
    {
        RadColourMap = map;
    }
}
