import {Component, OnInit} from '@angular/core';
import {AuthService} from '../../core/services/auth.service';
import {Observable, Subscription} from 'rxjs';
import {filter, switchMap} from "rxjs/operators";
import {User} from "oidc-client";

@Component({
    selector: 'app-footer',
    templateUrl: './footer.component.html',
    styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit {
    user: Observable<User | null>;

    constructor(private authService: AuthService) {
    }

    ngOnInit(): void {
        this.user = this.authService.loginChanged
            .pipe(
                filter(isLoggedIn => isLoggedIn === true),
                switchMap(isLoggedIn => {
                    return this.authService.getUser();
                }),
            );

    }

}
