import { IMessage } from "../common/interfaces";
import { TelemetryFactory } from "../factory/telemetry.factory";

export class TelemetryMessage implements IMessage {
  private data: string;

  constructor(data: string) {
    this.data = data;
  }

  serializer(): string {
    const header = 'telemetry';
    const radio = this.getRadioFromData(this.data);
    return JSON.stringify({ header, data: TelemetryFactory.createTelemetry(radio, this.data) });
  }

  private getRadioFromData(data: string): string {
    return JSON.parse(data)['servingcell'].split(': ')[1].replaceAll('"', '').split(',')[2];
  }
}