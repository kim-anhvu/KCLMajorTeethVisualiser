/* 

    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
    This Package has been appened to, acting as the controller for the Graphs
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using TMPro;

/**
 * Handles the creation of each graph visual and displays the graph
 * in the correct graph tab.
 */
public class Graph : MonoBehaviour {
    private static Graph instance;
    // Template objects for graph visual
    public GameObject graphPrefab;
    private GameObject yAxis1;
    private RectTransform scrollView;
    private RectTransform yPanel;
    private RectTransform graphContainer;
    private RectTransform xLabelTemplate;
    private RectTransform yLabelTemplate;
    private RectTransform xDashTemplate;
    private RectTransform yDashTemplate;
    private List<GameObject> gameObjectList;
    private GameObject tooltipObject;
    // Number of graphs created
    private int counter;
    // Cached values
    private List<GraphDataPoint> valueList;
    private List<GraphDataPoint> boxplotValueList;
    private List<string> xValues;
    private List<Patient> patients;
    private string xVariable;
    private int indexFromDataType;
    private List<int> teethNumbers;

    List<Tuple<string, double>> xValuesRadiationPairsList;
    private IGraphVisual graphVisual;

    private Func<string, string> getXAxisLabel; 
    private Func<double, string> getYAxisLabel; 

    public GraphData graphData;

    public GraphAxisSorter graphAxisSorter;

    [SerializeField] private Sprite dotSprite;
    private bool makeBoxPlot = false;
    private bool showBoxPlot = false;

    private void Awake()
    {
        instance = this;
        GraphData graphData = GameObject.Find("GraphData").GetComponent("GraphData") as GraphData;
        graphAxisSorter = new GraphAxisSorter();
        // Assign initial graph objects
        yAxis1 = transform.Find("yAxis1").gameObject;
        scrollView = transform.Find("ScrollView").GetComponent<RectTransform>();
        yPanel = transform.Find("yAxis1").GetComponent<RectTransform>();
        counter = 0;
        yLabelTemplate = yPanel.Find("yLabelTemplate").GetComponent<RectTransform>();
        CreateGraph();
    }

    /*
     * Reassign cached values, depending on which graph is in focus
     * @param RectTransform newGraphContainer
     *   graph container object of tab in focus
     * @param List<Tuple<string, double>> values
     *   xValuesRadiationPairsList for graph in tab in focus
     * @param List<GameObject> gameObjects
     *   gameObjectList for graph in tab in focus
     * @param List<Patient> patients
     *   list of patients with filters for graph in tab in focus applied
     * @param string xVariable
     *   xVariable for graph in tab in focus
     * @param int index
     *   indexFromDataType for graph in tab in focus
     * @param List<int> teeth
     *   list of teeth selected for graph in tab in focus
     */
    public void SwitchTabs(RectTransform newGraphContainer, List<Tuple<string, double>> values, List<GameObject> gameObjects, List<Patient> patients, string xVariable, int index, List<int> teeth)
    {
        graphContainer = newGraphContainer;
        xLabelTemplate = graphContainer.Find("xLabelTemplate").GetComponent<RectTransform>();
        xDashTemplate = graphContainer.Find("xDashTemplate").GetComponent<RectTransform>();
        yDashTemplate = graphContainer.Find("yDashTemplate").GetComponent<RectTransform>();
        tooltipObject = graphContainer.Find("tooltip").gameObject;
        yPanel = transform.Find("yAxis" + scrollView.GetChild(scrollView.childCount - 1).name[scrollView.GetChild(scrollView.childCount - 1).name.Length - 1]).GetComponent<RectTransform>();
        xValuesRadiationPairsList = values;
        gameObjectList = gameObjects;
        this.patients = patients;
        this.xVariable = xVariable;
        indexFromDataType = index;
        teethNumbers = teeth;
        
    }

