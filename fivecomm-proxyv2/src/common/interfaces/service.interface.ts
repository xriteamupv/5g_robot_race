import mqtt from "mqtt";
import { Address } from "./address.interface";
import { SendFunction } from "./send-function.interface";

export interface ISocketService extends IService {
  sendByIndex(data: string, addressIndex: number, msgType: string): void
}

export interface IRosService extends IService {
  publishControlByIndex(data: string, index: number): void
}

export interface IYoloService extends IService {
  
}

export interface IInfluxService extends IService {
  getTelemetryByMeasurementId(table: string, measurementId: string): Promise<any>
}

export interface IMqttService extends IService {
  mqttClient: mqtt.MqttClient,
  modemImeis: string[]
}

export interface IService {
  address?: Address[]
  connection(address?: Address ): void
  manageCallbacks?(sendTo: SendFunction): void
}