using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Handles sliding animation and changing text for pop out side buttons
 */
public class SidebarButton : MonoBehaviour
{
    /*
     * Activate or deactivate sliding animation
     * @param Animator currAnimator
     *   animator component for button
     */
    public void MainButtonClicked(Animator currAnimator)
    {
        currAnimator.SetBool("Activated", !currAnimator.GetBool("Activated"));
    }

    /*
     * Flip text for button
     * @param GameObject b
     *   button that needs text changed
     */
    public void changeText(GameObject b)
    {
        Text txt = b.GetComponentInChildren<Text>();
        if (txt.text == ">")
        {
            txt.text = "<";
        }
        else
        {
            txt.text = ">";
        }
    }
}