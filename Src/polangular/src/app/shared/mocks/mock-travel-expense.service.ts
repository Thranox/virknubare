import {Injectable} from '@angular/core';
import {Observable, of, throwError} from 'rxjs';
import {TravelExpense} from '../model/travel-expense.model';
import {FlowStep} from '../model/flow-step.model';


@Injectable()
export class MockTravelExpenseService {

    entries: TravelExpense[] = [
        {
            id: '5afd5843-8456-4dbe-de20-08d802c4e857',
            description: 'Test opgaver fra mock service',
            customerId: '3fa85f64-5717-4562-b3fc-2c963f66afa6',
            isCertified: false,
            isReportedDone: false,
            isAssignedPayment: false,
            stageId: '1ecafa3b-5cdc-44c6-a9c5-ab5777e9c77a',
            stageText: 'Anvist til betaling',
            allowedFlows: [{flowStepId: '22539e03-4d6f-4761-808e-1e38298147d1', description: 'Udbetal'}],
            ownedByUserId: '7dca6bd3-f94b-4b74-989f-fb19f795d841',
            arrivalDateTime: '2020-06-27T15:20:14.401Z',
            departureDateTime: '2020-06-29T15:20:14.401Z',
            committeeId: 2,
            purpose: 'General forsamling',
            isEducationalPurpose: true,
            expenses: 10.45,
            isAbsenceAllowance: true,
            destinationPlace: {
                street: 'Mødevej',
                streetNumber: '3',
                zipCode: '8000'
            },
            arrivalPlace: {
                street: 'Hjemkomstvej',
                streetNumber: '3',
                zipCode: '8000'
            },
            departurePlace: {
                street: 'Afrejsevej',
                streetNumber: '3',
                zipCode: '8000'
            },
            transportSpecification: {
                method: 'car',
                kilometersCalculated: 144,
                kilometersCustom: 150,
                kilometersOverTaxLimit: 10,
                kilometersTax: 10,
                numberPlate: 'AJ12345'
            },
            dailyAllowanceAmount: {
                daysLessThan4hours: 2,
                daysMoreThan4hours: 3
            },
            foodAllowances: {
                morning: 2,
                lunch: 2,
                dinner: 1
            },
            lossOfEarnings: [
                {id:  '12', numberOfHours:  3, date: '2020-10-23'},
                {id:  '13', numberOfHours:  1, date: '2020-10-23'},
                {id:  '15', numberOfHours:  0, date: '2020-10-23'}
            ],
        },
    ];

    constructor() {
    }

    createTravelExpense(newTravelExpense: TravelExpense) {
        newTravelExpense.id = String(Math.random());
        this.entries.push(newTravelExpense);
        return of({});
    }

    getTravelExpenses(): Observable<TravelExpense[]> {
        return of(this.entries);
    }

    getTravelExpenseById(id: string): Observable<TravelExpense> {
        const foundEntry = this.entries.find(entry => entry.id === id);
        if (!foundEntry) {
            return throwError({status: 404});
        }
        return of(foundEntry);
    }

    updateTravelExpense(travelExpense: TravelExpense): Observable<TravelExpense> {
        const foundEntry = this.entries.find(entry => entry.id === travelExpense.id);
        if (!foundEntry) {
            return throwError({status: 404});
        }
        this.entries = this.entries.map(entry => {
            if (entry.id === travelExpense.id) {
                Object.assign(entry, travelExpense);
            }
            return entry;
        });
        return of(travelExpense);
    }

    processStep(travelExpense: TravelExpense, flowStep: FlowStep) {
        return of({});
    }

}
