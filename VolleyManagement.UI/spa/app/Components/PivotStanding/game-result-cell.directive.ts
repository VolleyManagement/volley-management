import { Directive, Input, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { ShortGameResult } from '../../Models/Pivot/ShortGameResult';
import { CSS_CLASSES } from '../../Constants/CssClassConstants';

@Directive({
    selector: '[vmGameResultCell]'
})
export class GameResultCellDirective implements OnInit {

    // tslint:disable-next-line:no-input-rename
    @Input()
    gameResult: ShortGameResult;

    constructor(private _renderer: Renderer2, private _el: ElementRef) { }

    ngOnInit(): void {
        this._renderer.addClass(this._el.nativeElement, this._getCssClass());
    }

    private _getCssClass(): string {

        let cssClass = CSS_CLASSES.NORESULT;

        if (this.gameResult) {
            const homeScore = this.gameResult.HomeSetsScore;
            const awayScore = this.gameResult.AwaySetsScore;
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
