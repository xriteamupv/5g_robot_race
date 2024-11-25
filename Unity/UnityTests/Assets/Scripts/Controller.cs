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

    public RectTransform canvas;
    public RectTransform robotRect;

    public float scaleFactor;

    public struct BoundingBox
    {
        public float[] coords;
        public string type;
        public float timestamp;
        public float detectionPercent;
    }

    public BoundingBox bb;
    public LayerMask layerMask;

    void Start()
    {
        //Debug.Log("displays connected: " + Display.displays.Length);
        // Display.displays[0] is the primary, default display and is always ON, so start at index 1.
        // Check if additional displays are available and activate each.

        for (int i = 1; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }
        bb.coords = new float[] { 3070, 601, 3342, 1088 };
        bb.type = "stop";
        PlaceBB();
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

    public void PlaceBB()
    {
        float sizeX = bb.coords[2] - bb.coords[0];
        float sizeY = bb.coords[3] - bb.coords[1];

        //float totalSizeX = 1920.0f;
        //float totalSizeY = 960.0f;
        float totalSizeX = 3840.0f;
        float totalSizeY = 1920.0f;

        Vector2 center = new Vector2(bb.coords[0] + sizeX * 0.5f, bb.coords[1] + sizeY * 0.5f);
        Debug.Log(center);

        float elevation = center.y / totalSizeY * 180.0f;
        float azimuth = (center.x - totalSizeX * 0.5f) / totalSizeX * 360.0f;

        Debug.Log("Elevation: " + elevation);
        Debug.Log("Azimuth: " + azimuth);
#if false
        Vector3 forward = Camera.main.transform.forward;
        Vector3 up = Camera.main.transform.up;

        Quaternion azimuthRotation = Quaternion.Euler(0.0f, azimuth, 0.0f);
        Quaternion elevationRotation = Quaternion.Euler(elevation, 0.0f, 0.0f);
        Vector3 v = elevationRotation * up + azimuthRotation * forward;
        canvas.localPosition = v * 200.0f;
        //canvas.localPosition = new Vector3(canvas.localPosition.x * -angleX, canvas.localPosition.y * angleY, canvas.localPosition.z * 500.0f);
#else
        float elevationRad = elevation * Mathf.Deg2Rad;
        float azimuthRad = azimuth * Mathf.Deg2Rad;
        float x = Mathf.Sin(azimuthRad) * Mathf.Sin(elevationRad);
        float y = Mathf.Cos(elevationRad);
        float z = Mathf.Cos(azimuthRad) * Mathf.Sin(elevationRad);

        Vector3 v = new Vector3(x, y, z).normalized;
        canvas.position = (v * 500.0f);
#endif

        canvas.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sizeX * scaleFactor);
        canvas.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sizeY * scaleFactor);

        robotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sizeX * scaleFactor);
        robotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sizeY * scaleFactor);

        Vector3 cameraToCanvas = canvas.position - Camera.main.transform.position;
        canvas.rotation = Quaternion.LookRotation(cameraToCanvas, Vector3.up);
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
