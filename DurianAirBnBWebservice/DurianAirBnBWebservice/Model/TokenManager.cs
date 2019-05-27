using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DurianAirBnBWebservice.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DurianAirBnBWebservice.Model
{
    public class TokenManager
    {
        private readonly string _secretKey;
        private readonly IConfiguration _configuration;

        public TokenManager(IConfiguration configuration)
        {
            _secretKey = configuration.GetValue<string>("SecurityKey");
            _configuration = configuration;
        }

        public string GenerateToken(string username, string userId)
        {
            var claims = new[]
            {
                new Claim("user", username),
                new Claim("uid", userId)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "alexfarokhyans.com",
                audience: "alexfarokhyans.com",
                claims: claims,
                expires: DateTime.Now.AddMinutes(_configuration.GetValue<double>("AccessTokenSpan")),
                signingCredentials: creds);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string ValidateToken(string token)
        {
            string username = null;
            var principal = GetPrincipal(token);

            if (principal == null)
                return null;

            ClaimsIdentity identity;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }

            var usernameClaim = identity.FindFirst(ClaimTypes.Name);
            username = usernameClaim.Value;

            return username;
        }

        public ClaimsPrincipal GetPrincipal(string token, bool validateExpiration = true)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);

                if (jwtToken == null)
                    return null;

                var parameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = validateExpiration,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "alexfarokhyans.com",
                    ValidAudience = "alexfarokhyans.com",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey))
                };

                var principal = tokenHandler.ValidateToken(token, parameters, out var securityToken);
                if (securityToken == null)
                {
                    throw new SecurityTokenException("Invalid Token");
                }
                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public UserWebSession GenerateWebSession()
        {
            return new UserWebSession
            {
                ExpirationDateTime = DateTime.UtcNow.AddDays(_configuration.GetValue<int>("RefreshTokenSpan")),
                RefreshToken = GenerateRefreshToken(),
                IsActive = true
            };
        }
    }

    public class TokenAdmin
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
