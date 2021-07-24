using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
* The TeethDisplayPanel class is a script which is assigned to every
* tooth that is currently being displayed and is called upon when
* the tooth is hovered over.
* When hovering over a tooth it will display the value of radiation that
* the tooth holds.
*/
public class TeethDisplayPanel : MonoBehaviour
{

    public Text text;
    public GameObject[] teeth;
    double value;

    public void Setup(Text text, double value)
    {
        this.text = text;
        this.value = value;
    }

    void OnMouseOver()
    {
      if (text != null)
      {
          text.enabled = true;
          text.text = "" + value;
      }
    }

    void OnMouseExit()
    {
      if (text != null)
          text.enabled = false;
    }
}
