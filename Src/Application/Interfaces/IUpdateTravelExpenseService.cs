using System;
using System.Threading.Tasks;
using API.Shared.Services;
using Application.Dtos;

namespace Application.Interfaces
{
    public interface IUpdateTravelExpenseService
    {
        Task<TravelExpenseUpdateResponse> UpdateAsync(PolApiContext polApiContext, Guid id,
            TravelExpenseUpdateDto travelExpenseUpdateDto);
    }
}