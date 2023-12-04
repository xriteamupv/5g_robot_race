import socket
import time
import json
import sys
 
HOST = "127.0.0.1"  # The server's hostname or IP address
PORT = int(sys.argv[1]) # The port used by the server
robot = sys.argv[2] 
 
with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    s.connect((HOST, PORT))
    while True:
        # data = s.recv(1024)
        # print(f"Received {data!r}")
        rd = {robot: [0.2222,1.1550]}

        s.sendall(json.dumps(rd).encode())
        time.sleep(0.1) #min: 0.05 #recommended: 0.1