import {
    HttpEvent,
    HttpHandler,
    HttpHeaders,
    HttpInterceptor,
    HttpRequest,
    HttpErrorResponse
} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Router} from '@angular/router';
import {from, Observable} from 'rxjs';
import {tap} from 'rxjs/operators';
import {AuthService} from '../services/auth.service';
import {environment} from '../../../environments/environment';

@Injectable()
export class AuthInterceptorService implements HttpInterceptor {
    constructor(
        private authService: AuthService,
        private router: Router) {

    }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const requestRequiresAuthorization = req.url.startsWith(environment.apiUrl);

        if (!requestRequiresAuthorization) {
            return next.handle(req);
        }
        return from(this.authService.getAccessToken().then(token => {
            const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
            const authReq = req.clone({headers});

            return next.handle(authReq).pipe(tap(_ => {
            }, error => {
                const respError = error as HttpErrorResponse;
                if (respError && (respError.status === 401 || respError.status === 403)) {
                    this.router.navigate(['/unauthorized']);
                }
            })).toPromise();
        }));
    }
}
