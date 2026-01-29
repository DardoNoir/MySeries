import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { WatchlistSerieDto } from '../models/WatchlistSerieDto';

@Injectable({ providedIn: 'root' })
export class WatchlistService {
  private baseUrl = '/api/app/watchlists';

  constructor(private http: HttpClient) {}

  getWatchlist(userId: number): Observable<WatchlistSerieDto[]> {
    return this.http.get<WatchlistSerieDto[]>(
      `${this.baseUrl}/watchlist/${userId}`
    );
  }


  addSeries(imdbId: string, userId: number): Observable<void> {
    const params = new HttpParams()
      .set('imdbId', imdbId)
      .set('userId', userId);

    return this.http.get<void>(`${this.baseUrl}/series-from-api`, { params });
  }

  removeSeries(serieId: number, userId: number): Observable<void> {
    const params = new HttpParams()
      .set('serieId', serieId)
      .set('userId', userId);

    return this.http.get<void>(`${this.baseUrl}/remove-series`, { params });
  }
}
