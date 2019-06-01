using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DurianAirBnBWebservice.Model;
using DurianAirBnBWebservice.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DurianAirBnBWebservice.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ListingController : Controller
    {
        private readonly IMongoDbManager _mongoDbManager;
        private readonly ILogger<ListingController> _logger;

        public ListingController(IMongoDbManager mongoDbManager, ILogger<ListingController> logger)
        {
            _mongoDbManager = mongoDbManager;
            _logger = logger;
        }

        /// <summary>
        /// Get complete data for a listing
        /// </summary>
        /// <param name="listingId"></param>
        /// <returns></returns>
        [HttpGet("{listingId}")]
        public IActionResult GetListingDetailById(int listingId)
        {
            try
            {
                var listings = _mongoDbManager.GetListingDetailById(listingId);
                listings.Reviews = _mongoDbManager.GetReviewsForListing(listingId).Result;
                return Ok(JsonConvert.SerializeObject(listings));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception thrown while getting list of listings: {e.Message}");
            }

            return BadRequest("Error getting all listings");
        }

        /// <summary>
        /// Get the data required for display in the listing Card
        /// </summary>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <param name="guests"></param>
        /// <returns></returns>
        [HttpGet("paged")]
        public async Task<IActionResult> GetListingsByPagination([FromQuery]string from, [FromQuery]string count, [FromQuery]string guests)
        {
            try
            {
                List<LazyListing> listings;

                if(string.IsNullOrEmpty(guests) || guests.Equals("undefined"))
                    listings = await _mongoDbManager.GetListingsByPaging(from, count);
                else
                    listings = await _mongoDbManager.GetListingsByPagingAndFilter(from, count, guests);

                return Ok(JsonConvert.SerializeObject(listings));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception thrown while getting list of listings: {e.Message}");
            }

            return BadRequest("Error getting all listings");
        }
    }
}
