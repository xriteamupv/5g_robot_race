using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualRobotController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 100f;
    public UIManager manager;

    RaycastHit rayHitForward;
    RaycastHit rayHitBackward;
    Ray rayForward;
    Ray rayBackward;

    void Start()
    {
    }

    void FixedUpdate()
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

        rayForward = new Ray(transform.position, -transform.forward);
        rayBackward = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(rayForward, out rayHitForward, 1.75f) || Physics.Raycast(rayBackward, out rayHitBackward, 1.75f))
        {
            if((rayHitForward.collider?.gameObject.tag != "walls" || moveDirection > 0.0f) && (rayHitBackward.collider?.gameObject.tag != "walls" || moveDirection < 0.0f))
            {
                transform.position += movement;
            }
        }
        else
        {
            transform.position += movement;
        }


        // Aplicar giro
        transform.Rotate(Vector3.up, turnDirection * turnSpeed * Time.deltaTime);
    }
}
