import { IMessage } from "../common/interfaces";

export class RobotRaceMessage implements IMessage {
  private data: string;

  constructor(data: string) {
    this.data = data;
  }

  serializer(): string {
    const header = 'robot';
    return JSON.stringify({ header, data: JSON.parse(this.data) });
  }
}

