import { Component, OnInit } from '@angular/core';
import { ModalController } from '@ionic/angular';

@Component({
  selector: 'app-show-travel-expense',
  templateUrl: './show-travel-expense.page.html',
  styleUrls: ['./show-travel-expense.page.css'],
})
export class ShowTravelExpensePage implements OnInit {

    travelExpense: any

    constructor(private modalController: ModalController) { }

    ngOnInit() {
        console.log(this.travelExpense)
    }

    dismissModal() {
        this.modalController.dismiss();
    }
}
