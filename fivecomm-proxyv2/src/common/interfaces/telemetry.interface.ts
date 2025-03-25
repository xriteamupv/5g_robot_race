export interface Telemetry {
  rsrp: number;
  rsrq: number;
  sinr: number;
  latency: number;
}

export interface Latency {
  min: number;
  max: number;
  avg: number;
}