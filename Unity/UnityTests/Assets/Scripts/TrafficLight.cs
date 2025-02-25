using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bhaptics.SDK2;

public class TrafficLight : MonoBehaviour
{
    [SerializeField]
    private float time = 0.0f;
    [SerializeField]
    private List<GameObject> signals = new List<GameObject>();
    [SerializeField]
    private List<float> sectorTimes = new List<float>();
    private bool doOnce;
    public ProxyConnection proxyConnection;

    // Start is called before the first frame update
    void Start()
    {
        proxyConnection = GameObject.Find("Controller").GetComponent<ProxyConnection>();
        ResetLight();
    }

    public void ResetLight()
    {
        gameObject.SetActive(true);
        this.enabled = true;
        time = 0.0f;
        doOnce = true;
        proxyConnection.ChangeRobotSpeed(0.0f);
        foreach (var signal in signals)
        {
            signal.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        for (int i = 0; i < signals.Count; i++)
        {
            if (time >= sectorTimes[i])
            {
                signals[i].SetActive(true);
            }
        }
        if(time >= sectorTimes[sectorTimes.Count - 1] && doOnce)
        {
            BhapticsLibrary.PlayParam(BhapticsEvent.FORBIDDEN_ZONE,
                                    intensity: 1f,   // The value multiplied by the original value
                                    duration: 0.8f,    // The value multiplied by the original value
                                    angleX: 0f,     // The value that rotates around global Vector3.up(0~360f)
                                    offsetY: 0f  // The value to move up and down(-0.5~0.5)
                                );
            proxyConnection.ChangeRobotSpeed(0.8f);
            StartCoroutine(GameObject.Find("Controller").GetComponent<UIManager>().ShowMessageBox(UIManager.Sign.kSignGo, true, 2.0f));
            doOnce = false;
        }

        if (time >= sectorTimes[sectorTimes.Count - 1] + 3.0f)
        {
            gameObject.SetActive(false);
            this.enabled = false;
        }
    }
}
