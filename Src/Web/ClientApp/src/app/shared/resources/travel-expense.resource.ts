import {Injectable} from "@angular/core";
import {PolAPIResponse} from "../model/api-response.model";
import {TravelExpense} from "../model/travel-expense.model";
import {map} from "rxjs/operators";
import {HttpClient} from "@angular/common/http";
import {AuthService} from "../../core/services/auth.service";
import {environment} from "../../../environments/environment";
import {Observable, of} from "rxjs";
import {FlowStep} from "../model/flow-step.model";

@Injectable()
export class TravelExpenseResource {

    private baseUrl: string;

    constructor(private _httpClient: HttpClient,
                private _authService: AuthService) {

        this.baseUrl = environment.apiUrl;
    }

    getTravelExpenses(): Observable<TravelExpense[]> {
        return this._httpClient.get<PolAPIResponse<TravelExpense[]>>(this.baseUrl + 'travelexpenses').pipe(
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

    getTravelExpenseById(id: string): Observable<TravelExpense> {
        return this._httpClient.get<PolAPIResponse<TravelExpense>>(this.baseUrl + 'travelexpenses/' + id).pipe(
            map(response => {
                return response.result;
            })
        );
    }

    updateTravelExpense(travelExpense: TravelExpense): Observable<void> {
        return this._httpClient.put<void>(this.baseUrl + 'travelexpenses/' + travelExpense.id, travelExpense);
    }

    processStep(travelExpense: TravelExpense, flowStep: FlowStep) {
        const url = this.baseUrl + 'travelexpenses/' + travelExpense.id + '/FlowStep/' + flowStep.flowStepId;
        return this._httpClient.post<any>(url, {});

    }

    getFlowSteps() {
        return this._httpClient.get<any>(this.baseUrl + 'flowsteps');
    }
}
