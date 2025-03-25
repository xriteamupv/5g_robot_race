import { IService } from "../common/interfaces";
import { InfluxService, MqttService, RosService, SocketService, VideoAdaptativeService, YoloService } from "../services";

export class ServiceFactory {
  

  static createService(type: string, addrs: any): IService {
    switch (type) {
      case 'socket':
        return new SocketService(addrs, new VideoAdaptativeService());
      case 'ros':
        return new RosService(addrs);
      case 'yolo':
        return new YoloService(addrs);
      case 'influx':
        return new InfluxService(addrs);
      case 'mqtt':
        return new MqttService(addrs);
      default:
        throw new Error(`Unknown message type: ${type}`);
    }
  }
}
