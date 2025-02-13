using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualRobotController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 100f;
    public UIManager manager;

    void Start()
    {
    }

    void Update()
    {
        MoveRobot();
    }

    private void MoveRobot()
    {
        // Obtener las entradas de los pedales y el volante
        float pedal2Input = Input.GetAxis("Pedal"); // Acelerador
        float pedalInput = Input.GetAxis("Back"); // Reversa
        float wheelInput = Input.GetAxis("Wheel"); // Volante

        manager.ChangeSpeed(Mathf.Abs(pedal2Input - 1.0f));

        // Calcular la dirección de movimiento y giro
        float moveDirection = pedal2Input - pedalInput;
        float turnDirection = wheelInput;
        
        // Aplicar movimiento hacia adelante/atrás
        Vector3 movement = transform.forward * moveDirection * moveSpeed * Time.deltaTime;
        transform.position += movement;
        
        // Aplicar giro
        transform.Rotate(Vector3.up, turnDirection * turnSpeed * Time.deltaTime);
    }
}
