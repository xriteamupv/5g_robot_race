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

    public Position gps, otherGPS;
    //public GPS otherGPS;
    public GameObject padre; //objeto a rotar
    public GameObject objetoAR; //objeto a posicionar
    public UIManager ui;
    public float orientationOffset;
    public string selectedRobot;

    public TrafficLight trafficSign;

    private bool isTrafficEnabled = false;

    public class Message
    {
        public int battery1;
        public int[] lidar1;
        public double[] gps1;
        public float speed1;
        public string[] time1;

        public int battery2;
        public int[] lidar2;
        public double[] gps2;
        public float speed2;
        public string[] time2;
    }

    private Message storedMessage;
    private bool updateValues;
    private bool loop;
    private bool isRobot1;

    public float rotationSpeed = 1.0f;
    private Quaternion targetRotation;
    public float robotSpeedMultiplier;

    void Start()
    {
        //string s = "{\"battery1\":5, \"lidar1\":[0,0], \"gps1\":[39.479353,-0.336108,0], \"speed1\":0, \"battery2\":0, \"lidar2\":[0,1], \"gps2\":[39.479353,-0.336108,-0.336108], \"speed2\":0}";
        //var o = JsonUtility.FromJson<Message>(s);
        updateValues = true;
        loop = true;
        robotSpeedMultiplier = 0.0f;

        if (IP != "") Connect();
        //UpdateValues(o);
        if(selectedRobot == "Robot 1") 
            isRobot1 = true;
        InvokeRepeating("WheelInput", 0.0f, 0.1f);

        
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

    private void Update()
    {
        if (updateValues)
        {
            UpdateValues(storedMessage);
            updateValues = false;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            SendNetworkMessage("timer");
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
        float lev = Input.GetAxis("lev");
        string pedal = pedalInput.ToString().Replace(',', '.');
        string back = pedal2Input.ToString().Replace(',', '.');
        string wheel = wheelInput.ToString().Replace(',', '.');

        ui.ChangeWheelRotation(-Input.GetAxis("Wheel") * 360.0f);

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
        //Debug.Log(inputMessage);
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
                        //Debug.Log(serverMessage);
                        if (!updateValues)
                        {
                            if (serverMessage == "start")
                            {
                                isTrafficEnabled = true;
                            }
                            else
                            {
                                storedMessage = JsonUtility.FromJson<Message>(serverMessage);
                                Debug.Log(serverMessage);
                                updateValues = true;
                            }
                        }
                    }
                }
            }
            if (socketConnection != null)
            {
                socketConnection.Close();
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

        //gps.latitude = m.gps1[0];
        //Debug.Log(m.gps1[0]);
        //gps.longitude = m.gps1[1];

        //double roll = (m.gps1[2] * Mathf.Rad2Deg);
        //double pitch = (m.gps1[3] * Mathf.Rad2Deg); 
        //double yaw = orientationOffset + (m.gps1[4] * Mathf.Rad2Deg);
        //Debug.Log(yaw);
        
        //gps.transform.rotation = Quaternion.Euler(0.0f, -(float)yaw, 0.0f);
        //gps.transform.rotation = Quaternion.Euler((float)roll, -(float)yaw, (float)pitch);
        

        otherGPS.latitude = m.gps2[0];
        otherGPS.longitude = m.gps2[1];

        //brujula
        //double roll = (m.gps2[2] * Mathf.Rad2Deg);
        //double pitch = (m.gps2[3] * Mathf.Rad2Deg);
        //double yaw = orientationOffset + (m.gps2[4] * Mathf.Rad2Deg);
        //targetRotation = Quaternion.Euler(0.0f, (float)yaw, 0.0f);
        
        targetRotation.y = (float)m.gps2[2];
        targetRotation.w = (float)m.gps2[3];
        targetRotation*=Quaternion.Euler(0f, (float)orientationOffset, 0f);
        //Debug.Log(targetRotation);

        padre.transform.localRotation = Quaternion.Slerp(padre.transform.localRotation, targetRotation, Time.deltaTime * rotationSpeed);
        //float newYaw = (float)yaw - padre.transform.rotation.eulerAngles.y;
        //padre.transform.RotateAround(Vector3.zero, Vector3.up, newYaw);

        if (isRobot1)
        {
            ui.ChangeSpeed(m.speed1); 
            //ui.ChangeBattery(m.battery1);
            ui.SetLeftLidar(m.lidar1[1]);
            ui.SetRightLidar(m.lidar1[0]);
            if (m.time1.Length > 0)
            {
                ui.ChangeLapTime(m.time1[m.time1.Length - 1]);
            }
        }
        else
        {
            ui.ChangeSpeed(m.speed2); 
            //ui.ChangeBattery(m.battery2);
            ui.SetLeftLidar(m.lidar2[1]);
            ui.SetRightLidar(m.lidar2[0]);
            if (m.time2.Length > 0)
            {
                ui.ChangeLapTime(m.time2[m.time2.Length - 1]);
            }
        }
    }

    private void OnApplicationQuit()
    {
        loop = false;
    }
}