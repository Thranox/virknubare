import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TravelExpensesRoutingModule } from './travel-expenses-routing.module';
import { IndexComponent } from './pages/index/index.component';


@NgModule({
  declarations: [IndexComponent],
  imports: [
    CommonModule,
    TravelExpensesRoutingModule
  ]
})
export class TravelExpensesModule { }
