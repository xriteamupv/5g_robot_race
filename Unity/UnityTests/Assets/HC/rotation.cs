using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotation : MonoBehaviour
{
    public GameObject robot;
    // Almacena la posición anterior del objeto 1
    //private Vector3 previousPositionObject1;
    private Vector3 previousRotationObject;
    //private Quaternion previousRotationObject1;

    // Indica si el objeto 1 ha comenzado a moverse
    private bool hasObject1Moved = false;

    void Start()
    {
        previousRotationObject = robot.transform.rotation.eulerAngles;
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(previousRotationObject.x, -previousRotationObject.y, previousRotationObject.z);
        Debug.Log(previousRotationObject);
    }
}
