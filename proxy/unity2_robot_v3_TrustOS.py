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
import httpx


class UnityRobotProxy:
    def start(self):

        self.token = None
        # Initialize token from TrustOS
        self.login()

        # self.IP1 = os.environ["ROBOT1"]
        # self.IP2 = os.environ["ROBOT2"]
        self.IP1 = "10.45.21.103"
        self.IP2 = "10.45.21.102"

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
        self.update_freq = 20  # Update TrustOS every 20 seconds

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

        # Start refresh token thread from TrustOS
        self.refresh_thread = newThread(target=self.refresh_token_periodically, daemon=True)
        self.refresh_thread.start()

        # Update asset thread from TrustOS
        self.update_thread = newThread(target=self.update_periodically, daemon=True)
        # self.update_thread.start()
        
        while 0 == 0:
            try:
                if (
                    not self.p1.is_alive()
                    or not self.p2.is_alive()
                    or not self.th1.is_alive()
                    or not self.th2.is_alive()
                    or not self.th3.is_alive()
                    or not self.th4.is_alive()
                    or not self.refresh_thread.is_alive()
                    or not self.update_thread.is_alive()
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
                        " refresh_thread_",
                        self.refresh_thread.is_alive(),
                        " update_thread_",
                        self.update_thread.is_alive()
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
                if not self.refresh_thread.is_alive():
                    self.refresh_thread = newThread(target=self.refresh_token_periodically, daemon=True)
                    self.refresh_thread.start()
                if not self.update_thread.is_alive():
                    self.update_thread = newThread(target=self.update_periodically, daemon=True)
                    # self.update_thread.start()
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
                self.refresh_thread.kill()
                self.update_thread.kill()
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
                "linear": {"x": vel[0], "y": 0.0, "z": 0.0},
                "angular": {"x": 0.0, "y": 0.0, "z": vel[1]},
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
                if not self.update_thread.is_alive():
                    self.update_thread.start()
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

    def login(self):
        response = httpx.post(
            "http://localhost:9090/id/v2/login",
            headers={"Accept": "application/json", "Content-Type": "application/json"},
            json={"username": "did:user:dfef45d3d5401a34de94f6941ca69d64c54a02371a34475812c199efc0704ecf", "password": "test"},
            verify=False
        )

        if response.status_code == 200:
            response_json = response.json()
            # print("Response JSON:", response_json)  # Debugging line to print the entire response JSON
            self.token = response_json['data'].get('token')
            print("¡Login correcto!")
            # print(f"Token recibido: {self.token}")

        else:
            print("Error en el login")
            print(f"Response: {response.status_code}, {response.text}")

        time.sleep(5)

    def refresh_token_periodically(self):
        while True:
            self.refresh_token()
            time.sleep(1200) # Refresh each 20 mins

    def refresh_token(self):
        response = httpx.post(
            "http://localhost:9090/id/v2/refresh",
            headers={
                "Authorization": f"Bearer {self.token}",
                "Cookie": "7c17deb912d4b5ec3a5f29190bf93056=b3b5433bb235ee5bcf285f2ce11c08cb"
            },
            verify=False
        )

        if response.status_code == 200:
            response_json = response.json()
            # print("Response JSON:", response_json)  # Debugging line to print the entire response JSON
            self.token = response_json['data'].get('token')
            print("¡Token refrescado correctamente!")
            # print(f"Nuevo token: {self.token}")
        else:
            print("Error al refrescar el token")
            print(f"Response: {response.status_code}, {response.text}")

    def update_periodically(self):
        while True:
            time.sleep(20) # Refresh each 20 secs
            self.update_asset_in_trustos(self.unity_msg)

    def update_asset_in_trustos(self, data):
        while True:
            url = 'http://localhost:9090/track/v2/assets/asset-agv-test-01?networkId=10004'
            headers = {
                'Content-Type': 'application/json',
                'Authorization': f'Bearer {self.token}',
                'Cookie': '7c17deb912d4b5ec3a5f29190bf93056=b3b5433bb235ee5bcf285f2ce11c08cb'
            }
            payload = {
                "metadata": {
                    "battery1": str(data["battery1"]),
                    "lidar1": json.dumps(data["lidar1"]),
                    "gps1": json.dumps(data["gps1"]),
                    "speed1": str(data["speed1"]),
                    "battery2": str(data["battery2"]),
                    "lidar2": json.dumps(data["lidar2"]),
                    "gps2": json.dumps(data["gps2"]),
                    "speed2": str(data["speed2"])
                }
            }
            print(payload)
            try:
                response = httpx.patch(url, headers=headers, json=payload, verify=False, timeout=60.0)
                if response.status_code == 200:
                    print("Asset updated successfully")
                else:
                    print(f"Failed to update asset: {response.status_code}, {response.text}")
            except httpx.RequestError as exc:
                print(f"An error occurred while requesting {exc.request.url!r}.")
                print(f"Exception: {exc}")
            except Exception as e:
                print(f"Exception occurred while updating asset: {e}")
            
            time.sleep(10) #Update asset metadata each 10 seconds

proxy = UnityRobotProxy()
proxy.start()
