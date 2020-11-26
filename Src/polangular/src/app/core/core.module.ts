import {ModuleWithProviders, NgModule} from '@angular/core';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import { AuthInterceptorService } from './intercepters/auth-interceptor.service';
import { AuthService } from './services/auth.service';


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
  static forRoot(): ModuleWithProviders<CoreModule> {
    return {
      ngModule: CoreModule,
      providers: [
        AuthService,
        
        { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptorService, multi: true },
        ...PROVIDERS
      ]
    };
  }
}
