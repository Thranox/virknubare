using System.Collections.Generic;

namespace Web.Validation.ValidationRules
{
    public class TravelExpenseValidatorRule_ReportDone_FieldsCompleted : ITravelExpenseValidatorRule
    {
        public bool RuleApplies(IValidationInput validationInput)
        {
            return
                validationInput.Context == ValidationInputContextEnum.ReportDone;
        }

        public IEnumerable<TravelExpenseValidationItem> GetValidations(IValidationInput validationInput)
        {
            if (string.IsNullOrEmpty(validationInput?.TravelExpenseEntity?.Description ))
                yield return new TravelExpenseValidationItem(TravelExpenseValidationLevelEnum.Error, "Den angivne rejseafregning mangler description");
        }
    }
}