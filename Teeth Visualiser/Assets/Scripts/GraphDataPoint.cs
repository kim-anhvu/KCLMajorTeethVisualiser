using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Stores a list of double values, associated with a data point.
 * Dynamically calculates the min and max values on insertion. 
 */

public class GraphDataPoint
{
    private List<double> datapoints; 
    private double min = -1; 
    private double max = -1;

    public GraphDataPoint()
    {
        datapoints = new List<double>(1);
    }

    public List<double> GetGraphDataPoints()
    {
        return datapoints;
    }

    /**
     * Appends to the datapoint list
     * @param v - value to append
     */
    public void AddDataPoint(double v)
    {
        datapoints.Add(v);
        SetMin(v);  
        SetMax(v);
    }

    /**
     * Compares value to min
     * Sets value as min iff (min not set or value < min)
     * @param value - value to compare
     */
    private void SetMin(double value)
    {
        if(min == -1 || value < min)
        {
            min = value;
        }
    }

    /**
     * Compares value to max
     * Sets value as max iff (max not set or value > max)
     * @param value - value to compare
     */
    private void SetMax(double value)
    {
        if(max == -1 || value > max)
        {
            max = value;
        }
    }

    /**
     * Appends to the datapoint list
     * @param values - values to append
     */
    public void AddDataPointRange(List<double> values)
    {
        if(values == null || values.Count == 0){
            return;
        }
        datapoints.AddRange(values);
        foreach (double v in values)
        {
            SetMin(v);
            SetMax(v);
        }
    }

    public double GetMin()
    {
        return min;
    }
    public double GetMax()
    {
        return max;
    }

    /**
     * @param i - index within the list
     * @ return 0 - if index out of bounds else return value at index position
     */
    public double GetPoint(int i){
        if(i < 0 || i >= datapoints.Count){
            return 0;
        }else{
            return datapoints[i];
        }
        
    }

    //Clears The datapoint list
    public void Clear()
    {
        datapoints.Clear();
        min = -1;
        max = -1;
    }


}
