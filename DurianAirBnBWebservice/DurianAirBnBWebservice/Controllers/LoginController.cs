using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using DurianAirBnBWebservice.Identity;
using DurianAirBnBWebservice.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DurianAirBnBWebservice.Controllers
{
    /// <summary>
    /// Class LoginController.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/Login")]
    public class LoginController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly TokenManager _tokenManager;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IConfiguration configuration, ILogger<LoginController> logger)
        {
            _userRepository = new UserRepository(configuration);
            _tokenManager = new TokenManager(configuration);
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] User user)
        {
            try
            {
                if (user == null || string.IsNullOrEmpty(user.UserName))
                    return BadRequest("Invalid User data");

                var validatedUser = _userRepository.ValidateUser(user, out var userExists);

                if (validatedUser == null)
                {
                    return NotFound(userExists ? "Invalid username or password" : "The user was not found.");
                }

                var userName = validatedUser.UserName;
                var accessToken = _tokenManager.GenerateToken(userName, validatedUser.Id);
                //Generate a new WebSession/Refresh token
                var session = _tokenManager.GenerateWebSession();
                if (validatedUser.WebSessions == null)
                {
                    validatedUser.WebSessions = new List<UserWebSession>();
                }

                //Clear all sessions with expired refresh tokens
                validatedUser = _userRepository.RemoveExpiredUserWebSessions(validatedUser);
                //Add the new session
                validatedUser.WebSessions.Add(session);
                //Add the refreshToken to User object and save in DB
                await _userRepository.UpdateUserAsync(validatedUser);

                //Generate dynamic object to send to client
                dynamic tokens = new ExpandoObject();
                tokens.accessToken = accessToken;
                tokens.refreshToken = session.RefreshToken;

                return Ok(tokens);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception thrown while user login: {e.Message}");
            }

            return BadRequest("Unable to login User");
        }
    }
}
