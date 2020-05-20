import {Inject, LOCALE_ID, Pipe, PipeTransform} from '@angular/core';
import {DatePipe} from '@angular/common';

@Pipe({name: 'appDate'})
export class AppDatePipe extends DatePipe implements PipeTransform {

    constructor(@Inject(LOCALE_ID) locale: string) {
        super(locale);
    }

    transform(value: any, format = 'mediumDate', timezone?: string, locale?: string): string {
        return super.transform(this.parseDate(value), format, timezone, locale);
    }


    // safari fix: https://stackoverflow.com/questions/6427204/date-parsing-in-javascript-is-different-between-safari-and-chrome
    parseDate(date) {
        if (date === null) {
            return '';
        }
        const parsed = Date.parse(date);
        if (!isNaN(parsed)) {
            return parsed;
        }

        return Date.parse(date.replace(/-/g, '/').replace(/[a-z]+/gi, ' '));
    }
}
