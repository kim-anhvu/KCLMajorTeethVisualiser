using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using TMPro;

    /**
     * Displays data points as a Bar Chart
     * 
     */
public class BarChartVisual : IGraphVisual
{
    private RectTransform graphContainer;
    private Color barColor;
    private double barWidthMultiplier;

    public BarChartVisual(RectTransform graphContainer, Color barColor, double barWidthMultiplier)
    {
        this.graphContainer = graphContainer;
        this.barColor = barColor;
        this.barWidthMultiplier = barWidthMultiplier;
    }

    public List<GameObject> AddGraphVisual(List<Vector2> graphPositions, double graphPositionWidth, string tooltipText)
    {
        Vector2 graphPosition = graphPositions[0];
        GameObject barGameObject = CreateBar(graphPosition, graphPositionWidth);
        // Add Button_UI Component which captures UI Mouse Events
        Button_UI barButtonUI = barGameObject.AddComponent<Button_UI>();

        // Show Tooltip on Mouse Over
        barButtonUI.MouseOverOnceFunc += () => {
            Graph.ShowTooltip_Static(tooltipText, graphPosition);
        };

        // Hide Tooltip on Mouse Out
        barButtonUI.MouseOutOnceFunc += () => {
            Graph.HideTooltip_Static();
        };

        return new List<GameObject>() { barGameObject };
    }

    private GameObject CreateBar(Vector2 graphPosition, double barWidth)
    {
        GameObject gameObject = new GameObject("bar", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = barColor;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.SetAsLastSibling();
        rectTransform.anchoredPosition = new Vector2(graphPosition.x, 0f);
        rectTransform.sizeDelta = new Vector2((float)barWidth * (float)barWidthMultiplier, graphPosition.y);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.pivot = new Vector2(.5f, 0f);
        return gameObject;
    }
}
