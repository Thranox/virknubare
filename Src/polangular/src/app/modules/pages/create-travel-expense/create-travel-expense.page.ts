import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ModalController } from '@ionic/angular';
import { LossOfEarningsPage } from '../loss-of-earnings/loss-of-earnings.page';
import { DateTimeManagerService } from '../../../shared/services/datetime-manager.service';
import { AccessHTMLElementService } from '../../../shared/services/access-html-element.service';
import { UserInfoService } from '../../../shared/services/user-info.service';
import * as moment from 'moment';

@Component({
  selector: 'app-create-travel-expense',
  templateUrl: './create-travel-expense.page.html',
    styleUrls: ['./create-travel-expense.page.css'],

})

export class CreateTravelExpensePage implements OnInit {
    travelExpense: FormGroup

    ownCar: HTMLElement
    ownBike: HTMLElement

    isDrivingOwnCar: boolean
    isRidingOwnBike: boolean
    isTripUnder24Hours: boolean

    todaysDate: String = new Date(Date.now()).toISOString();
    constructor(
        private formBuilder: FormBuilder,
        private modalController: ModalController,
        private dateManagerService: DateTimeManagerService,
        private accessHTMLElementService: AccessHTMLElementService,
        private userInfoService: UserInfoService
    )
    {
        this.travelExpense = this.formBuilder.group({
            startDateTime: '',
            endDateTime: '',
            placeOfDeparture: ['', [Validators.required, Validators.minLength(1)]],
            placeOfHomecoming: ['', [Validators.required, Validators.minLength(1)]],
            meetingPlace: ['', [Validators.required, Validators.minLength(1)]],
            purposeOfMeeting: ['', [Validators.required, Validators.minLength(1)]],
            committee: ['', [Validators.required, Validators.minLength(1)]],
            isCourse: false,
            isOwnCar: false,
            isOwnBike: false,
            numberOfKilometers: ['', [Validators.required, Validators.minLength(1)]],
            numberPlate: [''],
            numberOfKilometersTaxable: [''],
            numberOfKilometersOver20000: [''],
            isTravelAbroad: false,
            numberOfBreakfastsConsumed: ['', [Validators.required, Validators.minLength(1)]],
            numberOfDinnersConsumed: ['', [Validators.required, Validators.minLength(1)]],
            numberOfSuppersConsumed: ['', [Validators.required, Validators.minLength(1)]],
            numberOfDietsUnder4Hours: ['', [Validators.required, Validators.minLength(1)]],
            numberOfDietsOver4Hours: ['', [Validators.required, Validators.minLength(1)]],
            totalWorkHoursLost: '',
            remarks: '',
            selectedDietOnTripUnder24Hours: [''],
            status: 'Oprettet'
        });
        this.travelExpense.get('startDateTime').setValue(this.todaysDate)
        this.travelExpense.get('endDateTime').setValue(this.todaysDate)
    }

    ngOnInit() {
        this.ownCar = this.accessHTMLElementService.getById('ownCarToggle')
        this.ownBike = this.accessHTMLElementService.getById('ownBikeToggle')
        this.setTransportValidators()
        this.setDietValidators()
    }

    setTransportValidators() {
        this.travelExpense.get('isOwnCar').valueChanges
            .subscribe(isOwnCar => {
                if (isOwnCar) {
                    this.travelExpense.get('numberPlate').setValidators([Validators.required, Validators.minLength(1)])
                    this.travelExpense.get('numberOfKilometersTaxable').setValidators([Validators.required, Validators.minLength(1)])
                    this.travelExpense.get('numberOfKilometersOver20000').setValidators([Validators.required, Validators.minLength(1)])
                } else {
                    this.travelExpense.get('numberPlate').setValidators(null)
                    this.travelExpense.get('numberOfKilometersTaxable').setValidators(null)
                    this.travelExpense.get('numberOfKilometersOver20000').setValidators(null)
                }
                this.travelExpense.get('numberPlate').updateValueAndValidity()
                this.travelExpense.get('numberOfKilometersTaxable').updateValueAndValidity()
                this.travelExpense.get('numberOfKilometersOver20000').updateValueAndValidity()
            });
    }

