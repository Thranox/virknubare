import { Component } from '@angular/core';
import { AuthService } from './core/services/auth.service';

import { Platform } from '@ionic/angular';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'ClientApp';
  private isLoggedIn: boolean;

    constructor(private authService: AuthService, private platform: Platform,
      ) {
    this.authService.isLoggedIn().subscribe(loggedIn => {
        this.isLoggedIn = loggedIn;
    },
    );
  }
}
