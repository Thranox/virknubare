import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LossOfEarningsPage } from './loss-of-earnings.page';

const routes: Routes = [
  {
    path: '',
    component: LossOfEarningsPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class LossOfEarningsPageRoutingModule {}
