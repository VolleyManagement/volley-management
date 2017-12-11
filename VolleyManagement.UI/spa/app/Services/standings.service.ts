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
import { JsonService } from './json.service';
import { DivisionStandings } from '../Models/Standings/DivisionStandings';

@Injectable()
export class StandingsService {

    private pivotStandingsUrl = id => `api/v1/Tournaments/${id}/PivotStandings`;
    private standingsUrl = id => `api/v1/Tournaments/${id}/Standings`;

    constructor(private jsonService: JsonService) { }

    getPivotStandings(id: number): Observable<PivotStandings[]> {
        const url = environment.apiUrl.concat(this.pivotStandingsUrl(id));

        return this.jsonService
            .getJson<PivotStandings[]>(url)
            .map((data: PivotStandings[]) => {

                return data.map(pivot => ({
                    LastUpdateTime: pivot.LastUpdateTime,
                    TeamsStandings: pivot.TeamsStandings.map((item, index) => ({
                        ...item,
                        Position: index + 1
                    })),
                    GamesStandings: pivot.GamesStandings.map(item =>
                        new PivotStandingsGame(
                            item.HomeTeamId,
                            item.AwayTeamId,
                            item.Results))
                }));
            });
    }

    getStandings(id: number): Observable<DivisionStandings[]> {
        const url = environment.apiUrl.concat(this.standingsUrl(id));

        return this.jsonService.getJson<DivisionStandings[]>(url);
    }
}
