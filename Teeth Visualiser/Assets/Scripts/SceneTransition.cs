using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Used for switching between Graph And Display Scenes
 * Screen Management Controller
 */

public class SceneTransition : MonoBehaviour
{
    public void GraphScene()
    {
        if (GraphInput.ageRange > 0 )
        {
            Debug.Log(GraphInput.ageRange);
            SceneManager.LoadScene("GraphScene");
        }
    }

    public void HomeScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
