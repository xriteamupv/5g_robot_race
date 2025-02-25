using Bhaptics.SDK2;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tires : MonoBehaviour
{
    private DateTime startTime;  // Variable para registrar el tiempo en que se envía la velocidad a 0
    public GameObject player = null;  // Asigna el objeto del jugador desde el inspector
    public float visibilityDistance = 50.0f;  // Distancia a la que los objetos se vuelven visibles
    Controller controller;


    private void Start()
    {
        controller = GameObject.Find("Controller").GetComponent<Controller>();
        if (player == null)
        {
            player = GameObject.Find("Robot");
        }

    }
    void Update()
    {
        // Calcula la distancia entre el jugador y el objeto
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        // Activa o desactiva los ParticleSystem en función de la distancia
        if (distanceToPlayer <= visibilityDistance)
        {
            GetComponent<Renderer>().enabled = true;
        }
        else
        {
            GetComponent<Renderer>().enabled = false;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "robot")
        {
            // Nueva lógica: siempre establecer la velocidad a 0 y registrar el tiempo de envío
            startTime = DateTime.Now;  // Almacenamos el tiempo en que enviamos el comando de velocidad 0
            string currentTime = startTime.ToString("HH:mm:ss.fff");  // Formato de hora con milisegundos
            Debug.Log("****EL ROBOT HA TENIDO CONTACTO CON LA RUEDA****: " + currentTime);

            controller.ReduceCoins();
        }
        //Destroy(gameObject);

    }
}
