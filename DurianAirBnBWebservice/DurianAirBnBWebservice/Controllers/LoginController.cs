using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using DurianAirBnBWebservice.Helper;
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
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration, ILogger<LoginController> logger)
        {
            _userRepository = new UserRepository(configuration);
            _tokenManager = new TokenManager(configuration);
            _logger = logger;
            _configuration = configuration;
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

        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ProcessForgotPassword([FromBody] User user)
        {
            try
            {
                var dbUser = _userRepository.GetUserByUsername(user.UserName);
                if (dbUser == null)
                {
                    //For security, don't let user know if the provided username is invalid
                    return Ok();
                }

                var token = _userRepository.GeneratePasswordResetTokenUrl(dbUser).Result;
                var mailJet = new MailjetManager(_configuration);
                await mailJet.SendPasswordResetEmailAsync(dbUser, token);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception thrown while user login: {e.Message}");
            }

            return BadRequest("Unable to update user password");
        }

        [HttpPost("passwordreset/{token}")]
        public async Task<IActionResult> ResetPassword([FromBody] User user, string token)
        {
            try
            {
                var dbUser = _userRepository.GetUserByPasswordResetToken(token);

                //if no user found or reset token is expired or reset token is not active, return
                //for security reason, we don't need to let user know if there was any issue with password update
                if (dbUser == null || DateTime.UtcNow > dbUser.ResetTokenExpiration || !dbUser.ResetTokenActive)
                {
                    return Ok();
                }

                dbUser.Password = _userRepository.GeneratePasswordHash(user.Password);
                dbUser.ResetTokenActive = false;

                await _userRepository.UpdateUserAsync(dbUser);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception thrown while reseting user password: {e.Message}");
            }

            return BadRequest("Unable to update user password");
        }
    }
}
