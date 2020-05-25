using System.Threading.Tasks;
using Application.Dtos;

namespace Application.Interfaces
{
    public interface IGetUserInfoService
    {
        Task<UserInfoGetResponse> GetAsync(PolApiContext polApiContext);
    }
}