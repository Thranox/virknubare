using System.Threading.Tasks;
using Application.Dtos;

namespace Application.Interfaces
{
    public interface IReportDoneTravelExpenseService
    {
        Task<TravelExpenseReportDoneResponse> ReportDoneAsync(TravelExpenseReportDoneDto travelExpenseReportDoneDto);
    }
}