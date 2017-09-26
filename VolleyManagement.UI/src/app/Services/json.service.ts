import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { environment } from '../../environments/environment';

import 'rxjs/add/operator/map';
import { Observable } from 'rxjs/Observable';

import { TournamentJson } from '../Models/TournamentJson/TournamentJson';

@Injectable()
export class JsonService {
    constructor(private http: Http) { }

    getTournamentJson(fileName: string): Observable<TournamentJson> {
        const url = environment.jsonBaseUrl.concat(fileName);
        return this.http
            .get(url)
            .map(response => response.json() as TournamentJson);
    }
}
