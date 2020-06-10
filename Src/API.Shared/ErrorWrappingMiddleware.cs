using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace API.Shared
{
    public class ErrorWrappingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorWrappingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILogger logger)
        {
            try
            {
                var isNonProduction = context.Request.Host.Host.ToLower().Contains("localhost") ||
                                      context.Request.Host.Host.ToLower().Contains("andersathome.dk")||
                                      context.Request.Host.Host.ToLower().Contains("dev.politikerafregning.dk");

                logger.Debug("About to execute: " + context.Request.Host.Value + context.Request.Path.Value+
                              (isNonProduction? "\n"+context.Request.Headers["Authorization"]:""));
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);

                context.Response.StatusCode = 500;
            }

            if (!context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";

                var response = new BadRequestObjectResult(context.Response.StatusCode);

                var json = JsonConvert.SerializeObject(response);
                logger.Error(json);
                await context.Response.WriteAsync(json);
            }
        }
    }
}