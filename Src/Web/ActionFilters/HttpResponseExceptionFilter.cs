﻿using System;
using System.Net;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace Web.ActionFilters
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
            if (context.Exception != null) _logger.Error(context.Exception, "Exception occured: {ErrorId}",errorId);

            if (context.Exception is ItemNotFoundException travelExpenseNotFoundByIdException)
            {
                context.Result = new ObjectResult(new {travelExpenseNotFoundByIdException.Id})
                {
                    StatusCode = (int) HttpStatusCode.NotFound // 404
                };
                context.ExceptionHandled = true;
            }

            if (context.Exception is BusinessRuleViolationException businessRuleViolationException)
            {
                context.Result = new ObjectResult(new
                    {Id = businessRuleViolationException.EntityId, businessRuleViolationException.Message})
                {
                    StatusCode = (int) HttpStatusCode.UnprocessableEntity // 422
                };
                context.ExceptionHandled = true;
            }

            if (context.Exception is ItemNotAllowedException itemNotAllowedException)
            {
                context.Result = new ObjectResult(new { itemNotAllowedException.Id, itemNotAllowedException.Message })
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized // 401
                };
                context.ExceptionHandled = true;
            }

            if (context.Exception!=null && !context.ExceptionHandled )
            {
                context.Result = new ObjectResult(new
                {
                    Message ="Internal server error -- please see log and look for errorId="+errorId.ToString(),
                    context.Exception
                })
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError // 500
                };
                context.ExceptionHandled = true;
            }
        }

        public int Order { get; set; } = int.MaxValue - 10;
    }
}