    /*
     * Gets relevant data and creates the graph
     */
    private void InitialiseGraph()
    {
        // If the graph is a patient or age graph, don't create a box plot
        if(graphAxisSorter.GetXvariable() != "Patient" && graphAxisSorter.GetXvariable() != "Age" ){
            makeBoxPlot = true;
            //Assign functions to the bar chart and box plot buttons to allow switching between the two
            transform.Find("BarChartButton").GetComponent<Button_UI>().ClickFunc = () => {
                showBoxPlot = false;
                InitialiseGraph();
            };
            transform.Find("BoxPlotButton").GetComponent<Button_UI>().ClickFunc = () => {
                showBoxPlot = true;
                InitialiseGraph();
            };
        }
        
        valueList = new List<GraphDataPoint>();
        List<double> yValues;
        xValues = new List<string>();
        yValues = GetRadiationList(xValuesRadiationPairsList);
        xValues = GetXAxisList(xValuesRadiationPairsList);

        SetValueListForBarChart(yValues);

        // Initial graph is a bar chart
        IGraphVisual barChartVisual = new BarChartVisual(graphContainer, new Color(1, 0, 1, 1), .8f);
        ShowGraph(valueList, barChartVisual, (string _s) => (_s), (double _d) => "" + (_d));

        //Scale the graph container, depending on the number of values
        if (xValues.Count > 42)
        {
            graphContainer.sizeDelta = new Vector2(15 + (25 * xValues.Count), graphContainer.sizeDelta.y);
        }

        if (makeBoxPlot)
        {
            boxplotValueList = CreateBoxPlotData(xValues, patients, xVariable);
        }

         SetGraphVisual();
        
    }

    /* 
     * @param List<string> xVals
     *   list of x values for graph
     * @param List<Patient> patientsList
     *   list of patients after filtering
     * @param string dependantVaraible
     *   x variable
     */
    private List<GraphDataPoint> CreateBoxPlotData(List<string> xVals, List<Patient> patientsList, string dependantVaraible){
        return BoxPlotBackend.SortIntoXVariables(xVals,patientsList,dependantVaraible, indexFromDataType, teethNumbers);
    }

    /*
     * @param List<double> vals
     * list of y values for graph
     */
    private void SetValueListForBarChart(List<double> vals)
    {
        valueList.Clear();
        foreach (double value in vals)
        {
            GraphDataPoint p = new GraphDataPoint();
            p.AddDataPoint(value);
            valueList.Add(p);   
        }
    }

    /*
     * Delete the old graph in tab and create replacement
     */
    public void NewGraph()
    {
        DestroyGraph();
        CreateGraph();
    }
 
    /*
     * Delete all graph objects for selected tab
     */
    private void DestroyGraph()
    {
        RectTransform graphPanel = scrollView.GetChild(scrollView.childCount - 1).GetComponent<RectTransform>();
        yPanel = transform.Find("yAxis" + scrollView.GetChild(scrollView.childCount - 1).name[scrollView.GetChild(scrollView.childCount - 1).name.Length - 1]).GetComponent<RectTransform>();
        Destroy(graphPanel.GetComponent<Transform>().GetChild(graphPanel.childCount - 1).gameObject);
        for(int i = 1; i < yPanel.childCount; i++)
        {
            Destroy(yPanel.GetChild(i).gameObject);
        }   
    }

    /*
     * Create new graph in selected tab
     */
    private void CreateGraph()
    {
        RectTransform graphPanel = scrollView.GetChild(scrollView.childCount - 1).GetComponent<RectTransform>();
        NewPanel();
        graphContainer = graphPanel.Find("graphContainer" + counter).GetComponent<RectTransform>();
        xLabelTemplate = graphContainer.Find("xLabelTemplate").GetComponent<RectTransform>();
        xDashTemplate = graphContainer.Find("xDashTemplate").GetComponent<RectTransform>();
        yDashTemplate = graphContainer.Find("yDashTemplate").GetComponent<RectTransform>();
        tooltipObject = graphContainer.Find("tooltip").gameObject;
        gameObjectList = new List<GameObject>();
        KeepFilters();
        patients = graphData.RunAllFilters(new GraphDataFilter(graphData.GetPatients()));
        graphPanel.Find("Tab").GetComponent<Tab>().SetPatients(patients);
        xVariable = graphAxisSorter.GetXvariable();
        graphPanel.Find("Tab").GetComponent<Tab>().SetXVariable(xVariable);
        indexFromDataType = graphData.GetDataType();
        graphPanel.Find("Tab").GetComponent<Tab>().SetIndex(indexFromDataType);
        teethNumbers = graphData.GetTeethSelected();
        graphPanel.Find("Tab").GetComponent<Tab>().SetTeeth(teethNumbers);
        graphPanel.Find("xAxis").GetComponent<TextMeshProUGUI>().text = graphAxisSorter.GetXvariable();
        InitialiseGraph();
        graphPanel.Find("Tab").GetComponent<Tab>().SetValues(xValuesRadiationPairsList);
        graphPanel.Find("Tab").GetComponent<Tab>().SetGameObjectList(gameObjectList);

    }

