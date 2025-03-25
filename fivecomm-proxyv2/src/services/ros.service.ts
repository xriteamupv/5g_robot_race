import ROSLIB from 'roslib';
import { SendFunction, RobotRace, RosTopics, IService, Address } from '../common/interfaces';
import { MessageFactory } from '../factory/message.factory';
import { rosTopics } from '../config/environmet.config';

export class RosService implements IService{
  public address: Address[];
  public rosConnections: ROSLIB.Ros[] = [];
  private rosTopics: RosTopics = rosTopics;

  constructor(
    address: Address[]
  ) {
    this.address = address;
  }

  connection(
    address: Address
  ): void {
    const rosConnection = new ROSLIB.Ros({
      url: `ws://${address.address}:${address.portRx}`
    });
    this.rosConnections.push(rosConnection);
    rosConnection.on('error', function(error) { //TODO: Change logs
      console.log('Error connecting to websocket server: ', error);
    });
  }

  manageCallbacks(sendToCockpit: SendFunction): void {
    this.rosConnections.forEach((rosConnection, index) => {
      rosConnection.on('connection', () => {
        this.subscribeOnTopic(rosConnection, index, sendToCockpit);
        console.log('Connected to websocket server.');
      });
    });
  }

  publishControlByIndex(data: string, index: number) {
    const message = MessageFactory.createMessage('control', data);
    if(!this.robotNumberValidator(index))
      return;
    var cmdVel = new ROSLIB.Topic({
      ros : this.rosConnections[index],
      ...this.rosTopics.control
    });

    var twist = new ROSLIB.Message(message.deserializer().data);
    cmdVel.publish(twist);
  }

  private subscribeOnTopic(
    rosConnection: ROSLIB.Ros, 
    index: number, 
    sendToCockpit: SendFunction,
  ): void {
    var listener = new ROSLIB.Topic({
      ros : rosConnection,
      ...this.rosTopics.subscriber
    });

    listener.subscribe((message: RobotRace) => {
      sendToCockpit(JSON.stringify(message), index);
    });
  } 

  private robotNumberValidator(robotNumber: number): boolean {
    if (robotNumber < 0 || robotNumber >= this.rosConnections.length)
      return false;
    return true;
  }
}