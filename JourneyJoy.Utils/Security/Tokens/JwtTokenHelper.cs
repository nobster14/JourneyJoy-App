using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Utils.Security.Tokens
{
    public static class JwtTokenHelper
    {
        #region Fields
        public const string User = "User";

        #endregion

        #region Public methods
        public static (string token, CookieOptions options) CreateToken(string email, string id, string key, string issuer, string host)
        {
            var expDate = DateTime.UtcNow.AddHours(12);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, email),
                    new Claim(JwtRegisteredClaimNames.Email, email),
                    new Claim(JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString())
                }),
                Expires = expDate,
                Issuer = issuer,
                Audience = $"{id}",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)), SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var stringToken = tokenHandler.WriteToken(token);
            var options = new CookieOptions()
            {
                Expires = expDate,
                Domain = null
            };
            //if (host == "localhost")
            //    options.Domain = null;

            return (stringToken, options);
        }
        public static bool AudiencesValidator(IEnumerable<string> audiences, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            return audiences.Count() == 1;
        }
        public static bool IsIdValid(this string JwtToken, string hashedId)
        {
            return hashedId == GetIdFromToken(JwtToken);
        }
        public static string GetIdFromToken(string JwtToken)
        {
            var encodedToken = new JwtSecurityToken(JwtToken);

            return encodedToken.Audiences.First().Split(';').First();
        }
        #endregion
    }
}
