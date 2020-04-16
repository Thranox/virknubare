using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;

namespace Application.Services
{
    public class ProcessStepTravelExpenseService : IProcessStepTravelExpenseService
    {
        public Task<TravelExpenseProcessStepResponse> ProcessStepAsync(TravelExpenseProcessStepDto travelExpenseProcessStepDto)
        {
            throw new System.NotImplementedException();
        }
    }
}