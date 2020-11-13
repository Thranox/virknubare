import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { LossOfEarningsPageRoutingModule } from './loss-of-earnings-routing.module';

import { LossOfEarningsPage } from './loss-of-earnings.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    LossOfEarningsPageRoutingModule
  ],
  declarations: [LossOfEarningsPage]
})
export class LossOfEarningsPageModule {}
