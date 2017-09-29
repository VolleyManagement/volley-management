import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { environment } from '../../environments/environment';

import 'rxjs/add/operator/map';
import { Observable } from 'rxjs/Observable';

import { GameResult } from '../Models/Schedule/GameResult';

@Injectable()
export class ScheduleService {
    private scheduleUrl = id => `Tournament/${id}/Schedule`;

    constructor(private http: Http) { }

    getSchedule(id: number): Observable<GameResult[]> {
        const url = environment.apiUrlNoVersion.concat(this.scheduleUrl(id));
        return this.http
            .get(url)
            .map(response => response.json() as GameResult[]);
    }
}
