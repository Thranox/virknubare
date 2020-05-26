import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {SigninRedirectCallbackComponent} from './modules/authentication/signin-redirect-callback.component';
import {SignoutRedirectCallbackComponent} from './modules/authentication/signout-redirect-callback.component';
import {UnauthorizedComponent} from './modules/authentication/unauthorized.component';
import {FetchDataComponent} from './modules/fetch-data/fetch-data.component';


const routes: Routes = [
  {path: 'fetch-data', component: FetchDataComponent},
  {path: 'signin-redirect-callback', component: SigninRedirectCallbackComponent},
  {path: 'signout-redirect-callback', component: SignoutRedirectCallbackComponent},
  {path: 'unauthorized', component: UnauthorizedComponent},
  {path: 'travel-expenses', loadChildren: () => import('./modules/travel-expenses/travel-expenses.module').then(m => m.TravelExpensesModule)},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
