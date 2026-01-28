import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MonitoringDto } from '../models/MonitoringDto';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class MonitoringService {
  private baseUrl = '/api/app/monitoring';

  constructor(private http: HttpClient) {}

  getStats(): Observable<MonitoringDto> {
    return this.http.get<MonitoringDto>(`${this.baseUrl}/api-stats`);
  }
}