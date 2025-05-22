
# Project description

The FIDAL-VLC: 5G Robot Race project is a Beyond 5G (B5G) testbed that demonstrates the potential of immersive remote robot driving by combining real-time control, digital twins, and immersive user interfaces over a private 5G network. The system is validated through a use case involving a competitive robot race between a physical and a virtual robot, both remotely operated from immersive cockpits. The physical mobile robot is operated at UPV’s outdoor velodrome, while the cockpits are located indoors at the Immersive laboratory of iTEAM-UPV, showcasing full separation between the operator and the physical environment.

The physical robot, equipped with sensors and cameras for remote control, sends telemetry data to the cockpits to create a digital twin of the race. The virtual robot, using this digital twin, replicates the physical robot's behaviour for simulated control. Each cockpit includes a racing seat, steering wheel and pedals, a triple-screen setup, a haptic vest, and a virtual reality headset. Users receive either a classic or VR-based view from the robot's perspective, as if they were inside it, via a real-time 360º video stream. The video is overlaid with augmented reality objects, consisting of power-up items that act as bonuses or penalties affecting the robot’s dynamics. The effects of these AR obstacles are felt through the haptic vest.

## Digital Twin-based User Interface

The UI consists of four fundamental components that enable the gamification of the teleoperation experience: real-time video streaming, AR object overlay, haptic feedback, and the integration of a digital twin. These components work together to facilitate more intuitive and efficient driving, offering a comprehensive view of the environment, providing relevant data, and allowing sensory responses to events occurring during the race. The next figure shows a representation of the UI integrating the mentioned components.

