import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from "rxjs";
import {TravelExpense} from "../model/travel-expense.model";
import {TravelExpenseResource} from "../resources/travel-expense.resource";
import {FlowStep} from "../model/flow-step.model";


@Injectable()
export class TravelExpenseService {

    constructor(private _httpClient: HttpClient,
                private _travelExpenseResource: TravelExpenseResource,
    ) {
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

    getFlowsteps(): Observable<any> {
        return this._travelExpenseResource.getFlowSteps();
    }

    updateTravelExpense(travelExpense: TravelExpense): Observable<void> {
        return this._travelExpenseResource.updateTravelExpense(travelExpense);
    }

    processStep(travelExpense: TravelExpense, flowStep: FlowStep) {
        return this._travelExpenseResource.processStep(travelExpense, flowStep);
    }
}