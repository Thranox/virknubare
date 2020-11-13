import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent implements OnInit {

    isLoggedIn = false;
    isExpanded = false;

    constructor(private authService: AuthService) { }

    ngOnInit() {
        this.authService.loginChanged.subscribe(loggedIn => {
            this.isLoggedIn = loggedIn;
        });
    }

    login($event: Event) {
        $event.preventDefault();
        this.authService.login();
    }
}
