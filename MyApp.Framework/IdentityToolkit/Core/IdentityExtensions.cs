using System;
using System.Security.Claims;
using System.Security.Principal;

namespace MyApp.Framework.IdentityToolkit.Core
{
    public static class IdentityExtensions
    {
        public static string GetUserName(this IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            var ci = identity as ClaimsIdentity;
            return ci?.FindFirstValue(ClaimsIdentity.DefaultNameClaimType);
        }
       
        public static long GetUserId(this IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            var claimsIdentity = identity as ClaimsIdentity;
            var id = claimsIdentity?.FindFirstValue(ClaimTypes.NameIdentifier);

            return id == null ? default(long) : long.Parse(id);
        }
        
        public static string FindFirstValue(this ClaimsIdentity identity, string claimType)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            var claim = identity.FindFirst(claimType);
            return claim?.Value;
        }
    }
}