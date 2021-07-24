using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Opens and closes the graph filter panel
 */
public class GraphFilter : MonoBehaviour
{
    public GameObject GraphFilterPanel;

    void Awake()
    {
        GraphFilterPanel.SetActive(false);
    }

    public void OpenPanel()
    {
        if (!GraphFilterPanel.activeSelf)
        {
            GraphFilterPanel.SetActive(true);

        }
        else
        {
            GraphFilterPanel.SetActive(false);
        }
    }
}
