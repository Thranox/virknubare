import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptorService } from './auth-interceptor.service';
import { AuthService } from './auth-service.component';
import {TravelExpenseService} from "../shared/services/travel-expense.service";

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
