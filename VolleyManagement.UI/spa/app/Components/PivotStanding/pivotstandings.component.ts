import { Component, Directive, OnInit, Input, Output, EventEmitter } from '@angular/core';

import 'rxjs/add/operator/toPromise';

import { StandingsService } from '../../Services/standings.service';
import { PivotStandings } from '../../Models/Pivot/PivotStandings';
import { PivotStandingsGame } from '../../Models/Pivot/PivotStandingsGame';
import { ShortGameResult } from '../../Models/Pivot/ShortGameResult';

import { CSS_CLASSES } from '../../Constants/CssClassConstants';
import { APP_CONSTANTS } from '../../Constants/Constants';

const NON_PLAYABLE_GAME_RESULT: ShortGameResult = {
    IsTechnicalDefeat: false,
    RoundNumber: APP_CONSTANTS.ZERO
};


@Component({
    selector: 'pivottable',
    templateUrl: './pivotstandings.component.html',
    styleUrls: ['./pivotstandings.component.scss']
})
export class PivotStandingsComponent implements OnInit {

    @Input() pivotId: number;
    @Output() ready: EventEmitter<void> = new EventEmitter<void>();

    pivotStandings: PivotStandings[];
    pivotTable: PivotStandingsGame[][][];

    constructor(private _standingsService: StandingsService) { }

    ngOnInit(): void {
        this._standingsService
            .getPivotStandings(this.pivotId)
            .toPromise()
            .then(data => {
                this.pivotStandings = data;
                this.pivotTable = data.map(item => this.getPivotTable(item));
                this.ready.emit();
            });
    }


    getPivotTable(pivot: PivotStandings): PivotStandingsGame[][] {
        const teamsCount = pivot.TeamsStandings.length;

        const table = Array.apply(null, Array(teamsCount)).map(() => {
            return new Array(teamsCount);
        });

        for (let i = 0; i < teamsCount; i++) {
            for (let j = 0; j < teamsCount; j++) {
                const tableCell = table[i][j];
                if (!tableCell) {
                    if (i === j) {
                        table[i][j] = this._getNonPlayableCell();
                    } else {
                        const rowTeamId = pivot.TeamsStandings[i].TeamId;
                        const colTeamId = pivot.TeamsStandings[j].TeamId;

                        const homeGameResult = pivot.GamesStandings
                            .find(game => (game.HomeTeamId === rowTeamId && game.AwayTeamId === colTeamId));
                        const awayGameResult = pivot.GamesStandings
                            .find(game => (game.HomeTeamId === colTeamId && game.AwayTeamId === rowTeamId));

                        if (homeGameResult) {
                            table[i][j] = homeGameResult.clone();
                            table[j][i] = homeGameResult.transposeResult();
                        } else if (awayGameResult) {
                            table[i][j] = awayGameResult.transposeResult();
                            table[j][i] = awayGameResult.clone();
                        }
                    }
                }
            }
        }

        return table;
    }

    getFormattedResult(gameResult: ShortGameResult): string {
        const homeSetsScore = gameResult.HomeSetsScore;
        const awaySetsScore = gameResult.AwaySetsScore;
        const isTechnicalDefeat = gameResult.IsTechnicalDefeat;

        let result = '';
        if (gameResult.RoundNumber > 0 && !homeSetsScore && !awaySetsScore) {
            result = `Тур ${gameResult.RoundNumber}`;
        } else if (homeSetsScore || awaySetsScore) {
            result = `${homeSetsScore} : ${awaySetsScore}${isTechnicalDefeat ? '*' : ''}`;
        }

        return result;
    }

    private _getNonPlayableCell(): PivotStandingsGame {
        return new PivotStandingsGame(
            0,
            0,
            [NON_PLAYABLE_GAME_RESULT]);
    }
}
