using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapTimeCalculator : MonoBehaviour
{

    public DateTime initialTime;
    private DateTime finishTime;

    public UIManager uiManager;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        finishTime = DateTime.Now;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Robot")
        {
            TimeSpan lapTime = finishTime - initialTime;
            string lapTimeString = lapTime.ToString().Split('.')[0];
            uiManager?.ChangeLapTime(lapTimeString);
        }
    }
}
