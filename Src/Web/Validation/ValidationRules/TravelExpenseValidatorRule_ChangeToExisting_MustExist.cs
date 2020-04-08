using System.Collections.Generic;

namespace Web.Validation.ValidationRules
{
    public class TravelExpenseValidatorRule_ChangeToExisting_MustExist : ITravelExpenseValidatorRule
    {
        public bool RuleApplies(IValidationInput validationInput)
        {
            return
                validationInput.Context == ValidationInputContextEnum.Update ||
                validationInput.Context == ValidationInputContextEnum.ReportDone ||
                validationInput.Context == ValidationInputContextEnum.Certify;
        }

        public IEnumerable<TravelExpenseValidationItem> GetValidations(IValidationInput validationInput)
        {
            if (validationInput.TravelExpenseEntity == null)
                yield return new TravelExpenseValidationItem(TravelExpenseValidationLevelEnum.Error, "Den angivne rejseafregning findes ikke");
        }
    }
}