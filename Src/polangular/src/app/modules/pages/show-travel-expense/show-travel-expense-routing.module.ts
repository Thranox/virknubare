import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ShowTravelExpensePage } from './show-travel-expense.page';

const routes: Routes = [
  {
    path: '',
    component: ShowTravelExpensePage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ShowTravelExpensePageRoutingModule {}
