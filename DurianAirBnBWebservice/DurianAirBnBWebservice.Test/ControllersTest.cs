using System.Collections.Generic;
using System.Threading.Tasks;
using DurianAirBnBWebservice.Controllers;
using DurianAirBnBWebservice.Model;
using Moq;
using Xunit;
using DurianAirBnBWebservice.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DurianAirBnBWebservice.Test
{
    public class ControllersTest
    {
        [Fact]
        public async Task Test1_ListingController_GetListingsByPagination()
        {            
            // Arrange
            var mockRepo = new Mock<IMongoDbManager>();
            var mockLogger = new Mock<ILogger<ListingController>>();

            mockRepo.Setup(repo => repo.GetListingsByPaging("0", "30")).ReturnsAsync(new List<LazyListing>());
            var listingController = new ListingController(mockRepo.Object, mockLogger.Object);

            // Act
            var result = await listingController.GetListingsByPagination("0", "30", null);

            // Assert            
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
