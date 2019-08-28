import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Response } from '@angular/http';
import { environment } from '../../environments/environment';

import 'rxjs/add/operator/map';
import { Observable } from 'rxjs/Observable';

import { TournamentMetadataJson } from '../Models/TournamentMetadataJson/TournamentMetadataJson';

@Injectable()
export class JsonService {
    constructor(private http: HttpClient) { }

    getJson<T>(url: string): Observable<T> {
        return this.http
            .get(url)
            .map(response => response as T);
    }
}
