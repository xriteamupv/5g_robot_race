using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Utilities : MonoBehaviour
{

    public CustomPipelinePlayer player;

    void Awake()
    {
        string path = Application.dataPath + "/ips.txt";

        string[] lines = File.ReadAllLines(path);

        if(lines.Length < 3)
        {
            Debug.LogError("Text file doesn't contain all necessary info");
            return;
        }

        player.pipeline = lines[0];
        ProxyConnection pc = GetComponent<ProxyConnection>();
        pc.IP = lines[1];
        pc.selectedRobot = lines[2];
    }
}