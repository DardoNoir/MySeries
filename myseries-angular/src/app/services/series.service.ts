import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { OmdbSeriesDto } from '../models/omdb-series.model';
import { OmdbSeriesSearchDto } from '../models/omdb-search.model';

@Injectable({
  providedIn: 'root'
})
export class SeriesService {
  // If using proxy, baseUrl can be empty string; otherwise set to your backend URL
  private baseUrl = '';

  constructor(private http: HttpClient) {}

  // === Use the variant that matches Swagger (GET vs POST) ===
  // 1) GET variant (common when ABP exposes simple parameters as query-string)
  getFromOmdb_get(imdbId: string): Observable<OmdbSeriesDto> {
    const params = new HttpParams().set('imdbId', imdbId);
    return this.http.get<OmdbSeriesDto>(`${this.baseUrl}/api/app/series/get-from-omdb`, { params });
  }

  searchFromOmdb_get(title: string, genre?: string): Observable<OmdbSeriesSearchDto> {
    let params = new HttpParams().set('title', title);
    if (genre) { params = params.set('genre', genre); }
    return this.http.get<OmdbSeriesSearchDto>(`${this.baseUrl}/api/app/series/search-from-omdb`, { params });
  }
}
