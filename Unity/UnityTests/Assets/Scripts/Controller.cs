using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    public RenderTexture rt;
    public CustomPipelinePlayer pipelinePlayer;
    public GameObject mainMenu;
    public GameObject robotScene;
    public Transform robot1;
    public Transform robot2;

    void Start()
    {
        Debug.Log("displays connected: " + Display.displays.Length);
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

        if(Input.GetButtonDown("Submit"))
        {
            SwitchToRobotScene();
        }

    }

    private void SwitchToRobotScene()
    {
        if(mainMenu.activeSelf)
        {
            mainMenu.SetActive(false);
            robotScene.SetActive(true);
            robot1.localScale *= 4.0f;
            robot2.localScale *= 4.0f;
        }
    }
}
