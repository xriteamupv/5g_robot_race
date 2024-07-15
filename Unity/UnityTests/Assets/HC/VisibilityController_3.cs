using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityController_3 : MonoBehaviour
{
    public GameObject player;  // Asigna el objeto del jugador desde el inspector
    public float visibilityDistance = 3.0f;  // Distancia a la que los objetos se vuelven visibles

    private MeshRenderer meshRenderer;

    void Start()
    {
        // Obtén el componente MeshRenderer para controlar la visibilidad del objeto
        meshRenderer = GetComponent<MeshRenderer>();

        // Inicialmente, oculta el objeto
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }
        else
        {
            Debug.LogError("MeshRenderer no encontrado en " + gameObject.name);
        }
    }

    void Update()
    {
        if (meshRenderer == null) return;

        // Calcula la distancia entre el jugador y el objeto
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        // Si la distancia es menor o igual a la distancia de visibilidad, muestra el objeto
        // De lo contrario, oculta el objeto
        meshRenderer.enabled = distanceToPlayer <= visibilityDistance;
    }
}