using System.Threading.Tasks;
using API.Shared.Services;
using Application.Dtos;

namespace Application.Interfaces
{
    public interface ICreateTravelExpenseService
    {
        Task<TravelExpenseCreateResponse> CreateAsync(PolApiContext polApiContext, TravelExpenseCreateDto travelExpenseCreateDto);
    }
}