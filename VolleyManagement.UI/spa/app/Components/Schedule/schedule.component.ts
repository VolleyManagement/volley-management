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

    private subscription: ISubscription;

    constructor(private scheduleService: ScheduleService) { }

    ngOnInit() {
        this.subscription = this.scheduleService
            .getSchedule(this.scheduleId)
            .subscribe(scheduleByRounds => this.gameResults = scheduleByRounds);
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
    }
}
