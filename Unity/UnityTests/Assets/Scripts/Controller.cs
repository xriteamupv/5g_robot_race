using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Controller : MonoBehaviour
{

    public RenderTexture rt;
    public CustomPipelinePlayer pipelinePlayer;
    public GameObject mainMenu;
    public GameObject robotScene;
    public V_5[] boxes;
    public TrafficLight trafficLight;
    public ProxyConnection proxyConnection;

    public RectTransform robotRect;
    public RectTransform redRectPrefab;
    public RectTransform orangeRectPrefab;
    public RectTransform yellowRectPrefab;
    public RectTransform greenRectPrefab;
    public float scaleFactor;

    public RectTransform detectionCanvas;

    public int poolSize;
    public RectTransform[] redRect;
    public RectTransform[] orangeRect;
    public RectTransform[] yellowRect;
    public RectTransform[] greenRect;


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
        CreateBBPools();
    }

    public void ConvertTexture(Texture texture)
    {
        if (texture != null && rt != null)
            Graphics.Blit(texture, rt);
    }

    private void Update()
    {
        ConvertTexture(pipelinePlayer.VideoTexture);

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetScene();
        }
    }

    void CreateBBPools()
    {
        redRect = new RectTransform[poolSize];
        orangeRect = new RectTransform[poolSize];
        yellowRect = new RectTransform[poolSize];
        greenRect = new RectTransform[poolSize];

        for (int i = 0; i < poolSize; ++i)
        {
            redRect[i] = Instantiate(redRectPrefab);
            orangeRect[i] = Instantiate(orangeRectPrefab);
            yellowRect[i] = Instantiate(yellowRectPrefab);
            greenRect[i] = Instantiate(greenRectPrefab);

            redRect[i].parent = detectionCanvas;
            orangeRect[i].parent = detectionCanvas;
            yellowRect[i].parent = detectionCanvas;
            greenRect[i].parent = detectionCanvas;

            redRect[i].gameObject.SetActive(false);
            orangeRect[i].gameObject.SetActive(false);
            yellowRect[i].gameObject.SetActive(false);
            greenRect[i].gameObject.SetActive(false);
        }
    }

    RectTransform GetBB(string type)
    {
        switch(type)
        {
            case "person":
                {
                    for (int i = 0; i < poolSize; ++i)
                    {
                        if (!redRect[i].gameObject.activeSelf)
                        {
                            return redRect[i];
                        }
                    }
                }
                break;
            case "orange":
                {
                    for (int i = 0; i < poolSize; ++i)
                    {
                        if (!orangeRect[i].gameObject.activeSelf)
                        {
                            return orangeRect[i];
                        }
                    }
                }
                break;
            case "yellow":
                {
                    for (int i = 0; i < poolSize; ++i)
                    {
                        if (!yellowRect[i].gameObject.activeSelf)
                        {
                            return yellowRect[i];
                        }
                    }
                }
                break;
            case "green":
                {
                    for (int i = 0; i < poolSize; ++i)
                    {
                        if (!greenRect[i].gameObject.activeSelf)
                        {
                            return greenRect[i];
                        }
                    }
                }
                break;
        }
        return null;
    }

    public void SetBB(string type, float[] coords)
    {
        PlaceBB(GetBB(type), coords);
    }

    public void ResetBBs()
    {
        for (int i = 0; i < poolSize; ++i)
        {
            redRect[i].gameObject.SetActive(false);
            orangeRect[i].gameObject.SetActive(false);
            yellowRect[i].gameObject.SetActive(false);
            greenRect[i].gameObject.SetActive(false);
        }
    }

    public void PlaceBB(RectTransform rect, float[] bbcoords)
    {
        if (rect == null) return;
        rect.gameObject.SetActive(true);
        float sizeX = bbcoords[2] - bbcoords[0];
        float sizeY = bbcoords[3] - bbcoords[1];

        float totalSizeX = 2560.0f;
        float totalSizeY = 1280.0f;
        //float totalSizeX = 3840.0f;
        //float totalSizeY = 1920.0f;

        Vector2 center = new Vector2(bbcoords[0] + sizeX * 0.5f, bbcoords[1] + sizeY * 0.5f);
        // Debug.Log(center);

        float elevation = center.y / totalSizeY * 180.0f;
        float azimuth = (center.x - totalSizeX * 0.5f) / totalSizeX * 360.0f;

        // Debug.Log("Elevation: " + elevation);
        // Debug.Log("Azimuth: " + azimuth);
#if false
        Vector3 forward = Camera.main.transform.forward;
        Vector3 up = Camera.main.transform.up;

        Quaternion azimuthRotation = Quaternion.Euler(0.0f, azimuth, 0.0f);
        Quaternion elevationRotation = Quaternion.Euler(elevation, 0.0f, 0.0f);
        Vector3 v = elevationRotation * up + azimuthRotation * forward;
        rect.localPosition = v * 200.0f;
        //rect.localPosition = new Vector3(rect.localPosition.x * -angleX, rect.localPosition.y * angleY, rect.localPosition.z * 500.0f);
#else
        float elevationRad = elevation * Mathf.Deg2Rad;
        float azimuthRad = azimuth * Mathf.Deg2Rad;
        float x = Mathf.Sin(azimuthRad) * Mathf.Sin(elevationRad);
        float y = Mathf.Cos(elevationRad);
        float z = Mathf.Cos(azimuthRad) * Mathf.Sin(elevationRad);

        Vector3 v = new Vector3(x, y, z).normalized;
        rect.position = (v * 500.0f);
#endif

        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sizeX * scaleFactor);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sizeY * scaleFactor);

        Vector3 cameraToCanvas = rect.position - Camera.main.transform.position;
        rect.rotation = Quaternion.LookRotation(cameraToCanvas, Vector3.up);
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
