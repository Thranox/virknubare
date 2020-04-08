using System.Collections.Generic;

namespace Web.Validation.ValidationRules
{
    public class TravelExpenseValidatorRule_Certify_CorrectState : ITravelExpenseValidatorRule
    {
        public bool RuleApplies(IValidationInput validationInput)
        {
            return
                validationInput.Context == ValidationInputContextEnum.Certify;
        }

        public IEnumerable<TravelExpenseValidationItem> GetValidations(IValidationInput validationInput)
        {
            if (validationInput.TravelExpenseEntity.IsCertified )
                yield return new TravelExpenseValidationItem(TravelExpenseValidationLevelEnum.Error, "Den angivne rejseafregning er allerede attesteret.");
        }
    }
}