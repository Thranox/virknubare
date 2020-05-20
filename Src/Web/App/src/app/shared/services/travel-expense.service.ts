import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {environment} from '../../../environments/environment';
import {AuthService} from '../../core/auth-service.component';
import {Observable, of} from "rxjs";
import {TravelExpense} from "../model/travel-expense.model";
import {FlowStep} from "../model/flow-step.model";
import {PolAPIResponse} from "../model/api-response.model";
import {map} from "rxjs/operators";
import {TravelExpenseResource} from "../resources/travel-expense.resource";


@Injectable()
export class TravelExpenseService {

    private token: string;

    constructor(private _httpClient: HttpClient,
                private _travelExpenseResource: TravelExpenseResource,
                private _authService: AuthService) {
        this._authService.getAccessToken().then(token => {
            this.token = token;
        });
    }

    createTravelExpense(newTravelExpense: TravelExpense) {
        return this._travelExpenseResource.createTravelExpense(newTravelExpense);
    }

    getTravelExpenses(): Observable<TravelExpense[]> {
        return this._travelExpenseResource.getTravelExpenses();
    }

    getTravelExpenseById(id: string): Observable<TravelExpense> {
        return this._travelExpenseResource.getTravelExpenseById(id);
    }

    updateTravelExpense(travelExpense: TravelExpense): Observable<void> {
        return this._travelExpenseResource.updateTravelExpense(travelExpense);
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
    return this._httpClient.get<PolAPIResponse<FlowStep[]>>(baseUrl + 'flowsteps').pipe(
      map(response => {
        return response.result;
      })
    );
  }

}
