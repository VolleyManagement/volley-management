import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { environment } from '../../environments/environment';

import 'rxjs/add/operator/map';
import { Observable } from 'rxjs/Observable';

import { ScheduleByRounds } from '../Models/Schedule/ScheduleByRounds';
import { GameResult } from '../Models/Schedule/GameResult';

@Injectable()
export class ScheduleService {
    private scheduleUrl = id => `api/Tournament/${id}/Schedule`;

    constructor(private http: Http) { }

    getSchedule(id: number): Observable<ScheduleByRounds[]> {
        const url = environment.apiUrl.concat(this.scheduleUrl(id));
        return this.http
            .get(url)
            .map(response => {
                const data = response.json() as ScheduleByRounds[];
                let scheduleByRounds: ScheduleByRounds[] = new Array();
                scheduleByRounds = data.map(d => ({
                    Round: d.Round,
                    GameResults: d.GameResults.map(game => new GameResult(game.Id,
                        game.HomeTeamName,
                        game.AwayTeamName,
                        game.GameDate,
                        game.Round,
                        game.Result))
                }));

                return scheduleByRounds;
            });
    }
}
