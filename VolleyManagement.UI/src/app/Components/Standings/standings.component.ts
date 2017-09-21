import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { StandingsService } from '../../Services/standings.service';
import { StandingsEntry } from '../../Models/Standings/StandingsEntry';
import { FormatterHelper } from '../../Helpers/FormatterHelper';

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
        private route: ActivatedRoute,
        private formatter: FormatterHelper) { }

    ngOnInit(): void {
        this.route.paramMap
            .switchMap((params: ParamMap) => this.standingsService.getStandings(+params.get('id')))
            .subscribe(standings => {
                this.standingsEntry = standings;
                this.standingsEntry.forEach(entry => {
                    entry.SetsRatioText = this.formatter.formatDecimal(entry.SetsRatio);
                    entry.BallsRatioText = this.formatter.formatDecimal(entry.BallsRatio);
                });
            });
    }
}
