using System;
using System.Collections.Generic;
using System.Linq;
using API;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Web
{
    [ApiController]
    [Route("Config")]
    public class ConfigController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger _logger;

        public ConfigController(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public AngularConfig Get()
        {
            return new AngularConfig();
        }
    }

    public class AngularConfig
    {
        public string Sts = "This is sts";
    }
}
