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
            this.allDatesFromStartDateToEndDate.push(moment(currentDate).format('DD-MMM, YYYY'));
            currentDate = moment(currentDate).add(1, 'days');
        }
    }

    getEndDateInMilliseconds() {
        return this.endDateTime.getTime()
    }

    getStartDateInMilliseconds() {
        return this.startDateTime.getTime()
    }

    checkIfTripIsUnder24Hours() {
        // 86400000 milliseconds is equal to 24 hours
        return this.getEndDateInMilliseconds() - this.getStartDateInMilliseconds() < 86400000    
    }
}
