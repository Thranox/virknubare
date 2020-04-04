using Domain.SharedKernel;

namespace Web.ApiModels
{
    public class TravelExpenseDto:ValueObject
    {
        public string Description { get; set; }
    }
}