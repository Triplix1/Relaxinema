using System.Web;

namespace Relaxinema.Core.Helpers;

public class QueryHelper
{
    public static string BuildUrlWithQueryStringUsingStringConcat(
        string basePath, Dictionary<string, string> queryParams)
    {
        var queryString = string.Join("&",
            queryParams.Select(kvp => $"{HttpUtility.UrlEncode(kvp.Key)}={HttpUtility.UrlEncode(kvp.Value)}"));
        return $"{basePath}?{queryString}";
    }
}