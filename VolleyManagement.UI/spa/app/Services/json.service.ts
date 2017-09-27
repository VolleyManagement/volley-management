import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { environment } from '../../environments/environment';

import 'rxjs/add/operator/map';
import { Observable } from 'rxjs/Observable';

import { TournamentMetadataJson } from '../Models/TournamentMetadataJson/TournamentMetadataJson';

@Injectable()
export class JsonService {
    constructor(private http: Http) { }

    getTournamentJson(fileName: string): Observable<TournamentMetadataJson> {
        const url = environment.jsonBaseUrl.concat(fileName);
        return this.http
            .get(url)
            .map(response => response.json() as TournamentMetadataJson);
    }
}
