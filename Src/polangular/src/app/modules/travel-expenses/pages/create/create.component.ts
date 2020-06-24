import {Component, OnInit} from '@angular/core';
import {TravelExpense} from '../../../../shared/model/travel-expense.model';
import {from, Observable} from 'rxjs';
import {MockTravelExpenseService} from "../../../../shared/mocks/mock-travel-expense.service";

@Component({
    selector: 'app-index',
    templateUrl: './create.component.html',
    styleUrls: ['./create.component.scss']
})
export class CreateComponent implements OnInit {
    travelExpense$: Observable<TravelExpense> = from([]);
    travelExpense: TravelExpense;

    constructor(
        private travelExpenseService: MockTravelExpenseService,
    ) {
    }

    ngOnInit(): void {

        this.travelExpense = new TravelExpense();

        /*this.travelExpense$ = this.activatedRoute.paramMap.pipe(
            map((param) => param.get('id')),
            switchMap((id: string) => this.travelExpenseService.getTravelExpenseById(id))
        );*/
    }

}
