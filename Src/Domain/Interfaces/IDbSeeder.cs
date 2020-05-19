using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IDbSeeder
    {
        Task SeedAsync();
        Task RemoveTestDataAsync();
    }
}