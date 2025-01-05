using BhoomiGlobalAPI.HelperClass;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BhoomiGlobalAPI.Api.Controllers
{
    public abstract class BaseApiController : ControllerBase
    {
        public BaseApiController()
        {
        }

        // Safely retrieves the current ClaimsPrincipal.
        protected ClaimsPrincipal CurrentPrincipal
        {
            get
            {
                if (User == null || User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return null; // Or throw an exception depending on your business needs
                }
                return User as ClaimsPrincipal;
            }
        }

        // Checks if the user is an Admin (Admin or SuperAdmin).
        public bool IsAdmin
        {
            get
            {
                var role = GetRoles();
                if (string.IsNullOrEmpty(role)) return false;

                role = role.ToLower();
                var adminRole = EnumHelper.GetDescription(Enums.Role.SUPERADMINISTRATOR).ToLower();
                var adminRoleAlt = EnumHelper.GetDescription(Enums.Role.ADMINISTRATOR).ToLower();

                return role.Equals(adminRole, StringComparison.OrdinalIgnoreCase) || role.Equals(adminRoleAlt, StringComparison.OrdinalIgnoreCase);
            }
        }

        // Checks if the user is a Member (General).
        public bool IsMember
        {
            get
            {
                var role = GetRoles();
                if (string.IsNullOrEmpty(role)) return false;
                return role.Equals(EnumHelper.GetDescription(Enums.Role.GENERAL), StringComparison.OrdinalIgnoreCase);
            }
        }

        // Gets the username from the claims.
        [NonAction]
        protected string GetUserName()
        {
            if (CurrentPrincipal == null)
            {
                return string.Empty; // Fallback if CurrentPrincipal is null
            }
            return AuthHelper.GetClaim<string>(CurrentPrincipal.Claims, Constants.API_CLAIM_USERNAME);
        }

        // Gets the user ID from the claims.
        [NonAction]
        protected string GetUserId()
        {
            if (CurrentPrincipal == null)
            {
                return string.Empty; // Fallback if CurrentPrincipal is null
            }
            return AuthHelper.GetClaim<string>(CurrentPrincipal.Claims, ClaimTypes.NameIdentifier);
        }

        // Gets the Partner's User ID (for Partner tokens).
        [NonAction]
        protected string GetPartnerUserId()
        {
            if (CurrentPrincipal == null)
            {
                return string.Empty; // Fallback if CurrentPrincipal is null
            }

            var partnerClaim = CurrentPrincipal.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier && c.Issuer == "PartnerTokenIdentity");

            return partnerClaim?.Value ?? string.Empty; // Return empty if no claim found
        }

        // Gets the Partner's Roles (for Partner tokens).
        [NonAction]
        protected string GetPartnerRoles()
        {
            if (CurrentPrincipal == null)
            {
                return string.Empty; // Fallback if CurrentPrincipal is null
            }

            var partnerClaim = CurrentPrincipal.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Role && c.Issuer == "PartnerTokenIdentity");

            return partnerClaim?.Value ?? string.Empty; // Return empty if no claim found
        }

        // Gets the roles of the user from the claims.
        [NonAction]
        protected string GetRoles()
        {
            if (CurrentPrincipal == null)
            {
                return string.Empty; // Fallback if CurrentPrincipal is null
            }
            return AuthHelper.GetClaim<string>(CurrentPrincipal.Claims, ClaimTypes.Role);
        }

        // Gets the first name from the claims.
        [NonAction]
        protected string GetFirstName()
        {
            if (CurrentPrincipal == null)
            {
                return string.Empty; // Fallback if CurrentPrincipal is null
            }
            return AuthHelper.GetClaim<string>(CurrentPrincipal.Claims, Constants.FIRSTNAME);
        }

        // Gets the last name from the claims.
        [NonAction]
        protected string GetLastName()
        {
            if (CurrentPrincipal == null)
            {
                return string.Empty; // Fallback if CurrentPrincipal is null
            }
            return AuthHelper.GetClaim<string>(CurrentPrincipal.Claims, Constants.LASTNAME);
        }

        // Gets the email from the claims.
        [NonAction]
        protected string GetEmail()
        {
            if (CurrentPrincipal == null)
            {
                return string.Empty; // Fallback if CurrentPrincipal is null
            }
            return AuthHelper.GetClaim<string>(CurrentPrincipal.Claims, Constants.EMAIL);
        }

        // Gets the phone number from the claims.
        [NonAction]
        protected string GetPhone()
        {
            if (CurrentPrincipal == null)
            {
                return string.Empty; // Fallback if CurrentPrincipal is null
            }
            return AuthHelper.GetClaim<string>(CurrentPrincipal.Claims, Constants.PHONE);
        }
    }
}
