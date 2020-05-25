using System.Threading.Tasks;
using API.Shared.Services;
using Application.Dtos;

namespace Application.Interfaces
{
    public interface ICreateSubmissionService
    {
        Task<SubmissionPostResponse> CreateAsync(PolApiContext polApiContext);
    }
}