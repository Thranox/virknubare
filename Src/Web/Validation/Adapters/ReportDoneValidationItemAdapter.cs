using Domain.Entities;
using Web.ApiModels;

namespace Web.Validation.Adapters
{
    public class ReportDoneValidationItemAdapter : IValidationInput
    {
        public ReportDoneValidationItemAdapter(TravelExpenseReportDoneDto travelExpenseReportDoneDto, TravelExpenseEntity travelExpenseEntity)
        {
            Context = ValidationInputContextEnum.ReportDone;
            TravelExpenseEntity = travelExpenseEntity;
        }

        public ValidationInputContextEnum Context { get; }
        public TravelExpenseEntity TravelExpenseEntity { get; }
    }
}