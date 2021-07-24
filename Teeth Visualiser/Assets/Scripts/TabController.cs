using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Handles the creation and switching of graph tabs
 */
public class TabController : MonoBehaviour
{
    public List<GameObject> tabs; // List of existing tabs
    public RectTransform scrollView; // Scroll view which tabs are children of
    public Button newTabButton; // Button pressed to create new tabs
    public List<GameObject> yAxisPrefab; // Panel containing y values for each active graph

    private int counter; // Used to number graph panels

    void Start()
    {
        counter = 1;
    }

    /*
     * Increments counter each time it's called and keeps it between 1 and 4
     * @return int counter
     */
    private int GetCounter()
    {
        counter++;
        if(counter == 5)
        {
            counter = 1;
        }
        return counter;
    }

    /*
     * Deselects any other selected tabs and creates new tab
     */
    public void NewTab()
    {
        if (tabs.Count < 4)
        {
            foreach (GameObject tab in tabs)
            {
                tab.GetComponent<Tab>().DeselectTab();
            }
            CreateTab();
        }
    }

    /*
     * Creates new graph object 
     */
    private void CreateTab()
    {
        Vector3 temp = new Vector3(114.3f, 0, 0);
        GameObject newTab = (GameObject)GameObject.Instantiate(tabs[tabs.Count-1].transform.parent.gameObject);
        newTab.transform.name = "Graph Panel " + GetCounter();
        newTab.transform.SetParent(scrollView);
        newTab.transform.localScale = new Vector3(1, 1, 1);
        newTab.transform.position = tabs[tabs.Count - 1].transform.parent.transform.position;
        Transform tabButton = newTab.transform.Find("Tab");
        tabButton.transform.position += temp;
        tabButton.GetComponentInChildren<Text>().text = "Graph " + (char.GetNumericValue(tabs[tabs.Count - 1].GetComponentInChildren<Text>().text[tabs[tabs.Count - 1].GetComponentInChildren<Text>().text.Length -1]) + 1);
        GameObject yAxis = (GameObject)GameObject.Instantiate(yAxisPrefab[yAxisPrefab.Count - 1]);
        yAxis.transform.name = "yAxis" + newTab.transform.name[newTab.transform.name.Length - 1];
        yAxis.transform.position = yAxisPrefab[yAxisPrefab.Count - 1].transform.position;
        yAxis.transform.SetParent(scrollView.parent);
        yAxis.transform.localScale = new Vector3(1, 1, 1);
        yAxisPrefab.Add(yAxis);
        tabButton.GetComponent<Tab>().SetYAxis(yAxis);
        Transform graphContainer = newTab.transform.GetChild(newTab.transform.childCount - 1);
        SetData(tabButton.GetComponent<Tab>(),graphContainer, yAxis.transform);
        tabButton.GetComponent<Tab>().SelectTab();
        newTabButton.transform.position += temp;
        tabs.Add(tabButton.gameObject);
        newTab.SetActive(true);
        if (tabs.Count == 4)
        {
            newTabButton.gameObject.SetActive(false);
        }
    }

    /*
     * Deselect all other tabs and select clicked tab
     * @param Button clickedTab
     *   clicked tab button
     */
    public void SwitchTabs(Button clickedTab)
    {
        foreach (GameObject tab in tabs)
        {
            if (tab != clickedTab)
            {
                tab.GetComponent<Tab>().DeselectTab();
            }
        }
        clickedTab.GetComponent<Tab>().SelectTab();
    }

    /*
     * Delete clicked tab if there is more than 1 tab
     * @param Button deleteButton
     *   clicked delete button for tab
     */
    public void DeleteTab(Button deleteButton)
    {
        if (tabs.Count > 1)
        {
            for (int i = 0; i < tabs.Count; i++)
            {
                if(tabs[i] == deleteButton.transform.parent.gameObject)
                {
                    SwitchTabs(tabs[(i + 1) % tabs.Count].GetComponent<Button>());
                    tabs.Remove(tabs[i]);
                    for(int j = i; j < tabs.Count; j++)
                    {
                        tabs[j].transform.position -= new Vector3(114.3f, 0, 0);
                    }
                    newTabButton.transform.position -= new Vector3(114.3f, 0, 0);
                }
            }
            Destroy(deleteButton.transform.parent.parent.gameObject);
            newTabButton.gameObject.SetActive(true);
        }
    }

    /*
     * Set graph data for newly created graph
     * @param Tab tab
     *   newly created tab
     * @param Transform graphContainer
     *   graph container for new graph
     */
    public void SetData(Tab tab, Transform graphContainer, Transform yAxis)
    {
        Debug.Log(graphContainer);
        List<GameObject> gameObjects = new List<GameObject>();
        for(int i = 1; i < graphContainer.childCount; i++)
        {
            if (!(graphContainer.Find("tooltip") == graphContainer.GetChild(i) || graphContainer.Find("xDashTemplate") == graphContainer.GetChild(i) || graphContainer.Find("xLabelTemplate") == graphContainer.GetChild(i) || graphContainer.Find("yDashTemplate") == graphContainer.GetChild(i)))
            {
                gameObjects.Add(graphContainer.GetChild(i).gameObject);
            }
        }
        for(int i = 1; i < yAxis.childCount; i++)
        {
            gameObjects.Add(yAxis.GetChild(i).gameObject);
        }

        Tab oldTab = tabs[tabs.Count - 1].transform.GetComponent<Tab>();
        tab.SetValues(oldTab.GetValues());
        tab.SetGameObjectList(gameObjects);
        tab.SetPatients(oldTab.GetPatients());
        tab.SetXVariable(oldTab.GetXVariable());
        tab.SetIndex(oldTab.GetIndex());
        tab.SetTeeth(oldTab.GetTeeth());
    }
}
