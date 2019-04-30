using System;
using DurianBookstoreWebservice.Model;
using DurianBookstoreWebservice.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DurianBookstoreWebservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IMongoDbManager _mongoDbManager;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IMongoDbManager mongoDbManager, ILogger<OrderController> logger)
        {
            _mongoDbManager = mongoDbManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAllOrders()
        {
            try
            {
                var orders = _mongoDbManager.GetAllOrders();
                return Ok(JsonConvert.SerializeObject(orders.Result));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception thrown while getting list of orders: {e.Message}");
            }

            return BadRequest("Error getting all orders");
        }

        // Add new order
        [HttpPost]
        public IActionResult CreateNewOrder([FromBody] Order order)
        {
            try
            {
                if (order == null)
                {
                    return BadRequest("Invalid order format");
                }

                var orderId = _mongoDbManager.AddNewOrder(order).Result;
                if (string.IsNullOrEmpty(orderId))
                {
                    return NotFound("Not able to create new order");
                }

                return Ok(orderId);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception thrown while creating new order: {e.Message}");
            }

            return BadRequest("Error creating new order");
        }

        [HttpPut("{orderId}")]
        public IActionResult UpdateInventory(string orderId)
        {
            try
            {
                _mongoDbManager.OrderFulfilment(orderId);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception thrown while updating inventory for order: {e.Message}");
            }

            return BadRequest("Error updating inventory");
        }
    }
}