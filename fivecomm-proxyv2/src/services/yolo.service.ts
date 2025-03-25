import dgram from 'dgram';
import { Address, SendFunction } from '../common/interfaces';

export class YoloService {
  public address: Address[];
  private socketConnections: dgram.Socket[] = [];

  constructor(
    address: Address[]
  ) {
    this.address = address;
  }

  connection(
    address: Address
  ): void {
    const socket = dgram.createSocket('udp4');
    const port = address.portRx;
    const host = address.address;
    socket.bind(port, host);
    this.socketConnections.push(socket);
    socket.on('error', (err) => {
      console.log(`Error: ${err.message}`);
      socket.close();
    });
  }

  manageCallbacks(sendToCockpit: SendFunction): void {
    this.socketConnections.forEach((socket, index) => {
      socket.on('message', (msg, rinfo) => {
        sendToCockpit(JSON.stringify(JSON.parse(msg.toString()).data), index);
      });
    });
  }
}