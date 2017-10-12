import { Pipe, PipeTransform } from '@angular/core';
import { DecimalPipe } from '@angular/common';

@Pipe({
    name: 'customNumber'
})
export class InfinityDecimalPipe extends DecimalPipe implements PipeTransform {
    transform(value: string, digits?: string): any {
        return value === 'Infinity' ? 'Max' : super.transform(value, digits);
    }
}
