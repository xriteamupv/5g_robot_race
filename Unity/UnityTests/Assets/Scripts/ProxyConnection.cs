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

    private bool isTrafficEnabled = false;
    public class RobotData
    {
        public string header;
        public class Data
        {
            public float battery;
            public float[] gps;
            public float speed;
            public float[] time;
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
            public float lat;
        }
        public Data data;
    }

    public class BoundingData
    {
        public string header;
        public class Data
        {
            public float[] coords;
            public string type;
        }
        public Data data;
    }

    private RobotData robotData;
    private TelemetryData telemetryData;
    private ControlData controlData;
    private BoundingData boundingData;
    private bool updateValues;
    private bool loop;
    private bool isRobot1;

    public float rotationSpeed = 1.0f;
    private Quaternion targetRotation;
    public float robotSpeedMultiplier;

    public Controller controller;

    void Start()
    {
        //string d = "{\"battery\":\"5\", \"lidar\":\"[0,0]\", \"gps\":\"[39.479353,-0.336108,0]\", \"speed\":\"0\"}";
        string s = "{\"header\":\"robot\",\"data\":{\"battery\":5,\"lidar\":[0,0],\"gps\":[39.479353,-0.336108,0],\"speed\":0,\"time\":[39.479353,-0.336108,0]}}";
        //Debug.Log(s);
        //robotData = JsonConvert.DeserializeObject<RobotData>(s);
        //Debug.Log(robotData.data.time[0]);
        updateValues = true;
        loop = true;
        robotSpeedMultiplier = 0.0f;
        currentIP = new IPEndPoint(IPAddress.Parse(IP), receivingPort);

        if (IP != "") Connect();
        //UpdateValues(o);
        if(selectedRobot == "Robot 1") 
            isRobot1 = true;
        InvokeRepeating("WheelInput", 0.0f, 0.1f);

        float[] bbcoords = new float[] { 3070, 601, 3342, 1088 };
        string bbtype = "yellow";
        controller = GetComponent<Controller>();
        controller.SetBB(bbtype, bbcoords);

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
        //if (Input.GetAxis("Pedal") == 0.0f && Input.GetAxis("Wheel") == 0.0f) return;

        float pedalInput = (Input.GetAxis("Pedal") - 1.0f) * -robotSpeedMultiplier; //cambiar multiplicador para modificar velocidad
        //Debug.Log(pedalInput);
        float pedal2Input = (Input.GetAxis("Back") - 1.0f) * 1.0f;
        float wheelInput = Input.GetAxis("Wheel") * -3.0f;

        ui.ChangeWheelRotation(-Input.GetAxis("Wheel") * 360.0f);

        controlData.data.linear.x = pedalInput;
        controlData.data.angular.z = wheelInput;

        string inputMessage = JsonConvert.SerializeObject(controlData); ;
        SendNetworkMessage(inputMessage);
        Debug.Log(inputMessage);
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
                    } 
                    else if(serverMessage.Contains("telemetry"))
                    {
                        telemetryData = JsonConvert.DeserializeObject<TelemetryData>(serverMessage);
                    }
                    else if(serverMessage.Contains("boxes"))
                    {
                        boundingData = JsonConvert.DeserializeObject<BoundingData>(serverMessage);
                    }
                    updateValues = true;
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

    public void UpdateValues(RobotData m)
    {
        if (m == null) return;
        if (m.data == null) return;

        //gps.latitude = m.gps1[0];
        //Debug.Log(m.gps1[0]);
        //gps.longitude = m.gps1[1];

        //double roll = (m.gps1[2] * Mathf.Rad2Deg);
        //double pitch = (m.gps1[3] * Mathf.Rad2Deg); 
        //double yaw = orientationOffset + (m.gps1[4] * Mathf.Rad2Deg);
        //Debug.Log(yaw);

        //gps.transform.rotation = Quaternion.Euler(0.0f, -(float)yaw, 0.0f);
        //gps.transform.rotation = Quaternion.Euler((float)roll, -(float)yaw, (float)pitch);


        otherGPS.latitude = m.data.gps[0];
        otherGPS.longitude = m.data.gps[1];

        //brujula
        //double roll = (m.gps2[2] * Mathf.Rad2Deg);
        //double pitch = (m.gps2[3] * Mathf.Rad2Deg);
        //double yaw = orientationOffset + (m.gps2[4] * Mathf.Rad2Deg);
        //targetRotation = Quaternion.Euler(0.0f, (float)yaw, 0.0f);
        
        targetRotation.y = (float)m.data.gps[2];
        targetRotation.w = (float)m.data.gps[3];
        targetRotation*=Quaternion.Euler(0f, (float)orientationOffset, 0f);
        //Debug.Log(targetRotation);

        padre.transform.localRotation = Quaternion.Slerp(padre.transform.localRotation, targetRotation, Time.deltaTime * rotationSpeed);
        //float newYaw = (float)yaw - padre.transform.rotation.eulerAngles.y;
        //padre.transform.RotateAround(Vector3.zero, Vector3.up, newYaw);

        ui.ChangeSpeed(m.data.speed); 
        //ui.ChangeBattery(m.battery1);
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
    }

    private void OnApplicationQuit()
    {
        loop = false;
        clientReceiveThread.Abort();
        s.Close();
    }
}