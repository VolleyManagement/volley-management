import { Injectable } from '@angular/core';
import { Headers, Http } from '@angular/http';

import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';
import { Observable } from 'rxjs/Observable';

import { StandingsEntry } from '../Models/Standings/StandingsEntry';
import { Constants } from '../Constants/Constants';


@Injectable()
export class StandingsService {
    private placeHolderToReplace = '${id}';
    private standingsUrl = 'Tournaments/${id}/Standings';

    private headers = new Headers({ 'Content-Type': 'applcation/json' });

    constructor(private http: Http) { }

    getStandings(id: number): Observable<StandingsEntry[]> {
        const url = Constants.BASE_API_URL.concat(this.standingsUrl.replace(this.placeHolderToReplace, id.toString()));
        return this.http
            .get(url)
            .map(response => response.json() as StandingsEntry[]);
    }
}
