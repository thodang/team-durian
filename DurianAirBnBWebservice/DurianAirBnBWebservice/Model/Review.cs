using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DurianAirBnBWebservice.Model
{
    [BsonIgnoreExtraElements]
    public class Review
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string Id { get; set; }
        [BsonElement("listing_id")]
        public int ListingId { get; set; }
        [BsonElement("date")]
        public string Date { get; set; }
        [BsonElement("reviewer_id")]
        public int ReviewerId { get; set; }
        [BsonElement("reviewer_name")]
        public string ReviewerName { get; set; }
        [BsonElement("comments")]
        public string Comments { get; set; }
    }
}
