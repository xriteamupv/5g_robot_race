using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualCameraOverlay : MonoBehaviour
{
    public Camera virtualCamera;
    public RenderTexture renderTexture;
    public RawImage rawImage;
    public Vector2 size = new Vector2(256, 256);  // Tamaño del recuadro
    public Vector2 position = new Vector2(10, 10);  // Posición del recuadro desde la esquina inferior izquierda

    void Start()
    {
        // Asignar RenderTexture a la cámara virtual
        if (virtualCamera != null && renderTexture != null)
        {
            virtualCamera.targetTexture = renderTexture;
        }

        // Configurar RawImage
        if (rawImage != null && renderTexture != null)
        {
            rawImage.texture = renderTexture;
            rawImage.material = new Material(Shader.Find("Custom/TransparentShader"));
            RectTransform rt = rawImage.GetComponent<RectTransform>();
            rt.sizeDelta = size;  // Ajustar tamaño
            rt.anchorMin = new Vector2(0, 0);  // Anclar en la esquina inferior izquierda
            rt.anchorMax = new Vector2(0, 0);
            rt.anchoredPosition = position;  // Ajustar posición
        }
    }
}
