using System;
using System.Net;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace API.Shared.ActionFilters
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        private readonly ILogger _logger;

        public HttpResponseExceptionFilter(ILogger logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var errorId = Guid.NewGuid();

            if (context.Exception is ItemNotFoundException travelExpenseNotFoundByIdException)
            {
                _logger.Debug("Exception => 404");
                context.Result = new ObjectResult(new
                {
                    travelExpenseNotFoundByIdException.Id,
                    travelExpenseNotFoundByIdException.Item,
                    travelExpenseNotFoundByIdException.Message
                })
                {
                    StatusCode = (int) HttpStatusCode.NotFound // 404
                };
                context.ExceptionHandled = true;
            }

            if (context.Exception is BusinessRuleViolationException businessRuleViolationException)
            {
                _logger.Debug("Exception => 422");
                context.Result = new ObjectResult(new
                {
                    Id = businessRuleViolationException.EntityId,
                    businessRuleViolationException.Message,
                    businessRuleViolationException
                })
                {
                    StatusCode = (int) HttpStatusCode.UnprocessableEntity // 422
                };
                context.ExceptionHandled = true;
            }

            if (context.Exception is ItemNotAllowedException itemNotAllowedException)
            {
                _logger.Debug("Exception => 401");
                context.Result = new ObjectResult(new
                {
                    itemNotAllowedException.Id,
                    itemNotAllowedException.Message,
                    itemNotAllowedException.Item
                })
                {
                    StatusCode = (int) HttpStatusCode.Unauthorized // 401
                };
                context.ExceptionHandled = true;
            }

            if (context.Exception != null && !context.ExceptionHandled)
            {
                _logger.Debug("Exception => 500");
                context.Result = new ObjectResult(new
                {
                    Message = "Internal server error -- please see log and look for errorId=" + errorId,
                    context.Exception
                })
                {
                    StatusCode = (int) HttpStatusCode.InternalServerError // 500
                };
                context.ExceptionHandled = true;
            }
        }

        public int Order { get; set; } = int.MaxValue - 10;
    }
}