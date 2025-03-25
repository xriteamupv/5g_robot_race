import { IMessage, UnityControl } from "../common/interfaces";

export class ControlMessage implements IMessage {
  private data: string;

  constructor(data: string) {
    this.data = data;
  }

  serializer(): string {
    const header = 'boxes';
    return JSON.stringify({ header, data: this.data });
  }

  deserializer(): UnityControl {
    if (!this.validateControlData(this.data))
      throw new Error('Invalid control message data from Unity');
    return JSON.parse(this.data);
  }

  private validateControlData(data: string): boolean {
    const parsedData = JSON.parse(data);
    return (
      typeof parsedData === 'object' &&
      parsedData.header === 'control' &&
      typeof parsedData.data.linear.x === 'number' &&
      typeof parsedData.data.linear.y === 'number' &&
      typeof parsedData.data.linear.z === 'number' &&
      typeof parsedData.data.angular.x === 'number' &&
      typeof parsedData.data.angular.y === 'number' &&
      typeof parsedData.data.angular.z === 'number'
    );
  }
}
