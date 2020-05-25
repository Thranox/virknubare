using System.Threading.Tasks;
using API.Shared.Services;
using Application.Dtos;

namespace Application.Interfaces
{
    public interface IGetUserInfoService
    {
        Task<UserInfoGetResponse> GetAsync(PolApiContext polApiContext);
    }
}