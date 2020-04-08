using Domain.Entities;
using Web.ApiModels;

namespace Web.Validation.Adapters
{
    public class CertifyValidationItemAdapter : IValidationInput
    {
        public CertifyValidationItemAdapter(TravelExpenseCertifyDto travelExpenseCertifyDto, TravelExpenseEntity travelExpenseEntity)
        {
            Context = ValidationInputContextEnum.Certify;
            TravelExpenseEntity = travelExpenseEntity;
        }

        public ValidationInputContextEnum Context { get; }
        public TravelExpenseEntity TravelExpenseEntity { get; }
    }
}