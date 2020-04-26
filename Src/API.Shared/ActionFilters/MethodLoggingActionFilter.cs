using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Serilog;

namespace API.Shared.ActionFilters
{
    public class MethodLoggingActionFilter : IActionFilter
    {
        private ILogger _loggerForContext;

        public MethodLoggingActionFilter(ILogger logger)
        {
            _loggerForContext = logger.ForContext(new TransactionIdEnricher(Guid.NewGuid()));
        }

        public int Order { get; set; } = int.MaxValue - 20;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _loggerForContext.Information("{MethodName} -- Entry", context.ActionDescriptor.DisplayName);
            _loggerForContext.Information("{MethodName} -- Parameter(s): {MethodParameters}",
                context.ActionDescriptor.DisplayName, JsonConvert.SerializeObject(context.ActionArguments));
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _loggerForContext.Information("{MethodName} -- Exit", context.ActionDescriptor.DisplayName);
        }
    }
}