export interface IMqttConfig {
  url: string;
  clean: boolean;
  connectTimeout: number;
  reconnectPeriod: number;
  username: string;
  password: string;
}