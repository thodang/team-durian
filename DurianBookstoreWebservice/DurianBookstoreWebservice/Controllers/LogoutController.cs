using System;
using System.Linq;
using System.Threading.Tasks;
using DurianBookstoreWebservice.Identity;
using DurianBookstoreWebservice.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DurianBookstoreWebservice.Controllers
{
    [Route("api/Logout")]
    public class LogoutController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly TokenManager _tokenManager;
        private readonly ILogger<LogoutController> _logger;

        public LogoutController(IConfiguration configuration, ILogger<LogoutController> logger)
        {
            _userRepository = new UserRepository(configuration);
            _tokenManager = new TokenManager(configuration);
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> LogoutAsync([FromBody] TokenAdmin tokenAdmin)
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
                var uid = principal.Claims.SingleOrDefault(c => c.Type == "uid")?.Value;
                var dbUser = _userRepository.GetUserById(uid);

                var savedRefreshToken = dbUser.web_sessions.FirstOrDefault(x => x.RefreshToken == refreshToken);
                dbUser.web_sessions.Remove(savedRefreshToken);
                await _userRepository.UpdateUserAsync(dbUser);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception thrown while Logging out: {e.Message}");
                return BadRequest(e.Message);
            }

            return Ok();
        }
    }
}
