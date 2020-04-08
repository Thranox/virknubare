using Domain.Entities;

namespace Web.Validation
{
    public interface IValidationInput
    {
        ValidationInputContextEnum Context { get; }
        TravelExpenseEntity TravelExpenseEntity { get; }
    }
}