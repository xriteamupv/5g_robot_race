using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityController_2 : MonoBehaviour
{
    public GameObject player;  // Asigna el objeto del jugador desde el inspector
    public float visibilityDistance = 10.0f;  // Distancia a la que los objetos se vuelven visibles
    public float startFade = 20.0f;

    private List<ParticleSystem> particleSystems = new List<ParticleSystem>();

    void Start()
    {
        // Encuentra todos los ParticleSystem en el objeto y sus hijos
        particleSystems.AddRange(GetComponentsInChildren<ParticleSystem>());

        if (particleSystems.Count == 0)
        {
            Debug.LogError("No se encontraron ParticleSystem en " + gameObject.name);
        }
    }

    void Update()
    {
        // Calcula la distancia entre el jugador y el objeto
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        // Activa o desactiva los ParticleSystem en función de la distancia
        if (distanceToPlayer <= startFade)
        {
            SetParticleSystemsActive(true);
        }
        else
        {
            SetParticleSystemsActive(false);
        }
    }

    void SetParticleSystemsActive(bool state)
    {
        foreach (var ps in particleSystems)
        {
            var emission = ps.emission;
            emission.enabled = state;
        }
    }
}

