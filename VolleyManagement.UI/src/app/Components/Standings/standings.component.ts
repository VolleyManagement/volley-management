import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { DecimalPipe } from '@angular/common';
import { StandingsService } from '../../Services/standings.service';
import { StandingsEntry } from '../../Models/Standings/StandingsEntry';

import 'rxjs/add/operator/switchMap';

@Component({
    selector: 'app-standings-component',
    templateUrl: './standings.component.html',
    styleUrls: ['./standings.component.css']
})

export class StandingsComponent {
    standingsEntry: StandingsEntry[];

    constructor(
        private standingsService: StandingsService,
        private route: ActivatedRoute) { }

    ngOnInit(): void {
        this.route.paramMap
            .switchMap((params: ParamMap) => this.standingsService.getStandings(+params.get('id')))
            .subscribe(standings => {
                this.standingsEntry = standings;
            });
    }
}
