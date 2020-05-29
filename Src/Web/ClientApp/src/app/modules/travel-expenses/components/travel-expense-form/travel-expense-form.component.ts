import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {TravelExpense} from '../../../../shared/model/travel-expense.model';
import {TravelExpenseService} from "../../../../shared/services/travel-expense.service";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {catchError, switchMap, tap} from "rxjs/operators";
import * as dayjs from 'dayjs';
import {MockTravelExpenseService} from "../../../../shared/mocks/mock-travel-expense.service";

@Component({
    selector: 'app-travel-expense-form',
    templateUrl: './travel-expense-form.component.html',
    styleUrls: ['./travel-expense-form.component.scss']
})
export class TravelExpenseFormComponent implements OnInit {
    @Input() travelExpense: TravelExpense;
    @Output() travelExpenseChange: EventEmitter<TravelExpense> = new EventEmitter<TravelExpense>();
    travelExpenseFormGroup: FormGroup;

    constructor(
        private travelExpenseService: MockTravelExpenseService,
        private fb: FormBuilder,
    ) {
    }

    ngOnInit(): void {
        this.initReactiveForm();
        this.prefillFormFromTravelExpense(this.travelExpense);
    }

    private prefillFormFromTravelExpense(travelExpense: TravelExpense) {
        this.travelExpenseFormGroup.patchValue(travelExpense);
    }

    private initReactiveForm() {
        this.travelExpenseFormGroup = this.fb.group({
            description: [null, Validators.required],
            startDate: [dayjs(), Validators.required],
            startTime: ['08:00', Validators.required],
            endDate: [dayjs(), Validators.required],
            endTime: ['16:00', Validators.required],
            purpose: [null, Validators.required],
        });
    }

    save() {
        this.travelExpenseFormGroup.markAllAsTouched();
        this.travelExpenseFormGroup.updateValueAndValidity();
        if (this.travelExpenseFormGroup.invalid) {
            return;
        }

        const formValues = this.travelExpenseFormGroup.getRawValue();

        const travelExpense = {
            id: this.travelExpense.id,
            description: formValues.description,
        } as TravelExpense;

        this.travelExpenseService.updateTravelExpense(travelExpense).pipe(
            switchMap(result => this.travelExpenseService.getTravelExpenseById(travelExpense.id)),
        ).subscribe((fetchedTravelExpense) => {

            this.travelExpense = fetchedTravelExpense;
            this.travelExpenseChange.emit(this.travelExpense);
            this.prefillFormFromTravelExpense(this.travelExpense);

            console.log('fresh model TravelExpense', fetchedTravelExpense);
        }, (error) => {
            console.error('App Error', error);
        });


    }
}
