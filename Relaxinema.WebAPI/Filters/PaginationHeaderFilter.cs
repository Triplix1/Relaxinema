using API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Relaxinema.Core.Extentions;
using Relaxinema.Core.Helpers;

namespace Relaxinema.WebAPI.Filters;

public class PaginationHeaderFilter : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        await next();
        
        var result = context.Result as ObjectResult;

        if (result != null && result.Value != null)
        {
            // Check if the response object is of type PagedList<T>
            if (result.Value.GetType().IsGenericType &&
                result.Value.GetType().GetGenericTypeDefinition() == typeof(PagedList<>))
            {
                var resultType = result.GetType();
                var currentPage = resultType.GetProperty("CurrentPage")?.GetValue(result);
                var pageSize = resultType.GetProperty("PageSize")?.GetValue(result);
                var totalCount = resultType.GetProperty("TotalCount")?.GetValue(result);
                var totalPages = resultType.GetProperty("TotalPages")?.GetValue(result);

                context.HttpContext.Response.AddPaginationHeader(new PaginationHeader((int)currentPage, (int)pageSize,
                    (int)totalCount, (int)totalPages));
            }
        }

    }
}