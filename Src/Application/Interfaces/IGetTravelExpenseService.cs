using System;
using System.Threading.Tasks;
using API.Shared.Services;
using Application.Dtos;

namespace Application.Interfaces
{
    public interface IGetTravelExpenseService
    {
        Task<TravelExpenseGetResponse> GetAsync(PolApiContext polApiContext);

        Task<TravelExpenseGetByIdResponse> GetByIdAsync(PolApiContext polApiContext, Guid id);
    }
}