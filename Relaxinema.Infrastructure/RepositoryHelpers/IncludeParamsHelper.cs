using Microsoft.EntityFrameworkCore;

namespace Relaxinema.Infrastructure.RepositoryHelpers;

public static class IncludeParamsHelper<T> where T : class
{
    public static IQueryable<T> IncludeStrings(string[]? includeStrings, IQueryable<T> query)
    {
        if (includeStrings is not null)
        {
            foreach (var str in includeStrings)
                query = query.Include(str);
        }

        return query;
    }
}