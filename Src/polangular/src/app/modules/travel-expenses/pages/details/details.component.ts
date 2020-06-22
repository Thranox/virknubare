import {Component, OnInit} from '@angular/core';
import {TravelExpense} from '../../../../shared/model/travel-expense.model';
import {from, Observable, of} from 'rxjs';
import {TravelExpenseService} from '../../../../shared/services/travel-expense.service';
import {ActivatedRoute, Router} from '@angular/router';
import {catchError, map, switchMap} from "rxjs/operators";
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
        private router: Router,
    ) {
    }

    ngOnInit(): void {
        this.travelExpense$ = this.activatedRoute.paramMap.pipe(
            map((param) => param.get('id')),
            switchMap((id: string) => this.travelExpenseService.getTravelExpenseById(id)),
            catchError((error, caught) => {
                if (error.status === 404 || error.status === 400) {
                    return this.router.navigate(['404']);
                }
                console.dir(error);
                console.dir(caught);
            }),
        );
    }


    moveToFlowStep(travelExpense, flowStep) {
        this.travelExpenseService.processStep(travelExpense, flowStep).subscribe(response => {
            console.log('Got response from processStep:', response);
            this.travelExpenseIsMoved = true;
        });
    }

}
