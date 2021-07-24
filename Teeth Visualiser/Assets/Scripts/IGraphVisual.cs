using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Interface definition for showing visual for a data point
 */
public interface IGraphVisual
{
    List<GameObject> AddGraphVisual(List<Vector2> graphPosition, double graphPositionWidth, string tooltipText);
}
