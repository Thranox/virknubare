using System;
using System.Threading.Tasks;
using Application.Dtos;

namespace Application.Interfaces
{
    public interface IGetTravelExpenseService
    {
        Task<TravelExpenseGetResponse> GetAsync(string sub);

        Task<TravelExpenseGetByIdResponse> GetByIdAsync(Guid id, string sub);
    }
}