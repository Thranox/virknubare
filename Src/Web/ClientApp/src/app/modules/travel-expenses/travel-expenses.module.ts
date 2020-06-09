import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TravelExpensesRoutingModule } from './travel-expenses-routing.module';
import { IndexComponent } from './pages/index/index.component';
import {DetailsComponent} from './pages/details/details.component';
import { TravelExpenseFormComponent } from './components/travel-expense-form/travel-expense-form.component';
import {ReactiveFormsModule} from '@angular/forms';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';


@NgModule({
  declarations: [IndexComponent, DetailsComponent, TravelExpenseFormComponent],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        TravelExpensesRoutingModule,
        NgbModule
    ]
})
export class TravelExpensesModule { }
