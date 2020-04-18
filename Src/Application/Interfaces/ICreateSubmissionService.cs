using System.Threading.Tasks;
using Application.Dtos;

namespace Application.Interfaces
{
    public interface ICreateSubmissionService
    {
        Task<SubmissionPostResponse> CreateAsync(string sub);
    }
}