using Domain.Entities;
using Web.ApiModels;

namespace Web.Validation.Adapters
{
    public class ApproveValidationItemAdapter : IValidationInput
    {
        public ApproveValidationItemAdapter(TravelExpenseApproveDto travelExpenseApproveDto, TravelExpenseEntity travelExpenseEntity)
        {
            Context = ValidationInputContextEnum.Approve;
            TravelExpenseEntity = travelExpenseEntity;
        }

        public ValidationInputContextEnum Context { get; }
        public TravelExpenseEntity TravelExpenseEntity { get; }
    }
}