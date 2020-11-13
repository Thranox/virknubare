import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { TravelExpensesPage } from './travel-expenses.page';

const routes: Routes = [
  {
    path: '',
    component: TravelExpensesPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TravelExpensesPageRoutingModule {}
