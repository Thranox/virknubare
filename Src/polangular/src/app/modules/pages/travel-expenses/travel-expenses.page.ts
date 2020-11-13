import { Component, OnInit } from '@angular/core';
import { ModalController } from '@ionic/angular';
import { CreateTravelExpensePage } from '../create-travel-expense/create-travel-expense.page';
import { FormGroup } from '@angular/forms';
import * as moment from 'moment';

@Component({
  selector: 'app-travel-expenses',
  templateUrl: './travel-expenses.page.html',
    styleUrls: ['./travel-expenses.page.css'],

})
export class TravelExpensesPage implements OnInit {
    travelExpensesList = []
    constructor(public modalController: ModalController) { }

    ngOnInit() {

    }

    async presentModal() {
        const modal = await this.modalController.create({
            component: CreateTravelExpensePage,
            backdropDismiss: false,         
        });
        modal.onDidDismiss().then((newTravelExpense) => {
            console.log(newTravelExpense['data'].value.startDateTime)
            var formattedStartDateTime = moment(newTravelExpense['data'].value.startDateTime).format('DD-MMM, YYYY HH:mm')
            newTravelExpense['data'].value.startDateTime = formattedStartDateTime
            var formattedEndDateTime = moment(newTravelExpense['data'].value.endDateTime).format('DD-MMM, YYYY HH:mm')
            newTravelExpense['data'].value.endDateTime = formattedEndDateTime
            this.travelExpensesList.push(newTravelExpense['data'])


        })
        return await modal.present();
    }

    openCreateTravelExpensePage() {
        this.presentModal();
    }

    showSelectedTravelExpense(travelExpense) {
        console.log(travelExpense)
    }
}
