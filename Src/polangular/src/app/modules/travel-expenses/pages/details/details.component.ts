import {Component, OnInit, ViewChild} from '@angular/core';
import {TravelExpense} from '../../../../shared/model/travel-expense.model';
import {from, Observable, of} from 'rxjs';
import {TravelExpenseService} from '../../../../shared/services/travel-expense.service';
import {ActivatedRoute, Router} from '@angular/router';
import {catchError, map, switchMap, tap} from 'rxjs/operators';
import {FormBuilder, FormGroup, ValidationErrors} from '@angular/forms';
import {TravelExpenseFormComponent} from '../../components/travel-expense-form/travel-expense-form.component';
import {MockTravelExpenseService} from "../../../../shared/mocks/mock-travel-expense.service";


@Component({
    selector: 'app-index',
    templateUrl: './details.component.html',
    styleUrls: ['./details.component.scss']
})
export class DetailsComponent implements OnInit {
    travelExpense$: Observable<TravelExpense> = from([]);
    travelExpenseIsMoved = false;
    travelExpenseForm: FormGroup = this.formBuilder.group({
        travelExpense: [],
    });
    travelExpense: TravelExpense;
    @ViewChild('travelExpenseFormComponent') form: TravelExpenseFormComponent;

    constructor(
        private travelExpenseService: MockTravelExpenseService,
        private activatedRoute: ActivatedRoute,
        private router: Router,
        private formBuilder: FormBuilder
    ) {
    }

    ngOnInit(): void {
        this.travelExpense$ = this.activatedRoute.paramMap.pipe(
            map((param) => param.get('id')),
            switchMap((id: string) => this.travelExpenseService.getTravelExpenseById(id)),
            tap((travelExpense) => {
                console.log('t', travelExpense);
                this.travelExpense = travelExpense;
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
        );

        this.travelExpense$.subscribe();
    }

    save() {
        this.form.updateValueAndValidity();
        this.travelExpenseForm.markAllAsTouched();
        this.travelExpenseForm.updateValueAndValidity();
        if (this.travelExpenseForm.invalid) {
            this.logValidationErrors();
            return;
        }

        const formValues = this.travelExpenseForm.getRawValue();

        const modifiedTravelExpense = formValues.travelExpense as TravelExpense;
        modifiedTravelExpense.id = this.travelExpense.id;

        this.travelExpenseService.updateTravelExpense(modifiedTravelExpense).pipe(
            switchMap(result => this.travelExpenseService.getTravelExpenseById(modifiedTravelExpense.id)),
        ).subscribe((fetchedTravelExpense) => {

            this.travelExpenseForm.get('travelExpense').patchValue(fetchedTravelExpense);

            /*this.modifiedTravelExpense = fetchedTravelExpense;
            this.travelExpenseChange.emit(this.modifiedTravelExpense);*/
            // this.prefillFormFromTravelExpense(this.modifiedTravelExpense);

            console.log('fresh model TravelExpense', fetchedTravelExpense);
        }, (error) => {
            console.error('App Error', error);
        });

    }

    private logValidationErrors() {
        const controlErrors: ValidationErrors = this.travelExpenseForm.get('travelExpense').errors;
        if (controlErrors != null) {
            Object.keys(controlErrors).forEach(keyError => {
                console.log('Key control: ' + 'travelExpense' + ', keyError: ' + keyError + ', err value: ', controlErrors[keyError]);
            });
        }
    }

    moveToFlowStep(travelExpense, flowStep) {
        this.travelExpenseService.processStep(travelExpense, flowStep).subscribe(response => {
            console.log('Got response from processStep:', response);
            this.travelExpenseIsMoved = true;
        });
    }

}
