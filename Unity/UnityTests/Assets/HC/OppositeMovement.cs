using UnityEngine;

public class OppositeMovement : MonoBehaviour
{
    public GameObject objectToMoveOpposite;
    public GameObject objectToMoveWithGPS;

    private Vector3 initialPositionObject1;
    private Quaternion initialRotationObject1;
    private Vector3 initialPositionObject2;
    private Quaternion initialRotationObject2;
    private Vector3 previousPositionObject1;
    private Quaternion previousRotationObject1;
    private bool hasObject1Moved = false;

    void Start()
    {
        initialPositionObject1 = objectToMoveWithGPS.transform.position;
        initialPositionObject2 = objectToMoveOpposite.transform.position;
        initialRotationObject1 = objectToMoveWithGPS.transform.rotation;
        initialRotationObject2 = objectToMoveOpposite.transform.rotation;
        previousPositionObject1 = initialPositionObject1;
        previousRotationObject1 = initialRotationObject1;
    }

    void Update()
    {
        Vector3 currentPositionObject1 = objectToMoveWithGPS.transform.position;
        Quaternion currentRotationObject1 = objectToMoveWithGPS.transform.rotation;

        if (!hasObject1Moved && currentPositionObject1 != initialPositionObject1)
        {
            hasObject1Moved = true;
        }

        if (hasObject1Moved)
        {
            // Calcula el desplazamiento del objeto 1 desde la �ltima posici�n
            Vector3 displacementObject1 = currentPositionObject1 - previousPositionObject1;

            // Calcula la rotaci�n del objeto 1 desde la �ltima rotaci�n
            //Quaternion rotationDelta = currentRotationObject1 * Quaternion.Inverse(previousRotationObject1);

            // Calcula el �ngulo de rotaci�n en el eje Y
            //float angleY = rotationDelta.eulerAngles.y;

            // Calcula el desplazamiento opuesto
            Vector3 oppositeDisplacement = new Vector3(-displacementObject1.x, displacementObject1.y, -displacementObject1.z);

            // Aplica el desplazamiento opuesto a la posici�n inicial del objeto 2
            objectToMoveOpposite.transform.position += oppositeDisplacement;

            // Aplica la rotaci�n opuesta en funci�n del �ngulo de rotaci�n en Y
            //objectToMoveOpposite.transform.Rotate(Vector3.up, -angleY, Space.World);

            // Actualiza la posici�n y rotaci�n anterior del objeto 1
            previousPositionObject1 = currentPositionObject1;
            //previousRotationObject1 = currentRotationObject1;
        }
    }
}
