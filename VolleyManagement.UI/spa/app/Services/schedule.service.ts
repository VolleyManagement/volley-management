import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

import { Observable } from 'rxjs/Observable';

import { JsonService } from './json.service';

import { ScheduleByRounds } from '../Models/Schedule/ScheduleByRounds';

@Injectable()
export class ScheduleService {
    private scheduleUrl = id => `api/Tournament/${id}/Schedule`;

    constructor(private _jsonService: JsonService) { }

    getSchedule(id: number): Observable<ScheduleByRounds[]> {
        const url = environment.apiUrl.concat(this.scheduleUrl(id));
        return this._jsonService
            .getJson<ScheduleByRounds[]>(url);
    }
}
