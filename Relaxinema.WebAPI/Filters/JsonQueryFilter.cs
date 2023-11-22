using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Relaxinema.WebAPI.Attributes;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Relaxinema.WebAPI.Filters;

public class JsonQueryFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ActionDescriptor is ControllerActionDescriptor descriptor)
        {
            var jsonQueryAttribute = descriptor.MethodInfo
                .GetCustomAttributes(typeof(JsonQueryAttribute), false)
                .FirstOrDefault() as JsonQueryAttribute;

            if (jsonQueryAttribute != null)
            {
                var parameterName = jsonQueryAttribute.ParameterName;
                var paramType = jsonQueryAttribute.ParamType;
                var queryStringValue = context.HttpContext.Request.Query[parameterName].ToString();

                if (!string.IsNullOrEmpty(queryStringValue))
                {
                    try
                    {
                        var parameterDescriptor = descriptor.Parameters
                            .FirstOrDefault(p => p.ParameterType == paramType); // or a specific type
                        var jsonObject = JsonSerializer.Deserialize(queryStringValue, paramType); // or a specific type
                        context.ActionArguments[parameterDescriptor?.Name] = jsonObject;
                    }
                    catch (JsonException)
                    {
                        // Handle JSON parsing error, maybe set a bad request result
                        context.Result = new BadRequestResult();
                    }
                }
            }
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // No implementation needed here for this scenario
    }
}