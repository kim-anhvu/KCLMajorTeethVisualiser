using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Sets the error message text
 */
public class ErrorMessage : MonoBehaviour
{
    public Text errorMessageText;

    public void SetErrorMessage(string message)
    {
        errorMessageText.text = message;
        errorMessageText.color = Color.red;
        errorMessageText.fontSize = 40;
    }

    public void ResetErrorMessage()
    {
        errorMessageText.text = "";
        errorMessageText.color = Color.black;
    }
}
