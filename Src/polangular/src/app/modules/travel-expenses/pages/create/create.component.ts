import {Component, OnInit, ViewChild} from '@angular/core';
import {TravelExpense} from '../../../../shared/model/travel-expense.model';
import {from, Observable} from 'rxjs';
import {MockTravelExpenseService} from "../../../../shared/mocks/mock-travel-expense.service";
import {FormBuilder, FormGroup} from "@angular/forms";
import {TravelExpenseService} from "../../../../shared/services/travel-expense.service";
import {switchMap} from "rxjs/operators";
import {TravelExpenseFormComponent} from "../../components/travel-expense-form/travel-expense-form.component";

@Component({
    selector: 'app-index',
    templateUrl: './create.component.html',
    styleUrls: ['./create.component.scss']
})
export class CreateComponent implements OnInit {
    travelExpense$: Observable<TravelExpense> = from([]);
    travelExpense: TravelExpense;
    @ViewChild('travelExpenseFormComponent') travelExpenseFormComponent: TravelExpenseFormComponent;
    form: FormGroup = this.formBuilder.group({
        travelExpense: [],
    });

    constructor(
        private travelExpenseService: TravelExpenseService,
        private formBuilder: FormBuilder
    ) {
    }

    ngOnInit(): void {

        this.travelExpense = new TravelExpense();

        /*this.travelExpense$ = this.activatedRoute.paramMap.pipe(
            map((param) => param.get('id')),
            switchMap((id: string) => this.travelExpenseService.getTravelExpenseById(id))
        );*/
    }

    save() {
        this.travelExpenseFormComponent.updateValueAndValidity();
        this.form.markAllAsTouched();
        this.form.updateValueAndValidity();
        if (this.form.invalid) {
            return;
        }

        const formValues = this.form.getRawValue();

        const travelExpense = formValues.travelExpense as TravelExpense;

        this.travelExpenseService.createTravelExpense(travelExpense).pipe(
            switchMap(result => this.travelExpenseService.getTravelExpenseById(travelExpense.id)),
        ).subscribe((fetchedTravelExpense) => {

            // this.prefillFormFromTravelExpense(this.travelExpense);

            console.log('fresh model TravelExpense', fetchedTravelExpense);
        }, (error) => {
            console.error('App Error', error);
        });


    }

}
