using System.Threading.Tasks;
using Application.Dtos;

namespace Application.Interfaces
{
    public interface ICertifyTravelExpenseService
    {
        Task<TravelExpenseCertifyResponse> CertifyAsync(TravelExpenseCertifyDto travelExpenseCertifyDto);
    }
}