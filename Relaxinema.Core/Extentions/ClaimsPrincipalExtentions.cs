using System.Security.Claims;

namespace Relaxinema.Core.Extentions;

public static class ClaimsPrincipalExtentions
{
    public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        var userIdString = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Guid userId;
        
        if (userIdString is not null)
            userId = Guid.Parse(userIdString);
        else
            throw new ApplicationException("User is not authorized");
        
        return userId;
    }

    public static bool IsAdmin(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.IsInRole("admin");
    }
}