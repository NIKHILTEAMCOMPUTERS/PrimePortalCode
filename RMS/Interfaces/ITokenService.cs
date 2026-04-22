using RMS.Entity.Account;
using System.Security.Claims;

namespace RMS.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
        string CreateRefreshToken();
        //ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
