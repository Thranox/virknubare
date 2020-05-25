using System.Threading.Tasks;
using API.Shared.Services;
using Application.Dtos;

namespace Application.Interfaces
{
    public interface IGetStatisticsService
    {
        Task<StatisticsGetResponse> GetAsync(PolApiContext polApiContext);
    }
}