    /*
     * Create new panel object to contain new graph
     */
    private void NewPanel()
    {
        counter++;
        GameObject graph = (GameObject)GameObject.Instantiate(graphPrefab);
        graph.transform.name = "graphContainer" + counter;
        graph.transform.SetParent(scrollView.GetChild(scrollView.childCount - 1).GetComponent<RectTransform>());
        graph.transform.localScale = new Vector3(0.9691074f, 0.8810066f, 1);
        graph.transform.localPosition = new Vector3(-514, 1, 0);
        graph.transform.SetAsLastSibling();
        scrollView.GetComponent<ScrollRect>().content = graph.GetComponent<RectTransform>();
    }

    /*
     * Sets all filters to be kept in graph data for the graph being created
     */
    private void KeepFilters()
    {
        SetGendersToKeep();
        SetAgeMin();
        SetAgeMax();
        SetTumoursToKeep();
        SetNodalsToKeep();
        SetSitesToKeep();
        SetSidesToKeep();
        SetTreatmentsToKeep();
        SetTotalRTsToKeep();
        SetToothNumbers();
        SetDataType();
        SetXVariable();
        SetAgeRange();
        xValuesRadiationPairsList = graphAxisSorter.SortIntoXVariables(graphData.GetPatientsRadiationPairsList());
    }

    /*
     * Hides the tool tip object
     */
    private void HideTooltip() 
    {
        tooltipObject.SetActive(false);
    }

    public static void HideTooltip_Static() {
        instance.HideTooltip();
    }

    /*
     * Displays tool tip object
     * @param string radiation
     *   radiation value displayed in tool tip
     * @param Vector2 anchoredPosition
     *   position of tool tip
     */
    private void ShowTooltip(string radiation, Vector2 anchoredPosition) 
    {
        tooltipObject.SetActive(true);
        tooltipObject.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;

        TextMeshProUGUI tooltipText = tooltipObject.transform.Find("text").GetComponent<TextMeshProUGUI>();
        float textPaddingSize = 4f;
        tooltipText.text = radiation;
        Vector2 backgroundSize = new Vector2(
            tooltipText.preferredWidth + textPaddingSize * 2f,
            tooltipText.preferredHeight + textPaddingSize * 2f
        );
        
        tooltipObject.transform.Find("background").GetComponent<RectTransform>().sizeDelta = backgroundSize;

        tooltipObject.transform.SetAsLastSibling();
    }
    
    public static void ShowTooltip_Static(string radiation, Vector2 anchoredPosition) {
        instance.ShowTooltip(radiation, anchoredPosition);
    }

    /*
     * @return List<double> temp
     *   a list of radiation values for each of the corresponding x values
     */
    private List<double> GetRadiationList (List<Tuple<string, double>> xValuesRadiationPairsList)
    {
        List<double> temp = new List<double>();
        for(int i = 0; i < xValuesRadiationPairsList.Count; i++) 
        {
            temp.Add(xValuesRadiationPairsList[i].Item2);
        }
        return temp;
    }

    /*
     * @return List<string> temp
     *   a list of values for the x axis
     */
    private List<string> GetXAxisList (List<Tuple<string, double>> xValuesRadiationPairsList)
    {
        List<string> temp = new List<string>();
        for(int i = 0; i < xValuesRadiationPairsList.Count; i++) 
        {
            temp.Add(xValuesRadiationPairsList[i].Item1);
        }
        return temp;
    }

