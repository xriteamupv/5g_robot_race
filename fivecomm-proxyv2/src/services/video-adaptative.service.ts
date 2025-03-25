import { VideoResolutions } from "../common/enums/video-resolutions.enum";

export class VideoAdaptativeService {
  private rsrp: number;
  private rsrq: number;
  private sinr: number;
  private latency: number;

  private rsrpBrackets = [-110, -90];
  private rsrqBrackets = [-19, -10];
  private sinrBrackets = [0, 10];
  private latencyBrackets = [50, 100];

  private rsrpMultiplier = 0;
  private rsrqMultiplier = 0;
  private sinrMultiplier = 0;
  private latencyMultiplier = 1;

  private qualityPoints: number[] = [];
  private qualityPointsBrackets = [1, 1];

  private hysteresisCount: number = 0;
  private hysterisisThreshold: number = 3;

  private currentResolution: VideoResolutions;
  private videoResolutions = [
    VideoResolutions.LOW,
    VideoResolutions.MEDIUM,
    VideoResolutions.HIGH,
  ]

  constructor() {}

  setNetworkQuality(
    rsrp: number,
    rsrq: number,
    sinr: number,
    latency: number
  ): void {
    this.rsrp = rsrp;
    this.rsrq = rsrq;
    this.sinr = sinr;
    this.latency = latency;
    this.addQualityPoints();
    if (this.hysterisisThreshold === this.hysteresisCount)
      this.changeResolution();
  }

  private changeResolution(): void {
    this.hysteresisCount = 0;
    this.currentResolution = this.getVideoResolution();
    console.log(`Changing resolution to ${this.currentResolution}`); //TODO: API call
  }

  private getVideoResolution(): VideoResolutions {
    const pointsAvg = this.qualityPoints.reduce((a, b) => a + b, 0) / this.qualityPoints.length;
    const resolutionIndex = this.getPoints(pointsAvg, this.qualityPointsBrackets);
    this.qualityPoints = [];
    return this.videoResolutions[resolutionIndex];
  }

  private addQualityPoints(){
    let points = 0;
    points += this.getPoints(this.rsrp, this.rsrpBrackets) * this.rsrpMultiplier;
    points += this.getPoints(this.rsrq, this.rsrqBrackets) * this.rsrqMultiplier;
    points += this.getPoints(this.sinr, this.sinrBrackets) * this.sinrMultiplier;
    points += this.getPoints(this.latency, this.latencyBrackets, true) * this.latencyMultiplier;
    const resolution = this.videoResolutions[this.getPoints(points, this.qualityPointsBrackets)];
    if(this.currentResolution != resolution) {
      this.hysteresisCount += 1;
      this.qualityPoints.push(points);
    } else {
      this.hysteresisCount = 0;
      this.qualityPoints = [];
    }
  }

  private getPoints(value: number, brackets: number[], reverse = false): number {
    const array = [...brackets, value].sort((a, b) => a - b);
    if (reverse) array.reverse();
    return array.indexOf(value);
  }

}