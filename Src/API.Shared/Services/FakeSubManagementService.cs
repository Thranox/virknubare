using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace API.Shared.Services
{
    public class FakeSubManagementService : ISubManagementService
    {
        public FakeSubManagementService(PolApiContext polApiContext)
        {
            PolApiContext = polApiContext;
        }

        public async Task<PolApiContext> GetPolApiContext(HttpContext httpContext)
        {
            return await Task.FromResult( PolApiContext);
        }

        public PolApiContext PolApiContext { get; set; }
    }
}