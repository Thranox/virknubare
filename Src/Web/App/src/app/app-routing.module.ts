import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {Page2Component} from './modules/page2/page2.component';
import {SigninRedirectCallbackComponent} from './modules/authentication/signin-redirect-callback.component';
import {SignoutRedirectCallbackComponent} from './modules/authentication/signout-redirect-callback.component';
import {UnauthorizedComponent} from './modules/authentication/unauthorized.component';
import {FetchDataComponent} from "./modules/fetch-data/fetch-data.component";


const routes: Routes = [
  {
    path: '',
    redirectTo: '/auth/login',
    pathMatch: 'full'
  },
  {path: 'page2', component: Page2Component},
  {path: 'fetch-data', component: FetchDataComponent},
  {path: 'sign-out', component: Page2Component},
  {path: 'signin-redirect-callback', component: SigninRedirectCallbackComponent},
  {path: 'signout-redirect-callback', component: SignoutRedirectCallbackComponent},
  {path: 'unauthorized', component: UnauthorizedComponent},
  {path: 'travel-expense', loadChildren: () => import('./modules/travel-expenses/travel-expenses.module').then(m => m.TravelExpensesModule)},
  // Fallback when no prior routes is matched
  { path: '**', redirectTo: '/auth/login', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
