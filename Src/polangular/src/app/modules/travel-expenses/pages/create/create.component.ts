import {Component, OnInit, ViewChild} from '@angular/core';
import {TravelExpense} from '../../../../shared/model/travel-expense.model';
import {from, Observable} from 'rxjs';
import {MockTravelExpenseService} from "../../../../shared/mocks/mock-travel-expense.service";
import {FormBuilder, FormGroup} from "@angular/forms";
import {TravelExpenseService} from "../../../../shared/services/travel-expense.service";
import {map, switchMap} from "rxjs/operators";
import {TravelExpenseFormComponent} from "../../components/travel-expense-form/travel-expense-form.component";
import {UserInfoService} from "../../../../shared/services/user-info.service";
import { Router } from "@angular/router";

@Component({
    selector: 'app-index',
    templateUrl: './create.component.html',
    styleUrls: ['./create.component.scss']
})
export class CreateComponent implements OnInit {
    travelExpense: TravelExpense;
    @ViewChild('travelExpenseFormComponent') travelExpenseFormComponent: TravelExpenseFormComponent;
    form: FormGroup = this.formBuilder.group({
        travelExpense: [],
    });

    constructor(
        private travelExpenseService: TravelExpenseService,
        private userInfoService: UserInfoService,
        private formBuilder: FormBuilder,
        private router: Router,

    ) {
    }

    ngOnInit(): void {

        this.travelExpense = new TravelExpense();

        this.form.patchValue({
            travelExpense: {
                purpose: 'General forsamling',
                isEducationalPurpose: true,
                description: 'Test \n 234',
                transportSpecification: {
                    method: 'car',
                    numberPlate: 'AJ12345',
                },
                committeeId: 12345,
                arrivalPlace: {
                    street: 'Aarhusvej',
                    streetNumber: '34',
                    zipCode: '8000',
                },
                departurePlace: {
                    street: 'Vibyvej',
                    streetNumber: '99',
                    zipCode: '8260',
                },
            }
        });
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
        this.userInfoService.getUserInfo().pipe(
            map((userInfo) =>  userInfo.userCustomerInfo[0].customerId),
            switchMap((customerId) => this.travelExpenseService.createTravelExpense(travelExpense, customerId)),
            switchMap(id => {
                return this.travelExpenseService.getTravelExpenseById(id);
            }),
        ).subscribe((fetchedTravelExpense) => {
            this.router.navigate([`travel-expenses/${fetchedTravelExpense.id}`]).then();
        }, (error) => {
            console.error('App Error', error);
        });

    }

}
