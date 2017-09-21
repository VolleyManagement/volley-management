import { Injectable } from '@angular/core';
import { Constants } from '../Constants/Constants';

@Injectable()
export class FormatterHelper {
    constructor() { }
    private static getFormatter(): Intl.NumberFormat {
        const language = navigator.language || Constants.DEFAULT_LOCALE;
        return new Intl.NumberFormat(language, {
            style: 'decimal',
            minimumFractionDigits: Constants.ZERO,
            maximumFractionDigits: Constants.THREE
        });
    }

    formatDecimal(value: number): string {
        const formatter = FormatterHelper.getFormatter();

        return isFinite(value) ?
            formatter.format(value) :
            'MAX';
    }
}
