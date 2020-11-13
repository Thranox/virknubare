import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { HomePageRoutingModule } from './home-routing.module';

import { HomePage } from './home.page';
import { RouterModule } from '@angular/router';
import { MaterialModule } from '../shared/material/material.module';


@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        IonicModule,
        HomePageRoutingModule,
        RouterModule,
        ReactiveFormsModule,
        MaterialModule
    ],
    declarations: [
        HomePage
    ]
})
export class HomePageModule {}
