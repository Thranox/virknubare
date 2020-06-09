using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Web
{
    [ApiController]
    [Route("Config")]
    public class ConfigController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public ConfigController(Serilog.ILogger logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public AngularConfig Get()
        {
            return new AngularConfig
            {
                stsAuthorityUrl=_configuration.GetValue<string>("IDP_URL"),
                apiUrl= _configuration.GetValue<string>("API_URL"),
                clientUrl= _configuration.GetValue<string>("CLIENT_URL"),
                stsClientId = "polangularclient"

            };
        }
    }

    public class AngularConfig
    {
        public string stsAuthorityUrl { get; set; }
        public string stsClientId { get; set; }
        public string apiUrl { get; set; }
        public string clientUrl { get; set; }
    }
}
