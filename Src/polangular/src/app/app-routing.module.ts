import {NgModule} from '@angular/core';
import {Routes, RouterModule, UrlSegment} from '@angular/router';
import {SigninRedirectCallbackComponent} from './modules/authentication/signin-redirect-callback.component';
import {SignoutRedirectCallbackComponent} from './modules/authentication/signout-redirect-callback.component';
import {UnauthorizedComponent} from './modules/authentication/unauthorized.component';
import {PageNotFoundComponent} from './components/page-not-found/page-not-found.component';
import {UserSignedInGuard} from "./core/guards/user-signed-in.guard";
import {SignInComponent} from "./modules/authentication/sign-in/sign-in.component";


const routes: Routes = [
    {path: '', redirectTo: 'home', pathMatch: 'full'},
    {path: 'sign-in', component: SignInComponent},
    {path: 'signin-redirect-callback', component: SigninRedirectCallbackComponent},
    {path: 'signout-redirect-callback', component: SignoutRedirectCallbackComponent},
    {path: 'unauthorized', component: UnauthorizedComponent},
    {
        path: 'home',
        loadChildren: () => import('./home/home.module').then(m => m.HomePageModule),
        canActivate: [UserSignedInGuard]
    },
    {path: '404', component: PageNotFoundComponent},
    {path: '**', redirectTo: '/404'},
  {
    path: 'create-travel-expense',
    loadChildren: () => import('./modules/pages/create-travel-expense/create-travel-expense.module').then( m => m.CreateTravelExpensePageModule)
  },
  {
    path: 'loss-of-earnings',
    loadChildren: () => import('./modules/pages/loss-of-earnings/loss-of-earnings.module').then( m => m.LossOfEarningsPageModule)
  },
  {
    path: 'reports',
    loadChildren: () => import('./modules/pages/reports/reports.module').then( m => m.ReportsPageModule)
  },
  {
    path: 'archives',
    loadChildren: () => import('./modules/pages/archives/archives.module').then( m => m.ArchivesPageModule)
  },
  {
    path: 'help',
    loadChildren: () => import('./modules/pages/help/help.module').then( m => m.HelpPageModule)
  },
  {
    path: 'show-travel-expense',
    loadChildren: () => import('./modules/pages/show-travel-expense/show-travel-expense.module').then( m => m.ShowTravelExpensePageModule)
  },

];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule {
}
