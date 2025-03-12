using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapTimeCalculator : MonoBehaviour
{

    public DateTime initialTime;
    private DateTime endTime;

    public UIManager uiManager;

    bool firstTime;

    // Start is called before the first frame update
    void Start()
    {
        firstTime = true;
    }

    // Update is called once per frame
    void Update()
    {
        endTime = DateTime.Now;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Robot")
        {
            if(firstTime)
            {
                initialTime = DateTime.Now;
                firstTime = false;
            } else
            {
                TimeSpan lapTime = endTime - initialTime;
            
                string lapTimeString = lapTime.ToString().Split('.')[0];
                Debug.Log(lapTimeString);
                uiManager?.ChangeLapTime(lapTimeString);
                initialTime = DateTime.Now;
            }
        }
    }
}
