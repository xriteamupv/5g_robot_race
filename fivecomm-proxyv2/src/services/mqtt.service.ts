import mqtt from "mqtt";
import { IMqttConfig, IService, SendFunction } from "../common/interfaces";
import { mqttConfig } from "../config/environmet.config";

export class MqttService implements IService {
  private mqttClient: mqtt.MqttClient;
  private mqttConfig: IMqttConfig = mqttConfig;
  public modemImeis: string[];

  constructor(
    modemImeis: string[]
  ) {
    this.modemImeis = modemImeis;
  }

  public connection(): void {
    const clientId = `mqtt_${Math.random().toString(16).slice(3)}`;
    const connectUrl = this.mqttConfig.url;
    const clean = this.mqttConfig.clean;
    const connectTimeout = this.mqttConfig.connectTimeout;
    const reconnectPeriod = this.mqttConfig.reconnectPeriod;
    const username = this.mqttConfig.username;
    const password = this.mqttConfig.password;
    this.mqttClient = mqtt.connect(connectUrl, {
      clientId,
      clean,
      connectTimeout,
      reconnectPeriod,
      username,
      password,
    });

    try {
      this.mqttClient.on("connect", () => {
        this.modemImeis.forEach((imei) => {
          this.mqttClient.subscribe(`t${imei}`, (err: any) => {
            console.log(`Subscribed to t${imei}`);
          });
        });
      });
    } catch (error) {
      console.error(error);
    }
  }

  public manageCallbacks(sendToCockpit: SendFunction){
    this.mqttClient.on("message", (topic, msg) => {
      const index = this.modemImeis.indexOf(topic.slice(1));
      sendToCockpit(msg.toString(), index);
    });
  }
}