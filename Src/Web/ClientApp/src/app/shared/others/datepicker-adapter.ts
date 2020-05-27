import {Injectable} from '@angular/core';
import {NgbDateAdapter, NgbDateStruct} from '@ng-bootstrap/ng-bootstrap';
import * as dayjs from 'dayjs';
import {Dayjs} from 'dayjs';

/**
 * Example of a String Time adapter
 */
@Injectable()
export class NgbDateDayjsAdapter extends NgbDateAdapter<Dayjs> {

    fromModel(value: Dayjs): NgbDateStruct {
        if (!value) {
            return null;
        }
        return {
            year: value.year(),
            month: value.month() + 1,
            day: value.date()
        };
    }

    toModel(date: NgbDateStruct): Dayjs {
        if (!date) {
            return null;
        }
        return dayjs().startOf('day').date(date.day).month(date.month - 1).year(date.year);
    }
}
