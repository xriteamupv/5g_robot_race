import { IService, Address } from '../common/interfaces';
import axios, { AxiosInstance } from 'axios';

export class InfluxService implements IService{
  private httpClient: AxiosInstance;

  constructor(
    baseURL: string
  ) {
    this.httpClient = axios.create({baseURL});
  }

  connection(
    address: Address
  ): void {

  }

  async getTelemetryByMeasurementId(table: string, measurementId: string): Promise<any> {
    const query = `SELECT * FROM ${table} WHERE "measurement_id"='${measurementId}'`;
    const url = `/query?pretty=true&db=nacar&q=${encodeURIComponent(query)}`;
    const response = await this.httpClient.get<any>(url);
    return response.data;
}

}