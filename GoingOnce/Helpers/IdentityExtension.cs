using System;
using System.Security.Claims;
using System.Security.Principal;
using GoingOnce.Models;

namespace GoingOnce.Helpers
{
    public static class IdentityExtension
    {
        public static bool IsUserAssociatedWithOrg(this IIdentity identity)
        {
            return identity.GetUserOrgId() != null;
        }

        public static Guid? GetUserOrgId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("OrganizationId");
            Guid orgId;

            return Guid.TryParse(claim?.Value, out orgId) ? (Guid?)orgId : null;
        }

        public static string GetUserOrganizationName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("OrganizationName");

            return claim?.Value;
        }

        public static Guid? GetEventId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("EventId");
            Guid eventId;

            return Guid.TryParse(claim?.Value, out eventId) ? (Guid?)eventId : null;
        }
    }
}