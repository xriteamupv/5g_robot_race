using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.XR.CoreUtils;

public class RecenterOrigin : MonoBehaviour
{
    public Transform target;

    public void Recenter()
    {
        XROrigin xrOrigin = GetComponent<XROrigin>();
        //xrOrigin.MoveCameraToWorldLocation(target.position);
        //xrOrigin.MatchOriginUpCameraForward(target.up, target.forward);

        Vector3 desplazamiento = target.position - xrOrigin.Camera.transform.position;
        xrOrigin.CameraFloorOffsetObject.transform.position += desplazamiento;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Recenter();
        }
    }
}
