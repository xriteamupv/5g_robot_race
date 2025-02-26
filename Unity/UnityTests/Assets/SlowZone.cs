using Bhaptics.SDK2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowZone : MonoBehaviour
{
    private Controller controller; // Guarda referencia al Controller

    void Start()
    {
        GameObject controllerObj = GameObject.Find("Controller");
        if (controllerObj != null)
        {
            controller = controllerObj.GetComponent<Controller>();
        }
        else
        {
            Debug.LogError("Controller not found in scene");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("robot") && controller != null)
        {
            controller.setBaseSpeed(0.4f);
            BhapticsLibrary.PlayParam(BhapticsEvent.FORBIDDEN_ZONE,
                        intensity: 1f,
                        duration: 0.8f,
                        angleX: 0f,
                        offsetY: 0f
                    );
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("robot") && controller != null)
        {
            controller.setBaseSpeed(1.0f);
        }
    }
}
