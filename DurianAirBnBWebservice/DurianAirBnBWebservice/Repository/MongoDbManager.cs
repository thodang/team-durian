using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DurianAirBnBWebservice.Model;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace DurianAirBnBWebservice.Repository
{
    public class MongoDbManager : IMongoDbManager
    {
        private readonly IMongoDatabase _database;
        private const string ListingsCollection = "listings";
        private const string ReviewsCollection = "reviews";

        public MongoDbManager(IConfiguration configuration)
        {
            var mongoClient = new MongoClient(configuration.GetValue<string>("MongoDb:ConnectionString"));
            _database = mongoClient.GetDatabase(configuration.GetValue<string>("MongoDb:Database"));
        }

        public async Task<List<LazyListing>> GetAllListings()
        {
            var listings = _database.GetCollection<LazyListing>(ListingsCollection);
            var filter = new FilterDefinitionBuilder<LazyListing>().Empty;
            try
            {
                var results = await listings.Find(filter).Limit(50).ToListAsync();
                return results;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<LazyListing>> GetListingsByPaging(string from, string count)
        {
            var listings = _database.GetCollection<LazyListing>(ListingsCollection);
            var filter = new FilterDefinitionBuilder<LazyListing>().Empty;
            try
            {
                var results = await listings.Find(filter).Skip(Convert.ToInt32(from)).Limit(Convert.ToInt32(count)).ToListAsync();
                return results;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<string> GetDistinctStates()
        {
            var listings = _database.GetCollection<Listing>(ListingsCollection);
            var filter = new FilterDefinitionBuilder<Listing>().Empty;
            FieldDefinition<Listing, string> state = "state";
            try
            {
                var results =  listings.DistinctAsync(state, filter).GetAwaiter().GetResult().ToListAsync().GetAwaiter().GetResult();
                results.Sort();
                return results;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<int> GetDistinctZipcodes()
        {
            var listings = _database.GetCollection<Listing>(ListingsCollection);
            var filter = new FilterDefinitionBuilder<Listing>().Empty;
            FieldDefinition<Listing, int> state = "zipcode";
            try
            {
                var results = listings.DistinctAsync(state, filter).GetAwaiter().GetResult().ToListAsync().GetAwaiter()
                    .GetResult();
                results.Sort();
                return results;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Listing GetListingDetailById(int listingId)
        {
            var listings = _database.GetCollection<Listing>(ListingsCollection);
            var filter = new FilterDefinitionBuilder<Listing>().Eq("id", listingId);

            try
            {
                var result = listings.Find(filter).FirstOrDefault();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<Review>> GetReviewsForListing(int listingId)
        {
            var reviews = _database.GetCollection<Review>(ReviewsCollection);
            var filter = new FilterDefinitionBuilder<Review>().Eq("listing_id", listingId);

            try
            {
                var result = await reviews.Find(filter).ToListAsync();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
