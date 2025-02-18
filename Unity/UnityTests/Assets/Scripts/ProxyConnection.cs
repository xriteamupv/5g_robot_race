using System;
using System.IO;
using System.Globalization;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; //Contains Prefab utility, not available to build
using TMPro;
//using SG; // Access to SenseGlove classes.
using System.Net.Sockets;
using System.Net;
using Newtonsoft.Json;
using static ProxyConnection;
using static ProxyConnection.ControlData.Data;
using static Stats;

public class ProxyConnection : MonoBehaviour
{
    public string IP;
    public int receivingPort;
    public int sendingPort;
    private UdpClient client;
    Socket s;
    private Thread clientReceiveThread;
    public IPEndPoint anyIP;
    public IPEndPoint currentIP;

    public Position gps, otherGPS;
    //public GPS otherGPS;
    public GameObject padre; //objeto a rotar
    public GameObject objetoAR; //objeto a posicionar
    public UIManager ui;
    public float orientationOffset;
    public string selectedRobot;

    public TrafficLight trafficSign;
    public Stats stats;

    private bool isTrafficEnabled = false;
    public class RobotData
    {
        public string header;
        public class Data
        {
            public float battery;
            public float[] gps;
            public float speed;
            public string[] time;
            public int[] lidar;
        }
        public Data data;
    }

    public class ControlData
    {
        public string header;
        public class Data
        {
            public class Linear
            {
                public float x;
                public float y;
                public float z;
            }

            public class Angular
            {
                public float x;
                public float y;
                public float z;
            }
            public Linear linear;
            public Angular angular;
        }
        public Data data;
    }

    public class TelemetryData
    {
        public string header;
        public class Data
        {
            public float rsrp;
            public float rsrq;
            public float sinr;
            public float latency;
        }
        public Data data;
    }

    public class BoundingData
    {
        public string header;
        public class Data
        {
            public class Box
            {
                public float[] coords;
                public string type;
                public float conf;
                public float timestamp;
            }
            public Box[] boxes;
        }
        public Data data;
    }

    public class RealRobotPositionData
    {
        public string header;
        public class Data
        {
            public float[] gps;
        }
        public Data data;
    }

    public class VirtualRobotPositionData
    {
        public string header;
        public class Data
        {
            public float posX;
            public float posY;
            public float posZ;
            public float rotX;
            public float rotY;
            public float rotZ;
            public float rotW;
        }
        public Data data;
    }

    private RobotData robotData;
    private TelemetryData telemetryData;
    private ControlData controlData;
    private BoundingData boundingData;
    private RealRobotPositionData realRobotPositionData;
    private VirtualRobotPositionData virtualRobotPositionData;
    private bool updateValues;
    private bool updateTelemetry;
    private bool updateBoxes;
    private bool updateRobot;
    private bool loop;
    private bool isRobot1;

    public float rotationSpeed = 1.0f;
    private Quaternion targetRotation;
    public float robotSpeedMultiplier;

    public Controller controller;
    public GameObject virtualRobot;

    void Start()
    {
        updateValues = true;
        updateTelemetry = false;
        updateBoxes = false;
        updateRobot = false;
        loop = true;
        robotSpeedMultiplier = 0.0f;
        currentIP = new IPEndPoint(IPAddress.Parse(IP), receivingPort);

        if (IP != "") Connect();
        if(selectedRobot == "Robot 1") 
            isRobot1 = true;
        InvokeRepeating("WheelInput", 0.0f, 0.1f);

        float[] bbcoords = new float[] { 3070, 601, 3342, 1088 };
        string bbtype = "yellow";
        controller = GetComponent<Controller>();
        controller.SetBB(bbtype, bbcoords);

        realRobotPositionData = new RealRobotPositionData();
        realRobotPositionData.data = new RealRobotPositionData.Data();
        realRobotPositionData.header = "cockpit-broadcast";

        controlData = new ControlData();
        controlData.data = new ControlData.Data();
        controlData.data.linear = new Linear();
        controlData.data.angular = new Angular();
        controlData.header = "control";
    }

    public void ChangeRobotSpeed(float newSpeed)
    {
        robotSpeedMultiplier = newSpeed;
    }

    public IEnumerator ChangeRobotSpeedCo(float newSpeed, float time)
    {
        yield return new WaitForSeconds(time);
        robotSpeedMultiplier = newSpeed;
        yield return null;
    }

    public void ResetProxy()
    {
        isTrafficEnabled = false;
        ui.RemoveLapTime();
    }

    private void Update()
    {
        if (!Application.isFocused) return;
        UpdateTelemetry();
        UpdateBoxes();
        UpdateRobot();
        if (updateValues)
        {
            UpdateValues(robotData);
            updateValues = false;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            isTrafficEnabled = true;
            //SendNetworkMessage("timer");
        }
        trafficSign.enabled = isTrafficEnabled;
    }

