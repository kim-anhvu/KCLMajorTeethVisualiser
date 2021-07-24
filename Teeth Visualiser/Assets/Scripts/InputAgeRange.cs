using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class InputAgeRange : MonoBehaviour
{
    public GameObject inputField;
    public TMP_Dropdown xVariable;
    public GameObject text;

    void Start()
    {
        inputField.SetActive(false);
        text.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(xVariable.options[xVariable.value].text.Equals("Age")) {
            inputField.SetActive(true);
            text.SetActive(true);
        }
        else {
            inputField.SetActive(false);
             text.SetActive(false);
        }
    }
}
