using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dtos;

namespace Application.Interfaces
{
    public interface IGetTravelExpenseService
    {
        Task<TravelExpenseGetResponse> GetAsync();

        Task<TravelExpenseGetByIdResponse> GetByIdAsync(Guid id);
    }
}