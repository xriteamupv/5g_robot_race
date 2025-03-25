import { cockpitAddrs, influxUrl, modemImeis, robotUrls, yoloAddrs } from "./config/environmet.config";
import { MqttSocketMediator, RosSocketMediator, YoloSocketMediator } from './mediator';
import { ServiceFactory } from "./factory/service.factory";
import { IInfluxService, IMqttService, IRosService, ISocketService, IYoloService } from "./common/interfaces";


function initializeApp() {
  const socketService = ServiceFactory.createService('socket', cockpitAddrs) as ISocketService;
  const rosService = ServiceFactory.createService('ros', robotUrls) as IRosService;
  const yoloService = ServiceFactory.createService('yolo', yoloAddrs) as IYoloService;
  // const influxService = ServiceFactory.createService('influx', influxUrl) as IInfluxService;
  const mqttService = ServiceFactory.createService('mqtt', modemImeis) as IMqttService;

  const rosSocketMediator = new RosSocketMediator(socketService, rosService);
  const yoloSocketMediator = new YoloSocketMediator(socketService, yoloService);
  const mqttSocketMediator = new MqttSocketMediator(socketService, mqttService);

  rosSocketMediator.manageCockpitsConnections();
  rosSocketMediator.manageRobotsConnections();
  rosSocketMediator.manageCockpitsCallbacks();
  rosSocketMediator.manageRobotsCallbacks();

  yoloSocketMediator.manageYolosConnections();
  yoloSocketMediator.manageYolosCallbacks();

  mqttSocketMediator.manageModemsConnections();
  mqttSocketMediator.manageModemsCallbacks();
}

initializeApp();