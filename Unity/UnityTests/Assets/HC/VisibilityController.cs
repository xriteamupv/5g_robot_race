using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityController : MonoBehaviour
{
    public GameObject player;  // Asigna el objeto del jugador desde el inspector
    public float visibilityDistance = 10.0f;  // Distancia a la que los objetos se vuelven visibles
    public float startFade = 20.0f;

    private MeshRenderer meshRenderer;

    void Start()
    {

        //// Obtén el componente MeshRenderer para controlar la visibilidad del objeto
        meshRenderer = GetComponent<MeshRenderer>();

        //// Inicialmente, oculta el objeto
        //if (meshRenderer != null)
        //{
        //    meshRenderer.material.color = new Color(1f, 1f, 1f, 0.5f);
        //}
        //else
        //{
        //    Debug.LogError("MeshRenderer no encontrado en " + gameObject.name);
        //}
    }

    void Update()
    {
        if (meshRenderer == null) return;

        // Calcula la distancia entre el jugador y el objeto
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        float div = startFade - visibilityDistance;
        // Si la distancia es menor o igual a la distancia de visibilidad, muestra el objeto
        // De lo contrario, oculta el objeto
        if (distanceToPlayer <= startFade)
        {
            meshRenderer.material.color = new Color(1f, 1f, 1f, (startFade - distanceToPlayer) / div);
        } else
        {
            meshRenderer.material.color = new Color(1f, 1f, 1f, 0f);
        }



    }
}
