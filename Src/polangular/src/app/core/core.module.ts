import {ModuleWithProviders, NgModule} from '@angular/core';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import { AuthInterceptorService } from './intercepters/auth-interceptor.service';
import { AuthService } from './services/auth.service';
import {TravelExpenseService} from '../shared/services/travel-expense.service';

const PROVIDERS = [

];

@NgModule({
    imports: [
    ],
    exports: [
    ],
    declarations: [
    ],
})
export class CoreModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: CoreModule,
      providers: [
        AuthService,
        TravelExpenseService,
        { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptorService, multi: true },
        ...PROVIDERS
      ]
    };
  }
}
