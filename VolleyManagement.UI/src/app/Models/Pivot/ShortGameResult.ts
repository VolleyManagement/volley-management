import { CssClassConstants } from '../../Constants/CssClassConstants';
import { Constants } from '../../Constants/Constants';

export class ShortGameResult {
    public FormattedResult: string;
    public CssClass: string;

    constructor(
        public HomeSetsScore: number,
        public AwaySetsScore: number,
        public IsTechnicalDefeat: boolean
    ) {
        this.FormattedResult = ShortGameResult.formattedResult(this.HomeSetsScore,
            this.AwaySetsScore,
            this.IsTechnicalDefeat);

        this.CssClass = ShortGameResult.getCssClass(this.HomeSetsScore,
            this.AwaySetsScore);
    }

    public static getNonPlayableCell(): ShortGameResult {
        const nonPlayableCell = new ShortGameResult(0, 0, false);
        nonPlayableCell.FormattedResult = CssClassConstants.NON_PLAYABLE_CELL;
        nonPlayableCell.CssClass = CssClassConstants.NON_PLAYABLE_CELL;
        return nonPlayableCell;
    }
    private static getCssClass(homeScore: number, awayScore: number): string {
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
}