    /*
     * Creates visuals for bar chart and box plot and displays the correct one
     */
    private void SetGraphVisual() {
        IGraphVisual barChartVisual = new BarChartVisual(graphContainer, new Color(1, 0, 1, 1), .8f);
        IGraphVisual boxPlotVisual = new BoxPlotVisual(graphContainer, .6f, dotSprite, Color.green, Color.blue);
        if (showBoxPlot){
            ShowGraph(boxplotValueList, boxPlotVisual, this.getXAxisLabel, this.getYAxisLabel);
        }
        else {
            ShowGraph(valueList, barChartVisual, this.getXAxisLabel, this.getYAxisLabel);
        }
    }

    /*
     * Creates and displays graph visual in the graph container
     * @param List<GraphDataPoint> valueList
     *   list of data points for the graph
     * @param IGraphVisual graphVisual
     *   type of graph to be displayed
     * @param Func<string, string> getXAxisLabel
     *   function to populate x axis with labels
     * @param Func<double, string> getYAxisLabel
     *   function to populate y axis with labels
     */
    private void ShowGraph(List<GraphDataPoint> valueList, IGraphVisual graphVisual, Func<string, string> getXAxisLabel = null, Func<double, string> getYAxisLabel = null) {
        this.graphVisual = graphVisual;
        this.getXAxisLabel = getXAxisLabel;
        this.getYAxisLabel = getYAxisLabel;

        // Clean up previous graph
        
        foreach (GameObject gameObject in gameObjectList) {
            Destroy(gameObject);
        }
        gameObjectList.Clear();
        
        // Grab the width and height from the container
        double graphWidth = graphContainer.sizeDelta.x;
        double graphHeight = graphContainer.sizeDelta.y;

        // Identify y Min and Max values
        double yMaximum = 0;
        double yMinimum = 0; 
         
        for (int i = Mathf.Max(valueList.Count - valueList.Count, 0); i < valueList.Count; i++) {
            GraphDataPoint value = valueList[i];
            if (value.GetMax() > yMaximum) {
                yMaximum = value.GetMax();
            }
            if (value.GetMin() < yMinimum) {
                yMinimum = value.GetMin();
            }
        }

        double yDifference = yMaximum - yMinimum;
        if (yDifference <= 0) {
            yDifference = 5f;
        }
        yMaximum = yMaximum + (yDifference * 0.2f);
        yMinimum = yMinimum - (yDifference * 0.2f);

        yMinimum = 0f; // Start the graph at zero



        // Set the distance between each point on the graph 
        double xSize;
        if (xValues.Count > 42)
        {
            xSize = 25;
        }else if(xValues.Count == 0){
            return;
        }
        else
        {
            xSize = (1050 / xValues.Count);
        }
        // Cycle through all visible data points
        int xIndex = 0;
        for (int i = Mathf.Max(valueList.Count - valueList.Count, 0); i < valueList.Count; i++) {
            double xPosition;
            // Scale distance between positions depending on number of values to fit container
            if (xIndex == 0)
            {
                xPosition = xSize/2;
            }
            else
            {
                xPosition = xSize/2 + xIndex * xSize;
            }
            List<Vector2> positions = GetPosistions(xPosition,yMinimum, yMaximum, graphHeight, valueList[i]);
            // Add data point visual for bar chart
            string tooltipText = "";
            if(!showBoxPlot){
                tooltipText = getYAxisLabel(valueList[i].GetPoint(0));
            }
            gameObjectList.AddRange(graphVisual.AddGraphVisual(positions, (float)xSize, tooltipText));

            // Duplicate the x label template
            RectTransform xLabel = Instantiate(xLabelTemplate);
            xLabel.SetParent(graphContainer, false);
            xLabel.gameObject.SetActive(true);
            xLabel.anchoredPosition = new Vector2((float)xPosition, -7f);
            xLabel.GetComponent<Text>().text = getXAxisLabel(xValues[i]);
            if(xVariable == "Age")
            {
                xLabel.transform.Rotate(0, 0, 90);
                xLabel.GetComponent<Text>().alignment = TextAnchor.LowerRight;
                xLabel.anchoredPosition += new Vector2(5, 0);
            }
            gameObjectList.Add(xLabel.gameObject);
            
            RectTransform yDash = Instantiate(yDashTemplate);
            yDash.SetParent(graphContainer, false);
            yDash.gameObject.SetActive(true);
            yDash.anchoredPosition = new Vector2((float)xPosition, 0);
            yDash.SetSiblingIndex(1);
            gameObjectList.Add(yDash.gameObject);
            xIndex++;
        }

        // Set up separators on the y axis
        int separatorCount = 10;
        for (int i = 0; i <= separatorCount; i++) {
            // Duplicate the label template
            RectTransform yLabel = Instantiate(yLabelTemplate);
            yLabel.SetParent(yPanel, false);
            yLabel.gameObject.SetActive(true);
            float normalizedValue = i * 1f / separatorCount;
            yLabel.anchoredPosition = new Vector2(120f, normalizedValue * (float)graphHeight * 0.88f);
            yLabel.GetComponent<Text>().text = getYAxisLabel(yMinimum + (normalizedValue * (yMaximum - yMinimum)));
            gameObjectList.Add(yLabel.gameObject);

            // Duplicate the dash template
            RectTransform xDash = Instantiate(xDashTemplate);
            xDash.SetParent(graphContainer, false);
            xDash.gameObject.SetActive(true);
            xDash.anchoredPosition = new Vector2(0, (float)normalizedValue * (float)graphHeight);
            if (xValues.Count > 42)
            {
                xDash.sizeDelta = new Vector2(15 + (25 * xValues.Count), xDash.sizeDelta.y);
            }
            xDash.SetSiblingIndex(1);
            gameObjectList.Add(xDash.gameObject);
        } 
    }

