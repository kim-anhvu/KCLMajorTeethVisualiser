using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using TMPro;

public class BoxPlotVisual : IGraphVisual
{
    private RectTransform graphContainer;
    private double boxWidthMultiplier; // = .8f;  //IDEALY SETTING 
    private Sprite dotSprite;
    private Color dotColor;
    private Color dotConnectionColor;
    public List<GameObject> gameObjectList = new List<GameObject>();

    public BoxPlotVisual(RectTransform graphContainer, double boxWidthMultiplier, Sprite dotSprite, Color dotConnectionColor, Color dotColor)
    {
        this.graphContainer = graphContainer;
        this.boxWidthMultiplier = boxWidthMultiplier;
        this.dotConnectionColor = dotConnectionColor;
        this.dotColor = dotColor;
    }

    //make graphPositions a List<Vector2> ((x,min), (x,LQ),(x,M),(x,UQ), (x,max) , ....(x,outliers))
    public List<GameObject> AddGraphVisual(List<Vector2> graphPosition, double graphPositionWidth, string tooltipText)
    {
        //Actual Data
        //List<Vector2> quartiles = graphPositions;

        //mock data
        var xpos = graphPosition[0].x;
        List<Vector2> quartiles = graphPosition;

        if (quartiles.Count < 5)
        {
            return null;
        }
        gameObjectList.AddRange(CreateBoxPlot(quartiles, (float)graphPositionWidth));
        return gameObjectList;
    }
    /** Create the components of a boxplot
        @param quartiles - a list of vectors2, indicating the position of each point on the boxplot
                         - format in the order (min, q1, q2, q3, max, ...outliers)
        @param width - designated width for boxplot.
        @return boxplot
     */
    private List<GameObject> CreateBoxPlot(List<Vector2> quartiles, float width)
    {
        List<GameObject> boxplot = new List<GameObject>();
        float whiskerOffset = 0.5f;
        float boxOffset = 0.2f;

        float whiskerTrim = WidthOffset(width, whiskerOffset);
        float boxTrim = WidthOffset(width, boxOffset);

        //plot min horizontal line
        GameObject minimum = CreateLine(new Vector2(quartiles[0].x - whiskerTrim, quartiles[0].y), new Vector2(quartiles[0].x + whiskerTrim, quartiles[0].y));
        boxplot.Add(minimum);

        //min to q1 vertical line.
        GameObject minUpLine = CreateLine(quartiles[0], quartiles[1]);
        boxplot.Add(minUpLine);

        //horizontal line for q1,q2,q3
        for (int i = 1; i <= 3; i++)
        {
            GameObject quartile = CreateLine(new Vector2(quartiles[i].x - boxTrim, quartiles[i].y),
                                             new Vector2(quartiles[i].x + boxTrim, quartiles[i].y));
            boxplot.Add(quartile);
        }

        //plot max horizontal line
        GameObject maximum = CreateLine(new Vector2(quartiles[4].x - whiskerTrim, quartiles[4].y),
                                        new Vector2(quartiles[4].x + whiskerTrim, quartiles[4].y));
        boxplot.Add(maximum);

        //max to q3 vertical line.
        GameObject maxDownLine = CreateLine(quartiles[4], quartiles[3]);
        boxplot.Add(maxDownLine);

        //Side vertical-lines of the box
        GameObject leftBoxLine = CreateLine(new Vector2(quartiles[1].x - boxTrim, quartiles[1].y),
                                            new Vector2(quartiles[3].x - boxTrim, quartiles[3].y));
        boxplot.Add(leftBoxLine);

        GameObject rightBoxLine = CreateLine(new Vector2(quartiles[1].x + boxTrim, quartiles[1].y),
                                            new Vector2(quartiles[3].x + boxTrim, quartiles[3].y));
        boxplot.Add(rightBoxLine);

        //If outliers exists... after index >= 5 add a Dot.
        for (int i = 5; i < quartiles.Count; i++)
        {
            GameObject outlier = CreateDot(quartiles[i]);
            outlier.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 1.5f);  //size of dot
            outlier.GetComponent<Image>().color = dotColor;
            boxplot.Add(outlier);
        }

        return boxplot;

    }

    // returns a dot connection between the Vector points, a and b.
    private GameObject CreateLine(Vector2 a, Vector2 b)
    {
        return CreateDotConnection(CreateDot(a).GetComponent<RectTransform>().anchoredPosition,
                                    CreateDot(b).GetComponent<RectTransform>().anchoredPosition);
    }

    // How wide to draw the horizontal line
    private float WidthOffset(float w, float percent)
    {
        return (w / 2) - ((w / 2) * percent);
    }

    //Creates a Dot/Position on the Grpah
    private GameObject CreateDot(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("dot", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(0, 0);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        gameObjectList.Add(gameObject);
        return gameObject;
    }

    //Connects dots on the graph.
    private GameObject CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = dotConnectionColor;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;

        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
        gameObjectList.Add(gameObject);
        return gameObject;
    }

}
