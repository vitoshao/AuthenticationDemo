using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Demo3.Models;
using Microsoft.IdentityModel.Tokens;

namespace Demo3.AppCode
{
    public class TokenService : ITokenService
    {
        private const double EXPIRY_DURATION_MINUTES = 30;

        public string BuildToken(string key, string issuer, string audience, LoginUserDto user)
        {
            var claims = new[] {
                new Claim("Id", user.Id.ToString()),
                new Claim("Account", user.Account)
            };

            var tokenDescriptor = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(EXPIRY_DURATION_MINUTES),
                signingCredentials: GetCreds(key));

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public bool IsTokenValid(string key, string issuer, string token)
        {
            var mySecret = Encoding.UTF8.GetBytes(key);
            var mySecurityKey = new SymmetricSecurityKey(mySecret);

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidAudience = issuer,
                    IssuerSigningKey = mySecurityKey,
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 生成對稱加密簽名憑證
        /// </summary>
        /// <returns></returns>
        private SigningCredentials? GetCreds(string key)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            return credentials;
        }

    }
}
