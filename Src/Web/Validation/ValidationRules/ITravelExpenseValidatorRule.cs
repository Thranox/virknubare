using System.Collections.Generic;

namespace Web.Validation.ValidationRules
{
    public interface ITravelExpenseValidatorRule
    {
        bool RuleApplies(IValidationInput validationInput);
        IEnumerable<TravelExpenseValidationItem> GetValidations(IValidationInput validationInput);
    }
}