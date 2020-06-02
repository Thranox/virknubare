import {Injectable} from '@angular/core';
import {Observable, of, throwError} from 'rxjs';
import {TravelExpense} from '../model/travel-expense.model';


@Injectable()
export class MockTravelExpenseService {

    entries: TravelExpense[] = [
        {description: 'From kata', id: '5afd5843-8456-4dbe-de20-08d802c4e857', isCertified: false, isReportedDone: false, isAssignedPayment: false, stageId: '1ecafa3b-5cdc-44c6-a9c5-ab5777e9c77a', stageText: 'Anvist til betaling', allowedFlows: [{flowStepId: '22539e03-4d6f-4761-808e-1e38298147d1', description: 'Udbetal'}], ownedByUserId: '7dca6bd3-f94b-4b74-989f-fb19f795d841'},
        {description: 'Description5', id: 'de2343e4-e915-4691-b765-2ae183132a1c', isCertified: false, isReportedDone: false, isAssignedPayment: false, stageId: '1ecafa3b-5cdc-44c6-a9c5-ab5777e9c77a', stageText: 'Anvist til betaling', allowedFlows: [{flowStepId: '22539e03-4d6f-4761-808e-1e38298147d1', description: 'Udbetal'}], ownedByUserId: '7dca6bd3-f94b-4b74-989f-fb19f795d841'},
        {description: 'Description6', id: '7f665522-ca4e-46f3-9bad-2e1479e4e53d', isCertified: false, isReportedDone: false, isAssignedPayment: false, stageId: '1ecafa3b-5cdc-44c6-a9c5-ab5777e9c77a', stageText: 'Anvist til betaling', allowedFlows: [{flowStepId: '22539e03-4d6f-4761-808e-1e38298147d1', description: 'Udbetal'}], ownedByUserId: '7dca6bd3-f94b-4b74-989f-fb19f795d841'},
        {description: 'Description7', id: '45fe1eea-a179-4fb9-942c-2f3a11ccfb86', isCertified: false, isReportedDone: false, isAssignedPayment: false, stageId: '1ecafa3b-5cdc-44c6-a9c5-ab5777e9c77a', stageText: 'Anvist til betaling', allowedFlows: [{flowStepId: '22539e03-4d6f-4761-808e-1e38298147d1', description: 'Udbetal'}], ownedByUserId: '7dca6bd3-f94b-4b74-989f-fb19f795d841'},
        {description: 'Description3', id: '40243919-d256-42e1-93c6-587f528b51da', isCertified: false, isReportedDone: false, isAssignedPayment: false, stageId: 'b7c6915f-9406-427b-a482-34e2b8b6e2eb', stageText: 'Kladde', allowedFlows: [{flowStepId: '172b314b-17be-4b8a-80ed-49a5f77c50f3', description: 'F�rdigmeld'}], ownedByUserId: '7dca6bd3-f94b-4b74-989f-fb19f795d841'},
        {description: 'Rettet fra Angular 123', id: '8c55e4f8-4cb5-4a04-89cb-70c7d403dae3', isCertified: false, isReportedDone: false, isAssignedPayment: false, stageId: 'b7c6915f-9406-427b-a482-34e2b8b6e2eb', stageText: 'Kladde', allowedFlows: [{flowStepId: '172b314b-17be-4b8a-80ed-49a5f77c50f3', description: 'F�rdigmeld'}], ownedByUserId: '7dca6bd3-f94b-4b74-989f-fb19f795d841'},
        {description: 'Afregning 1', id: 'aef4521d-8a8f-4d8c-9b6d-9d0135e5d83f', isCertified: false, isReportedDone: false, isAssignedPayment: false, stageId: 'b7c6915f-9406-427b-a482-34e2b8b6e2eb', stageText: 'Kladde', allowedFlows: [{flowStepId: '172b314b-17be-4b8a-80ed-49a5f77c50f3', description: 'F�rdigmeld'}], ownedByUserId: '7dca6bd3-f94b-4b74-989f-fb19f795d841'},
        {description: 'Description1', id: '7c258aa1-a792-4248-9707-d93a52dc1ea4', isCertified: false, isReportedDone: false, isAssignedPayment: false, stageId: 'b7c6915f-9406-427b-a482-34e2b8b6e2eb', stageText: 'Kladde', allowedFlows: [{flowStepId: '172b314b-17be-4b8a-80ed-49a5f77c50f3', description: 'F�rdigmeld'}], ownedByUserId: '7dca6bd3-f94b-4b74-989f-fb19f795d841'},
        {description: 'Description9', id: '4164e53d-e84b-459a-bfa1-e7ac8e11f295', isCertified: false, isReportedDone: false, isAssignedPayment: false, stageId: '1ecafa3b-5cdc-44c6-a9c5-ab5777e9c77a', stageText: 'Anvist til betaling', allowedFlows: [{flowStepId: '22539e03-4d6f-4761-808e-1e38298147d1', description: 'Udbetal'}], ownedByUserId: '7dca6bd3-f94b-4b74-989f-fb19f795d841'},
        {description: 'Description8', id: '4fb39d8b-c991-4470-8fb4-ed18f3eb6cf3', isCertified: false, isReportedDone: false, isAssignedPayment: false, stageId: '1ecafa3b-5cdc-44c6-a9c5-ab5777e9c77a', stageText: 'Anvist til betaling', allowedFlows: [{flowStepId: '22539e03-4d6f-4761-808e-1e38298147d1', description: 'Udbetal'}], ownedByUserId: '7dca6bd3-f94b-4b74-989f-fb19f795d841'}
    ];

    constructor(
    ) {
    }

    createTravelExpense(newTravelExpense: TravelExpense) {
        this.entries.push(newTravelExpense);
        return of({});
    }

    getTravelExpenses(): Observable<TravelExpense[]> {
        return of(this.entries);
    }

    getTravelExpenseById(id: string): Observable<TravelExpense> {
        const foundEntry = this.entries.find(entry => entry.id === id);
        if (! foundEntry) {
            return throwError(404);
        }
        return of(foundEntry);
    }

    updateTravelExpense(travelExpense: TravelExpense): Observable<TravelExpense> {
        const foundEntry = this.entries.find(entry => entry.id === travelExpense.id);
        if (! foundEntry) {
            return throwError(404);
        }
        this.entries = this.entries.map(entry => {
            if (entry.id === travelExpense.id) {
                Object.assign(entry, travelExpense);
            }
            return entry;
        });
        return of(travelExpense);
    }

}
