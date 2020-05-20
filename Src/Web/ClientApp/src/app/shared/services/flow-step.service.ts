import {Injectable} from '@angular/core';
import {Observable} from "rxjs";
import {TravelExpense} from "../model/travel-expense.model";
import {FlowStep} from "../model/flow-step.model";
import {FlowStepResource} from "../resources/flow-step.resource";


@Injectable()
export class FlowStepService {

    constructor(
        private _flowStepResource: FlowStepResource,
    ) {
    }

    processStep(travelExpense: TravelExpense, flowStep: FlowStep) {
        return this._flowStepResource.processStep(travelExpense, flowStep);

    }

    getFlowsteps(): Observable<FlowStep[]> {
        return this._flowStepResource.getFlowsteps();
    }

}
