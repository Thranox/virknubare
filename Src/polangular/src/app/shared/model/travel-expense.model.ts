export class TravelExpense {
  description: string | null;
  id: string;
  isAssignedPayment: boolean;
  isCertified: boolean;
  isReportedDone: boolean;
  stageId: string | null;
  stageText: string | null;
  ownedByUserId: string;
  allowedFlows: {flowStepId: string, description: string}[];
}
