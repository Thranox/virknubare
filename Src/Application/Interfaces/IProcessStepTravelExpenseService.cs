using System.Threading.Tasks;
using Application.Dtos;

namespace Application.Interfaces
{
    public interface IProcessStepTravelExpenseService
    {
        Task<TravelExpenseProcessStepResponse> ProcessStepAsync(TravelExpenseProcessStepDto travelExpenseProcessStepDto);
    }
}