using System.Threading.Tasks;
using Application;
using Microsoft.AspNetCore.Http;

namespace API.Shared.Services
{
    public interface ISubManagementService
    {
        Task<PolApiContext> GetPolApiContext(HttpContext httpContext);
    }
}