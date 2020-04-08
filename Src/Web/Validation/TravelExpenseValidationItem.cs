namespace Web.Validation
{
    public class TravelExpenseValidationItem
    {
        public TravelExpenseValidationLevelEnum TravelExpenseValidationLevelEnum { get; }
        public string Message { get; }

        public TravelExpenseValidationItem(TravelExpenseValidationLevelEnum travelExpenseValidationLevelEnum, string message)
        {
            TravelExpenseValidationLevelEnum = travelExpenseValidationLevelEnum;
            Message = message;
        }
    }
}