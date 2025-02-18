using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private DateTime startTime;  // Variable para registrar el tiempo en que se envía la velocidad a 0
    public ParticleSystem CoinGlow;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "robot")
        {
            // Nueva lógica: siempre establecer la velocidad a 0 y registrar el tiempo de envío
            startTime = DateTime.Now;  // Almacenamos el tiempo en que enviamos el comando de velocidad 0
            string currentTime = startTime.ToString("HH:mm:ss.fff");  // Formato de hora con milisegundos
            Debug.Log("****EL ROBOT HA TENIDO CONTACTO CON LA MONEDA****: " + currentTime);

            GameObject.Find("Controller").GetComponent<Controller>().IncrementCoins();
        }
        CoinGlow.Play();
        Destroy(gameObject);
        
    }
}
