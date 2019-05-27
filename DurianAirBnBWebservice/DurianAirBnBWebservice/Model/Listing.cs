using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DurianAirBnBWebservice.Model
{
    [BsonIgnoreExtraElements]
    public class Listing
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string Id { get; set; }
        [BsonElement("id")]
        public int ListingId { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("summary")]
        public string Summary { get; set; }
        [BsonElement("description")]
        public string Description { get; set; }
        [BsonElement("listing_url")]
        public string ListingUrl { get; set; }
        [BsonElement("picture_url")]
        public string PictureUrl { get; set; }
        [BsonElement("thumbnail_url")]
        public string ThumbnailUrl { get; set; }
        [BsonElement("street")]
        public string Street { get; set; }
        [BsonElement("city")]
        public string City { get; set; }
        [BsonElement("state")]
        public string State { get; set; }
        [BsonElement("zipcode")]
        public int Zipcode { get; set; }
        [BsonElement("market")]
        public string Market { get; set; }
        [BsonElement("country")]
        public string Country { get; set; }
        [BsonElement("latitude")]
        public double Latitude { get; set; }
        [BsonElement("longitude")]
        public double Longitude { get; set; }
        [BsonElement("property_type")]
        public string PropertyType { get; set; }
        [BsonElement("room_type")]
        public string RoomType { get; set; }
        [BsonElement("accommodates")]
        public int Accommodates { get; set; }
        [BsonElement("bathrooms")]
        public double Bathrooms { get; set; }
        [BsonElement("bedrooms")]
        public int Bedrooms { get; set; }
        [BsonElement("beds")]
        public int Beds { get; set; }
        [BsonElement("amenities")]
        public string Amenities { get; set; }
        [BsonElement("square_feet")]
        public int SquareFeet { get; set; }
        [BsonElement("price")]
        public string Price { get; set; }
        [BsonElement("security_deposit")]
        public string SecurityDeposit { get; set; }
        [BsonElement("cleaning_fee")]
        public string CleaningFee { get; set; }
        [BsonElement("extra_people")]
        public string ExtraPeople { get; set; }
        [BsonElement("minimum_nights")]
        public int MinimumNights { get; set; }
        [BsonElement("maximum_nights")]
        public int MaximumNights { get; set; }
        [BsonElement("number_of_reviews")]
        public int NumberOfReviews { get; set; }
        [BsonElement("review_scores_rating")]
        public int? ReviewScoresRating { get; set; }
        [BsonElement("cancellation_policy")]
        public string CancellationPolicy { get; set; }
        //Host Info
        [BsonElement("host_name")]
        public string HostName { get; set; }
        [BsonElement("host_thumbnail_url")]
        public string HostThumbnailUrl { get; set; }
        [BsonElement("host_picture_url")]
        public string HostPictureUrl { get; set; }
        [BsonElement("host_about")]
        public string HostAbout { get; set; }

        public List<Review> Reviews { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class LazyListing
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string Id { get; set; }

        [BsonElement("id")]
        public int ListingId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("summary")]
        public string Summary { get; set; }
        [BsonElement("picture_url")]
        public string PictureUrl { get; set; }
        [BsonElement("latitude")]
        public double Latitude { get; set; }
        [BsonElement("longitude")]
        public double Longitude { get; set; }
        [BsonElement("price")]
        public string Price { get; set; }
        [BsonElement("number_of_reviews")]
        public int NumberOfReviews { get; set; }
        [BsonElement("review_scores_rating")]
        public int? ReviewScoresRating { get; set; }
    }
}
