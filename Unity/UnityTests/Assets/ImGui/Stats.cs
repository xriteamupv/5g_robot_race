using DataVisualizer;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

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
        SINR,
        LAT
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
        //Vector3 lookDirection = canvas.GetComponent<RectTransform>().position - Camera.main.transform.position;
        //canvas.GetComponent<RectTransform>().rotation = Quaternion.LookRotation(lookDirection.normalized, Vector3.up);
        /*timer += Time.deltaTime;
        if(timer >= 0.5f)
        {
            timer = 0.0f;
            for (int i = 0; i < chart.Length; ++i)
            {
                //Vector3 lookDirection = chart[i].GetComponent<RectTransform>().position - Camera.main.transform.position;
                //chart[i].GetComponent<RectTransform>().rotation = Quaternion.LookRotation(lookDirection.normalized, Vector3.up);
                float value = Random.Range(0.0f, 200.0f);
                AppendValue(value, ChartType.RSRP);
                AppendValue(value, ChartType.SINR);
                AppendValue(value, ChartType.RSRQ);
                AppendValue(value, ChartType.LAT);
            }
        }*/
    }
}
