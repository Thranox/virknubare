using System.Collections.Generic;

namespace Web.Validation.ValidationRules
{
    public class TravelExpenseValidatorRule_AssignPayment_CorrectState : ITravelExpenseValidatorRule
    {
        public bool RuleApplies(IValidationInput validationInput)
        {
            return
                validationInput.Context == ValidationInputContextEnum.AssignPayment;
        }

        public IEnumerable<TravelExpenseValidationItem> GetValidations(IValidationInput validationInput)
        {
            if (validationInput.TravelExpenseEntity.IsAssignedPayment )
                yield return new TravelExpenseValidationItem(TravelExpenseValidationLevelEnum.Error, "Den angivne rejseafregning er allerede anvist til betaling.");
        }
    }
}