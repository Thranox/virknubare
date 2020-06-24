import {Component, OnInit} from '@angular/core';
import {TravelExpense} from '../../../../shared/model/travel-expense.model';
import {from, Observable, of} from 'rxjs';
import {TravelExpenseService} from '../../../../shared/services/travel-expense.service';
import {ActivatedRoute, Router} from '@angular/router';
import {catchError, map, switchMap, tap} from "rxjs/operators";
import {MockTravelExpenseService} from "../../../../shared/mocks/mock-travel-expense.service";
import {FormBuilder, FormGroup} from "@angular/forms";

@Component({
    selector: 'app-index',
    templateUrl: './details.component.html',
    styleUrls: ['./details.component.scss']
})
export class DetailsComponent implements OnInit {
    travelExpense$: Observable<TravelExpense> = from([]);
    travelExpenseIsMoved = false;
    travelExpenseForm: FormGroup;

    constructor(
        private travelExpenseService: TravelExpenseService,
        private activatedRoute: ActivatedRoute,
        private router: Router,
        private formBuilder: FormBuilder
    ) {
        this.travelExpenseForm = this.formBuilder.group({
            travelExpense: [],
        });
    }

    ngOnInit(): void {
        this.activatedRoute.paramMap.pipe(
            map((param) => param.get('id')),
            switchMap((id: string) => this.travelExpenseService.getTravelExpenseById(id)),
            tap((travelExpense) => {
                this.travelExpenseForm.get('travelExpense').patchValue(travelExpense);
            }),
            catchError((error, caught) => {
                if (error.status === 404 || error.status === 400) {
                    this.router.navigate(['404']);
                    return of(new TravelExpense());
                }
                console.dir(error);
                console.dir(caught);
            }),
        ).subscribe();
    }


    moveToFlowStep(travelExpense, flowStep) {
        this.travelExpenseService.processStep(travelExpense, flowStep).subscribe(response => {
            console.log('Got response from processStep:', response);
            this.travelExpenseIsMoved = true;
        });
    }

}
