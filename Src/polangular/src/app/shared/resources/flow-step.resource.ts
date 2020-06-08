import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {TravelExpense} from "../model/travel-expense.model";
import {FlowStep} from "../model/flow-step.model";
import {Observable, of} from "rxjs";
import {PolAPIResponse} from "../model/api-response.model";
import {map} from "rxjs/operators";

@Injectable()
export class FlowStepResource {

    private readonly baseUrl: string;

    constructor(private _httpClient: HttpClient,
    ) {
        this.baseUrl = environment.apiUrl;
    }

    processStep(travelExpense: TravelExpense, flowStep: FlowStep) {
        const url = this.baseUrl + 'travelexpenses/' + travelExpense.id + '/ProcessStep/' + flowStep.key;
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
