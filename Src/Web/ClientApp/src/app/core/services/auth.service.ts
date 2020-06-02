import {Injectable} from '@angular/core';
import {UserManager, User, UserManagerSettings} from 'oidc-client';
import {Subject, from, Observable} from 'rxjs';
import {environment} from '../../../environments/environment';
import {map} from 'rxjs/operators';

@Injectable()
export class AuthService {
    private userManager: UserManager;
    private user: User;
    private loginChangedSubject = new Subject<boolean>();

    loginChanged = this.loginChangedSubject.asObservable();

    constructor() {
        const stsSettings = {
            authority: environment.stsAuthorityUrl,
            client_id: environment.stsClientId,
            redirect_uri: `${window.location.origin}/signin-redirect-callback`,
            scope: 'openid profile roles teapi',
            response_type: 'code',
            post_logout_redirect_uri: `${window.location.origin}/signout-redirect-callback`,
            automaticSilentRenew: true,
            loadUserInfo: true
        } as UserManagerSettings;
        this.userManager = new UserManager(stsSettings);
    }

    login() {
        return this.userManager.signinRedirect();
    }

    isLoggedIn(): Observable<boolean> {
        return this.getUser().pipe(
            map(user => {
                const userCurrent = !!user && !user.expired;
                if (this.user !== user) {
                    this.loginChangedSubject.next(userCurrent);
                }
                this.user = user;
                return userCurrent;
            })
        );
    }

    completeLogin() {
        return this.userManager.signinRedirectCallback().then(user => {
            this.user = user;
            this.loginChangedSubject.next(!!user && !user.expired);
            return user;
        }, (error) => {
            console.log('login error, session might have expired', error);
            this.user = null;
            this.logout();
        });
    }

    logout() {
        this.userManager.signoutRedirect();
    }

    completeLogout() {
        this.user = null;
        return this.userManager.signoutRedirectCallback();
    }

    getAccessToken() {
        return this.userManager.getUser().then(user => {
            if (!!user && !user.expired) {
                return user.access_token;
            } else {
                return null;
            }
        });
    }

    getUser() {
        return from(this.userManager.getUser());
    }
}
