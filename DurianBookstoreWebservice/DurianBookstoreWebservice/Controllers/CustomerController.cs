using System;
using DurianBookstoreWebservice.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DurianBookstoreWebservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly IMongoDbManager _mongoDbManager;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(IMongoDbManager mongoDbManager, ILogger<CustomerController> logger)
        {
            _mongoDbManager = mongoDbManager;
            _logger = logger;
        }

        // Get list of all the customers
        [HttpGet]
        public IActionResult GetAllCustomers()
        {
            try
            {
                var customers = _mongoDbManager.GetAllCustomers();
                return Ok(JsonConvert.SerializeObject(customers.Result));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception thrown while getting list of customers: {e.Message}");
            }

            return BadRequest("Error getting list of all customers");
        }
    }
}