    /*
     * Calculate positions of data points
     * @param double xPos
     *   scale for x position
     * @param double yMin
     *   minimum value on y axis
     * @param double yMax
     *   maximum value on y axis
     * @param double graphHeight
     *   height of container
     * @param GraphDataPoint value
     *   value of data point
     * @return List<Vector2> graphPositions
     *   list of positions for the data points
     */
    private List<Vector2> GetPosistions(double xPos, double yMin, double yMax, double graphHeight, GraphDataPoint value)
    {
        List<Vector2> graphPositions = new List<Vector2>(3);
        double yPos; 
        foreach (double point in value.GetGraphDataPoints())
        {
            yPos = ((point - yMin) / (yMax - yMin)) * graphHeight;
            graphPositions.Add(new Vector2((float)xPos, (float)yPos));
        }
        return graphPositions;

    }

    /*
     * Set filters to be kept in the graph data
     */
    public void SetGendersToKeep() { graphData.SetGendersToKeep(GraphInput.genderSelected); }

    public void SetAgeMin() { graphData.SetAgeMin(GraphInput.fromAge); }

    public void SetAgeMax() {  graphData.SetAgeMax(GraphInput.toAge); }

    public void SetTumoursToKeep() { graphData.SetTumoursToKeep(GraphInput.tumourSelected);}

    public void SetNodalsToKeep() { graphData.SetNodalsToKeep(GraphInput.nodalSelected);}

    public void SetSitesToKeep() { graphData.SetSitesToKeep(GraphInput.siteSelected);}

    public void SetSidesToKeep() { graphData.SetSidesToKeep(GraphInput.jawSideSelected);}

    public void SetTreatmentsToKeep() { graphData.SetTreatmentsToKeep(GraphInput.treatmentSelected);}

    public void SetTotalRTsToKeep() { graphData.SetTotalRTsToKeep(GraphInput.totalRTSelected);}

    public void SetToothNumbers() { graphData.SetToothNumbers(GraphInput.toothTypeSelected); }
    
    public void SetDataType() { graphData.SetDataType(GraphInput.dataTypeSelected);}

    public void SetXVariable() { graphAxisSorter.SetXVariable(GraphInput.xVariableSelected);}

    public void SetAgeRange(){graphAxisSorter.SetAgeRange(GraphInput.ageRange);}
}


