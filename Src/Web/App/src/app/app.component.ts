import { Component } from '@angular/core';
import {AuthService} from "./core/auth-service.component";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'App';
  private isLoggedIn: boolean;

  constructor(private authService: AuthService) {
    this.authService.isLoggedIn().then(loggedIn => {
      this.isLoggedIn = loggedIn;
    });
  }
}
