using RMS.Entity.DTO;
using System.Security.Claims;
using System.Xml.Linq;

namespace RMS.Extensions
{
    public class AuthenticUserDetails
    {
        public static JwtLoginDetailDto GetCurrentUserDetails(ClaimsIdentity identity)
        {
            if (identity != null)
            {
                var userClaims = identity.Claims;

                return new JwtLoginDetailDto
                {
                    TmcId = userClaims.FirstOrDefault(o => o.Type == "id")?.Value,
                    EmailId = userClaims.FirstOrDefault(o => o.Type == "email")?.Value,
                    Name = userClaims.FirstOrDefault(o => o.Type == "name")?.Value,
                    Contact = userClaims.FirstOrDefault(o => o.Type == "contact")?.Value,
                    Designation = userClaims.FirstOrDefault(o => o.Type == "designation")?.Value,

                };
            }
            return null;
        }

    }
}
