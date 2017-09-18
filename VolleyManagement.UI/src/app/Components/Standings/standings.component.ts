import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { StandingsService } from '../../Services/standings.service';
import { StandingsEntry } from '../../Models/Standings/StandingsEntry';
import 'rxjs/add/operator/switchMap';


@Component({
    selector: 'standings',
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
                this.standingsEntry.forEach(entry => {
                    this.setRatioText(entry);
                });
            });
    }

    private setRatioText(entry: StandingsEntry): void {
        entry.SetsRatioText = this.ratioText(entry.SetsRatio);
        entry.BallsRatioText = this.ratioText(entry.BallsRatio);
    }

    private ratioText(value: number): string {
        const formatter = this.getFormatter();

        return isFinite(value) ?
            formatter.format(value) :
            'MAX';
    }

    private getFormatter(): Intl.NumberFormat {
        const language = navigator.language || 'uk-UA';
        return new Intl.NumberFormat(language, {
            style: 'decimal',
            minimumFractionDigits: 1,
            maximumFractionDigits: 3
        });
    }
}
