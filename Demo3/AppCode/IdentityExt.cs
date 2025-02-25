using System.Security.Claims;
using System.Security.Principal;

namespace Demo2.AppCode
{
    public static class IdentityExt
    {
        public static int GetUserId(this IIdentity identity)
        {
            string? id = GetClaimsValue(identity, "Id");
            if (id == null)
                return 0;
            else
                return int.Parse(id);
        }

        public static string? GetAccount(this IIdentity identity)
        {
            return GetClaimsValue(identity, "Account");
        }

        public static string? FindFirstValue(this ClaimsIdentity identity, string claimType)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }
            return identity.FindFirst(claimType)?.Value;
        }

        private static string? GetClaimsValue(IIdentity identity, string claimType)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }

            ClaimsIdentity? claimsIdentity = identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                return FindFirstValue(claimsIdentity, claimType);
            }
            else
            {
                return null;
            }
        }
    }
}
