import { Address, ISocketService, IYoloService } from '../common/interfaces';

export class YoloSocketMediator {
  private socketService: ISocketService;
  private yoloService: IYoloService;
  private yoloAddrs: Address[];

  constructor(
    socketService: ISocketService,
    yoloService: IYoloService
  ) {
    this.socketService = socketService;
    this.yoloService = yoloService;
    this.yoloAddrs = this.yoloService.address;
  }

  manageYolosConnections(): void {
    try {
      if (!this.yoloAddrs || this.yoloAddrs.length === 0)
        return;
      this.yoloAddrs.map((address) =>
        this.yoloService.connection(address)
      );
    } catch (error) {
      console.error('Error managing Yolo connections:', error);
    }
  }
  manageYolosCallbacks(): void {
    this.yoloService.manageCallbacks(this.sendToCockpit);
  }

  sendToCockpit = (data: string, index: number) => {
    this.socketService.sendByIndex(data, index, 'boxes');
  }
}