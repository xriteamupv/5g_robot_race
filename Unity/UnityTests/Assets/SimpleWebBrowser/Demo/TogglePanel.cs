using UnityEngine;
using TMPro; // Importa el espacio de nombres para TextMeshPro

public class TogglePanel : MonoBehaviour
{
    public GameObject panel; // Referencia al panel
    public TMP_Text buttonText; // Referencia al texto del botón usando TextMeshPro

    public void TogglePanelVisibility()
    {
        // Cambia la visibilidad del panel
        bool isActive = panel.activeSelf;
        panel.SetActive(!isActive);

        // Cambia el texto del botón en función del estado del panel
        if (isActive)
        {
            buttonText.text = "Abrir Browser";
        }
        else
        {
            buttonText.text = "Cerrar Browser";
        }
    }
}

