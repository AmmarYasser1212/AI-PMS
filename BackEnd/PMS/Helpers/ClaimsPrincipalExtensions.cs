using System.Security.Claims;

namespace PMS.Helpers
{
    static class ClaimsPrincipalExtensions
    {

        public static int GetBusinessUserId(this ClaimsPrincipal user)
        {
            var value = user.FindFirst("business_user_id")?.Value;

            if (string.IsNullOrEmpty(value))
                throw new Exception("business_user_id claim not found");

            return int.Parse(value);
        }
    }
}
