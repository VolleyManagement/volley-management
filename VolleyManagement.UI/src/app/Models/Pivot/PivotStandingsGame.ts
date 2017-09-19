import { ShortGameResult } from './ShortGameResult';
import { CssClassConstants } from '../../Constants/CssClassConstants';
import { Constants } from '../../Constants/Constants';

export class PivotStandingsGame {
    constructor(
        public HomeTeamId: number,
        public AwayTeamId: number,
        public Results: ShortGameResult
    ) { }

    public static create(gameResult: PivotStandingsGame, isReverseGame: boolean): PivotStandingsGame {
        if (isReverseGame) {
            return new PivotStandingsGame(
                gameResult.AwayTeamId,
                gameResult.HomeTeamId,
                new ShortGameResult(
                    gameResult.Results[0].AwaySetsScore,
                    gameResult.Results[0].HomeSetsScore,
                    gameResult.Results[0].IsTechnicalDefeat,
                    PivotStandingsGame.formattedResult(
                        gameResult.Results[0].AwaySetsScore,
                        gameResult.Results[0].HomeSetsScore,
                        gameResult.Results[0].IsTechnicalDefeat),
                    PivotStandingsGame.getCssClass(
                        gameResult.Results[0].AwaySetsScore,
                        gameResult.Results[0].HomeSetsScore)));
        }
        return new PivotStandingsGame(
            gameResult.HomeTeamId,
            gameResult.AwayTeamId,
            new ShortGameResult(
                gameResult.Results[0].HomeSetsScore,
                gameResult.Results[0].AwaySetsScore,
                gameResult.Results[0].IsTechnicalDefeat,
                PivotStandingsGame.formattedResult(
                    gameResult.Results[0].HomeSetsScore,
                    gameResult.Results[0].AwaySetsScore,
                    gameResult.Results[0].IsTechnicalDefeat),
                PivotStandingsGame.getCssClass(
                    gameResult.Results[0].HomeSetsScore,
                    gameResult.Results[0].AwaySetsScore)));
    }

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
        if (homeScore === Constants.THREE && awayScore === Constants.ZERO) {
            cssClass = CssClassConstants.WIN_3_0;
        } else if (homeScore === Constants.THREE && awayScore === Constants.ONE) {
            cssClass = CssClassConstants.WIN_3_1;
        } else if (homeScore === Constants.THREE && awayScore === Constants.TWO) {
            cssClass = CssClassConstants.WIN_3_2;
        } else if (homeScore === Constants.TWO && awayScore === Constants.THREE) {
            cssClass = CssClassConstants.LOSS_2_3;
        } else if (homeScore === Constants.ONE && awayScore === Constants.THREE) {
            cssClass = CssClassConstants.LOSS_1_3;
        } else if (homeScore === Constants.ZERO && awayScore === Constants.THREE) {
            cssClass = CssClassConstants.LOSS_0_3;
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
}
