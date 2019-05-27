using System.Collections.Generic;
using System.Threading.Tasks;
using DurianAirBnBWebservice.Model;

namespace DurianAirBnBWebservice.Repository
{
    public interface IMongoDbManager
    {
        /// <summary>
        /// Get All Listings
        /// </summary>
        /// <returns></returns>
        Task<List<LazyListing>> GetAllListings();

        /// <summary>
        /// Get Listings by pagination
        /// </summary>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<List<LazyListing>> GetListingsByPaging(string from, string count);

        /// <summary>
        /// Get list of distinct states
        /// </summary>
        /// <returns></returns>
        List<string> GetDistinctStates();

        /// <summary>
        /// Get list of distinct zipcodes
        /// </summary>
        /// <returns></returns>
        List<int> GetDistinctZipcodes();

        /// <summary>
        /// Get Listing Detail by listing Id
        /// </summary>
        /// <param name="lisingId"></param>
        /// <returns></returns>
        Listing GetListingDetailById(int lisingId);

        /// <summary>
        /// Get reviews for a listing
        /// </summary>
        /// <param name="listingId"></param>
        /// <returns></returns>
        Task<List<Review>> GetReviewsForListing(int listingId);
    }
}
