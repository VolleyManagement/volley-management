import { Injectable } from '@angular/core';
import { Headers, Http } from '@angular/http';

import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';
import { Observable } from 'rxjs/Observable';

import { PivotStandings } from '../Models/Pivot/PivotStandings';
import { StandingsEntry } from '../Models/Standings/StandingsEntry';
import { Constants } from '../Constants/Constants';


@Injectable()
export class StandingsService {
    private headers = new Headers({ 'Content-Type': 'applcation/json' });
    private pivotStandingsUrl = id => `Tournaments/${id}/PivotStandings`;
    private standingsUrl = id => `Tournaments/${id}/Standings`;

    constructor(private http: Http) { }

    getPivotStandings(id: number): Observable<PivotStandings> {
        const url = Constants.BASE_API_URL.concat(this.pivotStandingsUrl(id));
        return this.http
            .get(url)
            .map(response => response.json() as PivotStandings);
    }

    getStandings(id: number): Observable<StandingsEntry[]> {
        const url = Constants.BASE_API_URL.concat(this.standingsUrl(id));
        return this.http
            .get(url)
            .map(response => response.json() as StandingsEntry[]);
    }
}
