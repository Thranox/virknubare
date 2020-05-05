import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from "../core/auth-service.component";

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public forecasts: WeatherForecast[];

  constructor(http: HttpClient, authService: AuthService, @Inject('BASE_URL') baseUrl: string) {
    authService.getAccessToken().then(token => {
      if (token === null) {
        console.info('No token, not logged in:' + token);
        this.forecasts = null;
      } else {
        //baseUrl = 'https://localhost:44324/'; // Api via WebApp's own API
        baseUrl = 'https://localhost:44348/'; //PolAPI
        //baseUrl = 'https://dev.politikerafregning.dk/polapi/'; //PolAPI
        //baseUrl = 'https://andersathome.dk/polapi/'; //PolAPI
        //baseUrl = 'https://ajf-prod-02/polapi/'; //PolAPI
        console.info('Calling with token:' + token);
        const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
        http.get<WeatherForecast[]>(baseUrl + 'weatherforecast', { headers: headers }).subscribe(result => {
            this.forecasts = result;
          },
          error => console.error(error));
      }
  });
  }
}

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
