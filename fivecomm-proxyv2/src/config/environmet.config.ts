import { RosTopics } from "../common/interfaces";

require('dotenv').config()

export const serverHost = process.env.SERVER_HOST || 'localhost';

export const robotUrls = Object.keys(process.env)
  .filter((key) => key.startsWith('URL_ROBOT'))
  .map((key) => {
    const [address, portRx] = process.env[key].split(':');
    return { address, portRx: parseInt(portRx)};
  });

export const cockpitAddrs = Object.keys(process.env)
  .filter((key) => key.startsWith('ADDR_COCKPIT'))
  .map((key) => {
    const [address, portTx, portRx] = process.env[key].split(':');
    return { address, portTx: parseInt(portTx), portRx: parseInt(portRx)};
  });

export const yoloAddrs = Object.keys(process.env)
  .filter((key) => key.startsWith('ADDR_YOLO'))
  .map((key) => {
    const [address, portRx] = process.env[key].split(':');
    return { address, portRx: parseInt(portRx)};
  });

export const modemImeis = Object.keys(process.env)
  .filter((key) => key.startsWith('IMEI_MODEM'))
  .map((key) => {
    return process.env[key];
  });

export const mediaMtxAddrs = Object.keys(process.env)
  .filter((key) => key.startsWith('ADDR_MTX'))
  .map((key) => {
    const [address, portRx] = process.env[key].split(':');
    return { address, portRx: parseInt(portRx)};
  });

export const influxUrl = process.env.URL_INFLUX

export const mqttConfig = {
  url: process.env.BROKER_URL,
  clean: process.env.BROKER_CLEAN === 'true' || true,
  connectTimeout: +process.env.BROKER_CONNECT_TIMEOUT || 4000,
  reconnectPeriod: +process.env.BROKER_RECONNECT_PERIOD || 1000,
  username: process.env.BROKER_USERNAME || 'device',
  password: process.env.BROKER_PASSWORD || 'device'
};

export const rosTopics: RosTopics = {
  subscriber: {
    name: '/fivecomm_robotrace/robotrace',
    messageType: 'fivecomm_robotrace/robotrace'
  },
  control: {
    name : '/robot/cmd_vel',
    messageType : 'geometry_msgs/Twist'
  }
}