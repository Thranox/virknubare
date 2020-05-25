using System.Threading.Tasks;
using Application.Dtos;

namespace Application.Interfaces
{
    public interface IGetFlowStepService
    {
        Task<FlowStepGetResponse> GetAsync(PolApiContext polApiContext);
    }
}