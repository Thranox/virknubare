using System;
using System.Threading.Tasks;
using Application.Dtos;

namespace Application.Interfaces
{
    public interface IUpdateTravelExpenseService
    {
        Task<TravelExpenseUpdateResponse> UpdateAsync(PolApiContext polApiContext, Guid id,
            TravelExpenseUpdateDto travelExpenseUpdateDto);
    }
}