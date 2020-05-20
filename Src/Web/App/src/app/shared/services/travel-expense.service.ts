import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {environment} from '../../../environments/environment';
import {AuthService} from '../../core/auth-service.component';
import {Observable, of} from "rxjs";
import {TravelExpense} from "../model/travel-expense.model";
import {FlowStep} from "../model/flow-step.model";
import {PolAPIResponse} from "../model/api-response.model";
import {map} from "rxjs/operators";


@Injectable()
export class TravelExpenseService {

  private token: string;

  constructor(private _httpClient: HttpClient,
              private _authService: AuthService) {
    this._authService.getAccessToken().then(token => {
      this.token = token;
    });
  }

  getTravelExpenses() : Observable<TravelExpense[]> {
    const baseUrl = environment.apiUrl;
    console.info('Calling with token:' + this.token);
    return this._httpClient.get<PolAPIResponse<TravelExpense[]>>(baseUrl + 'travelexpenses').pipe(
      map(response => {
        return response.result;
      })
    );
  }

  getTravelExpenseById(id: string) : Observable<TravelExpense> {
    const baseUrl = environment.apiUrl;
    console.info('Calling with token:' + this.token);
    return this._httpClient.get<PolAPIResponse<TravelExpense>>(baseUrl + 'travelexpenses/' + id).pipe(
      map(response => {
        return response.result;
      })
    );
  }

  updateTravelExpense(travelExpense: TravelExpense): Observable<void> {
    const baseUrl = environment.apiUrl;
    const postBody = travelExpense;
    return this._httpClient.put<void>(baseUrl + 'travelexpenses/' + travelExpense.id, postBody);
  }

  processStep(travelExpense: TravelExpense, flowStep: FlowStep) {
    const baseUrl = environment.apiUrl;
    const url = baseUrl + 'travelexpenses/' + travelExpense.id + '/ProcessStep/' + flowStep.key;
    console.log('Sending to url:', url);
    console.warn('Not yet implemented');
    return of([]);
    // return this._httpClient.post<TravelExpenseResponse>(url, {});

  }

  getFlowsteps(): Observable<FlowStep[]> {
    const baseUrl = environment.apiUrl;
    console.info('Calling with token:' + this.token);
    return this._httpClient.get<PolAPIResponse<FlowStep[]>>(baseUrl + 'flowsteps').pipe(
      map(response => {
        return response.result;
      })
    );
  }

  createTravelExpense(newTravelExpense: TravelExpense) {
    const baseUrl = environment.apiUrl;
    const postBody = {
      description: newTravelExpense.description,
      customerId: "00000000-0000-0000-0000-000000000000",
    };

    console.warn('Not yet implemented');
    return of([]);

    // return this._httpClient.post<void>(baseUrl + 'travelexpenses', postBody);

  }
}