    private void WheelInput()
    {
        float pedalInput = (Input.GetAxis("Pedal") - 1.0f) * -robotSpeedMultiplier; //cambiar multiplicador para modificar velocidad
        if(pedalInput > 1.0f) pedalInput = 1.0f;
        // float pedal2Input = (Input.GetAxis("Back") - 1.0f) * 1.0f;
        float wheelInput = Input.GetAxis("Wheel") * -3.0f;

        ui.ChangeWheelRotation(-Input.GetAxis("Wheel") * 360.0f);

        controlData.data.linear.x = pedalInput;
        controlData.data.angular.z = wheelInput;

        string inputMessage = JsonConvert.SerializeObject(controlData);
        SendNetworkMessage(inputMessage);
    }

    private void SendNetworkMessage(string message)
    {
        if (s == null)
        {
            return;
        }
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(IP), sendingPort);
            s.SendTo(data, data.Length, SocketFlags.None, ep);
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }

    private void Connect()
    {
        try
        {
            clientReceiveThread = new Thread(new ThreadStart(ListenForData));
            clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();

            s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }
        catch (Exception e)
        {
            Debug.Log("On client connect exception " + e);
        }
    }

    private void ListenForData()
    {
        try
        {
            client = new UdpClient(receivingPort);
            Byte[] bytes = new Byte[1024];
            while (loop)
            {
                byte[] incommingData = client.Receive(ref anyIP);
                string serverMessage = Encoding.ASCII.GetString(incommingData);
                if (!updateValues)
                {
                    if (serverMessage.Contains("robot"))
                    {
                        robotData = JsonConvert.DeserializeObject<RobotData>(serverMessage);
                        updateValues = true;
                    }
                    else if(serverMessage.Contains("telemetry"))
                    {
                        telemetryData = JsonConvert.DeserializeObject<TelemetryData>(serverMessage);
                        updateTelemetry = true;
                    }
                    else if (serverMessage.Contains("boxes"))
                    {
                        boundingData = JsonConvert.DeserializeObject<BoundingData>(serverMessage);
                        updateBoxes = true;
                    }
                    else if (serverMessage.Contains("cockpit-broadcast"))
                    {
                        virtualRobotPositionData = JsonConvert.DeserializeObject<VirtualRobotPositionData>(serverMessage);
                        updateRobot = true;
                        //Vector3 pos = new Vector3(virtualRobotPositionData.data.posX, virtualRobotPositionData.data.posY, virtualRobotPositionData.data.posZ);
                        //Debug.Log("RobotPosition: " + pos);
                        Debug.Log(serverMessage);
                    }
                }
            }
            if (client != null)
            {
                client.Close();
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }

    void UpdateTelemetry()
    {
        if (!updateTelemetry) return;
        stats.AppendValue(telemetryData.data.rsrp, ChartType.RSRP);
        stats.AppendValue(telemetryData.data.rsrq, ChartType.RSRQ);
        stats.AppendValue(telemetryData.data.sinr, ChartType.SINR);
        stats.AppendValue(telemetryData.data.latency, ChartType.LAT);
        updateTelemetry = false;
    }

    void UpdateBoxes()
    {
        if (!updateBoxes) return;
        controller.ResetBBs();
        foreach (BoundingData.Data.Box box in boundingData.data.boxes)
        {
            controller.SetBB(box.type, box.coords);
        }
        updateBoxes = false;
    }

    void UpdateRobot()
    {
        if (!updateRobot) return;
        Vector3 pos = new Vector3(virtualRobotPositionData.data.posX, virtualRobotPositionData.data.posY, virtualRobotPositionData.data.posZ);
        Quaternion rot = new Quaternion(virtualRobotPositionData.data.rotX, virtualRobotPositionData.data.rotY, virtualRobotPositionData.data.rotZ, virtualRobotPositionData.data.rotW);
        virtualRobot.transform.localPosition = pos;
        virtualRobot.transform.localRotation = rot;
    }

    public void UpdateValues(RobotData m)
    {
        if (m == null) return;
        if (m.data == null) return;

        otherGPS.latitude = m.data.gps[0];
        otherGPS.longitude = m.data.gps[1];

        targetRotation.y = (float)m.data.gps[2];
        targetRotation.w = (float)m.data.gps[3];
        targetRotation*=Quaternion.Euler(0f, (float)orientationOffset, 0f);

        padre.transform.localRotation = Quaternion.Slerp(padre.transform.localRotation, targetRotation, Time.deltaTime * rotationSpeed);

        ui.ChangeSpeed(m.data.speed);
        //ui.ChangeBattery(m.data.battery);
        ui.SetLeftLidar(m.data.lidar[1]);
        ui.SetRightLidar(m.data.lidar[0]);
        if (m.data.time.Length > 0)
        {
            ui.ChangeLapTime(m.data.time[m.data.time.Length - 1].ToString());
        }
        else
        {
            ui.RemoveLapTime();
        }

        realRobotPositionData.data.gps = m.data.gps;
        string inputMessage = JsonConvert.SerializeObject(realRobotPositionData);
        SendNetworkMessage(inputMessage);
    }

    private void OnApplicationQuit()
    {
        loop = false;
        clientReceiveThread.Abort();
        s.Close();
    }
}