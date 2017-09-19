import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { StandingsService } from '../../Services/standings.service';
import { PivotStandings } from '../../Models/Pivot/PivotStandings';
import { PivotStandingsGame } from '../../Models/Pivot/PivotStandingsGame';
import { ShortGameResult } from '../../Models/Pivot/ShortGameResult';
import 'rxjs/add/operator/switchMap';
import { FormatterHelper } from '../../Helpers/FormatterHelper';

@Component({
    selector: 'app-pivottable-component',
    templateUrl: './pivotstandings.component.html',
    styleUrls: ['./pivotstandings.component.css']
})

export class PivotStandingsComponent implements OnInit {
    pivotStandings: PivotStandings;
    pivotTable: PivotStandingsGame[][];

    constructor(
        private standingsService: StandingsService,
        private route: ActivatedRoute,
        private formatter: FormatterHelper) { }

    ngOnInit(): void {
        this.route.paramMap
            .switchMap((params: ParamMap) => this.standingsService.getPivotStandings(+params.get('id')))
            .subscribe(standings => {
                this.pivotStandings = standings;
                this.pivotStandings.TeamsStandings.forEach(entry => {
                    entry.SetsRatioText = this.formatter.setRatioText(entry.SetsRatio);
                });
                this.pivotTable = this.getPivotTable();
            });
    }

    getPivotTable(): PivotStandingsGame[][] {
        const teamsCount = this.pivotStandings.TeamsStandings.length;
        const table = new Array(teamsCount);
        for (let i = 0; i < teamsCount; i++) {
            table[i] = new Array(teamsCount);
        }
        for (let i = 0; i < teamsCount; i++) {
            for (let j = 0; j < teamsCount; j++) {
                const tableCell = table[i][j];
                if (tableCell === undefined || tableCell === null) {
                    if (i === j) {
                        table[i][j] = PivotStandingsGame.getNonPlayableCell();
                    } else {
                        const rowTeamId = this.pivotStandings.TeamsStandings[i].TeamId;
                        const colTeamId = this.pivotStandings.TeamsStandings[j].TeamId;
                        const homeGameResult = this.pivotStandings.GamesStandings.find(function (game) {
                            return (game.HomeTeamId === rowTeamId && game.AwayTeamId === colTeamId);
                        });
                        const awayGameResult = this.pivotStandings.GamesStandings.find(function (game) {
                            return (game.HomeTeamId === colTeamId && game.AwayTeamId === rowTeamId);
                        });

                        if (homeGameResult !== undefined && homeGameResult !== null) {
                            table[i][j] = PivotStandingsGame.create(homeGameResult, false);
                            table[j][i] = PivotStandingsGame.create(homeGameResult, true);
                        } else if (awayGameResult !== undefined && awayGameResult !== null) {
                            table[i][j] = PivotStandingsGame.create(awayGameResult, true);
                            table[j][i] = PivotStandingsGame.create(awayGameResult, false);
                        }
                    }
                }
            }
        }
        return table;
    }
}
