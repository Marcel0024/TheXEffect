using System;
using System.Security.Claims;
using TheXEffect.Data.Constants;

namespace TheXEffect.Data.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetDefaultCalendarId(this ClaimsPrincipal claimsPrincipal)
        {
            return Guid.Parse(claimsPrincipal.FindFirstValue(UserClaims.DefaultCalendarId));
        }

        public static string GetDefaultCalendarGoal(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirstValue(UserClaims.DefaultCalendarGoal);
        }
    }
}
