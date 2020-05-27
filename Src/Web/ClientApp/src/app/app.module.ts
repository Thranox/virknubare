import { BrowserModule } from '@angular/platform-browser';
import {LOCALE_ID, NgModule} from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import 'dayjs/locale/da'; // import locale
import * as dayjs from 'dayjs';
import {NavMenuComponent} from './components/nav-menu/nav-menu.component';
import {CoreModule} from './core/core.module';
import {HttpClientModule} from '@angular/common/http';
import { FooterComponent } from './components/footer/footer.component';
import {FetchDataComponent} from './modules/fetch-data/fetch-data.component';
import {FormsModule} from '@angular/forms';
import {SharedModule} from './shared/shared.module';
import localeDa from '@angular/common/locales/da';
import localeExtraDa from '@angular/common/locales/extra/da';
import {registerLocaleData} from '@angular/common';
registerLocaleData(localeDa, 'da', localeExtraDa);

dayjs.locale('da');

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    FooterComponent,
    FetchDataComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgbModule,
    CoreModule.forRoot(),
    HttpClientModule,
    FormsModule,
    SharedModule.forRoot(),
  ],
  providers: [
    { provide: LOCALE_ID, useValue: 'da-DK' } // For angular date-pipe
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
