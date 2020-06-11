import { Injectable } from '@angular/core';
import {CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router} from '@angular/router';
import { Observable } from 'rxjs';
import {AuthService} from "../services/auth.service";
import {map, tap} from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class UserSignedInGuard implements CanActivate {
    constructor(
        private router: Router,
        private authService: AuthService,
        ) {
    }
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.authService.isLoggedIn().pipe(
        tap((isLoggedIn) => {
            if (isLoggedIn === false) {
                this.router.navigate(['sign-in']);
            }
        }),
    );
  }

}
