import { Directive, Input, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { ShortGameResult } from '../../Models/Pivot/ShortGameResult';
import { CSS_CLASSES } from '../../Constants/CssClassConstants';
import { APP_CONSTANTS } from '../../Constants/Constants';

@Directive({
    selector: '[vmGameResultCell]'
})
export class GameResultCellDirective implements OnInit {

    @Input()
    gameResult: ShortGameResult;

    constructor(private _renderer: Renderer2, private _el: ElementRef) { }

    ngOnInit(): void {
        this._renderer.addClass(this._el.nativeElement, this._getCssClass());
    }

    private _getCssClass(): string {

        let cssClass = CSS_CLASSES.NORESULT;

        if (this.gameResult) {
            if (this._isNonPlayableCell(this.gameResult)) {
                cssClass = CSS_CLASSES.NON_PLAYABLE_CELL;
            } else {
                cssClass = this._getCssClassFromScore(this.gameResult);
            }
        }

        return cssClass;
    }

    private _isNonPlayableCell(result: ShortGameResult) {
        return !result.HomeSetsScore && !result.AwaySetsScore && result.RoundNumber === APP_CONSTANTS.ZERO;
    }

    private _getCssClassFromScore(result: ShortGameResult) {
        let cssClass = CSS_CLASSES.NORESULT;

        const homeScore = result.HomeSetsScore;
        const awayScore = result.AwaySetsScore;

        if (homeScore || awayScore) {
            const setDifference = homeScore - awayScore;

            switch (setDifference) {
                case 3:
                    cssClass = CSS_CLASSES.WIN_3_0;
                    break;
                case 2:
                    cssClass = CSS_CLASSES.WIN_3_1;
                    break;
                case 1:
                    cssClass = CSS_CLASSES.WIN_3_2;
                    break;
                case -1:
                    cssClass = CSS_CLASSES.LOSS_2_3;
                    break;
                case -2:
                    cssClass = CSS_CLASSES.LOSS_1_3;
                    break;
                case -3:
                    cssClass = CSS_CLASSES.LOSS_0_3;
                    break;
            }
        }

        return cssClass;
    }
}
