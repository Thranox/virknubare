import { Component, OnInit } from '@angular/core';
import { ModalController } from '@ionic/angular';

import { DateTimeManagerService } from '../../../shared/services/datetime-manager.service';
import { AccessHTMLElementService } from '../../../shared/services/access-html-element.service';

@Component({
  selector: 'app-loss-of-earnings',
  templateUrl: './loss-of-earnings.page.html',
  styleUrls: ['./loss-of-earnings.page.css'],
})
export class LossOfEarningsPage implements OnInit {
    dateData: any
    sumOfWorkHoursLost: number = 0

    constructor(
        private modalController: ModalController,
        private dateManagerService: DateTimeManagerService,
        private accessHTMLElementService: AccessHTMLElementService
    ) { }

    ngOnInit() {
        this.dateManagerService.getAllDatesFromStartDateToEndDate();
        this.dateData = this.dateManagerService
    }

    dismissModal() {
        this.modalController.dismiss();
    }

    calculateWorkHoursLost() {
        for (let i = 0; i < this.dateData.allDatesFromStartDateToEndDate.length; i++) {
            var numberOfWorkHoursLostInput = this.accessHTMLElementService.getById(i);
            this.sumOfWorkHoursLost += +numberOfWorkHoursLostInput["value"]
        }
        this.modalController.dismiss(this.sumOfWorkHoursLost);
    }
}
