import { ShortGameResult } from './ShortGameResult';
import { CssClassConstants } from '../../Constants/CssClassConstants';
import { Constants } from '../../Constants/Constants';

export class PivotStandingsGame {
    constructor(
        public HomeTeamId: number,
        public AwayTeamId: number,
        public Results: ShortGameResult
    ) { }

    public static getNonPlayableCell(): PivotStandingsGame {
        return new PivotStandingsGame(
            0,
            0,
            new ShortGameResult(
                0,
                0,
                false,
                CssClassConstants.NON_PLAYABLE_CELL,
                CssClassConstants.NON_PLAYABLE_CELL));
    }

    private static getCssClass(homeScore?: number, awayScore?: number): string {
        let cssClass = CssClassConstants.NORESULT;
        const setDifference = homeScore - awayScore;

        switch (setDifference) {
            case 3:
                cssClass = CssClassConstants.WIN_3_0;
                break;
            case 2:
                cssClass = CssClassConstants.WIN_3_1;
                break;
            case 1:
                cssClass = CssClassConstants.WIN_3_2;
                break;
            case -1:
                cssClass = CssClassConstants.LOSS_2_3;
                break;
            case -2:
                cssClass = CssClassConstants.LOSS_1_3;
                break;
            case -3:
                cssClass = CssClassConstants.LOSS_0_3;
                break;
        }

        return cssClass;
    }

    private static formattedResult(homeSetsScore: number, awaySetsScore: number, isTechnicalDefeat: boolean): string {
        let result = '';
        if (homeSetsScore !== Constants.ZERO || awaySetsScore !== Constants.ZERO) {
            result = homeSetsScore + ' : ' + awaySetsScore;
            if (isTechnicalDefeat) {
                result += '*';
            }
        }

        return result;
    }

    public clone(): PivotStandingsGame {
        return new PivotStandingsGame(
            this.HomeTeamId,
            this.AwayTeamId,
            new ShortGameResult(
                this.Results.HomeSetsScore,
                this.Results.AwaySetsScore,
                this.Results.IsTechnicalDefeat,
                PivotStandingsGame.formattedResult(
                    this.Results.HomeSetsScore,
                    this.Results.AwaySetsScore,
                    this.Results.IsTechnicalDefeat),
                PivotStandingsGame.getCssClass(
                    this.Results.HomeSetsScore,
                    this.Results.AwaySetsScore)));
    }


    public transposeResult(): PivotStandingsGame {
        return new PivotStandingsGame(
            this.AwayTeamId,
            this.HomeTeamId,
            new ShortGameResult(
                this.Results.AwaySetsScore,
                this.Results.HomeSetsScore,
                this.Results.IsTechnicalDefeat,
                PivotStandingsGame.formattedResult(
                    this.Results.AwaySetsScore,
                    this.Results.HomeSetsScore,
                    this.Results.IsTechnicalDefeat),
                PivotStandingsGame.getCssClass(
                    this.Results.AwaySetsScore,
                    this.Results.HomeSetsScore)));
    }
}
