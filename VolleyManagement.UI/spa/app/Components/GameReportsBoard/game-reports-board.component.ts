import { Component, OnInit } from '@angular/core';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/toPromise';

import { TournamentMetadataJson } from '../../Models/TournamentMetadataJson/TournamentMetadataJson';
import { TournamentDataService } from '../../Services/tournament-data.service';

@Component({
    selector: 'vm-game-reports-board',
    templateUrl: './game-reports-board.component.html',
    styleUrls: ['./game-reports-board.component.scss']
})
export class GameReportsBoardComponent implements OnInit {

    reportsStates = {
        standings: true,
        pivot: true,
        schedule: true
    };

    pivotId: number;
    standingsId: number;
    scheduleId: number;

    get isShowLoader() {
        return !(this.reportsStates.standings
            && this.reportsStates.pivot
            && this.reportsStates.schedule);
    }

    constructor(
        private _dataService: TournamentDataService) { }

    ngOnInit(): void {
        this._dataService
            .getTournamentMetadata()
            .toPromise()
            .then(data => this.updateIds(data.mode, data.id));
    }

    onReportReady(report: string) {
        this.reportsStates[report] = true;
    }

    private updateIds(modes: string[], id: number): void {
        modes.forEach(mode => {
            switch (mode) {
                case 'pivot':
                    this.pivotId = id;
                    this.reportsStates.pivot = false;
                    break;
                case 'standings':
                    this.standingsId = id;
                    this.reportsStates.standings = false;
                    break;
                case 'schedule':
                    this.scheduleId = id;
                    this.reportsStates.schedule = false;
                    break;
            }
        });
    }
}
