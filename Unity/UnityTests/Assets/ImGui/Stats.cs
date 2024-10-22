using DataVisualizer;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public DataSeriesChart chart;
    DoubleVector3[] dataArray = new DoubleVector3[20];
    public double TotalPoints = 20;
    double x = 0;

    private float timer;
    void Awake()
    {
        var data = chart.DataSource.GetCategory("dataseries-1").Data; //get the category data object 
        data.Clear(); // clear the category
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 0.5f)
        {
            timer = 0.0f;
            var data = chart.DataSource.GetCategory("dataseries-1").Data; // get the category data
            data.Append(x, Random.Range(0.0f, 200.0f)); // append a random point
            x++; // increase current position
            if (data.Count > TotalPoints) // if the amount of points in the chart is larger then the maximum
                data.RemoveAllBefore(chart.Axis.View.HorizontalViewStart); // remove all points that are before the beginning of the view portion.
        }
    }
}
