using System.Threading.Tasks;
using Application.Dtos;

namespace Application.Interfaces
{
    public interface IFlowStepTravelExpenseService
    {
        Task<TravelExpenseProcessStepResponse> ProcessStepAsync(TravelExpenseFlowStepDto travelExpenseFlowStepDto,
            string sub);
    }
}