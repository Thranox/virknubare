namespace Domain.ValueObjects
{
    public enum TravelExpenseStage
    {
        Initial=0,
        ReportedDone = 1,
        Certified=2,
        AssignedForPayment=3,
        Final=4
    }
}
