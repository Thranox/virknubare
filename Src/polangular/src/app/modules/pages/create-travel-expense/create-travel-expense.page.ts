import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ModalController } from '@ionic/angular';
import { LossOfEarningsPage } from '../loss-of-earnings/loss-of-earnings.page';
import { DateTimeManagerService } from '../../../shared/services/datetime-manager.service';
import { AccessHTMLElementService } from '../../../shared/services/access-html-element.service';

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
        private accessHTMLElementService: AccessHTMLElementService
    )
    {
        this.travelExpense = this.formBuilder.group({
            startDateTime: '',
            endDateTime: '',
            placeOfDeparture: '',
            placeOfHomecoming: '',
            meetingPlace: '',
            purposeOfMeeting: '',
            committee: '',
            isCourse: false,
            isOwnCar: false,
            isOwnBike: false,
            numberOfKilometers: '',
            numberPlate: '',
            numberOfKilometersTaxable: '',
            numberOfKilometersOver20000: '',
            isTravelAbroad: false,
            numberOfBreakfastsConsumed: '',
            numberOfDinnersConsumed: '',
            numberOfSuppersConsumed: '',
            numberOfDietsUnder4Hours: '',
            numberOfDietsOver4Hours: '',
            totalWorkHoursLost: '',
            remarks: '',
            selectedDietOnTripUnder24Hours: '',
            status: 'Oprettet'
        })
        this.travelExpense.get('startDateTime').setValue(this.todaysDate)
        this.travelExpense.get('endDateTime').setValue(this.todaysDate)
    }

    ngOnInit() {
        this.ownCar = this.accessHTMLElementService.getById('ownCarToggle')
        this.ownBike = this.accessHTMLElementService.getById('ownBikeToggle')
       
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

    createTravelExpense() {
        console.log(this.travelExpense.value);
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
}
