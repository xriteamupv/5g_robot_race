import { IMessage } from "../common/interfaces";

export class StartMessage implements IMessage {
  private data: string;

  constructor(data: string) {
    this.data = data;
  }

  serializer(): string {
    const header = 'start';
    return JSON.stringify({ header, data: JSON.parse(this.data) });
  }
}

