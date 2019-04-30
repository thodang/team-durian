using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DurianBookstoreWebservice.Model
{
    public class Customer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public Address address { get; set; }
    }

    public class Address
    {
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipcode { get; set; }
    }
}
