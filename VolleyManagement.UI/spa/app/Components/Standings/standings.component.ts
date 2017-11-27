import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { ParamMap } from '@angular/router';
import { DecimalPipe } from '@angular/common';

import { ISubscription } from 'rxjs/Subscription';
import 'rxjs/add/operator/switchMap';

import { StandingsService } from '../../Services/standings.service';
import { StandingsEntry } from '../../Models/Standings/StandingsEntry';


@Component({
    selector: 'standings',
    templateUrl: './standings.component.html',
    styleUrls: ['./standings.component.scss']
})

export class StandingsComponent implements OnInit, OnDestroy {

    @Input() standingsId: number;

    standingsEntry: StandingsEntry[][];

    private subscription: ISubscription;

    constructor(private standingsService: StandingsService) { }

    ngOnInit(): void {
        if (this.standingsId) {
            this.subscription = this.standingsService
                .getStandings(this.standingsId)
                .subscribe(standings => this.standingsEntry = standings);
        }
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
    }
}
