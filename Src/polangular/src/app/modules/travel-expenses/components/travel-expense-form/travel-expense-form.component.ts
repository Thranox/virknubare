import {Component, EventEmitter, forwardRef, Input, OnInit, Output} from '@angular/core';
import {TravelExpense} from '../../../../shared/model/travel-expense.model';
import {TravelExpenseService} from "../../../../shared/services/travel-expense.service";
import {
    ControlValueAccessor,
    FormBuilder,
    FormControl,
    FormGroup,
    NG_VALIDATORS,
    NG_VALUE_ACCESSOR,
    Validators
} from "@angular/forms";
import * as dayjs from 'dayjs';


@Component({
    selector: 'app-travel-expense-form',
    templateUrl: './travel-expense-form.component.html',
    styleUrls: ['./travel-expense-form.component.scss'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => TravelExpenseFormComponent),
            multi: true
        },
        {
            provide: NG_VALIDATORS,
            useExisting: forwardRef(() => TravelExpenseFormComponent),
            multi: true
        }
    ]
})
export class TravelExpenseFormComponent implements OnInit, ControlValueAccessor {
    @Input() travelExpense: TravelExpense;
    @Input() form: FormGroup;
    @Output() travelExpenseChange: EventEmitter<TravelExpense> = new EventEmitter<TravelExpense>();
    travelExpenseFormGroup: FormGroup;

    constructor(
        private travelExpenseService: TravelExpenseService,
        private fb: FormBuilder,
    ) {
    }

    ngOnInit(): void {
        this.initReactiveForm();
    }

    private onTouched: any = () => {};
    private onChange: any = () => {};

    get value(): TravelExpense {
        return this.travelExpenseFormGroup.value;
    }

    set value(value: TravelExpense) {
        this.travelExpenseFormGroup.patchValue(value);
        this.onChange(value);
        this.onTouched();
    }

    public updateValueAndValidity() {
        this.travelExpenseFormGroup.markAllAsTouched();
        this.travelExpenseFormGroup.updateValueAndValidity();
    }

    private initReactiveForm() {
        this.travelExpenseFormGroup = this.fb.group({
            description: [null, Validators.required],
            startDate: [dayjs()],
            startTime: ['08:00'],
            endDate: [dayjs()],
            endTime: ['16:00'],
            purpose: [null, Validators.required],
            isEducational: [false, Validators.required],
        });

        this.travelExpenseFormGroup.markAsTouched();
        this.travelExpenseFormGroup.updateValueAndValidity();

        this.travelExpenseFormGroup.valueChanges.subscribe(value => {
            this.onChange(value);
            this.onTouched();
        });
    }



    registerOnChange(fn: any): void {
        this.onChange = fn;
    }

    registerOnTouched(fn: any): void {
        this.onTouched = fn;
    }

    // communicate the inner form validation to the parent form
    validate(_: FormControl) {
        return this.travelExpenseFormGroup.valid ? null : { travelExpense: { valid: false }};
    }

    writeValue(value) {
        if (value) {
            this.value = value;
        }

        if (value === null) {
            this.travelExpenseFormGroup.reset();
        }
    }
}
