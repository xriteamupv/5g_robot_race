using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ImGuiNET;

public class Controller : MonoBehaviour
{

    public RenderTexture rt;
    public CustomPipelinePlayer pipelinePlayer;
    public GameObject mainMenu;
    public GameObject robotScene;
    public V_5[] boxes;
    public TrafficLight trafficLight;
    public ProxyConnection proxyConnection;

    void Start()
    {
        //Debug.Log("displays connected: " + Display.displays.Length);
        // Display.displays[0] is the primary, default display and is always ON, so start at index 1.
        // Check if additional displays are available and activate each.

        for (int i = 1; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }
    }

    public void ConvertTexture(Texture texture)
    {
        if (texture != null && rt != null)
            Graphics.Blit(texture, rt);
    }

    private void Update()
    {
        ConvertTexture(pipelinePlayer.VideoTexture);

        //if(Input.GetButtonDown("Submit"))
        //{
        //    SwitchToRobotScene();
        //}

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetScene();
        }

    }

    private void SwitchToRobotScene()
    {
        if(mainMenu.activeSelf)
        {
            mainMenu.SetActive(false);
            robotScene.SetActive(true);
        }
    }

    private void ResetScene()
    {
        proxyConnection.ResetProxy();
        trafficLight.ResetLight();
        foreach (var b in boxes) 
        {
            b.ActivateBox();
        }
    }
}
