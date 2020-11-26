import { BrowserModule } from '@angular/platform-browser';
import {LOCALE_ID, NgModule} from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import 'dayjs/locale/da'; // import locale

import * as dayjs from 'dayjs';

import {CoreModule} from './core/core.module';
import {HttpClientModule} from '@angular/common/http';


import {FormsModule} from '@angular/forms';
//import {SharedModule} from './shared/shared.module';
import localeDa from '@angular/common/locales/da';
import localeExtraDa from '@angular/common/locales/extra/da';
import {HashLocationStrategy, LocationStrategy, registerLocaleData} from '@angular/common';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { SignInComponent } from './modules/authentication/sign-in/sign-in.component';
import { IonicModule } from '@ionic/angular';
import { NoopAnimationsModule, BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './shared/material/material.module';
import { SharedModule } from './shared/shared.module';

registerLocaleData(localeDa, 'da', localeExtraDa);

dayjs.locale('da');

@NgModule({
    declarations: [
        AppComponent,
        PageNotFoundComponent,
        SignInComponent,
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        NgbModule,
        CoreModule.forRoot(),
        HttpClientModule,
        FormsModule,
        
        BrowserAnimationsModule,

        SharedModule.forRoot(),
        IonicModule.forRoot(),
        NoopAnimationsModule,
    ],
    providers: [
        { provide: LOCALE_ID, useValue: 'da-DK' }, // For angular date-pipe
        {provide: LocationStrategy, useClass: HashLocationStrategy}
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
