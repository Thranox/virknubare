import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';

import { CoreModule } from './core/core.module';
import { SigninRedirectCallbackComponent } from "./home/signin-redirect-callback.component";
import { SignoutRedirectCallbackComponent } from "./home/signout-redirect-callback.component";
import { UnauthorizedComponent } from "./home/unauthorized.component";

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    SigninRedirectCallbackComponent,
    SignoutRedirectCallbackComponent,
    UnauthorizedComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    CoreModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'signin-redirect-callback', component: SigninRedirectCallbackComponent },
      { path: 'signout-redirect-callback', component: SignoutRedirectCallbackComponent },
      { path: 'unauthorized', component: UnauthorizedComponent }
    ]),
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
