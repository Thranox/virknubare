import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { TravelExpensesPageRoutingModule } from './travel-expenses-routing.module';
import { TravelExpensesPage } from './travel-expenses.page';
import { RouterModule } from '@angular/router';

import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatSliderModule } from '@angular/material/slider';
import { MaterialModule } from '../../../shared/material/material.module';
@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        IonicModule,
        TravelExpensesPageRoutingModule,
        RouterModule,
    ],
    declarations: [
        TravelExpensesPage,

    ]
})
export class TravelExpensesPageModule {}
