using System.Collections.Generic;
using Web.Validation.ValidationRules;

namespace Web.Validation
{
    public class TravelExpenseValidator : ITravelExpenseValidator
    {
        private readonly IEnumerable<ITravelExpenseValidatorRule> _travelExpenseValidatorRules;

        public TravelExpenseValidator(IEnumerable<ITravelExpenseValidatorRule> travelExpenseValidatorRules)
        {
            _travelExpenseValidatorRules = travelExpenseValidatorRules;
        }
        public TravelExpenseValidationResult GetValidationResult(IValidationInput validationInput)
        {
            var list = new List<TravelExpenseValidationItem>();
            foreach (var travelExpenseValidatorRule in _travelExpenseValidatorRules)
            {
                if (travelExpenseValidatorRule.RuleApplies(validationInput))
                {
                    var travelExpenseValidationItems = travelExpenseValidatorRule.GetValidations(validationInput);
                    list.AddRange(travelExpenseValidationItems);
                }
            }

            return new TravelExpenseValidationResult(list.ToArray());
        }
    }
}