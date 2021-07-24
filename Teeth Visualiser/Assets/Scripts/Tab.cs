using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Handles selecting and deselecting the tab, and stores information for graph in tab
 */
public class Tab : MonoBehaviour
{
    // Game objects for tab
    public Button tab; 
    public Transform tabPanel;
    public GameObject yAxis;
    public ScrollRect scroll;
    // Stored data for graph in tab
    private List<Tuple<string, double>> xValuesRadiationPairsList;
    private List<GameObject> gameObjectList;
    private List<Patient> patients;
    private List<int> teethNumbers;
    private string xVariable;
    private int indexFromDataType;

    /*
     * Assign panel to be y axis for graph in tab
     * @param GameObject yAxisPanel
     *   panel being assigned as y axis
     */
    public void SetYAxis(GameObject yAxisPanel)
    {
        yAxis = yAxisPanel;
    }

    /*
     * Store data for graph in tab
     * @param List<Tuple<string, double>> values
     *   list of x values and corresponding radiation values
     * @param List<GameObject> gameObjects
     *   list of game objects that form graph
     * @param List<Patient> patients
     *   list of filtered patients displayed
     * @param string xVariable
     *   x axis variable
     * @param int index
     *   data type displayed on graph
     * @param List<int> teeth
     * list of teeth selected to be displayed
     */
    public void SetValues(List<Tuple<string, double>> values)
    {
        xValuesRadiationPairsList = values;
    }
    public void SetGameObjectList(List<GameObject> gameObjects)
    {
        gameObjectList = gameObjects;
    }
    public void SetPatients(List<Patient> patients)
    {
        this.patients = patients;
    }
    public void SetXVariable(string xVariable)
    {
        this.xVariable = xVariable;
    }
    public void SetIndex(int index)
    {
        indexFromDataType = index;
    }
    public void SetTeeth(List<int> teeth)
    {
        teethNumbers = teeth;
    }

    /*
     * Getters for graph data
     */
     public List<Tuple<string, double>> GetValues()
    {
        return xValuesRadiationPairsList;
    }
    public List<GameObject> GetGameObjectList()
    {
        return gameObjectList;
    }
    public List<Patient> GetPatients()
    {
        return patients;
    }
    public string GetXVariable()
    {
        return xVariable;
    }
    public int GetIndex()
    {
        return indexFromDataType;
    }
    public List<int> GetTeeth()
    {
        return teethNumbers;
    }

    /*
     * Set tab active
     */
    public void SelectTab()
    {
        tab.GetComponent<Image>().color = new Color32(48, 48, 48, 255);
        tabPanel.SetAsLastSibling();
        tabPanel.GetChild(tabPanel.childCount - 1).gameObject.SetActive(true);
        tabPanel.GetChild(tabPanel.childCount - 2).gameObject.SetActive(true);
        yAxis.SetActive(true);
        scroll.content = tabPanel.GetChild(tabPanel.childCount - 1).gameObject.GetComponent<RectTransform>();
        tabPanel.transform.parent.parent.gameObject.GetComponent<Graph>().SwitchTabs(tabPanel.GetChild(tabPanel.childCount - 1).GetComponent<RectTransform>(),xValuesRadiationPairsList,gameObjectList,patients,xVariable,indexFromDataType,teethNumbers);
    }

    /*
     * Set tab to be inactive
     */
    public void DeselectTab()
    {
        tab.GetComponent<Image>().color = new Color32(98, 98, 98, 255);
        tabPanel.GetChild(tabPanel.childCount - 1).gameObject.SetActive(false);
        tabPanel.GetChild(tabPanel.childCount - 2).gameObject.SetActive(false);

        yAxis.SetActive(false);
    }


}
