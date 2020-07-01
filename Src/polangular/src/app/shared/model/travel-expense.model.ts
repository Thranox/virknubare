export class TravelExpense {
    id: string;
    description: string | null;
    customerId: string | null;
    isAssignedPayment: boolean;
    isCertified: boolean;
    isReportedDone: boolean;
    stageId: string | null;
    stageText: string | null;
    ownedByUserId: string;
    allowedFlows: { flowStepId: string, description: string }[];
    arrivalDateTime: string;
    departureDateTime: string;
    committeeId: number;
    purpose: string;
    isEducationalPurpose: true;
    expenses: number;
    isAbsenceAllowance: true;
    destinationPlace: {
        street: string;
        streetNumber: string;
        zipCode: string
    };
    arrivalPlace: {
        street: string;
        streetNumber: string;
        zipCode: string
    };
    departurePlace: {
        street: string;
        streetNumber: string;
        zipCode: string
    };
    transportSpecification: {
        kilometersCalculated: number;
        kilometersCustom: number;
        kilometersOverTaxLimit: number;
        kilometersTax: number;
        method: string;
        numberPlate: string
    };
    dailyAllowanceAmount: {
        daysLessThan4hours: number,
        daysMoreThan4hours: number
    };
    foodAllowances: {
        morning: number,
        lunch: number,
        dinner: number
    };
    lossOfEarnings: {id: string, numberOfHours: number, date: string}[];
}
