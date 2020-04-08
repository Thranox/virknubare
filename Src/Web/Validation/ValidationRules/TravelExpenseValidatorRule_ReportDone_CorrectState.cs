using System.Collections.Generic;

namespace Web.Validation.ValidationRules
{
    public class TravelExpenseValidatorRule_ReportDone_CorrectState : ITravelExpenseValidatorRule
    {
        public bool RuleApplies(IValidationInput validationInput)
        {
            return
                validationInput.Context == ValidationInputContextEnum.ReportDone;
        }

        public IEnumerable<TravelExpenseValidationItem> GetValidations(IValidationInput validationInput)
        {
            if (validationInput.TravelExpenseEntity.IsReportedDone )
                yield return new TravelExpenseValidationItem(TravelExpenseValidationLevelEnum.Error, "Den angivne rejseafregning er allerede meldt færdig.");
        }
    }
}