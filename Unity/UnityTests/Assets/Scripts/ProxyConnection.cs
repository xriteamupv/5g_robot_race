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

public class ProxyConnection : MonoBehaviour
{
    public string IP;
    public int port;
    private TcpClient socketConnection;
    private Thread clientReceiveThread;

    public GPS gps;
    public GPS otherGPS;
    public UIManager ui;
    public float orientationOffset;
    public string selectedRobot;

    public class Message
    {
        public int battery1;
        public int[] lidar1;
        public double[] gps1;
        public float speed1;

        public int battery2;
        public int[] lidar2;
        public double[] gps2;
        public float speed2;
    }

    private Message storedMessage;
    private bool updateValues;
    private bool loop;
    private bool isRobot1;


    void Start()
    {
        //string s = "{\"battery1\":5, \"lidar1\":[0,0], \"gps1\":[39.479353,-0.336108,0], \"speed1\":0, \"battery2\":0, \"lidar2\":[0,1], \"gps2\":[39.479353,-0.336108,-0.336108], \"speed2\":0}";
        //var o = JsonUtility.FromJson<Message>(s);
        updateValues = true;
        loop = true;

        if (IP != "") Connect();
        //UpdateValues(o);
        if(selectedRobot == "Robot 1") 
            isRobot1 = true;
        InvokeRepeating("WheelInput", 0.0f, 0.1f);
    }

    private void Update()
    {
        if (updateValues)
        {
            UpdateValues(storedMessage);
            updateValues = false;
        }

    }

    private void WheelInput()
    {
        //if (Input.GetAxis("Pedal") == 0.0f && Input.GetAxis("Wheel") == 0.0f) return;

        float pedalInput = (Input.GetAxis("Pedal") - 1.0f) * -1.0f;
        float pedal2Input = (Input.GetAxis("Back") - 1.0f) * 1.0f;
        float wheelInput = Input.GetAxis("Wheel") * -2.0f;
        float lev = Input.GetAxis("lev");
        string pedal = pedalInput.ToString().Replace(',', '.');
        string back = pedal2Input.ToString().Replace(',', '.');
        string wheel = wheelInput.ToString().Replace(',', '.');

        string inputMessage;
        if(isRobot1)
        {
            //Debug.Log(pedal);
            if (back != "0")
            {
                inputMessage = "{\"robot1\":[0," + wheel + "]}";
            }
            else
            {
                if (lev == 1)
                {
                    inputMessage = "{\"robot1\":[-" + pedal + "," + wheel + "]}";
                }
                else
                {
                    inputMessage = "{\"robot1\":[" + pedal + "," + wheel + "]}";
                }
            }
        }
        else
        {
            //Debug.Log(pedal);
            if (back != "0")
            {
                inputMessage = "{\"robot2\":[0," + wheel + "]}";
            }
            else
            {
                if (lev == 1)
                {
                    inputMessage = "{\"robot2\":[-" + pedal + "," + wheel + "]}";
                }
                else
                {
                    inputMessage = "{\"robot2\":[" + pedal + "," + wheel + "]}";
                }
            }
        }
        Debug.Log(inputMessage);
        SendNetworkMessage(inputMessage);
    }

    private void SendNetworkMessage(string message)
    {
        if (socketConnection == null)
        {
            return;
        }
        try
        {
            // Get a stream object for writing. 			
            NetworkStream stream = socketConnection.GetStream();
            if (stream.CanWrite)
            {
                // Convert string message to byte array.
                byte[] clientMessageAsByteArray = Encoding.UTF8.GetBytes(message);
                // Write byte array to socketConnection stream.                 
                stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
            }
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
            socketConnection = new TcpClient(IP, port);
            Byte[] bytes = new Byte[1024];
            while (loop)
            {
                // Get a stream object for reading 				
                using (NetworkStream stream = socketConnection.GetStream())
                {
                    int length;
                    // Read incomming stream into byte arrary. 						
                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        var incommingData = new byte[length];
                        Array.Copy(bytes, 0, incommingData, 0, length);
                        // Convert byte array to string message. 							
                        string serverMessage = Encoding.ASCII.GetString(incommingData);
                        serverMessage = serverMessage.Replace('\'', '\"');
                        Debug.Log(serverMessage);
                        if (!updateValues)
                        {
                            storedMessage = JsonUtility.FromJson<Message>(serverMessage);
                            updateValues = true;
                        }
                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }

    public void UpdateValues(Message m)
    {
        if (m == null) return;

        gps.latitude = m.gps1[0];
        gps.longitude = m.gps1[1];

        double degrees = orientationOffset + (m.gps1[2] * Mathf.Rad2Deg);
        gps.transform.rotation = Quaternion.Euler(0.0f, -(float)degrees, 0.0f);

        otherGPS.latitude = m.gps2[0];
        otherGPS.longitude = m.gps2[1];

        degrees = orientationOffset + (m.gps2[2] * Mathf.Rad2Deg);
        otherGPS.transform.rotation = Quaternion.Euler(0.0f, -(float)degrees, 0.0f);

        if (isRobot1)
        {
            ui.ChangeSpeed(m.speed1);
            ui.ChangeBattery(m.battery1);
            ui.SetLeftLidar(m.lidar1[1]);
            ui.SetRightLidar(m.lidar1[0]);
        }
        else
        {
            ui.ChangeSpeed(m.speed2);
            ui.ChangeBattery(m.battery2);
            ui.SetLeftLidar(m.lidar2[1]);
            ui.SetRightLidar(m.lidar2[0]);
        }
    }

    private void OnApplicationQuit()
    {
        loop = false;
        if (socketConnection != null) socketConnection.Close();
    }
}