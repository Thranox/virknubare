import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TravelExpensesRoutingModule } from './travel-expenses-routing.module';
import { IndexComponent } from './pages/index/index.component';
import {DetailsComponent} from './pages/details/details.component';


@NgModule({
  declarations: [IndexComponent, DetailsComponent],
  imports: [
    CommonModule,
    TravelExpensesRoutingModule
  ]
})
export class TravelExpensesModule { }
