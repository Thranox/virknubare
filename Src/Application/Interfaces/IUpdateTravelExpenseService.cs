using System.Threading.Tasks;
using Application.Dtos;

namespace Application.Interfaces
{
    public interface IUpdateTravelExpenseService
    {
        Task<TravelExpenseUpdateResponse> UpdateAsync(TravelExpenseUpdateDto travelExpenseUpdateDto);
    }
}