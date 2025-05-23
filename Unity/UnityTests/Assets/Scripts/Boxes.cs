using Bhaptics.SDK2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boxes : MonoBehaviour
{
    public GameObject player;  // Asigna el objeto del jugador desde el inspector
    public float visibilityDistance = 10.0f;  // Distancia a la que los objetos se vuelven visibles
    public float startFade = 20.0f;
    public float buffTime = 20.0f;
    public ProxyConnection proxyConnection;
    public GameObject particleObject; // Agrega una referencia al objeto que contiene el sistema de part�culas

    private List<ParticleSystem> particleSystems = new List<ParticleSystem>();
    AudioSource audioSource;

    void Start()
    {
        // Encuentra todos los ParticleSystem en el objeto y sus hijos
        particleSystems.AddRange(GetComponentsInChildren<ParticleSystem>());
        audioSource = GameObject.Find("Cajas").GetComponent<AudioSource>();

        if (particleSystems.Count == 0)
        {
            Debug.LogError("No se encontraron ParticleSystem en " + gameObject.name);
        }
        proxyConnection = GameObject.Find("Controller").GetComponent<ProxyConnection>();

        // Aseg�rate de que el objeto de part�culas est� desactivado al inicio
        if (particleObject != null)
        {
            particleObject.SetActive(false);
        }
        else
        {
            Debug.LogError("No se asign� ning�n objeto de part�culas.");
        }
    }

    void Update()
    {
        // Calcula la distancia entre el jugador y el objeto
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        // Activa o desactiva los ParticleSystem en funci�n de la distancia
        if (distanceToPlayer <= startFade)
        {
            SetParticleSystemsActive(true);
        }
        else
        {
            SetParticleSystemsActive(false);
        }
    }

    public void ActivateBox()
    {
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    void SetParticleSystemsActive(bool state)
    {
        foreach (var ps in particleSystems)
        {
            var emission = ps.emission;
            emission.enabled = state;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "robot")
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            Invoke("ActivateBox", buffTime);
            Debug.Log("El robot ha tenido contacto con el objeto de las part�culas.");
            audioSource.Play();

            // Activa el objeto que contiene el sistema de part�culas
            if (particleObject != null)
            {
                particleObject.SetActive(true);
            }
            else
            {
                Debug.LogError("No se asign� ning�n objeto de part�culas.");
            }

            int rand = Random.Range(0, 2);
            if (rand == 0)
            {
                BhapticsLibrary.PlayParam(BhapticsEvent.FASTER_SPEED,
                                        intensity: 1f,   // The value multiplied by the original value
                                        duration: 0.8f,    // The value multiplied by the original value
                                        angleX: 0f,     // The value that rotates around global Vector3.up(0~360f)
                                        offsetY: 0f  // The value to move up and down(-0.5~0.5)
                                    );
                GameObject.Find("Controller").GetComponent<Controller>().setPowerUpSpeed(-0.1f);
                StartCoroutine(GameObject.Find("Controller").GetComponent<UIManager>().ShowMessageBox(UIManager.Sign.kSignSlower, true, buffTime));
            }
            else
            {
                BhapticsLibrary.PlayParam(BhapticsEvent.LOWER_SPEED,
                                        intensity: 1f,   // The value multiplied by the original value
                                        duration: 0.8f,    // The value multiplied by the original value
                                        angleX: 0f,     // The value that rotates around global Vector3.up(0~360f)
                                        offsetY: 0f  // The value to move up and down(-0.5~0.5)
                                    );
                GameObject.Find("Controller").GetComponent<Controller>().setPowerUpSpeed(0.1f);
                StartCoroutine(GameObject.Find("Controller").GetComponent<UIManager>().ShowMessageBox(UIManager.Sign.kSignFaster, true, buffTime));
            }
            //Llamar a funcion modificar velocidad en controller
            GameObject.Find("Controller").GetComponent<Controller>().modifySpeed();
            StartCoroutine(GameObject.Find("Controller").GetComponent<Controller>().setPowerUpSpeedCo(0.0f, buffTime));
        }
    }
}


