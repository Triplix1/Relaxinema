using API;
using Newtonsoft.Json;
using Relaxinema.Core.Extentions;
using Relaxinema.Core.Helpers;

namespace Relaxinema.WebAPI.Middlewares;

public class PaginationMiddleware
{
    private readonly RequestDelegate _next;

    public PaginationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        var originalBodyStream = httpContext.Response.Body;

        using (var responseBody = new MemoryStream())
        {
            httpContext.Response.Body = responseBody;

            await _next(httpContext);

            responseBody.Seek(0, SeekOrigin.Begin);
            var responseBodyText = await new StreamReader(responseBody).ReadToEndAsync();

            responseBody.Seek(0, SeekOrigin.Begin);

            var responseBodyObj = JsonConvert.DeserializeObject(responseBodyText);

            // Check if the response object is of type PagedList<T>
            if (responseBodyObj is not null && responseBodyObj.GetType().IsGenericType &&
                responseBodyObj.GetType().GetGenericTypeDefinition() == typeof(PagedList<>))
            {
                var currentPage = responseBodyObj.GetType().GetProperty("CurrentPage")?.GetValue(responseBodyObj);
                var pageSize = responseBodyObj.GetType().GetProperty("PageSize")?.GetValue(responseBodyObj);
                var totalCount = responseBodyObj.GetType().GetProperty("TotalCount")?.GetValue(responseBodyObj);
                var totalPages = responseBodyObj.GetType().GetProperty("TotalPages")?.GetValue(responseBodyObj);
                
                httpContext.Response.AddPaginationHeader(new PaginationHeader((int)currentPage,(int)pageSize, (int)totalCount, (int)totalPages));
            }

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}