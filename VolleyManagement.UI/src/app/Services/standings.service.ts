import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';

import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';
import { Observable } from 'rxjs/Observable';

import { PivotStandings } from '../Models/Pivot/PivotStandings';
import { PivotStandingsEntry } from '../Models/Pivot/PivotStandingsEntry';
import { PivotStandingsGame } from '../Models/Pivot/PivotStandingsGame';
import { StandingsEntry } from '../Models/Standings/StandingsEntry';
import { Constants } from '../Constants/Constants';

@Injectable()
export class StandingsService {
    private pivotStandingsUrl = id => `Tournaments/${id}/PivotStandings`;
    private standingsUrl = id => `Tournaments/${id}/Standings`;

    constructor(private http: Http) { }

    getPivotStandings(id: number): Observable<PivotStandings> {
        const url = Constants.BASE_API_URL.concat(this.pivotStandingsUrl(id));
        return this.http
            .get(url)
            .map((response: Response) => {
                const data = response.json() as PivotStandings;
                const teamStandings: PivotStandingsEntry[] = data.TeamsStandings;
                const gameStandings: PivotStandingsGame[] = new Array();
                data.GamesStandings.forEach(function (item) {
                    gameStandings.push(new PivotStandingsGame(item.HomeTeamId, item.AwayTeamId, item.Results[0]));
                });

                return new PivotStandings(teamStandings, gameStandings);
            });
    }

    getStandings(id: number): Observable<StandingsEntry[]> {
        const url = Constants.BASE_API_URL.concat(this.standingsUrl(id));
        return this.http
            .get(url)
            .map(response => response.json() as StandingsEntry[]);
    }
}
