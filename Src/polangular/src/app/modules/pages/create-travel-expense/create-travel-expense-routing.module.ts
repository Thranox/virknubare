import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CreateTravelExpensePage } from './create-travel-expense.page';

const routes: Routes = [
  {
    path: '',
    component: CreateTravelExpensePage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CreateTravelExpensePageRoutingModule {}
