import { Component, OnInit, Input, OnDestroy, Output, EventEmitter } from '@angular/core';

import 'rxjs/add/operator/toPromise';

import { ScheduleModel } from '../../Models/Schedule/Schedule';
import { ScheduleService } from '../../Services/schedule.service';
import { GameResult } from '../../Models/Schedule/GameResult';
import { DivisionHeader } from '../../Models/Schedule/DivisionHeader';
import { Result } from '../../Models/Schedule/Result';
import { ScheduleDay } from '../../Models/Schedule/ScheduleDay';
import { Week } from '../../Models/Schedule/Week';
import { DummyDivisionHeader } from './DummyDivisionHeader';


@Component({
    selector: 'schedule',
    templateUrl: './schedule.component.html',
    styleUrls: ['./schedule.component.scss']
})
export class ScheduleComponent implements OnInit {

    @Input() scheduleId: number;
    @Output() ready: EventEmitter<void> = new EventEmitter<void>();

    data: ScheduleModel = {} as ScheduleModel;
    divisionsIds: number[] = [];

    constructor(private scheduleService: ScheduleService) { }

    ngOnInit() {
        this.scheduleService
            .getSchedule(this.scheduleId)
            .toPromise()
            .then(data => {
                this.data = data;
                this._fillUpDivisionHeadersInDays();
                this.ready.emit();
                this._getSortedDivisionsIds();
            });
    }

    getDivisionsHeaderText(divisionHeader: DivisionHeader): string {
        if (divisionHeader.Id === DummyDivisionHeader.DummyHeaderId) {
            return '\u00A0';//$nbsp to preserve space
        } else if (divisionHeader.Id === DummyDivisionHeader.PlayOffHeaderId) {
            return divisionHeader.Rounds.join();
        }
        return `${divisionHeader.Name}: ${divisionHeader.Rounds.join()}`;
    }

    getDivisionAccentColor(divisionId: number): string {
        if (divisionId === DummyDivisionHeader.DummyHeaderId) {
            return '';
        } else if (divisionId === DummyDivisionHeader.PlayOffHeaderId) {
            return 'division7';
        }
        let index = this.divisionsIds.indexOf(divisionId);
        return `division${++index}`;
    }

    getNumberOfEmptyDivisionHeaders(day: ScheduleDay, week: Week): Array<number> {
        const maxHeaders = week.Days.map(item => item.Divisions.length).reduce((a, b) => Math.max(a, b));
        const difference = maxHeaders - day.Divisions.length;
        return difference > 0 ? new Array(difference) : new Array(0);
    }

    getGameTotalBallsScore(gameResult: Result): string {
        let result = "";

        if (!gameResult.IsTechnicalDefeat) {
            const totalHomeTeamBalls = gameResult.SetScores.map(item => item.Home).reduce((prev, next) => prev + next);
            const totalAwayTeamBalls = gameResult.SetScores.map(item => item.Away).reduce((prev, next) => prev + next);

            result = `${totalHomeTeamBalls}:${totalAwayTeamBalls}`;
        }

        return result;
    }

    isGameNumberVisible(gameResult: GameResult): boolean {
        return gameResult.GameNumber !== 0;
    }

    isVideoLinkVisible(gameResult: GameResult): boolean {
        return !!gameResult.UrlToGameVideo;
    }

    isGameResultVisible(gameResult: GameResult) {
        return gameResult.AwayTeamName &&
            gameResult.Result &&
            (!gameResult.Result.TotalScore.IsEmpty || gameResult.Result.IsTechnicalDefeat);
    }

    isGameDateVisible(gameResult: GameResult): boolean {
        return !!gameResult.GameDate &&
            !this.isFreeDayVisible(gameResult) &&
            !this.isGameResultVisible(gameResult);
    }

    isFreeDayVisible(gameResult: GameResult): boolean {
        return !gameResult.AwayTeamName;
    }

    isHomeWinner(gameResult: GameResult): boolean {
        if (!this.isGameResultVisible(gameResult)) {
            return false;
        }
        let score = gameResult.Result.TotalScore;

        return score.Home > score.Away;
    }

    isAwayWinner(gameResult: GameResult): boolean {
        if (!this.isGameResultVisible(gameResult)) {
            return false;
        }
        let score = gameResult.Result.TotalScore;

        return score.Home < score.Away;
    }

    private _getSortedDivisionsIds() {
        this.data.Schedule.forEach((week) => {
            week.Days.forEach((day) => {
                day.Divisions.forEach(division => {
                    if (division.Id !== DummyDivisionHeader.DummyHeaderId &&
                        this.divisionsIds.indexOf(division.Id) === -1) {
                        this.divisionsIds.push(division.Id);
                    }
                });
            });
        });

        this.divisionsIds.sort((a, b) => a - b);
    }

    private _fillUpDivisionHeadersInDays() {
        this.data.Schedule.forEach((week) => {
            const maxNumberOfRoundsInWeek = week.Days.map(item => item.Divisions.length).reduce(function (a, b) { return Math.max(a, b); });
            week.Days.forEach(day => {
                while (day.Divisions.length < maxNumberOfRoundsInWeek) {
                    day.Divisions.push(new DummyDivisionHeader())
                }
            });
        });
    }
}
