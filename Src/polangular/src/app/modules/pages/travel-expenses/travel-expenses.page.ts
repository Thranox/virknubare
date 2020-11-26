import { Component, OnInit } from '@angular/core';
import { ModalController } from '@ionic/angular';
import { CreateTravelExpensePage } from '../create-travel-expense/create-travel-expense.page';
import * as moment from 'moment';
import { ShowTravelExpensePage } from '../show-travel-expense/show-travel-expense.page';
import { AccessHTMLElementService } from '../../../shared/services/access-html-element.service';

@Component({
  selector: 'app-travel-expenses',
  templateUrl: './travel-expenses.page.html',
    styleUrls: ['./travel-expenses.page.css'],

})
export class TravelExpensesPage implements OnInit {
    travelExpensesList = []
    sortKey = ''
    sortDirection: number = 0
    moment = moment

    filterStatus = null
    filterYear = null
    filter = []

    constructor(
        public modalController: ModalController,
        public accessHTMLElementService: AccessHTMLElementService
    ) { }

    ngOnInit() {
        var testData = [
            {
                value: {
                    startDateTime: '2020-11-19',
                    endDateTime: '2020-11-20',
                    purposeOfMeeting: 'Intet',
                    status: 'Oprettet',
                    placeOfDeparture: 'Place of departure',
                    placeOfHomecoming: 'Place of homecoming',
                    meetingPlace: 'Place of meeting',
                    committee: 'Committee',
                    isCourse: false,
                    isOwnCar: true,
                    isOwnBike: false,
                    numberOfKilometers: '10000',
                    numberPlate: 'ACAB2000',
                    numberOfKilometersTaxable: '800',
                    numberOfKilometersOver20000: '0',
                    isTravelAbroad: false,
                    numberOfBreakfastsConsumed: '4',
                    numberOfDinnersConsumed: '1',
                    numberOfSuppersConsumed: '2',
                    numberOfDietsUnder4Hours: '3',
                    numberOfDietsOver4Hours: '2',
                    totalWorkHoursLost: '',
                    remarks: '',
                    selectedDietOnTripUnder24Hours: '',
                }
            },
            {
                value: {

                    startDateTime: '2020-11-20',
                    endDateTime: '2020-11-22',
                    placeOfDeparture: '',
                    placeOfHomecoming: '',
                    meetingPlace: '',
                    purposeOfMeeting: 'Fremtiden',
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
                    status: 'Færdigmeldt'
                }
            },
            {
                value: {
                    startDateTime: '2020-12-19',
                    endDateTime: '2020-12-20',
                    placeOfDeparture: '',
                    placeOfHomecoming: '',
                    meetingPlace: '',
                    purposeOfMeeting: 'Miljø',
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
                }
            },
            {
                value: {
                    startDateTime: '2020-09-10',
                    endDateTime: '2020-09-10',
                    placeOfDeparture: '',
                    placeOfHomecoming: '',
                    meetingPlace: '',
                    purposeOfMeeting: 'Miljø',
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
                }
            },
            {
                value: {
                    startDateTime: '2021-07-19',
                    endDateTime: '2021-07-20',
                    placeOfDeparture: '',
                    placeOfHomecoming: '',
                    meetingPlace: '',
                    purposeOfMeeting: 'Miljø',
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
                }
            },

        ]
        this.travelExpensesList = testData
    }

    async presentModal() {
        const modal = await this.modalController.create({
            component: CreateTravelExpensePage,
            backdropDismiss: false,         
        });
        modal.onDidDismiss().then((newTravelExpense) => {
            if (newTravelExpense['data'] != undefined) {
                console.log(newTravelExpense)
                this.travelExpensesList.push(newTravelExpense['data'])

            }
        })
        return await modal.present();
    }

    openCreateTravelExpensePage() {
        this.presentModal();
    }

    async showSelectedTravelExpense(travelExpense) {
        const modal = await this.modalController.create({
            component: ShowTravelExpensePage,
            backdropDismiss: false,
            componentProps: {
                travelExpense: travelExpense
            }
        });
        return await modal.present();
    }

    deleteTravelExpense(index) {
        if (this.travelExpensesList[index].value.status == 'Færdigmeldt') {
            return;
        }
        this.travelExpensesList.splice(index, 1);
    }

    sortBy(key) {
        this.sortKey = key
        this.sortDirection++;
        this.sort()
    }

    sort() {
        if (this.sortDirection == 1) {
            this.travelExpensesList.sort((a, b) => {
                const valA = a.value[this.sortKey];
                const valB = b.value[this.sortKey];
                return valA.localeCompare(valB)
            });
        } else if (this.sortDirection == 2) {
            this.travelExpensesList.sort((a, b) => {
                const valA = a.value[this.sortKey]
                const valB = b.value[this.sortKey]
                return valB.localeCompare(valA)
            });
        } else {
            this.sortDirection = 0
            this.sortKey = ''
        }
    }

    applyFilter() {
        this.filter[0] = this.filterYear
        this.filter[1] = this.filterStatus
    }

    clearFilter() {
        this.filter = []
        this.accessHTMLElementService.getById('filterByYear')["value"] = null
        this.accessHTMLElementService.getById('filterByStatus')["value"] = null
    }

    filterByStatus(event) {
        this.filterStatus = event.detail.value
    }
    filterByYear(event) {

        this.filterYear = event.detail.value

    }
}
