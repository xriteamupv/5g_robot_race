using Bhaptics.SDK2;
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

    public ProxyConnection proxyConnection;

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


        // Calcular la dirección de movimiento y giro
        float moveDirection = pedal2Input - pedalInput;
        float turnDirection = wheelInput;
        
        // Aplicar movimiento hacia adelante/atrás
        Vector3 movement = (transform.forward * moveDirection * moveSpeed * Time.deltaTime) * proxyConnection.robotSpeedMultiplier;

        rayForward = new Ray(transform.position, -transform.forward);
        rayBackward = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(rayForward, out rayHitForward, 1.75f) || Physics.Raycast(rayBackward, out rayHitBackward, 1.75f))
        {
            if((rayHitForward.collider?.gameObject.tag != "walls" || moveDirection > 0.0f) && (rayHitBackward.collider?.gameObject.tag != "walls" || moveDirection < 0.0f))
            {
                transform.position += movement;
                manager.SetRightLidar(0);
            }
            else
            {
                manager.SetRightLidar(1);
                BhapticsLibrary.PlayParam(BhapticsEvent.FORBIDDEN_ZONE,
                        intensity: 1f,
                        duration: 0.8f,
                        angleX: 0f,
                        offsetY: 0f
                    );
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
