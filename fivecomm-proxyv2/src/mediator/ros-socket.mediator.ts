import { Address, IRosService, ISocketService } from '../common/interfaces';

export class RosSocketMediator {
  private socketService: ISocketService;
  private rosService: IRosService;

  private cockpitsAddrs: Address[];
  private robotUrls: Address[];

  constructor(
    socketService: ISocketService,
    rosService: IRosService
  ) {
    this.socketService = socketService;
    this.rosService = rosService;
    this.cockpitsAddrs = this.socketService.address;
    this.robotUrls = this.rosService.address;
  }

  manageCockpitsConnections(): void {
    try {
      if (!this.cockpitsAddrs || this.cockpitsAddrs.length === 0)
        return;
      this.cockpitsAddrs.map((address) =>
        this.socketService.connection(address)
      );
    } catch (error) {
      console.error('Error managing cockpit connections:', error);
    }
  }

  manageRobotsConnections(): void {
    try {
      if (!this.robotUrls || this.robotUrls.length === 0)
        return;
      this.robotUrls.map((rosUrl) => {
        this.rosService.connection(rosUrl);
      });

    } catch (error) {
      console.error('Error managing robot connections:', error);
    }
  }

  manageCockpitsCallbacks(): void {
    this.socketService.manageCallbacks(this.sendToRobot);
  }
  manageRobotsCallbacks(): void {
    this.rosService.manageCallbacks(this.sendToCockpit);
  }

  sendToCockpit = (data: string, index: number) => {
    this.socketService.sendByIndex(data, index, 'robot');
  }
  sendToRobot = (data: string, index: number) => {
    this.rosService.publishControlByIndex(data, index);
  }
}