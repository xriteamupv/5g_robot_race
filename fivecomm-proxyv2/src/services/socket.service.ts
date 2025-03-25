import dgram from 'dgram';
import { Address, IMessage, IService, SendFunction } from '../common/interfaces';
import { MessageFactory } from '../factory/message.factory';
import { serverHost } from '../config/environmet.config';
import { VideoAdaptativeService } from './video-adaptative.service';

export class SocketService implements IService{
  public address: Address[];
  private socketConnections: dgram.Socket[] = [];

  constructor(
    address: Address[],
    private readonly videoAdaptativeService: VideoAdaptativeService,
  ) {
    this.address = address;

  }

  connection(
    address: Address
  ): void {
    const socket = dgram.createSocket('udp4');
    const port = address.portRx;
    // const host = address.address;
    const host = serverHost;
    socket.bind(port, host);
    this.socketConnections.push(socket);

    socket.on('error', (err) => {
      console.log(`Error: ${err.message}`);
      socket.close();
    });
  }

  manageCallbacks(sendToRobot: SendFunction): void {
    this.socketConnections.forEach((socket, index) => {
      socket.on('message', (msg, rinfo) => {
        const msgType = JSON.parse(msg.toString()).header;
        const msgData = JSON.parse(msg.toString()).data;
        switch (msgType) {
          case 'control':
            sendToRobot(msg.toString(), index);
            break;
          case 'init': 
            this.sendAll('true', 'start', undefined);
            break;
          case 'cockpit-broadcast':
            this.sendAll(msgData, 'cockpit-broadcast', index);
            break;
        }
      });
    });
  }

  sendByIndex(data: string, addressIndex: number, msgType: string): void {
    const port = this.address[addressIndex].portTx;
    const address = this.address[addressIndex].address;

    const socket = this.socketConnections[addressIndex];
    const message = MessageFactory.createMessage(msgType, data);
    if(msgType === 'telemetry')
      this.manageVideoStream(message)

    socket.send(message.serializer(), port, address, (error) => {
      if (error)
        console.error(error);
    });
  }

  sendAll(data: string, msgType: string, senderIndex?: number): void {
    this.address.forEach((address, index) => {
      if(senderIndex != undefined && (index === senderIndex)) return;
      const port = address.portTx;
      const socket = this.socketConnections[index];
      const message = MessageFactory.createMessage(msgType, data);
      socket.send(message.serializer(), port, address.address, (error) => {
        if (error)
          console.error(error);
      });
    });
  }

  private manageVideoStream(message: IMessage): void {
    const data = JSON.parse(message.serializer());
    this.videoAdaptativeService.setNetworkQuality(
      +data.data.rsrp,
      +data.data.rsrq,
      +data.data.sinr,
      +data.data.latency
    );
  }
}