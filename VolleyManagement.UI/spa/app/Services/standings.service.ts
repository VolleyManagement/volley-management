import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { environment } from '../../environments/environment';

import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';
import { Observable } from 'rxjs/Observable';

import { PivotStandings } from '../Models/Pivot/PivotStandings';
import { PivotStandingsEntry } from '../Models/Pivot/PivotStandingsEntry';
import { PivotStandingsGame } from '../Models/Pivot/PivotStandingsGame';
import { StandingsEntry } from '../Models/Standings/StandingsEntry';

@Injectable()
export class StandingsService {
    private pivotStandingsUrl = id => `api/v1/Tournaments/${id}/PivotStandings`;
    private standingsUrl = id => `api/v1/Tournaments/${id}/Standings`;

    constructor(private http: Http) { }

    getPivotStandings(id: number): Observable<PivotStandings[]> {
        const url = environment.apiUrl.concat(this.pivotStandingsUrl(id));
        return this.http
            .get(url)
            .map((response: Response) => {
                const data = response.json() as PivotStandings[];
                const result: PivotStandings[] = new Array();
                data.forEach(pivot => {
                    const teamStandings: PivotStandingsEntry[] = pivot.TeamsStandings;
                    this.setTeamsPositions(teamStandings);
                    const gameStandings: PivotStandingsGame[] = new Array();
                    pivot.GamesStandings.forEach((item) => {
                        gameStandings.push(new PivotStandingsGame(item.HomeTeamId, item.AwayTeamId, item.Results));
                    });
                    result.push({ TeamsStandings: teamStandings, GamesStandings: gameStandings });
                });

                return result;
            });
    }

    getStandings(id: number): Observable<StandingsEntry[][]> {
        const url = environment.apiUrl.concat(this.standingsUrl(id));
        return this.http
            .get(url)
            .map(response => response.json() as StandingsEntry[][]);
    }

    private setTeamsPositions(teamStandings: PivotStandingsEntry[]) {
        teamStandings.forEach((item, index) => item.Position = index + 1);
    }
}
