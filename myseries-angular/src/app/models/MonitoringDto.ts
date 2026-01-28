export interface EndpointStatDto {
  method: string;
  count: number;
}

export interface MonitoringDto {
  totalCalls: number;
  averageResponseTime: number;
  errorCount: number;
  omdbApiConsumptions: number;
  topEndpoints: EndpointStatDto[];
}