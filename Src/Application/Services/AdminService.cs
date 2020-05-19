using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Interfaces;

namespace Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IDbSeeder _dbSeeder;

        public AdminService(IDbSeeder dbSeeder)
        {
            _dbSeeder = dbSeeder;
        }

        public async Task ResetSeedningAsync()
        {
            await _dbSeeder.RemoveTestDataAsync();
            await _dbSeeder.SeedAsync();
        }
    }
}