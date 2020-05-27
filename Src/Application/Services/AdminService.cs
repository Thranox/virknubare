using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Interfaces;
using Serilog;

namespace Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IDbSeeder _dbSeeder;
        private readonly ILogger _logger;

        public AdminService(IDbSeeder dbSeeder, ILogger logger)
        {
            _dbSeeder = dbSeeder;
            _logger = logger;
        }

        public async Task ResetSeedningAsync()
        {
            _logger.Information("Migrating database...");
            await _dbSeeder.MigrateAsync();

            _logger.Information("Removing test data...");
            await _dbSeeder.RemoveTestDataAsync();

            _logger.Information("Seeding test data...");
            await _dbSeeder.SeedAsync();

            _logger.Information("Done seeding test data...");
        }
    }
}