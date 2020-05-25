using System.Threading.Tasks;
using API.Shared.Services;
using Application.Dtos;

namespace Application.Interfaces
{
    public interface IGetFlowStepService
    {
        Task<FlowStepGetResponse> GetAsync(PolApiContext polApiContext);
    }
}