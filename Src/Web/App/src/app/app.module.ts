import { BrowserModule } from '@angular/platform-browser';
import {LOCALE_ID, NgModule} from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import 'dayjs/locale/da'; // import locale
import * as dayjs from 'dayjs';
import { Page2Component } from './modules/page2/page2.component';
import {NavMenuComponent} from './components/nav-menu/nav-menu.component';
import {CoreModule} from './core/core.module';
import {HttpClientModule} from '@angular/common/http';
import { FooterComponent } from './components/footer/footer.component';
import {FetchDataComponent} from './modules/fetch-data/fetch-data.component';
import {FormsModule} from '@angular/forms';

dayjs.locale('da');

@NgModule({
  declarations: [
    AppComponent,
    Page2Component,
    NavMenuComponent,
    FooterComponent,
    FetchDataComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgbModule,
    CoreModule,
    HttpClientModule,
    FormsModule,
  ],
  providers: [
    { provide: LOCALE_ID, useValue: 'da-DK' } // For angular date-pipe and custom appDate pipe
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
