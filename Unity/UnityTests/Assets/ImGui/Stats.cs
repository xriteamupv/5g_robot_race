using DataVisualizer;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public GameObject canvas;
    public DataSeriesChart[] chart;
    public Text[] currentValues;
    DoubleVector3[] dataArray = new DoubleVector3[20];
    public double TotalPoints = 20;
    double x = 0;
    CategoryDataHolder[] data;

    private float timer;
    void Awake()
    {
        data = new CategoryDataHolder[chart.Length];
        for(int i = 0; i < chart.Length; ++i)
        {
            data[i] = chart[i].DataSource.GetCategory("dataseries-" + (i + 1)).Data; //get the category data object 
            data[i].Clear(); // clear the category
        }
    }

    void Update()
    {
        //Vector3 lookDirection = canvas.GetComponent<RectTransform>().position - Camera.main.transform.position;
        //canvas.GetComponent<RectTransform>().rotation = Quaternion.LookRotation(lookDirection.normalized, Vector3.up);
        timer += Time.deltaTime;
        if(timer >= 0.5f)
        {
            timer = 0.0f;
            for (int i = 0; i < chart.Length; ++i)
            {
                //Vector3 lookDirection = chart[i].GetComponent<RectTransform>().position - Camera.main.transform.position;
                //chart[i].GetComponent<RectTransform>().rotation = Quaternion.LookRotation(lookDirection.normalized, Vector3.up);
                float value = Random.Range(0.0f, 200.0f);
                data[i].Append(x, value); // append a random point
                currentValues[i].text = value.ToString();
                if (data[i].Count > TotalPoints) // if the amount of points in the chart is larger then the maximum
                    data[i].RemoveAllBefore(chart[i].Axis.View.HorizontalViewStart); // remove all points that are before the beginning of the view portion.
            }
            x++; // increase current position
        }
    }
}
