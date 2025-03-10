using Bhaptics.SDK2;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private DateTime startTime;  // Variable para registrar el tiempo en que se envía la velocidad a 0
    public ParticleSystem CoinGlow;
    public GameObject player = null;  // Asigna el objeto del jugador desde el inspector
    private float visibilityDistance = 30.0f;  // Distancia a la que los objetos se vuelven visibles
    Controller controller;
    AudioSource audioSource;

    float animationTime;
    float animationMultiplier;

    private void Start()
    {
        controller = GameObject.Find("Controller").GetComponent<Controller>();
        audioSource = GameObject.Find("Coins").GetComponent<AudioSource>();
        if(audioSource != null)
        {
            Debug.Log("Audio source found");
        }
        if (player == null)
        {
            player = GameObject.Find("Robot 2");
        }

    }
    void Update()
    {

        // Calcula la distancia entre el jugador y el objeto
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if(animationTime > 1.0f)
        {
            animationMultiplier = -1.0f;
        }
        if (animationTime < 0.0f)
        {
            animationMultiplier = 1.0f;
        }

        animationTime += Time.deltaTime * animationMultiplier;

        transform.localPosition = Vector3.Lerp(Vector3.zero, new Vector3(0.2f, 0.0f, 0.0f), animationTime);
        transform.Rotate(new Vector3(50.0f * Time.deltaTime, 0.0f, 0.0f));

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
        Debug.Log("tag = " + other.tag + "    game object" + other.name);
        if (other.tag == "robot")
        {
            // Nueva lógica: siempre establecer la velocidad a 0 y registrar el tiempo de envío
            startTime = DateTime.Now;  // Almacenamos el tiempo en que enviamos el comando de velocidad 0
            string currentTime = startTime.ToString("HH:mm:ss.fff");  // Formato de hora con milisegundos
            Debug.Log("****EL ROBOT HA TENIDO CONTACTO CON LA MONEDA****: " + currentTime);

            controller.IncrementCoins();
            BhapticsLibrary.PlayParam(BhapticsEvent.FASTER_SPEED,
                        intensity: 1f,   // The value multiplied by the original value
                        duration: 0.8f,    // The value multiplied by the original value
                        angleX: 0f,     // The value that rotates around global Vector3.up(0~360f)
                        offsetY: 0f  // The value to move up and down(-0.5~0.5)
                    );
            audioSource.Play();
            CoinGlow.Play();
        }
    }
}
