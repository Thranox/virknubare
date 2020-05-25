using System.Threading.Tasks;
using API.Shared.Services;
using Application.Dtos;

namespace Application.Interfaces
{
    public interface IFlowStepTravelExpenseService
    {
        Task<TravelExpenseProcessStepResponse> ProcessStepAsync(TravelExpenseFlowStepDto travelExpenseFlowStepDto,
            PolApiContext polApiContext);
    }
}