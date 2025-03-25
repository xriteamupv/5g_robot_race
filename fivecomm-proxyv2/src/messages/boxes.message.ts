import { IMessage } from "../common/interfaces";

export class BoxesMessage implements IMessage {
  private data: string;

  constructor(data: string) {
    this.data = data;
  }

  serializer(): string {
    const header = 'boxes';
    return JSON.stringify({ header, data: JSON.parse(this.data) });
  }
}

