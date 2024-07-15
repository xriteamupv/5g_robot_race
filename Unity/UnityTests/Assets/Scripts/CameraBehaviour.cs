using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform mainCamera;
    public float offset;

    void Update()
    {
        transform.position = mainCamera.position;   
        transform.rotation = Quaternion.Euler(mainCamera.rotation.eulerAngles.x, mainCamera.rotation.eulerAngles.y + offset, mainCamera.rotation.eulerAngles.z);
    }
}
