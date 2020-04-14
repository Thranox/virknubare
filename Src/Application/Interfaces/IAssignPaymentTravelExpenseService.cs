using System.Threading.Tasks;
using Application.Dtos;

namespace Application.Interfaces
{
    public interface IAssignPaymentTravelExpenseService
    {
        Task<TravelExpenseAssignPaymentResponse> AssignPaymentAsync(TravelExpenseAssignPaymentDto travelExpenseAssignPaymentDto);
    }
}