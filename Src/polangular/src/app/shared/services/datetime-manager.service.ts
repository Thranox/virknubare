import { Injectable } from '@angular/core';
import * as moment from 'moment';

@Injectable({
  providedIn: 'root'
})
export class DateTimeManagerService {
    startDateTime: Date
    endDateTime: Date
    allDatesFromStartDateToEndDate = []

    constructor() { }

    getAllDatesFromStartDateToEndDate() {
        var currentDate = moment(this.startDateTime)
        var stopDate = moment(this.endDateTime)
        while (currentDate < stopDate) {
            this.allDatesFromStartDateToEndDate.push(currentDate);
            currentDate = moment(currentDate).add(1, 'days');
        }
    }

    getEndDateInMilliseconds() {
        return this.endDateTime.getTime() + (this.endDateTime.getUTCMinutes() * 60000)
    }

    getStartDateInMilliseconds() {
        return this.startDateTime.getTime() + (this.startDateTime.getUTCMinutes() * 60000)
    }

    checkIfTripIsUnder24Hours() {
        // 86400000 milliseconds is equal to 24 hours
        return this.getEndDateInMilliseconds() - this.getStartDateInMilliseconds() < 86400000
    }
}
