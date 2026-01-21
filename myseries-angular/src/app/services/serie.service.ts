import { Injectable } from "@angular/core";
import { SerieDto } from "../models/SerieDto";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({
    providedIn: 'root',
})
export class SerieService{
    private baseUrl  = ''

    constructor(private http: HttpClient) {}

    searchSeries(title: string): Observable<SerieDto[]> {
        return this.http.get<SerieDto[]>(
            '/api/app/serie/search-by-title',
            { params: { title } }
        );
    }

}