using System.Security.Claims;

namespace BhoomiGlobalAPI.HelperClass
{
    public static class AuthHelper
	{
       
        internal static Guid GetCurrentUserId()
        {
            var user = System.Threading.Thread.CurrentPrincipal as ClaimsPrincipal;
            if (user != null)
            {
                return AuthHelper.GetClaim<Guid>(user.Claims, "UserId");
            }
            return Guid.Empty;
        }

        public static bool TryGetClaim(IEnumerable<Claim> claims, string claimType, out string claimValue)
		{
			claimValue = string.Empty;

			var foundClaim = claims.FirstOrDefault(x => x.Type.Equals(claimType, StringComparison.InvariantCultureIgnoreCase));
			if (foundClaim != null)
			{
				claimValue = foundClaim.Value;
				return true;
			}
			return false;
		}

		public static T GetClaim<T>(IEnumerable<Claim> claims, string claimType)
		{
			string claimValue;
			if (TryGetClaim(claims, claimType, out claimValue))
			{
				if (typeof(T) == typeof(Guid))
				{
					Guid claimValueHolder = Guid.Empty;
					Guid.TryParse(claimValue, out claimValueHolder);
					return (T)Convert.ChangeType(claimValueHolder, typeof(T));
				}
				return (T)Convert.ChangeType(claimValue, typeof(T));
			}
			return default(T);
		}
	}
}
