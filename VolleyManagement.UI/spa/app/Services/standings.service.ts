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
    private pivotStandingsUrl = id => `Tournaments/${id}/PivotStandings`;
    private standingsUrl = id => `Tournaments/${id}/Standings`;

    constructor(private http: Http) { }

    getPivotStandings(id: number): Observable<PivotStandings> {
        const url = environment.apiUrl.concat(this.pivotStandingsUrl(id));
        return this.http
            .get(url)
            .map((response: Response) => {
                const data = response.json() as PivotStandings;
                const teamStandings: PivotStandingsEntry[] = data.TeamsStandings;
                this.setTeamsPositions(teamStandings);
                const gameStandings: PivotStandingsGame[] = new Array();
                data.GamesStandings.forEach((item) => {
                    gameStandings.push(new PivotStandingsGame(item.HomeTeamId, item.AwayTeamId, item.Results));
                });

                return { TeamsStandings: teamStandings, GamesStandings: gameStandings };
            });
    }

    getStandings(id: number): Observable<StandingsEntry[]> {
        const url = environment.apiUrl.concat(this.standingsUrl(id));
        return this.http
            .get(url)
            .map(response => response.json() as StandingsEntry[]);
    }

    private setTeamsPositions(teamStandings: PivotStandingsEntry[]) {
        teamStandings.forEach((item, index) => item.Position = index + 1);
    }
}
