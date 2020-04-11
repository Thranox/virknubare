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
      baseUrl = 'https://localhost:44396/';
      console.info(token);
      const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      http.get<WeatherForecast[]>(baseUrl + 'weatherforecast', { headers: headers }).subscribe(result => {
        this.forecasts = result;
      }, error => console.error(error));
  });

  }
}

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
