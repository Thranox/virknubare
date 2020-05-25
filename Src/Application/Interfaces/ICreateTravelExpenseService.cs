using System.Threading.Tasks;
using Application.Dtos;

namespace Application.Interfaces
{
    public interface ICreateTravelExpenseService
    {
        Task<TravelExpenseCreateResponse> CreateAsync(PolApiContext polApiContext, TravelExpenseCreateDto travelExpenseCreateDto);
    }
}