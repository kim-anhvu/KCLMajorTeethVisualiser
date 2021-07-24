using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class changes the material colour on the teeth.
 */
public class ChangeTeethColour : MonoBehaviour
{
    Color32 white = new Color(1, 1, 1, 1);

    public Material[] bottomTeeth;
    public Material[] topTeeth;
    // Start is called before the first frame update
    void Start()
    {
      
       
        resetColour();

    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
     * Sets teeth colour to white
    */
    public void resetColour() {
        for (int i = 0; i < bottomTeeth.Length; ++i)
        {
            bottomTeeth[i].color = white;
            topTeeth[i].color = white;
        }
    }
   
    /*Apply the parsed colour to the indexed tooth
     * @param i,x,position   
    */
    public void setTeethColour(int i, Color x, string position) {
        if (position.Equals("top")) { 
        topTeeth[i].color = x; }
        else {
            bottomTeeth[i].color = x;
        }
    }
}
