import { Component, OnInit } from '@angular/core';
import {AuthService} from '../../core/services/auth-service.component';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {environment} from '../../../environments/environment';

@Component({
  selector: 'app-page2',
  templateUrl: './page2.component.html',
  styleUrls: ['./page2.component.scss']
})
export class Page2Component implements OnInit {
  rows: any[];

  constructor(
    private authService: AuthService,
    private http: HttpClient) { }

  ngOnInit(): void {
    this.rows = [1, 2, 4, 4];

    this.getMockData();
  }

  private getMockData() {
    this.authService.getAccessToken().then(token => {
      if (token === null) {
        console.info('No token, not logged in:' + token);
        this.rows = null;
      } else {
        const baseUrl = environment.apiUrl;
        console.info('Calling with token:' + token);
        const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
        this.http.get<any>(baseUrl + 'travelexpenses', {headers}).subscribe((result ) => {
            this.rows = result.result;
          },
          error => console.error(error));
      }
    });
  }
}
