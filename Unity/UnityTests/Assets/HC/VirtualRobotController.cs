using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualRobotController : MonoBehaviour
{
    public GameObject robotVirtual; // Referencia al objeto del robot virtual
    public float moveSpeed = 5f; // Velocidad de movimiento del robot virtual
    public float turnSpeed = 100f; // Velocidad de giro del robot virtual

    void Start()
    {
        // Asegúrate de que el objeto del robot virtual esté asignado
        if (robotVirtual == null)
        {
            Debug.LogError("Robot virtual no asignado. Asigna el objeto del robot virtual en el inspector.");
        }
    }

    void Update()
    {
        MoveRobot();
    }

    private void MoveRobot()
    {
        if (robotVirtual != null)
        {
            // Obtener las entradas de los pedales y el volante
            float pedal2Input = Input.GetAxis("Pedal"); // Acelerador
            float pedalInput = Input.GetAxis("Back"); // Reversa
            float wheelInput = Input.GetAxis("Wheel"); // Volante

            // Calcular la dirección de movimiento y giro
            float moveDirection = pedal2Input - pedalInput;
            float turnDirection = wheelInput;

            // Aplicar movimiento hacia adelante/atrás
            Vector3 movement = robotVirtual.transform.forward * moveDirection * moveSpeed * Time.deltaTime;
            robotVirtual.transform.position += movement;

            // Aplicar giro
            robotVirtual.transform.Rotate(Vector3.up, turnDirection * turnSpeed * Time.deltaTime);
        }
    }
}
