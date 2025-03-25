import { Telemetry } from "../common/interfaces";
import { ServingcellModel } from "../model/servingcell.model";

export class TelemetryFactory {
  static createTelemetry(radio: string, data: string): Telemetry {
    if(!radio) return;
    switch (radio) {
      case 'LTE':
        return ServingcellModel.lteTelemetryParser(data);
      case 'NR5G-SA':
        return ServingcellModel.saTelemetryParser(data);
      case 'NR5G-NSA':
        console.log('NR5G-NSA not implemented yet');
      default:
        throw new Error(`Unknown radio: ${radio}`);
    }
  }
}
