using System.Net;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web.Controllers
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; set; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is TravelExpenseNotFoundByIdException travelExpenseNotFoundByIdException)
            {
                context.Result = new ObjectResult(new{ Id=travelExpenseNotFoundByIdException.Id})
                {
                    StatusCode = (int)HttpStatusCode.NotFound // 404
                };
                context.ExceptionHandled = true;
            }
            if (context.Exception is BusinessRuleViolationException businessRuleViolationException)
            {
                context.Result = new ObjectResult(new{ Id=businessRuleViolationException.EntityId, Message=businessRuleViolationException.Message})
                {
                    StatusCode = (int)HttpStatusCode.UnprocessableEntity // 422
                };
                context.ExceptionHandled = true;
            }
        }
    }
}