import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'localDate'
})
export class LocalDatePipe implements PipeTransform {

    transform(value: string): any {

        if (!value) {
            return '';
        }

        const dateValue = new Date(value);

        const newDate = new Date(dateValue.getTime() + dateValue.getTimezoneOffset() * 60 * 1000);

        const offset = dateValue.getTimezoneOffset() / 60;
        const hours = dateValue.getHours();

        newDate.setHours(hours - offset);

        return newDate;
    }
}
