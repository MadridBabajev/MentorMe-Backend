using System.Security.Claims;

namespace Helpers;

public static class IdentityHelpers
{
    // extension function
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        return Guid.Parse(
            user.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
    }
}
