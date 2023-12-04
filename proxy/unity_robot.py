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
from newThread import newThread
import json


class UnityRobotProxy:
    def start(self):
        # self.IP1 = os.environ["ROBOT1"]
        # self.IP2 = os.environ["ROBOT2"]

        self.IP1 = "10.45.11.3"
        self.IP2 = "10.45.0.4"

        # Initialization of variables
        self.unity_msg = {
            "battery1": 0,
            "lidar1": [0, 0],
            "gps1": [0, 0, 0],
            "speed1": 0,
            "battery2": 0,
            "lidar2": [0, 0],
            "gps2": [0, 0, 0],
            "speed2": 0,
        }
        self.unity_rd = {"robot": 0, "move": [0, 0]}
        self.move_robot = {"robot1": [0, 0], "robot2": [0, 0]}
        self.socket_client = None  # Unity client
        self.pub_freq = 0.1

        # Connection with unity
        self.th1 = newThread(target=self.connectUnity, daemon=False)
        self.th1.start()
        # Parallel threads for sending data from robot to unity
        self.th2 = newThread(
            target=self.sendUnity, args=(0.5, self.unity_msg), daemon=False
        )
        self.th2.start()

        p1 = self.robot_connection(self.IP1, "1")
        p2 = self.robot_connection(self.IP2, "2")
        while 0 == 0:
            if (
                not p1.is_alive()
                or not p2.is_alive()
                or not self.th1.is_alive()
                or not self.th2.is_alive()
            ):
                print(
                    "p1-",
                    p1.is_alive(),
                    " p2-",
                    p2.is_alive(),
                    " th1-",
                    self.th1.is_alive(),
                    " th2-",
                    self.th2.is_alive(),
                )
            if not p1.is_alive():
                p1 = self.robot_connection(self.IP1, "1")
            if not p2.is_alive():
                p2 = self.robot_connection(self.IP2, "2")
            if not self.th1.is_alive():
                self.th2.kill()
                self.socket_client = None
                time.sleep(1)
                self.th1 = newThread(target=self.connectUnity, daemon=False)
                self.th1.start()
                self.th2 = newThread(
                    target=self.sendUnity, args=(0.5, self.unity_msg), daemon=False
                )
                self.th2.start()
            time.sleep(1)

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
            )
            while True:
                # remote_driving.publish(self.cmd(self.move_robot["robot" + robot]))
                # print(self.move_robot)
                self.move_robot = {"robot1": [0, 0], "robot2": [0, 0]}
                time.sleep(self.pub_freq)

    # Generates ROS cmd_vel message
    def cmd(self, vel):
        msg = roslibpy.Message(
            {
                "linear": {"x": vel[0], "y": 0.0, "z": 0.0},
                "angular": {"x": 0.0, "y": 0.0, "z": vel[1]},
            }
        )
        return msg

    def connectUnity(self):
        print("Waiting for Unity connection")
        s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        try:
            s.bind(("", 11996))
        except:
            self.th1.kill()
        else: 
            s.listen(10)
            self.socket_client, self.addr = s.accept()
            print(self.socket_client)
            # self.socket_client.settimeout(0.05)
            print("Connected with", self.addr)
        while True:
            try:
                self.receiveData()
            except:
                print("Error receiving data")
                time.sleep(1)

    # Sends data to unity
    def sendData(self, data):
        self.socket_client.send(str(data).encode())

    def receiveData(self):
        data = self.socket_client.recv(4096)
        dt = json.loads(data.decode())
        self.move_robot[list(dt.keys())[0]] = dt[list(dt.keys())[0]]

    def sendUnity(self, freq, data):
        print("Sending data to Unity")
        # Sends data to unity
        while True:
            if self.socket_client is not None:
                try:
                    self.sendData(data)
                except Exception as e:
                    print("Cannot send data: ", e)
                    # restart connectUnity here
                    # self.socket_client.close()
                    self.th1.kill()
            time.sleep(freq)

    def receive_robotrace(self, msg, robot):
        # Gets battery info from topic
        self.unity_msg["battery" + robot] = msg["battery"]
        self.unity_msg["lidar" + robot] = msg["lidar"]
        self.unity_msg["gps" + robot] = msg["gps"]
        self.unity_msg["speed" + robot] = msg["speed"]
        # print(self.unity_msg)


proxy = UnityRobotProxy()
proxy.start()
