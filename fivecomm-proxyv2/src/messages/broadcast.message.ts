import { IMessage } from "../common/interfaces";

export class BroadcastMessage implements IMessage {
  private data: string;

  constructor(data: string) {
    this.data = data;
  }

  serializer(): string {
    const header = 'cockpit-broadcast';
    let data: string;
    try{
      data = JSON.parse(this.data);
    } catch {
      data = this.data;
    }
    return JSON.stringify({ header, data: data});
  }
}