    setDietValidators() {
        this.travelExpense.get('endDateTime').valueChanges
            .subscribe(() => {
                this.tripUnder24Hours()
                if (this.isTripUnder24Hours) {
                    this.travelExpense.get('selectedDietOnTripUnder24Hours').setValidators([Validators.required, Validators.minLength(1)])
                    this.travelExpense.get('numberOfBreakfastsConsumed').setValidators(null)
                    this.travelExpense.get('numberOfDinnersConsumed').setValidators(null)
                    this.travelExpense.get('numberOfSuppersConsumed').setValidators(null)
                    this.travelExpense.get('numberOfDietsUnder4Hours').setValidators(null)
                    this.travelExpense.get('numberOfDietsOver4Hours').setValidators(null)
                } else {
                    console.log('her')
                    this.travelExpense.get('selectedDietOnTripUnder24Hours').setValidators(null)
                    this.travelExpense.get('numberOfBreakfastsConsumed').setValidators([Validators.required, Validators.minLength(1)])
                    this.travelExpense.get('numberOfDinnersConsumed').setValidators([Validators.required, Validators.minLength(1)])
                    this.travelExpense.get('numberOfSuppersConsumed').setValidators([Validators.required, Validators.minLength(1)])
                    this.travelExpense.get('numberOfDietsUnder4Hours').setValidators([Validators.required, Validators.minLength(1)])
                    this.travelExpense.get('numberOfDietsOver4Hours').setValidators([Validators.required, Validators.minLength(1)])
                }
                this.travelExpense.get('selectedDietOnTripUnder24Hours').updateValueAndValidity()
                this.travelExpense.get('numberOfBreakfastsConsumed').updateValueAndValidity()
                this.travelExpense.get('numberOfDinnersConsumed').updateValueAndValidity()
                this.travelExpense.get('numberOfSuppersConsumed').updateValueAndValidity()
                this.travelExpense.get('numberOfDietsUnder4Hours').updateValueAndValidity()
                this.travelExpense.get('numberOfDietsOver4Hours').updateValueAndValidity()
            });
    }


    async presentModal() {
        const modal = await this.modalController.create({
            component: LossOfEarningsPage,
            backdropDismiss: false,
            componentProps: {
                dateData: this.dateManagerService
            }
        });
        modal.onDidDismiss().then((totalWorkHoursLost) => {
            var totalWorkHoursLostInput = this.accessHTMLElementService.getById('totalWorkHoursLostInput');
            totalWorkHoursLostInput["value"] = totalWorkHoursLost['data']
        })
        return await modal.present();
    }

    dismissModal() {
        this.modalController.dismiss(this.travelExpense);
    }

    dismissModalWithoutSavingExpense() {
        this.modalController.dismiss();
    }

    get f() { return this.travelExpense.controls; }

    createTravelExpense() {
        if (this.travelExpense.invalid) {
            return;
        }
        this.dismissModal();
    }

    openLossOfEarningsPage() {
        this.initData();
        this.presentModal();
    }

    initData() {
        this.updateStartDateTimeOnDateTimeManagerService()
        this.updateEndDateTimeOnDateTimeManagerService()
    }

    getValueFromFormGroup(formControlName: string) {
        return this.travelExpense.controls[formControlName].value
    }

    updateStartDateTimeOnDateTimeManagerService() {
        this.dateManagerService.startDateTime = new Date(this.getValueFromFormGroup("startDateTime"))
    }

    updateEndDateTimeOnDateTimeManagerService() {  
        this.dateManagerService.endDateTime = new Date(this.getValueFromFormGroup("endDateTime"))
    }

    tripUnder24Hours() {
        this.updateStartDateTimeOnDateTimeManagerService()
        this.updateEndDateTimeOnDateTimeManagerService()
        this.isTripUnder24Hours = this.dateManagerService.checkIfTripIsUnder24Hours()
    }

    toggleTransportationByCar() {    
        if (this.ownCar["checked"]) {
            this.isDrivingOwnCar = true
            this.isRidingOwnBike = false
            this.ownBike["checked"] = false
        }
        if (!this.ownCar["checked"])
            this.isDrivingOwnCar = false
    }

    toggleTransportationByBike() {
        if (this.ownBike["checked"]) {
            this.isRidingOwnBike = true
            this.isDrivingOwnCar = false
            this.ownCar["checked"] = false
        }
        if (!this.ownBike["checked"])
            this.isRidingOwnBike = false
    }

    toggleInputField(event, ionInputElementId) {
        var ionInputElement = this.accessHTMLElementService.getById(ionInputElementId);
        if (event.detail.value == 'Andet') {
            ionInputElement['disabled'] = false
        } else {
            ionInputElement['disabled'] = true
        }
       
    }

    selectedHomeOrWorkAsDestination(event, ionInputElementId) {
        var ionInputElement = this.accessHTMLElementService.getById(ionInputElementId);
        if (event.detail.value == 'Hjem') {
            ionInputElement['value'] = this.userInfoService.getHomeAdress()
        }
        if (event.detail.value == 'Arbejde') {
            ionInputElement['value'] = this.userInfoService.getWorkAdress()
        } 
    }
}
