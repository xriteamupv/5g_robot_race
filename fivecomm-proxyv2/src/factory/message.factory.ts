import { IMessage } from "../common/interfaces";
import { BoxesMessage, BroadcastMessage, ControlMessage, RobotRaceMessage, StartMessage, TelemetryMessage } from "../messages";

export class MessageFactory {
  static createMessage(type: string, data: string): IMessage {
    switch (type) {
      case 'robot':
        return new RobotRaceMessage(data);
      case 'telemetry':
        return new TelemetryMessage(data);
      case 'boxes':
        return new BoxesMessage(data);
      case 'control':
        return new ControlMessage(data);
      case 'start':
        return new StartMessage(data);
      case 'cockpit-broadcast':
        return new BroadcastMessage(data);
      default:
        throw new Error(`Unknown message type: ${type}`);
    }
  }
}
