import { Component, OnInit, Input, OnDestroy, Output, EventEmitter } from '@angular/core';

import 'rxjs/add/operator/toPromise';

import { ScheduleByRounds } from '../../Models/Schedule/ScheduleByRounds';
import { ScheduleService } from '../../Services/schedule.service';
import { GameResult } from '../../Models/Schedule/GameResult';

@Component({
    selector: 'schedule',
    templateUrl: './schedule.component.html',
    styleUrls: ['./schedule.component.scss']
})
export class ScheduleComponent implements OnInit {

    @Input() scheduleId: number;
    @Output() ready: EventEmitter<void> = new EventEmitter<void>();

    data: ScheduleByRounds[];

    groupOne: number;
    groupTwo: number;

    constructor(private scheduleService: ScheduleService) { }

    ngOnInit() {
        this.scheduleService
            .getSchedule(this.scheduleId)
            .toPromise()
            .then(data => {
                this.data = data;
                this.ready.emit();
                this._setGroupIds();
            });
    }

    gameIsPlayed(gameResult: GameResult) {
        return gameResult.AwayTeamName &&
            (!gameResult.Result.TotalScore.IsEmpty || gameResult.Result.IsTechnicalDefeat);
    }

    private _setGroupIds() {
        this.groupOne = this.data[0].ScheduleByDate[0].GameResults[0].GroupId;

        this.data[0].ScheduleByDate.forEach((item, index, arr) => {
            const gameResult = item.GameResults.find(it => it.GroupId !== this.groupOne);
            if (gameResult) {
                this.groupTwo = gameResult.GroupId;
                return;
            }
        });
    }
}
