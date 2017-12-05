import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

import { Observable } from 'rxjs/Observable';

import { AppToolsService } from './app-tools.service';
import { JsonService } from './json.service';
import { TournamentMetadataJson } from '../Models/TournamentMetadataJson/TournamentMetadataJson';

@Injectable()
export class TournamentDataService {

    constructor(
        private _jsonService: JsonService,
        private _appTools: AppToolsService) { }

    getTournamentMetadata(): Observable<TournamentMetadataJson> {
        const fileName = this._appTools.getAppMetadataFileName();
        const url = environment.jsonBaseUrl.concat(fileName);
        return this._jsonService.getJson<TournamentMetadataJson>(url);
    }
}
