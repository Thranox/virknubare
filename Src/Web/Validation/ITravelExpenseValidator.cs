namespace Web.Validation
{
    public interface ITravelExpenseValidator
    {
        TravelExpenseValidationResult GetValidationResult(IValidationInput validationInput);
    }
}