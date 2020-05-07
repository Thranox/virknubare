using System.Threading.Tasks;
using Domain.Responses;

namespace Domain.Interfaces
{
    public interface ICreateCustomerService
    {
        Task<CreateCustomerResponse> CreateAsync(string name);
    }
}