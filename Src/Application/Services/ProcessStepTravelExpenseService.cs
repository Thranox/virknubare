using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;

namespace Application.Services
{
    public class ProcessStepTravelExpenseService : IProcessStepTravelExpenseService
    {
        public async Task<TravelExpenseProcessStepResponse> ProcessStepAsync(TravelExpenseProcessStepDto travelExpenseProcessStepDto)
        {
            var travelExpenseId = travelExpenseProcessStepDto.TravelExpenseId;

            return await Task.FromResult(new TravelExpenseProcessStepResponse
            {
            });
        }
    }
}