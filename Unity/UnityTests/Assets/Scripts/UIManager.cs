using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text speed;
    public Image battery;
    public Image leftLidar;
    public Image rightLidar;
    public PointerIndicator speedometer;

    public Sprite[] batterySprites;

    public void ChangeSpeed(float newSpeed)
    {
        speedometer.setValue(Mathf.Abs(newSpeed * 3.6f));
    }

    public void ChangeBattery(int newBattery)
    {
        if(newBattery > 75)
        {
            battery.sprite = batterySprites[0];
        } 
        else if (newBattery > 50)
        {
            battery.sprite = batterySprites[1];
        }
        else if(newBattery > 25)
        {
            battery.sprite = batterySprites[2];
        }
        else
        {
            battery.sprite = batterySprites[3];
        }
    }

    public void SetLeftLidar(int state)
    {
        if(state == 0)
        {
            leftLidar.enabled = false;
        } else
        {
            leftLidar.enabled = true;
            if(state == 1)
            {
                leftLidar.color = Color.yellow;
            } else
            {
                leftLidar.color = Color.red;
            }
        }
    }

    public void SetRightLidar(int state)
    {
        if (state == 0)
        {
            rightLidar.enabled = false;
        }
        else
        {
            rightLidar.enabled = true;
            if (state == 1)
            {
                rightLidar.color = Color.yellow;
            }
            else
            {
                rightLidar.color = Color.red;
            }
        }
    }
}