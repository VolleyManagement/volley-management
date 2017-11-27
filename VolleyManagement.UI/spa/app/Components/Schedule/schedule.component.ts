import { Component, OnInit, Input, OnDestroy, ViewEncapsulation } from '@angular/core';
import { DatePipe } from '@angular/common';
import { ISubscription } from 'rxjs/Subscription';

import { ScheduleByRounds } from '../../Models/Schedule/ScheduleByRounds';
import { ScheduleService } from '../../Services/schedule.service';

@Component({
    selector: 'schedule',
    templateUrl: './schedule.component.html',
    styleUrls: ['./schedule.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ScheduleComponent implements OnInit, OnDestroy {

    @Input() scheduleId: number;

    gameResults: ScheduleByRounds[];
    groupOne: number;
    groupTwo: number;
    private subscription: ISubscription;

    constructor(private scheduleService: ScheduleService) { }

    ngOnInit() {
        this.subscription = this.scheduleService
            .getSchedule(this.scheduleId)
            .subscribe(scheduleByRounds => {
                this.gameResults = scheduleByRounds;
                this.GetGroupIds();
            });
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
    }

    private GetGroupIds() {
        this.groupOne = this.gameResults[0].ScheduleByDate[0].GameResults[0].GroupId;
        this.gameResults[0].ScheduleByDate.forEach((item, index, arr) => {
            const gameResult = item.GameResults.find(it => it.GroupId !== this.groupOne);
            if (gameResult) {
                this.groupTwo  = gameResult.GroupId;
                return;
            }
        });
    }
}
