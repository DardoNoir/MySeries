import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';

export interface OmdbSeriesSearchItemDto {
    imdbID: string;
    title: string;
    year: string;
    Type: string;
    poster?: string;
    genre?: string;
}

export interface OmdbSeriesSearchDto {
    search: OmdbSeriesSearchItemDto[];
    totalResults?: string;
    Response?: string;
}

export interface OmdbSeriesDto {
    imdbID: string;
    title: string;
    year: string;
    Genre?: string;
    plot?: string;
    country?: string;
    Poster?: string;
    totalSeasons?: string;
    imdbRating?: string;
}

@Injectable({
    providedIn: 'root'
})
export class OmdbService {
    private baseUrl = '/api/series'; // redirige al backend a través del proxy

    constructor(private http: HttpClient) {}

    searchByTitle(title: string): Observable<OmdbSeriesSearchDto> {
        return this.http.get<OmdbSeriesSearchDto>
        (`${this.baseUrl}/search?title=${title}`);
    }

    getByImdb(imdbID: string): Observable<OmdbSeriesDto> {
        return this.http.get<OmdbSeriesDto>(`${this.baseUrl}/${imdbID}`);
    }
}