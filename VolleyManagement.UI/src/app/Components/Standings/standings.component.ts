import { Component, OnChanges, Input } from '@angular/core';
import { ParamMap } from '@angular/router';
import { DecimalPipe } from '@angular/common';
import { StandingsService } from '../../Services/standings.service';
import { StandingsEntry } from '../../Models/Standings/StandingsEntry';

import 'rxjs/add/operator/switchMap';

@Component({
    selector: 'standings-component',
    templateUrl: './standings.component.html',
    styleUrls: ['./standings.component.css']
})

export class StandingsComponent implements OnChanges {

    @Input() standingsId: number;
    standingsEntry: StandingsEntry[];

    constructor(private standingsService: StandingsService) { }

    ngOnChanges(): void {
        if (this.standingsId) {
            this.standingsService.getStandings(this.standingsId)
                .subscribe(standings => {
                    this.standingsEntry = standings;
                });
        }
    }
}
