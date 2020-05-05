using System;
using IDP.Services;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace SharedWouldBeNugets
{
    public class ConnectionStringService : IConnectionStringService
    {
        private const string EnvironmentPlaceholder = "{Environment}";
        private const string MachineNamePlaceholder = "{MachineName}";
        private readonly IConfiguration _configuration;
        private readonly string _environmentName;

        public ConnectionStringService(IConfiguration configuration, string environmentName)
        {
            _configuration = configuration;
            _environmentName = environmentName;
        }

        public string GetConnectionString(string connectionStringName)
        {
            var connectionString = _configuration.GetConnectionString(connectionStringName);
            Log.Logger.Debug("Read cs from config: {ConnectionString}", connectionString);

            if (connectionString.Contains(EnvironmentPlaceholder))
            {
                connectionString = connectionString
                    .Replace(EnvironmentPlaceholder, _environmentName);
                Log.Logger.Debug("Updated config by environment: {ConnectionString}", connectionString);
            }

            if (connectionString.Contains(MachineNamePlaceholder))
            {
                connectionString = connectionString
                    .Replace(MachineNamePlaceholder, Environment.MachineName);
                Log.Logger.Debug("Updated config by machine name: {ConnectionString}", connectionString);
            }

            return connectionString;
        }
    }
}