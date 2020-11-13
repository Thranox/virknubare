import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { CreateTravelExpensePageRoutingModule } from './create-travel-expense-routing.module';
import { CreateTravelExpensePage } from './create-travel-expense.page';
import { MaterialModule } from '../../../shared/material/material.module';
@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        IonicModule,
        CreateTravelExpensePageRoutingModule,
        ReactiveFormsModule,
        MaterialModule
    ],
    declarations: [
        CreateTravelExpensePage,

    ]
})
export class CreateTravelExpensePageModule {}
