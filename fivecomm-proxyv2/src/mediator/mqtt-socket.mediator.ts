import { IMqttService, ISocketService } from '../common/interfaces';

export class MqttSocketMediator {
  private socketService: ISocketService;
  private mqttService: IMqttService;
  private modemImeis: string[];

  constructor(
    socketService: ISocketService,
    mqttService: IMqttService
  ) {
    this.socketService = socketService;
    this.mqttService = mqttService;
    this.modemImeis = this.mqttService.modemImeis;
  }

  manageModemsConnections(): void {
    try {
      if (!this.modemImeis || this.modemImeis.length === 0)
        return;
      this.mqttService.connection()
    } catch (error) {
      console.error('Error managing Modem connections:', error);
    }
  }
  manageModemsCallbacks(): void {
    this.mqttService.manageCallbacks(this.sendToCockpit);
  }

  sendToCockpit = (data: string, index: number) => {
    this.socketService.sendByIndex(data, index, 'telemetry');
  }
}