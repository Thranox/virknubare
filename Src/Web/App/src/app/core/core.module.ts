import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptorService } from './intercepters/auth-interceptor.service';
import { AuthService } from './services/auth.service';
import {TravelExpenseService} from '../shared/services/travel-expense.service';

@NgModule({
    imports: [],
    exports: [
    ],
    declarations: [
    ],
    providers: [
        AuthService,
        TravelExpenseService,
        { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptorService, multi: true }
    ],
})
export class CoreModule { }
