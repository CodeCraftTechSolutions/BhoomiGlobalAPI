using System.Security.Claims;

namespace BhoomiGlobalAPI.HelperClass
{
    public static class AuthHelper
    {
        /// <summary>
        /// Gets the current user's ID from the claims, if available.
        /// </summary>
        /// <returns>The user ID as a Guid or Guid.Empty if not available.</returns>
        internal static Guid GetCurrentUserId()
        {
            // Use HttpContext.User instead of Thread.CurrentPrincipal
            var user = System.Threading.Thread.CurrentPrincipal as ClaimsPrincipal;

            if (user != null)
            {
                return GetClaim<Guid>(user.Claims, "UserId");
            }

            // Optionally, you could log a warning here if the user is null
            Console.WriteLine("Current principal is null. Unable to retrieve user ID.");
            return Guid.Empty;
        }

        /// <summary>
        /// Attempts to get a claim value from a list of claims.
        /// </summary>
        /// <param name="claims">The collection of claims to search through.</param>
        /// <param name="claimType">The type of the claim to find.</param>
        /// <param name="claimValue">The value of the claim if found, otherwise an empty string.</param>
        /// <returns>True if the claim is found, otherwise false.</returns>
        public static bool TryGetClaim(IEnumerable<Claim> claims, string claimType, out string claimValue)
        {
            claimValue = string.Empty;

            var foundClaim = claims.FirstOrDefault(x => x.Type.Equals(claimType, StringComparison.InvariantCultureIgnoreCase));
            if (foundClaim != null)
            {
                claimValue = foundClaim.Value;
                return true;
            }

            // Optionally log the missing claim
            Console.WriteLine($"Claim '{claimType}' not found.");
            return false;
        }

        /// <summary>
        /// Gets a strongly typed claim value from a collection of claims.
        /// </summary>
        /// <typeparam name="T">The expected type of the claim.</typeparam>
        /// <param name="claims">The collection of claims to search through.</param>
        /// <param name="claimType">The type of the claim to find.</param>
        /// <returns>The value of the claim converted to type T or the default value for T if not found.</returns>
        public static T GetClaim<T>(IEnumerable<Claim> claims, string claimType)
        {
            string claimValue;
            if (TryGetClaim(claims, claimType, out claimValue))
            {
                if (typeof(T) == typeof(Guid))
                {
                    // Attempt to parse the claim value to a Guid
                    Guid claimValueHolder = Guid.Empty;
                    if (Guid.TryParse(claimValue, out claimValueHolder))
                    {
                        return (T)Convert.ChangeType(claimValueHolder, typeof(T));
                    }

                    // If parsing fails, log it
                    Console.WriteLine($"Failed to parse '{claimType}' as Guid. Returning default value.");
                    return default(T);
                }

                // Otherwise, try to convert the claim value to the expected type
                try
                {
                    return (T)Convert.ChangeType(claimValue, typeof(T));
                }
                catch (Exception ex)
                {
                    // Log conversion failure
                    Console.WriteLine($"Failed to convert claim '{claimType}' to {typeof(T)}. Error: {ex.Message}");
                    return default(T);
                }
            }

            return default(T);
        }
    }
}
