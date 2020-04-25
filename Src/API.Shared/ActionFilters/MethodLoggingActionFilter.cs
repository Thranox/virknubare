using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Serilog;

namespace Web.ActionFilters
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
            _logger.Information("{MethodName} -- Entry", context.ActionDescriptor.DisplayName);
            _logger.Information("{MethodName} -- Parameter(s): {MethodParameters}",
                context.ActionDescriptor.DisplayName, JsonConvert.SerializeObject(context.ActionArguments));
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.Information("{MethodName} -- Exit", context.ActionDescriptor.DisplayName);
        }
    }
}