using Domain.Entities;
using Web.ApiModels;

namespace Web.Validation.Adapters
{
    public class UpdateValidationItemAdapter : IValidationInput
    {
        private readonly TravelExpenseUpdateDto _travelExpenseUpdateDto;

        public UpdateValidationItemAdapter(TravelExpenseUpdateDto travelExpenseUpdateDto, TravelExpenseEntity travelExpenseEntity)
        {
            _travelExpenseUpdateDto = travelExpenseUpdateDto;
            TravelExpenseEntity = travelExpenseEntity;
        }

        public ValidationInputContextEnum Context { get; }
        public TravelExpenseEntity TravelExpenseEntity { get; private set; }
    }
}