![image](https://github.com/user-attachments/assets/62653ba0-4d49-4f27-a6fb-2a132d2dd6d1)

The Digital Twin-based UI also allows the simulation of the teleoperation, what we call Virtual Mode. In contrast to the standard Physical Mode of teleoperation, the Digital Twin substitutes the video streaming as the primary feedback to the user. This allows the training of the users before driving the mobile robot, gaining basic skills for safe operation of the robot.

## Video Streaming

Video streaming is a core component of the UI, as it is crucial for the driver to have a real-time view of the environment where the mobile robot is operating in. Both the user and the robot are physically located in different places, making a reliable video feed essential for safe teleoperation. The video stream can be adjusted based on multiple parameters, including image quality (3840x1920, 2560x1280, 1920x960, 1280x640), transmission bitrate (3 to 50 Mbps depending on the quality), refresh rate (24 to 30 fps depending on the quality), encoding formats (H.264 or H.265), and real-time streaming protocols (RTSP or RTMP).

![image](https://github.com/user-attachments/assets/ab9942f3-de6d-4cc6-bdd5-e280ace4c5ef)

## AR Objects

Augmented Reality (AR) objects in the UI have been implemented based on GPS location to enhance the immersive user experience in remote driving. These virtual objects are strategically placed at different points around the velodrome circuit where the robot operates, some acting as speed boosters or reducers during the race. For instance, when the robot passes through certain objects, it may increase or decrease its speed or even stop completely if it approaches an obstacle that poses a potential hazard.  All the AR object are only displayed when the robot is within the range of 20 meters, when they gradually become visible to the user. If the distance exceeds this threshold, the object disappears completely. Furthermore, every interaction between the robot and the virtual objects is linked to haptic feedback in the vest worn by the driver, enhancing immersion by adding a physical dimension to events occurring in both the virtual and real worlds.

### Virtual Traffic Light

To signal the start and end of the race between the two robots, a virtual traffic light has been implemented in the user interface. The logic behind its operation is as follows: 
1. The robots are positioned at the starting line. The UI restricts the user from moving the robot until the race start signal is given, ensuring a more controlled and realistic driving experience.  
2. When the user presses the "P" key, a virtual “Start” object appears in the UI, triggering a traffic light sequence. The traffic light cycles between red and green for a total of 8 seconds. Once this countdown ends, a "Go" signal appears in the UI, indicating that the user can start accelerating the robot. Simultaneously, the user receives haptic feedback through the vest, reinforcing the immersive driving experience.  
3. Before completing a full lap around the velodrome, the virtual traffic light appears again, signaling the end of the race.  
This entire sequence ensures a synchronized experience between the virtual environment (objects) and the physical environment (users and robots). The following figure illustrates the virtual traffic light and "Go" signal in the user interface.

<img width="434" alt="image" src="https://github.com/user-attachments/assets/7da03459-2815-4c85-8755-41805075bebd" />

### Virtual Boosters and Reducers

Randomized Boxes:

A total of four virtual boxes are distributed per robot, placed at the beginning and end of the two curves of the velodrome. When the robot collides with a virtual box, three synchronized actions occur in real-time:  
1.	A visual explosion effect is triggered, making the box disappear. This effect is programmed using a particle animation, adding dynamism and realism to the scene. 
2.	The robot's speed is increased or decreased by 20% of its current speed (6 km/h) based on a randomized logic. A virtual signal appears in the UI to inform the user of the effect, displaying either “Faster Speed” or “Lower Speed.”  
3.	The user experiences haptic feedback through the vest and audio stimuli through the VR headset.  
The following figure illustrates the virtual box and the signals that appear in the user interface as well as a representation of the updated speed in the speedometer. 

<img width="431" alt="image" src="https://github.com/user-attachments/assets/fc72f5de-a0ab-4d9b-99b1-3962da94ade9" />

Coins, Slow Zones and Tires:

A total of 12 coins are placed in the scenario, specifically a set of 6 coins in each curve. In contrast to the boxes, coins always act as boosters to the robot speed, but their impact is lower. Each coin increases speed by 0,1 km/h up to a maximum of 10 coins (i.e., 1 km/h) in comparison to the standard speed (6 km/h). A coin counter is displayed to the user in the top left corner of the User Interface.
On the other hand, Slow Zones and Tires always act as reducers of the robot speed, both instantly (by reducing the robot speed to a 40% for the time it is on the slow zone) and permanently (by subtracting 1 coin for every second that the robot is on the slow zone or 3 coins every time it collides with a tire). This logic forces the user to follow a particular path to move at maximum speed. 
The following figure shows examples of coin, tire and slow zone and their effect to the robot’s speed.

<img width="433" alt="image" src="https://github.com/user-attachments/assets/85e9c951-2683-43f5-b465-87146c71acfa" />

### Virtual Curve Signalization

This virtual sign serves as a preventive driving aid by alerting the driver when the robot is approaching a curve. The signal remains visible until the robot has completely passed through the turn. The following figure displays the virtual curve warning in the user interface.  

![image](https://github.com/user-attachments/assets/cfa7c40f-97e8-400c-b34a-9fa8f9f7d3bb)

### Virtual Hands and Steering Wheel

These virtual objects enhance the immersive user experience by providing real-time feedback from the physical world into the virtual environment. The virtual hands provide a reference in the user interface by mirroring the user’s physical hand movements in real-time. For instance, when the user moves their hands in the real world, they will see the same movements replicated in the virtual environment with precise positioning. This feature is particularly useful when the user grips and turns the steering wheel to control the robot remotely. On the other hand, the virtual steering wheel allows the user to visualize the exact rotation angle of the physical wheel in real-time. The physical steering wheel’s rotation is accurately reflected in the virtual interface, with added interpolation to ensure smooth and realistic movements. Additionally, pressing the “L2” and “R2” buttons on the wheel centers all virtual objects in the scene and aligns the distance and angle of the steering wheel with the user’s hands.  
The following figure illustrates the virtual hands and steering wheel in the user interface.

![image](https://github.com/user-attachments/assets/583622ab-69e4-46be-9cdc-c231d8d0dcce)

## Haptic Feedback

The haptic vest enables the configuration of various feedback patterns for the user, leveraging its 40 vibration actuators evenly distributed across the front and back. These actuators can generate complex vibration patterns that simulate different types of physical interactions, enhancing the immersive experience in remote driving. The following figure illustrates a vibration pattern configured in the haptic vest for one of the virtual objects. 

<img width="323" alt="image" src="https://github.com/user-attachments/assets/53825da1-0de8-4341-ba69-90e7f25cf2ed" />

This specific pattern consists of a randomly moving vibration path that oscillates up and down with varying intensity. The number inside each circle represents the activation sequence of the vibrations.  Regarding vibration intensity, it can be adjusted within a range of 0 to 100. The threshold for perceiving a sensation varies by user, but generally, a value of around 20 is sufficient to be felt on the body.  

## Digital Twin 3D Model

The Digital Twin integrated into the User Interface (UI) provides a virtual representation of the velodrome and the robots. It displays the real-time position and orientation of the robots, extracted from the GPS and IMU sensors embedded in the robots, which allows their interaction with the AR objects previously described. In the standard physical mode of the teleoperation, the Digital Twin is represented in the form of a mini-map that displays the velodrome and UPV campus where the robots operate remotely.

![image](https://github.com/user-attachments/assets/bd10a6e6-6d0f-4905-8e87-90c1fb08b7cd)

Additionally, in the Virtual Mode, that simulates the teleoperation instead of actually driving the physical robot, the background representing the UPV campus can be switched to a completely virtual scenario designed from scratch. This scenario features virtual buildings and stadium stands for enhanced gamification experience.

![image](https://github.com/user-attachments/assets/c0d8d0b5-5933-4eb0-afef-011b1ebf685f)

In both modes, the digital twin is responsible for reading and displaying various real-time robot status data within the user interface. Some of the key parameters include:  
1. Robot Speed and Collision Alerts: The user can monitor the robot's speed in km/h, with a maximum configured speed of 7.5 km/h. A preventive “Collision Warning” alert is displayed when the robot is at risk of colliding with nearby objects. The threshold for this warning is set at a distance of 1.5 meters from an obstacle, based on real-time data from the robot’s integrated LiDAR sensor. The following figure shows the speedometer and "Collision Warning" alert as they appear in the user interface.  

<img width="330" alt="image" src="https://github.com/user-attachments/assets/445ea732-06dd-4639-b4b3-b833918e5af6" />

2. Lap Time Tracking: The total lap time recorded by the robot as it completes a circuit around the velodrome. This information is shown to the user at the moment the robot crosses the finish line. The following figure presents the last lap time asset in the user interface.  

![image](https://github.com/user-attachments/assets/86478e89-6e88-47f1-957d-88886bfbb765)

## AI analytics for person detection

An AI-driven application based on YOLO (You Only Look Once) for real-time person detection was implemented in the FIDAL-VLC testbed. YOLO is one of the most widely used algorithms for real-time object detection. It is known for its fast inference times, ability to understand the global context and high level of accuracy. YOLOv9 has been chosen due to its innovative approach to overcoming information loss challenges inherent in deep neural networks. YOLOv9 is also more efficient than previous versions, offering higher accuracy and faster inference times, making it ideal for real-time applications.

Once the model is trained, the YOLO algorithm processes a real-time video stream as the input source, while the output is the processed video by drawing a bounding box around a person. More in detail, once the algorithm has been trained, the person detection process will be as follows: 
1.	YOLO receives the input video stream from the robot via Gstreamer . 
2.	YOLO processes the video frame by frame, dividing the input image into an S×S grid. If the centre of an object falls within a grid, that grid is responsible for detecting such object. 
3.	YOLO processes the detection of each object. Each grid predicts B bounding boxes and their confidence scores. The bounding box is defined by several parameters: (I) the position of the bounding box centre relative to the grid; (II) the width and height of the bounding box relative to the entire image; and (III) the confidence score of an object. 
4.	YOLO calculates the probabilities of each bounding box containing objects of each class by multiplying the confidence score of each box with the conditional class probabilities. Threshold filtering and non-maximum suppression are then applied to obtain the final detection 
5.	Finally, YOLO sends to Unity the bounding box of each detected object, including messages with relevant information about each detection (type of object, number of detected objects, probability of detection, among others).

# Installation requirements

- Windows 11
- Unity 2021.3.4f1. Packages required: OpenXR (com.unity.xr.openxr), XRHands (com.unity.xr.hands), XR Interaction Toolkit (com.unity.xr.interaction.toolkit), bHaptics plugin, mrayGStreamerUnity plugin (https://github.com/mrayy/mrayGStreamerUnity)
- Visual Studio 2022 17.6.4 for Gstreamer plugin dependencies. Packages required: Desktop development with C++, Universal Windows Platform Development, Game Development with Unity, Game Development with C++
- GStreamer 1.22.6 complete
- Trustmaster T150 drivers
- bHaptics Player
- Meta Quest Link. Configuration required: ![image](https://github.com/user-attachments/assets/783d8e29-57a9-4fef-ac31-bc92dc63d18c)

# Unity structure

The Assets folder contains the following elements:
- Animated racing arrows: Unity Asset store package with assets that implement animated arrows to be placed in the circuit curves and indicate the player the circuit path.
- AudioEffects: Contains audio effects for different circunstances ingame such as coin catching or bumping into the tires.
- BEDRILL: Contains the models used for the tyres and stadium grades (virtual mode only).
- Bhaptics: Contains the plugin for bHaptis devices.
- Bitsplash: Contains the plugin Graph and Chart, used for the RSRP/RSRQ/SINR/latency graphs received via MQTT from the Fivecomm modem.
- GStreamerUnity: Contains the scripts for the GStreamer plugin
- HC: Contains the scripts for the AR boosters and reducers
- ImGui: Panel with graphical information about signal quality statistics activated with key S.
- JMO Assets: Contains the particles for the visual effect of the AR boosters and reducers.
- LiquidFire Package 4 - BSH games: Contains the models used for the virtual coins.
- MGAssets: Contains the plugin of the velocimeter and back mirror.
- Plugins: GStreamer DLLs.
- Powerup FX: Unity Asset Store package for the power up boxes placed in the circuit.
- Prefabs: Contains several assets with the proper configuration to be placed in the scene such as the virtual environment, the coins or the tires with the necessary scripts.
- RacingSignBoardsPack: Unity Asset Store package with Racing sign boards that are placed in the circuit.
- Resources: Contains the models, textures and images used by the application that do not belong to an external plugin, such as the velodrome 3D model, mini-map, skybox, sprites (logos, battery).
- Samples: Contains the XR plugins.
- Scenes: Contains the main scene, as well as other test scenes of the GStreamer plugin that have not been modified.
- Scripts: Contains the scripts created for the application, explained in the following section.
- SimplePoly City - Low Poly Assets: Contains the assets used for the virtual city/stadium (virtual mode only).
- SimpleWebBrowser: Unity Asset Store package including a web browser that was inteded to be used in game, but was substituted by Vuplex premium solution.
- Songs: Contains audio clips that are played as background music.
- TextMesh Pro: Unity package to format text strings including fonts or emojis.
- Traffic_light: Contains the resources needed for the virtual traffic light.
- UnityCam: Contains scripts used by GStreamer to convert from video to texture.
- Vuplex: NOT UPLOADED TO GITHUB, SINCE IT IS A PREMIUM NOT-FREE PACKAGE WITH RESTRICTED LICENSE: Contains the 3D Web View plugin for integrating the Robotnik HMI in the UI.
- XR: Contains XR configuration files that should not be modified.
- XRI: Contains XR configuration files that should not be modified.
- 1.fbx: Mesh created in Blender to complete the velodrome 3D object (size 1).
- 2.fbx: Mesh created in Blender to complete the velodrome 3D object (size 2).
- Canvas.prefab: Canvas including a 2D browser from the SimpleWebBrowser folder and an activation button.
- System.Runtime.CompilerServices.Unsafe.dll: File including Warning detected at compilation time in DLL format.
- System.Runtime.CompilerServices.Unsafe.xml: File including Warning detected at compilation time in XML format.
- coin.png: Png coin image used in the coin counter from the main Canvas.
- ips.txt: Configuration file for connecting to the proxy and receiving the video stream. Every time a build is created, make sure that this file is placed in the folder UnityTests_Data.

The main scene contains the following objects:
- EventSystem: Configures the UI, should not be modified
- bhaptics: Contains bHaptics SDK, allowing access to its functionaliities from other scripts and objects in the scene
- XR Origin Hands (XR Rig): XR camera and hands
- Robot 2: Physical robot
- Cube: Contains the GStreamer scripts
- Controller: Contains the core scripts of the logic
- Directional Light: Illumination parameters, should not be modified
- Back Camera: Camera used by the back mirror
- Virtual Camera Overlay: ?
- Robot Control: Contains the UI Canvas
- Wheel: Represents the driving wheel in the scene
- Padre: Contains all the 3D objects in the scene, including the AR objects, the UPV campus, the Digital Twin of the Velodrome, and the virtual robot
- Recenter: Reference object that marks the center of the scene

The role of the main scripts is:
- ProxyConnection.cs: Manages the connection between Unity and the Proxy through the Connect() method, reads the inputs from the wheel and pedals using Input.GetAxis(), sends the inputs to the proxy through the SendNetworkMessage() method, updates the speed of the robot via ChangeRobotSpeed(), stores the data received from the proxy in JSON format using the class Message, sends the received data to UIManager.cs and GPS.cs through the UpdateValues() method
- UIManager.cs: Enables or disables functionalities of the UI according to the data received from the proxy (e.g. in ShowMessageBox()), recenters the camera using Input.GetAxis() and enables the back mirror using PlaceSteeringWheel()
- GPS.cs: Transforms from latitude and longitude to Unity coordinates in meters
- Utiliities.cs: Reads the ips.txt file to update the Unity variables, manages the change between virtual models (virtual mode only)
- Controlles.cs: Enables the visualization in the 3 displays, converts the Gstreamer texture to a render texture and applies it to the Skybox
- RecenterOrigin.cs: Centers the camera when pressing Space key to align with the UI and the Skybox
- TrafficLight.cs: Manages the lights at the beginning of the race, calls the bHaptics plugin
- Position.cs: Positions the 3D objects contained in Obj_AR based on the GPS data received
- Boxes.cs: Implements the logic and visual effects of the boosters and reducers, including update of velocity, visual effect, haptics and sound
- Coins.cs: Implements the logic and visual effects of the coins, including update of velocity, visual effect, update of the UI, haptics and sound
- Tires.cs: Implements the logic and visual effects of the tires, including update of velocity, haptics and sound
- SlowZone.cs: Implements the logic and visual effects of the slow zones, including update of velocity, update of the UI and haptics
- RobotEngineSound.cs: Reproduces motor sounds adjusting the volume based on velocity
- VirtualRobotController.cs: Links the input of the wheel to the movement of the 3D object instead of communicating with the proxy and defines the boundaries based on collisions (virtual mode only)
- LapTimeCalculator.cs: Calculates the laptime of the virtual robot and communicates it to the UIManager (virtual mode only)
- Stats.cs: Manages the representation of the RSRP/RSRQ/SINR/latency graphs

# 5G Robot Race Setup Guide

## Powering On

1. **Power on robot (Green key)**
   - Turns on modem

2. **Power on robot PC (Green button)**
   - Launches ROS and allows control from PS4 controller

3. **Power on cockpit PC**
   - Password: `MCGiteam2021`
   - Configure static IP to access the MEC

4. **Power on camera**
   - Hold down the home button

## Checking Connections

1. **Check 5G connection on the modem**
   - WiFi: `shl00-211222aa_5G`
   - Password: `R0b0tn1K`
   - Access: `192.168.0.1`
     - Username: `admin`
     - Password: `R0b0tn1K`
   - Check port forwarding to the camera and the robot PC

2. **Check access to the robot**
   - `ssh robot@(robot_IP)`
   - Password: `R0b0tn1K`
   - Note: `robot_IP` is `192.168.0.200` locally or `modem_IP` via port forwarding

3. **Check connection of the 360 camera**
   - For local tests, use WiFi
   - For remote tests, check Ethernet connection
     - Reconnect USB-C cable if it shows as unconnected

## Launching the System

1. **Launch RTSP Stream**
   - Use the Live App on the 360 camera

2. **Open the Unity project folder on the cockpit PC**
   - `C:\Users\XR\Unity\5g_robot_race_newvel\5g_robot_race_newvel`
  
3. **Build the project (or use an existent build)**
   - Open the project from Unity Hub usign version 2022.3
   - Click file -> Build Settings -> Save into `C:\Users\XR\Unity\5g_robot_race_newvel\5g_robot_race_newvel\Builds`

3. **Modify configuration files**
   - `Unity\UnityTests\Assets\ips.txt` with `camera_IP`
   - `proxy\unity2_robot.py` with `robot_IP`
     - Note: There are two entries, one for each summit, but you can use just one

4. **Run the proxy**
   - `python proxy\unity2_robot.py`

5. **Run Unity**
   - `Builds\UnityTests.exe`
