using System;
using DurianAirBnBWebservice.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DurianAirBnBWebservice.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : Controller
    {
        private readonly IMongoDbManager _mongoDbManager;
        private readonly ILogger<AddressController> _logger;

        public AddressController(IMongoDbManager mongoDbManager, ILogger<AddressController> logger)
        {
            _mongoDbManager = mongoDbManager;
            _logger = logger;
        }

        [HttpGet("states")]
        public IActionResult GetDisticStates()
        {
            try
            {
                var listings =  _mongoDbManager.GetDistinctStates();
                return Ok(JsonConvert.SerializeObject(listings));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception thrown while getting distinct states: {e.Message}");
            }

            return BadRequest("Error getting distinct states");
        }

        [HttpGet("zipcode")]
        public IActionResult GetDisticZipcode()
        {
            try
            {
                var listings = _mongoDbManager.GetDistinctZipcodes();
                return Ok(JsonConvert.SerializeObject(listings));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception thrown while getting distinct zipcodes: {e.Message}");
            }

            return BadRequest("Error getting distinct zipcodes");
        }
    }
}
