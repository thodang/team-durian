using System;
using System.Linq;
using System.Threading.Tasks;
using DurianAirBnBWebservice.Identity;
using DurianAirBnBWebservice.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DurianAirBnBWebservice.Controllers
{
    /// <summary>
    /// Controller used for automatically refershing access token
    /// Expired access token and the refresh token will be checked against
    /// their values in database, if they are same and refresh token is not expired,
    /// a new access token will be generated
    /// </summary>
    [Route("api/TokenRefresh")]
    public class TokenRefreshController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly TokenManager _tokenManager;
        private readonly ILogger<TokenRefreshController> _logger;
        private readonly IConfiguration _configuration;

        public TokenRefreshController(IConfiguration configuration, ILogger<TokenRefreshController> logger)
        {
            _userRepository = new UserRepository(configuration);
            _tokenManager = new TokenManager(configuration);
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> TokenRefresh([FromBody] TokenAdmin tokenAdmin)
        {
            try
            {
                var accessToken = tokenAdmin.AccessToken;
                var refreshToken = tokenAdmin.RefreshToken;

                if (accessToken == null || refreshToken == null)
                {
                    return BadRequest("Invalid token");
                }

                var principal = _tokenManager.GetPrincipal(accessToken, false);
                var username = principal.Claims.SingleOrDefault(c => c.Type == "user")?.Value;
                var uid = principal.Claims.SingleOrDefault(c => c.Type == "uid")?.Value;
                var dbUser = _userRepository.GetUserById(uid);

                var savedRefreshToken = dbUser.WebSessions.FirstOrDefault(x => x.RefreshToken == refreshToken);

                if (savedRefreshToken != null &&
                    (savedRefreshToken.RefreshToken != refreshToken ||
                     !savedRefreshToken.IsActive ||
                     DateTime.Compare(savedRefreshToken.ExpirationDateTime, DateTime.UtcNow) < 0))
                    throw new SecurityTokenException("Invalid refresh token");

                var newAccessToken = _tokenManager.GenerateToken(username, dbUser.Id);
                var newRefreshToken = _tokenManager.GenerateRefreshToken();
                dbUser.WebSessions.Remove(savedRefreshToken);
                var session = new UserWebSession
                {
                    ExpirationDateTime = DateTime.UtcNow.AddDays(_configuration.GetValue<int>("RefreshTokenSpan")),
                    RefreshToken = refreshToken,
                    IsActive = true
                };

                dbUser.WebSessions.Add(session);
                await _userRepository.UpdateUserAsync(dbUser);

                return new ObjectResult(new
                {
                    token = newAccessToken,
                    refreshToken = newRefreshToken
                });
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception thrown while refreshing token: {e.Message}");
                return BadRequest(e.Message);
            }
        }
    }

}
