import { Component } from '@angular/core';
import {AuthService} from '../../core/services/auth.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent {
  isLoggedIn = false;
  isExpanded = false;

  constructor(private authService: AuthService) {
    this.authService.loginChanged.subscribe(loggedIn => {
      this.isLoggedIn = loggedIn;
    });
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
  login($event: Event) {
    $event.preventDefault();
    this.authService.login();
  }

  logout($event: Event) {
    $event.preventDefault();
    this.authService.logout();
  }

}
