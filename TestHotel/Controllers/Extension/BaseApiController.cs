using BhoomiGlobalAPI.HelperClass;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BhoomiGlobalAPI.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseApiController : ControllerBase
	{
        
		/// <summary>
		/// 
		/// </summary>
		protected ClaimsPrincipal CurrentPrincipal
		{
			get
			{
				return User as ClaimsPrincipal;
			}
		}

        public bool IsAdmin 
        {
            get 
            {
                var role = GetRoles();
                if (string.IsNullOrEmpty(role) == false)
                {
                    role = role.ToLower();
                }
                if (role == EnumHelper.GetDescription(Enums.Role.SUPERADMINISTRATOR).ToLower()
                    || role == EnumHelper.GetDescription(Enums.Role.ADMINISTRATOR).ToLower())
                {
                    return true;
                }
                return false;
            }

        }



        public bool IsMember
        {
            get
            {
                var role = GetRoles();
                if (string.IsNullOrEmpty(role) == false)
                {
                    role = role.ToLower();
                }
                return role == EnumHelper.GetDescription(Enums.Role.GENERAL).ToLower();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [NonAction]
        protected string GetUserName()
        {
            return AuthHelper.GetClaim<string>(CurrentPrincipal.Claims, Constants.API_CLAIM_USERNAME);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [NonAction]
        protected string GetUserId()
        {
            return AuthHelper.GetClaim<string>(CurrentPrincipal.Claims, ClaimTypes.NameIdentifier);
        }


        [NonAction]
        protected string GetPartnerUserId()
        {
            var partnerClaim = CurrentPrincipal.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier && c.Issuer == "PartnerTokenIdentity");

            return partnerClaim?.Value;
        }

        [NonAction]
        protected string GetPartnerRoles()
        {
            var partnerClaim = CurrentPrincipal.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Role && c.Issuer == "PartnerTokenIdentity");

            return partnerClaim?.Value;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>


        [NonAction]
		protected string GetRoles()
		{
			return AuthHelper.GetClaim<string>(CurrentPrincipal.Claims, ClaimTypes.Role);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[NonAction]
		protected string GetFirstName()
		{
			return AuthHelper.GetClaim<string>(CurrentPrincipal.Claims, Constants.FIRSTNAME);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[NonAction]
		protected string GetLastName()
		{
			return AuthHelper.GetClaim<string>(CurrentPrincipal.Claims, Constants.LASTNAME);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[NonAction]
		protected string GetEmail()
		{
			return AuthHelper.GetClaim<string>(CurrentPrincipal.Claims, Constants.EMAIL);
		}
        
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[NonAction]
		protected string GetPhone()
		{
			return AuthHelper.GetClaim<string>(CurrentPrincipal.Claims, Constants.PHONE);
		}
	}
}