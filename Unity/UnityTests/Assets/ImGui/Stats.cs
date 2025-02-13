using DataVisualizer;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using static ProxyConnection;

public class Stats : MonoBehaviour
{
    public GameObject canvas;
    public DataSeriesChart[] chart;
    public Text[] currentValues;
    DoubleVector3[] dataArray = new DoubleVector3[20];
    public double TotalPoints = 20;
    int[] pos;
    CategoryDataHolder[] data;

    public enum ChartType
    {
        RSRP = 0,
        RSRQ,
        LAT,
        SINR,
    }

    private float timer;
    void Awake()
    {
        data = new CategoryDataHolder[chart.Length];
        pos = new int[chart.Length];
        for(int i = 0; i < chart.Length; ++i)
        {
            data[i] = chart[i].DataSource.GetCategory("dataseries-" + (i + 1)).Data; //get the category data object 
            data[i].Clear(); // clear the category
            pos[i] = 0;
        }
    }

    public void AppendValue(float value, ChartType type)
    {
        int typeInt = (int)type;
        data[typeInt].Append(pos[typeInt], value); // append a random point
        currentValues[typeInt].text = type.ToString() + ": " + value.ToString();
        if (data[typeInt].Count > TotalPoints) // if the amount of points in the chart is larger then the maximum
            data[typeInt].RemoveAllBefore(chart[typeInt].Axis.View.HorizontalViewStart); // remove all points that are before the beginning of the view portion.
        pos[typeInt]++;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) 
        {
            canvas.SetActive(!canvas.activeSelf);
        }
    }
}
