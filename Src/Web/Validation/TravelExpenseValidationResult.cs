using System.Linq;

namespace Web.Validation
{
    public class TravelExpenseValidationResult
    {
        private readonly TravelExpenseValidationItem[] _travelExpenseValidationItems;

        public TravelExpenseValidationResult(TravelExpenseValidationItem[] travelExpenseValidationItems)
        {
            _travelExpenseValidationItems = travelExpenseValidationItems;
            IsValid = travelExpenseValidationItems.All(x => x.TravelExpenseValidationLevelEnum != TravelExpenseValidationLevelEnum.Error);
        }

        public bool IsValid { get; }

        public override string ToString()
        {
            return string.Join(";", _travelExpenseValidationItems.Select(x => x.Message));
        }
    }
}