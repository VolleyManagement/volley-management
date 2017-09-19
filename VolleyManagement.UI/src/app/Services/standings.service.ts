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
    private placeHolderToReplace = '${id}';
    private pivotStandingsUrl = 'Tournaments/${id}/PivotStandings';
    private standingsUrl = 'Tournaments/${id}/Standings';

    private headers = new Headers({ 'Content-Type': 'applcation/json' });

    constructor(private http: Http) { }

    getPivotStandings(id: number): Observable<PivotStandings> {
        const url = Constants.BASE_API_URL.concat(this.pivotStandingsUrl.replace(this.placeHolderToReplace, id.toString()));
        return this.http
            .get(url)
            .map(response => response.json() as PivotStandings);
    }

    getStandings(id: number): Observable<StandingsEntry[]> {
        const url = Constants.BASE_API_URL.concat(this.standingsUrl.replace(this.placeHolderToReplace, id.toString()));
        return this.http
            .get(url)
            .map(response => response.json() as StandingsEntry[]);
    }
}
