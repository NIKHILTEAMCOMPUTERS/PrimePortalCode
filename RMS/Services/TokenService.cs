//using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RMS.Entity.Account;
using RMS.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace RMS.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        //private readonly string _secretKey;
        //private readonly string _issuer;
        //private readonly string _audience;
        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
            //_secretKey = config["Jwt:Key"];
            //_issuer = config["Jwt:Issuer"];
            //_audience = config["Jwt:Audience"];
        }

        public async Task<string> CreateToken(AppUser account)
        {
            //generate token that is valid for 15 minutes'

          // Old Code

           var tokenDescriptor = new SecurityTokenDescriptor
           {
               Subject = new ClaimsIdentity(new[] 
                                                {             
                                                    new Claim("id", account.UserId.ToString()),
                                                    new Claim("email", account.Email),
                                                    new Claim("name", account.Name),
                                                    new Claim("contact", account.Mobile.ToString()),
                                                    new Claim("designation", account.Designation.ToString()),
                                                    
                                                }),
               Expires = DateTime.UtcNow.AddDays(1),
               SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature)
           };
           var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

            //var claims = new[]
            //  {
            //new Claim("id", account.UserId.ToString()),
            //            new Claim("email", account.UserId.ToString()),
            //            new Claim("name", account.UserId.ToString()),
            //            new Claim("contact", account.UserId.ToString()),
            //            new Claim("designation", account.UserId.ToString()),

            //    };

            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //var token = new JwtSecurityToken(
            //    issuer: _issuer,
            //    audience: _audience,
            //    claims: claims,
            //    expires: DateTime.Now.AddHours(3), // Set the access token expiration time
            //    signingCredentials: creds
            //);
            //return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        //public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        //{
        //    var tokenValidationParameters = new TokenValidationParameters
        //    {
        //        ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
        //        ValidateIssuer = false,
        //        ValidateIssuerSigningKey = true,
        //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)),
        //        ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
        //    };
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    SecurityToken securityToken;
        //    var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        //    var jwtSecurityToken = securityToken as JwtSecurityToken;
        //    if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        //        throw new SecurityTokenException("Invalid token");
        //    return principal;
        //}
    }
}
