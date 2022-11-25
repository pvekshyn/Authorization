﻿using Common.SDK;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Role.API.Filters;

public class StatusCodeFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        // do something before the action executes
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result != null)
        {
            var actionResult = (ObjectResult)context.Result;
            if (actionResult.Value is Result result)
            {
                context.HttpContext.Response.StatusCode = result.Status;
            }
        }
    }
}
