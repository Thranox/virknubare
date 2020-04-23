using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace IDP.Services
{
    public class ConnectionStringService : IConnectionStringService
    {
        private const string EnvironmentPlaceholder = "{Environment}";
        private const string MachineNamePlaceholder = "{MachineName}";
        private readonly IConfiguration _configuration;
        private readonly HostBuilderContext _hostBuilderContext;

        public ConnectionStringService(IConfiguration configuration, HostBuilderContext hostBuilderContext)
        {
            _configuration = configuration;
            _hostBuilderContext = hostBuilderContext;
        }

        public string GetConnectionString(string connectionStringName)
        {
            var connectionString = _configuration.GetConnectionString(connectionStringName);
            Log.Logger.Debug("Read cs from config: {ConnectionString}", connectionString);

            if (connectionString.Contains(EnvironmentPlaceholder))
            {
                connectionString = connectionString
                    .Replace(EnvironmentPlaceholder, _hostBuilderContext.HostingEnvironment.EnvironmentName);
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