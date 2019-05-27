using System;
using System.Collections.Generic;
using System.Dynamic;
using DurianAirBnBWebservice.Identity;
using DurianAirBnBWebservice.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DurianAirBnBWebservice.Controllers
{
    [Route("api/Signup")]
    public class SignupController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly TokenManager _tokenManager;
        private readonly ILogger<SignupController> _logger;

        public SignupController(IConfiguration configuration, ILogger<SignupController> logger)
        {
            _userRepository = new UserRepository(configuration);
            _tokenManager = new TokenManager(configuration);
            _logger = logger;
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> SignupAsync([FromBody] User user)
        {
            try
            {
                if(user == null || string.IsNullOrEmpty(user.UserName))
                    return BadRequest("Invalid User data");

                var userId = _userRepository.RegisterUser(user).Result;
                if (string.IsNullOrEmpty(userId))
                    return BadRequest($"Username {user.UserName} already exists");

                var accessToken = _tokenManager.GenerateToken(user.UserName, user.Id);
                //Add the refreshToken to User object and save in DB
                var session = _tokenManager.GenerateWebSession();
                user.WebSessions = new List<UserWebSession> { session };

                await _userRepository.UpdateUserAsync(user);

                //Generate dynamic object to send to client
                dynamic tokens = new ExpandoObject();
                tokens.accessToken = accessToken;
                tokens.refreshToken = session.RefreshToken;

                return Ok(tokens);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception thrown while registering new user: {e.Message}");
            }

            return BadRequest("Unable to Register user");
        }

        [HttpPost("checkuser")]
        public IActionResult CheckUser([FromBody] User user)
        {
            try
            {
                var existingUser = _userRepository.GetUserByUsername(user.UserName);

                return Ok(existingUser);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception thrown while validating user Token: {e.Message}");
                return BadRequest(e.Message);
            }
        }
    }
}