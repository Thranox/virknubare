import {NgModule} from '@angular/core';
import {Routes, RouterModule, UrlSegment} from '@angular/router';
import {SigninRedirectCallbackComponent} from './modules/authentication/signin-redirect-callback.component';
import {SignoutRedirectCallbackComponent} from './modules/authentication/signout-redirect-callback.component';
import {UnauthorizedComponent} from './modules/authentication/unauthorized.component';
import {FetchDataComponent} from './modules/fetch-data/fetch-data.component';
import {PageNotFoundComponent} from './components/page-not-found/page-not-found.component';
import {UserSignedInGuard} from "./core/guards/user-signed-in.guard";
import {SignInComponent} from "./modules/authentication/sign-in/sign-in.component";


const routes: Routes = [
    {path: '', redirectTo: 'travel-expenses', pathMatch: 'full'},
    {path: 'fetch-data', component: FetchDataComponent},
    {path: 'sign-in', component: SignInComponent},
    {path: 'signin-redirect-callback', component: SigninRedirectCallbackComponent},
    {path: 'signout-redirect-callback', component: SignoutRedirectCallbackComponent},
    {path: 'unauthorized', component: UnauthorizedComponent},
    {
        path: 'travel-expenses',
        loadChildren: () => import('./modules/travel-expenses/travel-expenses.module').then(m => m.TravelExpensesModule),
        canActivate: [UserSignedInGuard]
    },
    {path: '404', component: PageNotFoundComponent},
    {path: '**', redirectTo: '/404'}
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule {
}
export function PathExcluding(url: UrlSegment[]): any {
    return url.length === 1 && !(url[0].path.includes('url-Path-to-Exlude')) ? ({consumed: url}) : undefined;
}