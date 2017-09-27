import { Component, OnChanges, Input, OnDestroy } from '@angular/core';
import { ParamMap } from '@angular/router';
import { DecimalPipe } from '@angular/common';
import { ISubscription } from 'rxjs/Subscription';

import { StandingsService } from '../../Services/standings.service';
import { StandingsEntry } from '../../Models/Standings/StandingsEntry';

import 'rxjs/add/operator/switchMap';

@Component({
    selector: 'standings',
    templateUrl: './standings.component.html',
    styleUrls: ['./standings.component.css']
})

export class StandingsComponent implements OnChanges, OnDestroy {

    @Input() standingsId: number;
    standingsEntry: StandingsEntry[];

    private subscription: ISubscription;

    constructor(private standingsService: StandingsService) { }

    ngOnChanges(): void {
        if (this.standingsId) {
            this.subscription = this.standingsService.getStandings(this.standingsId)
                .subscribe(standings => {
                    this.standingsEntry = standings;
                });
        }
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
    }
}
