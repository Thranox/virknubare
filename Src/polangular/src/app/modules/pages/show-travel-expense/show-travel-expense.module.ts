import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { ShowTravelExpensePageRoutingModule } from './show-travel-expense-routing.module';

import { ShowTravelExpensePage } from './show-travel-expense.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ShowTravelExpensePageRoutingModule
  ],
  declarations: [ShowTravelExpensePage]
})
export class ShowTravelExpensePageModule {}
