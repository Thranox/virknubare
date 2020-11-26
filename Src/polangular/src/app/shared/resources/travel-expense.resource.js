//import {Injectable} from "@angular/core";
//import {PolAPIResponse} from "../model/api-response.model";
//import {TravelExpense} from "../model/travel-expense.model";
//import {map} from "rxjs/operators";
//import {HttpClient} from "@angular/common/http";
//import {environment} from "../../../environments/environment";
//import {Observable, of} from "rxjs";
//import {FlowStep} from "../model/flow-step.model";
//@Injectable()
//export class TravelExpenseResource {
//    private baseUrl: string;
//    constructor(private httpClient: HttpClient) {
//        this.baseUrl = environment.apiUrl;
//    }
//    getTravelExpenses(): Observable<TravelExpense[]> {
//        return this.httpClient.get<PolAPIResponse<TravelExpense[]>>(this.baseUrl + 'travelexpenses').pipe(
//            map(response => {
//                return response.result;
//            })
//        );
//    }
//    createTravelExpense(newTravelExpense: any): Observable<{id: string}> {
//        const baseUrl = environment.apiUrl;
//        const postBody = newTravelExpense as any;
//        return this.httpClient.post<{id: string}>(baseUrl + 'travelexpenses', postBody);
//    }
//    getTravelExpenseById(id: string): Observable<TravelExpense> {
//        return this.httpClient.get<PolAPIResponse<TravelExpense>>(this.baseUrl + 'travelexpenses/' + id).pipe(
//            map(response => {
//                return response.result;
//            })
//        );
//    }
//    updateTravelExpense(travelExpense: TravelExpense): Observable<void> {
//        return this.httpClient.put<void>(this.baseUrl + 'travelexpenses/' + travelExpense.id, travelExpense);
//    }
//    processStep(travelExpense: TravelExpense, flowStep: FlowStep) {
//        const url = this.baseUrl + 'travelexpenses/' + travelExpense.id + '/FlowStep/' + flowStep.flowStepId;
//        return this.httpClient.post<any>(url, {});
//    }
//    getFlowSteps() {
//        return this.httpClient.get<any>(this.baseUrl + 'flowsteps');
//    }
//}
//# sourceMappingURL=travel-expense.resource.js.map