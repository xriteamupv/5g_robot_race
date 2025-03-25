import { Telemetry } from "../common/interfaces";

export class ServingcellModel {
  static saTelemetryParser(data: string): Telemetry {
    if(!data) return;
    const servingcell = JSON.parse(data)['servingcell'].split(': ')[1].replaceAll('"', '').split(',');
    return {
      rsrp: +servingcell[12],
      rsrq: +servingcell[13],
      sinr: +servingcell[14],
      latency: JSON.parse(data)['latency']['avg'],
    };
  }

  static lteTelemetryParser(data: string): Telemetry {
    if(!data) return;
    const servingcell = JSON.parse(data)['servingcell'].split(': ')[1].replaceAll('"', '').split(',');
    return {
      rsrp: +servingcell[13],
      rsrq: +servingcell[14],
      sinr: +servingcell[15],
      latency: JSON.parse(data)['latency']['avg'],
    };
  }

}