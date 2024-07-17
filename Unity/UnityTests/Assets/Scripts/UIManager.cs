using Bhaptics.SDK2;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text speed;
    public Text lapTime;
    public Image battery;
    public Image leftLidar;
    public Image rightLidar;
    public PointerIndicator speedometer;

    public Sprite leftRedLidar;
    public Sprite rightRedLidar;
    public Sprite leftYellowLidar;
    public Sprite rightYellowLidar;
    public GameObject steeringWheel;

    public Transform leftHand;
    public Transform rightHand;
    public Vector3 wheelOffset;

    public GameObject messageBox;
    public Vector3 messageBoxMaxScale;
    public float messageBoxSpeed = 500.0f;
    public float messageBoxDistance = 50.0f;
    public float messageBoxAngle = 0.3f;

    public Sprite[] batterySprites;
    public float wheelSpeed = 5.0f;
    private float wheelValue;

    private int lastLeftLidarState = 0;
    private int lastRightLidarState = 0;

    List<Coroutine> coroutineList;

    private void Awake()
    {
        StartCoroutine(RotateWheel());
        messageBox.GetComponent<RectTransform>().localScale = Vector3.zero;
        coroutineList = new List<Coroutine>();
    }

    private void Update()
    {
        if (Input.GetAxis("L2") > 0.0f && Input.GetAxis("R2") > 0.0f)
        {
            PlaceSteeringWheel();
            Debug.Log("Test");
        }
    }

    public void PlaceSteeringWheel()
    {
        steeringWheel.transform.position = Vector3.Lerp(leftHand.position, rightHand.position, 0.5f);
        //steeringWheel.transform.position += wheelOffset;
    }

    /*public IEnumerator ShowMessageBox()
    {
        RectTransform rectTransform = messageBox.GetComponent<RectTransform>();
        float t = 0.0f;
        rectTransform.position = Vector3.Lerp(Camera.main.transform.forward, Camera.main.transform.up, messageBoxAngle).normalized * messageBoxDistance;
        Vector3 lookDirection = rectTransform.position - Camera.main.transform.position;
        rectTransform.rotation = Quaternion.LookRotation(lookDirection.normalized, Vector3.up);
        while (rectTransform.localScale.x < messageBoxMaxScale.x)
        {
            rectTransform.localScale = Vector3.Lerp(Vector3.zero, messageBoxMaxScale, t);
            t += messageBoxSpeed * Time.deltaTime;
            yield return null;
        }
        rectTransform.localScale = messageBoxMaxScale;
    }

    public IEnumerator HideMessageBox()
    {
        RectTransform rectTransform = messageBox.GetComponent<RectTransform>();
        float t = 0.0f;
        while (rectTransform.localScale.x > 0.0f)
        {
            rectTransform.localScale = Vector3.Lerp(messageBoxMaxScale, Vector3.zero, t);
            t += messageBoxSpeed * Time.deltaTime;
            yield return null;
        }
        rectTransform.localScale = Vector3.zero;
    }*/

    public void ChangeSpeed(float newSpeed)
    {
        speedometer.setValue(Mathf.Abs(newSpeed * 3.6f));
    }

    public void ChangeLapTime(string newLapTime)
    {
        lapTime.text = newLapTime;
        Debug.Log(newLapTime);
    }

    public IEnumerator RotateWheel()
    {
        while (true)
        {
            Quaternion newRotation = Quaternion.Lerp(steeringWheel.transform.rotation, Quaternion.Euler(0.0f, 0.0f, wheelValue), wheelSpeed * Time.deltaTime);
            steeringWheel.transform.rotation = newRotation;
            yield return null;
        }
    }

    public void ChangeWheelRotation(float newValue)
    {
        wheelValue = newValue;
    }

    public void ChangeBattery(int newBattery)
    {
        if (newBattery > 75)
        {
            battery.sprite = batterySprites[0];
        }
        else if (newBattery > 50)
        {
            battery.sprite = batterySprites[1];
        }
        else if (newBattery > 25)
        {
            battery.sprite = batterySprites[2];
        }
        else
        {
            battery.sprite = batterySprites[3];
        }
    }

    /*public void SetLeftLidar(int state)
    {
        if(state != lastLeftLidarState)
        {
            if (state == 0)
            {
                foreach (var l in coroutineList)
                {
                    if (l == null)
                    {
                        coroutineList.Remove(l);
                        continue;
                    }
                    StopCoroutine(l);
                }
                coroutineList.Add(StartCoroutine(HideMessageBox()));
            }
            else
            {
                foreach (var l in coroutineList)
                {
                    if (l == null)
                    {
                        coroutineList.Remove(l);
                        continue;
                    }
                    StopCoroutine(l);
                }
                coroutineList.Add(StartCoroutine(ShowMessageBox()));
                BhapticsLibrary.PlayParam(BhapticsEvent.CARWARNINGLEFT,
                                        intensity: 1f,   // The value multiplied by the original value
                                        duration: 0.8f,    // The value multiplied by the original value
                                        angleX: 0f,     // The value that rotates around global Vector3.up(0~360f)
                                        offsetY: 0f  // The value to move up and down(-0.5~0.5)
                                    );
            }
        }
        lastLeftLidarState = state;
    }

    public void SetRightLidar(int state)
    {
        if(state != lastRightLidarState)
        {
            if (state == 0)
            {
                foreach (var l in coroutineList)
                {
                    if (l == null)
                    {
                        coroutineList.Remove(l);
                        continue;
                    }
                    StopCoroutine(l);
                }
                coroutineList.Add(StartCoroutine(HideMessageBox()));
            }
            else
            {
                foreach (var l in coroutineList)
                {
                    if (l == null)
                    {
                        coroutineList.Remove(l);
                        continue;
                    }
                    StopCoroutine(l);
                }
                coroutineList.Add(StartCoroutine(ShowMessageBox()));
                BhapticsLibrary.PlayParam(BhapticsEvent.CARWARNINGRIGHT,
                                        intensity: 1f,   // The value multiplied by the original value
                                        duration: 0.8f,    // The value multiplied by the original value
                                        angleX: 0f,     // The value that rotates around global Vector3.up(0~360f)
                                        offsetY: 0f  // The value to move up and down(-0.5~0.5)
                                    );
            }
        }
        lastRightLidarState = state;
    }*/
}
