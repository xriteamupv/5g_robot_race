using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OppositeMovement : MonoBehaviour
{
    // Objeto que se moverá contrario al objeto 1
    public GameObject objectToMoveOpposite;

    // Referencia al objeto 1 que se mueve con las coordenadas GPS
    public GameObject objectToMoveWithGPS;

    // Almacena la posición inicial del objeto 1
    private Vector3 initialPositionObject1;
    private Vector3 initialRotationObject1;
    //private Quaternion initialRotationObject1;

    // Almacena la posición inicial del objeto 2
    private Vector3 initialPositionObject2;
    private Vector3 initialRotationObject2;
    //private Quaternion initialRotationObject2;

    // Almacena la posición anterior del objeto 1
    private Vector3 previousPositionObject1;
    private Vector3 previousRotationObject1;
    //private Quaternion previousRotationObject1;

    // Indica si el objeto 1 ha comenzado a moverse
    private bool hasObject1Moved = false;

    void Start()
    {
        // Guarda las posiciones iniciales de los objetos
        initialPositionObject1 = objectToMoveWithGPS.transform.position;
        //Debug.Log(objectToMoveWithGPS.transform.position);
        initialPositionObject2 = objectToMoveOpposite.transform.position;
        //Debug.Log(objectToMoveWithGPS.transform.rotation);
        initialRotationObject1 = objectToMoveWithGPS.transform.rotation.eulerAngles;
        initialRotationObject2 = objectToMoveOpposite.transform.rotation.eulerAngles;
        previousPositionObject1 = initialPositionObject1;
        previousRotationObject1 = initialRotationObject1;
    }

    void Update()
    {
        // Obtiene la posición actual del objeto 1 (objeto con GPS)
        Vector3 currentPositionObject1 = objectToMoveWithGPS.transform.position;
        Vector3 currentRotationObject1 = objectToMoveWithGPS.transform.rotation.eulerAngles;
        //Debug.Log(currentRotationObject1);
        //Quaternion currentRotationObject1 = objectToMoveWithGPS.transform.rotation;

        // Detecta si el objeto 1 ha comenzado a moverse
        if (!hasObject1Moved && currentPositionObject1 != initialPositionObject1)
        {
            hasObject1Moved = true;
        }

        // Si el objeto 1 ha comenzado a moverse, mueve el objeto 2 de forma contraria
        if (hasObject1Moved)
        {
            // Calcula el desplazamiento del objeto 1 desde la última posición
            Vector3 displacementObject1 = currentPositionObject1 - previousPositionObject1;
            Vector3 rotationObject1 = currentRotationObject1 - previousRotationObject1;
            //Quaternion rotationObject1 = currentRotationObject1 * Quaternion.Inverse(previousRotationObject1);

            // Calcula el desplazamiento opuesto
            Vector3 oppositeDisplacement = new Vector3(-displacementObject1.x, displacementObject1.y, -displacementObject1.z);
            Vector3 oppositeRotation = new Vector3(rotationObject1.x, rotationObject1.y, -rotationObject1.z);
            //Debug.Log(oppositeRotation);
            Quaternion oppositeRotationQuaternion = Quaternion.Euler(oppositeRotation);
            //Quaternion oppositeRotation = new Quaternion(rotationObject1.x, rotationObject1.y, -rotationObject1.z, rotationObject1.w);

            // Aplica el desplazamiento opuesto a la posición inicial del objeto 2
            objectToMoveOpposite.transform.position += oppositeDisplacement;
            objectToMoveOpposite.transform.rotation *= oppositeRotationQuaternion;

            // Actualiza la posición anterior del objeto 1
            previousPositionObject1 = currentPositionObject1;
            previousRotationObject1 = currentRotationObject1;
        }
    }
}

