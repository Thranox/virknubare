import {Component, OnInit} from '@angular/core';
import {TravelExpense} from '../../../../shared/model/travel-expense.model';
import {from, Observable} from 'rxjs';
import {TravelExpenseService} from '../../../../shared/services/travel-expense.service';
import {ActivatedRoute} from '@angular/router';
import {map, switchMap} from "rxjs/operators";
import {MockTravelExpenseService} from "../../../../shared/mocks/mock-travel-expense.service";

@Component({
    selector: 'app-index',
    templateUrl: './details.component.html',
    styleUrls: ['./details.component.scss']
})
export class DetailsComponent implements OnInit {
    travelExpense$: Observable<TravelExpense> = from([]);
    travelExpenseIsMoved = false;

    constructor(
        private travelExpenseService: TravelExpenseService,
        private activatedRoute: ActivatedRoute,
    ) {
    }

    ngOnInit(): void {
        this.travelExpense$ = this.activatedRoute.paramMap.pipe(
            map((param) => param.get('id')),
            switchMap((id: string) => this.travelExpenseService.getTravelExpenseById(id))
        );
    }


    moveToFlowStep(travelExpense, flowStep) {
        this.travelExpenseService.processStep(travelExpense, flowStep).subscribe(response => {
            console.log('Got response from processStep:', response);
            this.travelExpenseIsMoved = true;
        });
    }

}
