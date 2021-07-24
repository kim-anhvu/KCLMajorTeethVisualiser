using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Assigned to each colour Button that allows you to determine which colours are
 * assigned to which button and the times they are clicked.
 */
public class ColourSelect : MonoBehaviour
{
    public Button colour;
    private bool clicked;
    private float time;

    void Start()
    {
        //add listener for this button
        clicked = false;
        time = 0;
    }

    /*
     * Sets the colour button to being clicked or not.
     * @param currAnimator
     */   
    public void HandleClick(Animator currAnimator)
    {
        clicked = !clicked;
        currAnimator.SetBool("Clicked", !currAnimator.GetBool("Clicked"));
        time = Time.time;
    }

    /*
     * Return whether button is clicked or not.
     * @return bool
     */    
    public bool getClicked()
    {
        return clicked;
    }

    /*
     * Return time seconds after application is started.
     * @return float
     */
    public float getTime(){
        return time;
    }
}

