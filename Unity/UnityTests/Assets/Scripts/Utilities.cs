using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Utilities : MonoBehaviour
{

    public CustomPipelinePlayer player;
    public string pipeline;
    public string pipeline2;

    public GameObject modelo1;
    public GameObject modelo2;

    public GameObject grafanaBrowser;
    public GameObject robotnikBrowser;
    public GameObject browserKeyboard;

    void Awake()
    {
        string path = Application.dataPath + "/ips.txt";

        string[] lines = File.ReadAllLines(path);

        if(lines.Length < 5)
        {
            Debug.LogError("Text file doesn't contain all necessary info");
            return;
        }

        pipeline = lines[0];
        player.pipeline = pipeline;
        ProxyConnection pc = GetComponent<ProxyConnection>();
        pc.IP = lines[1];
        pc.receivingPort = int.Parse(lines[2]);
        pc.sendingPort = int.Parse(lines[3]);
        pc.selectedRobot = lines[4];
        pipeline2 = lines[5];
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            ChangeModels();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            grafanaBrowser.SetActive(!grafanaBrowser.activeSelf);
            //browserKeyboard.SetActive(grafanaBrowser.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            robotnikBrowser.SetActive(!robotnikBrowser.activeSelf);
            browserKeyboard.SetActive(robotnikBrowser.activeSelf);
        }
    }

    public void ChangeModels()
    {
        modelo1.SetActive(!modelo1.activeSelf);
        modelo2.SetActive(!modelo2.activeSelf);
    }
}