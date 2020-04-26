using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Serilog;

namespace API.Shared.ActionFilters
{
    public class MethodLoggingActionFilter : IActionFilter
    {
        private readonly ILogger _logger;

        public MethodLoggingActionFilter(ILogger logger)
        {
            _logger = logger;
        }

        public int Order { get; set; } = int.MaxValue - 20;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Create transaction id and create a new logger that will log enriched with that TransactionId
            var transactionId = Guid.NewGuid();
            var loggerForContext = _logger.ForContext(new TransactionIdEnricher(transactionId));

            loggerForContext.Information("{MethodName} -- Entry", context.ActionDescriptor.DisplayName);
            loggerForContext.Information("{MethodName} -- Parameter(s): {MethodParameters}",
                context.ActionDescriptor.DisplayName, JsonConvert.SerializeObject(context.ActionArguments));
            
            context.HttpContext.Items.Add("LoggerForContext",loggerForContext);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var logger = context.HttpContext.Items["LoggerForContext"] as ILogger;
            if (logger == null)
                logger = _logger;
            logger.Information("{MethodName} -- Exit", context.ActionDescriptor.DisplayName);
        }
    }
}