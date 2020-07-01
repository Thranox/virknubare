import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {TravelExpense} from '../model/travel-expense.model';
import {TravelExpenseResource} from '../resources/travel-expense.resource';
import {FlowStep} from '../model/flow-step.model';
import {map} from "rxjs/operators";


@Injectable()
export class TravelExpenseService {

    constructor(
        private _travelExpenseResource: TravelExpenseResource,
    ) {
    }

    createTravelExpense(newTravelExpense: TravelExpense, customerId: string): Observable<string> {
        const data = newTravelExpense as any;
        data.customerId = customerId;
        data.CustomerId = customerId;
        data.arrivalDateTime = '2020-07-01T19:44:15.747Z';
        data.departureDateTime = '2020-07-01T19:44:15.747Z';
        return this._travelExpenseResource.createTravelExpense(newTravelExpense).pipe(
            map(value => value.id),
        );
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
