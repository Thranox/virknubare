import {Component, EventEmitter, forwardRef, Input, OnInit, Output} from '@angular/core';
import {TravelExpense} from '../../../../shared/model/travel-expense.model';
import {
    ControlValueAccessor, FormArray,
    FormBuilder,
    FormControl,
    FormGroup,
    NG_VALIDATORS,
    NG_VALUE_ACCESSOR,
    Validators
} from '@angular/forms';
import * as dayjs from 'dayjs';
import {merge} from "rxjs";
import {Dayjs} from "dayjs";


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

    commitees = [{id: 1234, name: 'Regionsrådet'},
        {id: 12342, name: 'Forretningsudvalget'},
        {id: 2, name: 'Regionaludvalget/RU'},
        {id: 12345, name: 'Miljø-og grønvækst MGV'},
        {id: 12346, name: 'Fremtidens Uddannelse og Forskning/FUF'},
        {id: 12347, name: 'Udv.vedr.udsatte borgere/'},
        {id: 12348, name: 'Psykiatri og Socialudvalg'},
        {id: 12349, name: 'Patientudvalg/'},
        {id: 12134, name: 'Kvalitetsudvalg/KVU'},
        {id: 12234, name: 'Regionale dialogfora vedr. handicap. psyk, sundhed og Udd.'},
        {id: 12334, name: 'Dommerkomiteer/følgegrupper kvalitetsfondsbyggerier'},
        {id: 12434, name: 'Orienteringsmøde for RR, FU'},
        {id: 12534, name: 'Tværgående arbejdsgruppe, udvalgene'},
        {id: 12634, name: 'Seminar/Konference, skriv navn ell.emne i næste felt'},
        {id: 12634, name: 'Hverv, WOCO,'},
        {id: 12734, name: 'Hverv: Øresundskomite'},
        {id: 12834, name: 'Studierejse'},
        {id: 127834, name: 'Besigtelsestur'},
        {id: 19234, name: 'Møde m/direktion/stab/reg.formand, formand for udvalgsmøde'},
        {id: 12034, name: 'Budgetforhandling'},
        {id: 12345674, name: 'Andet, se i næste felt hvilke form'},
        {id: 1234674, name: 'Ansættelsesudvalg'},
        {id: 12314, name: 'Teknisk udvalg'},
        {id: 123454, name: 'Kultur- og fritidsudvalget'},
        {id: 1243234, name: 'Klima- og miljøudvalget'}];

    lossOfEarningRates = [
        {id:  '12', amount: 300}, // ex. sats 300
        {id:  '13', amount: 400}, // ex. sats 400
        {id:  '15', amount: 500}  // ex. sats 500
    ];
    private dayBetweenStartAndEnd = 0;


    constructor(
        private fb: FormBuilder,
    ) {
    }

    ngOnInit(): void {
        this.initReactiveForm();

        this.updateLossOfEarnings();
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
            startDate: [dayjs(), Validators.required],
            startTime: ['08:00', Validators.required],
            endDate: [dayjs(), Validators.required],
            endTime: ['16:00', Validators.required],
            committeeId: [null, Validators.required],
            purpose: [null, Validators.required],
            isEducationalPurpose: [false, Validators.required],
            departurePlace: this.fb.group({
                street: ['', Validators.required],
                streetNumber: ['', Validators.required],
                zipCode: ['', Validators.required],
            }),
            arrivalPlace: this.fb.group({
                street: ['', Validators.required],
                streetNumber: ['', Validators.required],
                zipCode: ['', Validators.required],
            }),
            destinationPlace: this.fb.group({
                street: ['', Validators.required],
                streetNumber: ['', Validators.required],
                zipCode: ['', Validators.required],
            }),
            transportSpecification: this.fb.group({
                method: [null],
                kilometersCalculated: [0, Validators.required],
                kilometersCustom: [null],
                kilometersTax: [0,  Validators.required],
                kilometersOverTaxLimit: [0,  Validators.required],
                numberPlate: [''],
            }),
            expenses: [0, Validators.required],
            description: [null, Validators.required],
            dailyAllowanceAmount: this.fb.group({
                daysLessThan4hours: [0],
                daysMoreThan4hours: [0],
            }),
            foodAllowances: this.fb.group({
                morning: [0],
                lunch: [0],
                dinner: [0]
            }),
            lossOfEarnings: this.fb.array([])
        });

        this.trackDateRangeChanges();

        this.travelExpenseFormGroup.valueChanges.subscribe(value => {
            this.onChange(value);
            this.onTouched();
        });
    }

    private trackDateRangeChanges() {
        merge(
            this.travelExpenseFormGroup.controls.startDate.valueChanges,
            this.travelExpenseFormGroup.controls.endDate.valueChanges,
        ).subscribe(date => {
            //console.log('Date range changed', date);

            const startDate = this.travelExpenseFormGroup.get('startDate').value as Dayjs;
            const endDate = this.travelExpenseFormGroup.get('endDate').value as Dayjs;
            this.updateLossOfEarnings();
            this.dayBetweenStartAndEnd = (endDate).diff(startDate, 'day');
        });
    }

    get lossOfEarnings() {
        return this.travelExpenseFormGroup.get('lossOfEarnings') as FormArray;
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

    addLossOfEarnings(lossOfEarnings: { id: string, amount: number }, date: Dayjs) {
        this.lossOfEarnings.push(this.fb.group({
            id: [lossOfEarnings.id, Validators.required],
            numberOfHours: [0, [
                Validators.required]
            ],
            amount: [lossOfEarnings.amount],
            date: [date.format('DD-MM-YYYY'), Validators.required],
        }));
    }

    private updateLossOfEarnings() {
        const startDate = this.travelExpenseFormGroup.get('startDate').value as Dayjs;
        const endDate = this.travelExpenseFormGroup.get('endDate').value as Dayjs;
        this.lossOfEarnings.clear();
        let currentDay = startDate;

        //console.log(startDate, endDate, endDate.diff(currentDay, 'day'));
        while (endDate.diff(currentDay) >= 0) {
            // console.log(startDate, currentDay, endDate, endDate.diff(currentDay, 'day'));
            this.lossOfEarningRates.forEach(rate => {
                this.addLossOfEarnings(rate, currentDay);
            });

            currentDay = currentDay.add(1, 'day');
        }

    }

}
