FROM ubuntu:18.04

RUN apt-get update -y

RUN apt-get install -y python-flask-sockets --assume-yes

RUN apt-get install -y python3 --assume-yes

RUN apt-get install -y python3-pip --assume-yes

RUN pip3 install --upgrade pip

RUN pip3 install roslibpy 

RUN apt-get install -y netcat --assume-yes

RUN apt-get install -y nano --assume-yes

RUN apt-get install -y tcpdump --assume-yes

WORKDIR /app
COPY  . .

CMD ["-u","unity2_robot.py"]
ENTRYPOINT ["python3"]