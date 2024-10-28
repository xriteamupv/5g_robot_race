# /* =====================================================
# * Copyright (c) 2022, Fivecomm - 5G COMMUNICATIONS FOR FUTURE INDUSTRY
#       VERTICALS S.L. All rights reserved.
# * unity_robot.py
# * Description: Python server for controlling Robotnik AGV from Unity environment
# * Author:  {<miriam.ortiz@fivecomm.eu>; <pablo.trelis@fivecomm.eu>}
# * Date:  20/10/2023
# * Version:  1.0
# * =================================================== */

import roslibpy
import time
import threading
from datetime import datetime
import socket
import multiprocessing
import os
import sys
import json
from newThread import newThread


class UnityRobotProxy:
    def start(self):
        # self.IP1 = os.environ["ROBOT1"]
        # self.IP2 = os.environ["ROBOT2"]
        self.IP1 = "10.45.21.103"
        self.IP2 = "192.168.0.200"

        # Unity ports
        self.PORT1 = 11997
        self.PORT2 = 11996

        self.start_timer = False

        # Initialization of variables
        self.unity_msg = {
            "battery1": 0,
            "lidar1": [0, 0],
            "gps1": [0, 0, 0, 0],
            "speed1": 0,
            "time1": [],
            "battery2": 0,
            "lidar2": [0, 0],
            "gps2": [0, 0, 0, 0],
            "speed2": 0,
            "time2": [],
        }
        self.unity_rd = {"robot": 0, "move": [0, 0]}
        self.move_robot = {"robot1": [0, 0], "robot2": [0, 0]}
        self.socket_client1 = None  # Unity client
        self.socket_client2 = None  # Unity client
        self.pub_freq = 0.2

        # Connection with unity
        self.th1 = newThread(target=self.connectUnity1, daemon=False)
        self.th1.start()
        self.th2 = newThread(target=self.connectUnity2, daemon=False)
        self.th2.start()
        # Parallel threads for sending data from robot to unity
        self.th3 = newThread(
            target=self.sendUnity1, args=(0.1, self.unity_msg), daemon=False
        )
        self.th3.start()
        self.th4 = newThread(
            target=self.sendUnity2,
            args=(0.1, self.unity_msg),
            daemon=False,
        )
        self.th4.start()

        self.p1 = self.robot_connection(self.IP1, "1")
        self.p2 = self.robot_connection(self.IP2, "2")
        
        while 0 == 0:
            try:
                if (
                    not self.p1.is_alive()
                    or not self.p2.is_alive()
                    or not self.th1.is_alive()
                    or not self.th2.is_alive()
                    or not self.th3.is_alive()
                    or not self.th4.is_alive()
                ):
                    print(
                        "p1-",
                        self.p1.is_alive(),
                        " p2-",
                        self.p2.is_alive(),
                        " th1-",
                        self.th1.is_alive(),
                        " th2-",
                        self.th2.is_alive(),
                        " th3-",
                        self.th3.is_alive(),
                        " th4-",
                        self.th4.is_alive(),
                    )
                if not self.p1.is_alive():
                    self.p1 = self.robot_connection(self.IP1, "1")
                if not self.p2.is_alive():
                    self.p2 = self.robot_connection(self.IP2, "2")
                if not self.th1.is_alive():
                    self.th3.kill()
                    self.socket_client1 = None
                    time.sleep(1)
                    self.th1 = newThread(target=self.connectUnity1, daemon=False)
                    self.th1.start()
                    self.th3 = newThread(
                        target=self.sendUnity1,
                        args=(0.1, self.unity_msg),
                        daemon=False,
                    )
                    self.th3.start()
                if not self.th2.is_alive():
                    self.th4.kill()
                    self.socket_client2 = None
                    time.sleep(1)
                    self.th2 = newThread(target=self.connectUnity2, daemon=False)
                    self.th2.start()
                    self.th4 = newThread(
                        target=self.sendUnity2,
                        args=(0.1, self.unity_msg),
                        daemon=False,
                    )
                    self.th4.start()
                time.sleep(1)
            except KeyboardInterrupt:
                None
                print("EXIT")
                self.p1.kill()
                self.p2.kill()
                self.th1.kill()
                self.th2.kill()
                self.th3.kill()
                self.th4.kill()
                os._exit(1)
                break
        

    def robot_connection(self, ip, robot):
        # p1 = multiprocessing.Process(target=self.main_robot, args=(ip, robot))
        p1 = newThread(target=self.main_robot, args=(ip, robot), daemon=False)
        p1.start()
        return p1

    # For controling Robotnik Summit XL
    def main_robot(self, IP, robot):
        # ROS Client definition.
        print("Connecting to ROS robot: ", IP)
        try:
            ROS_client = roslibpy.Ros(host=IP, port=9090)
            ROS_client.run()
        except:
            print("Error connecting to ROS robot: ", IP)
        else:
            print("Connected to ROS robot: ", IP)

            # ROS topics
            topic_robotrace = roslibpy.Topic(
                ROS_client,
                "fivecomm_robotrace/robotrace",
                "fivecomm_robotrace/robotrace",
            )
            topic_robotrace.subscribe(lambda msg: self.receive_robotrace(msg, robot))
            remote_driving = roslibpy.Topic(
                ROS_client, "/robot/web_teleop/cmd_vel_unsafe", "geometry_msgs/Twist"
                #ROS_client, "/robot/web_teleop/cmd_vel", "geometry_msgs/Twist"
            )
            timer_service = roslibpy.Service(ROS_client, "/lap_time_counter/start_race_server", "std_srvs/Trigger")
            request = roslibpy.ServiceRequest()

            while True:
                if self.start_timer:
                    print("SENDING SERVICE HERE")
                    resp = timer_service.call(request)
                    print(resp)
                    self.start_timer = False
                remote_driving.publish(self.cmd(self.move_robot["robot" + robot]))
                self.move_robot["robot" + robot] = [0, 0]
                time.sleep(self.pub_freq)

    # Generates ROS cmd_vel message
    def cmd(self, vel):
        msg = roslibpy.Message(
            {
                "linear": {"x": 0.2*vel[0], "y": 0.0, "z": 0.0},
                "angular": {"x": 0.0, "y": 0.0, "z": 0.2*vel[1]},
            }
        )
        return msg

    def connectUnity1(self):
        print("Waiting for Unity connection")
        s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        try:
            s.bind(("", self.PORT1))
        except:
            self.th1.kill()
        else: 
            s.listen(10)
            self.socket_client1, self.addr = s.accept()
            print("Connected with", self.addr)
        while True:
            try:
                self.receiveData(self.socket_client1)
            except:
                print("Error receiving data")
                time.sleep(1)

    def connectUnity2(self):
        print("Waiting for Unity connection")
        s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        try:
            s.bind(("", self.PORT2))
        except:
            self.th2.kill()
        else: 
            s.listen(10)
            self.socket_client2, self.addr = s.accept()
            print("Connected with", self.addr)
        while True:
            try:
                self.receiveData(self.socket_client2)
            except Exception as e:
                print("Error receiving data" + e)
                time.sleep(1)

    # Sends data to unity
    def sendData1(self, data):
        self.socket_client1.send(str(data).encode())
    # Sends data to unity
    def sendData2(self, data):
        self.socket_client2.send(str(data).encode())

    def receiveData(self, socket_client):
        data = socket_client.recv(4096)
        # print(data)
        if(data.decode() == "timer"):
            if self.socket_client1 is not None:
                self.socket_client1.send(str("start").encode())
            if self.socket_client2 is not None:
                self.socket_client2.send(str("start").encode())
            # Start robot timers here
            self.start_timer = True

        else:
            try:    
                dt = json.loads(data.decode())
                self.move_robot[list(dt.keys())[0]] = dt[list(dt.keys())[0]]
                # print(self.move_robot)
            except:
                print(data.decode())
                print("not json")

    def sendUnity1(self, freq, data):
        print("Sending data to Unity1")
        # Sends data to unity
        while True:
            if self.socket_client1 is not None:
                try:
                    self.sendData1(data)
                    # print(data)
                except Exception as e:
                    print("Cannot send data: ", e)
                    self.th1.kill()
            time.sleep(freq)

    def sendUnity2(self, freq, data):
        print("Sending data to Unity2")
        # Sends data to unity
        while True:
            if self.socket_client2 is not None:
                try:
                    self.sendData2(data)
                    #print(data)
                except Exception as e:
                    print("Cannot send data: ", e)
                    self.th2.kill()
            time.sleep(freq)

    def receive_robotrace(self, msg, robot):
        # Gets battery info from topic
        # self.unity_msg["battery" + robot] = msg["battery"]
        self.unity_msg["battery" + robot] = 74
        self.unity_msg["lidar" + robot] = msg["lidar"]
        # self.unity_msg["lidar1"] = [0,0] 
        self.unity_msg["gps" + robot] = msg["gps"]
        # print(msg["gps"])
        self.unity_msg["speed" + robot] = msg["speed"]
        times = []
        for time in msg["time"]:
            time = time.split(".")[0].split(":",1)[1]
            times.append(time)
        # self.unity_msg["time" + robot] = msg["time"]
        self.unity_msg["time" + robot] = times
        #print(self.unity_msg)

proxy = UnityRobotProxy()
proxy.start()
