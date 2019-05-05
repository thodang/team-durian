using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DurianMongoDbCRUD.Model
{
    public class Customer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        //public Guid customer_id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public Address address { get; set; }
        public override string ToString()
        {
            return string.Format($"Id: {Id}, Name: {name}, Email: {email}, Address: {address.street}, {address.city}, {address.state}, {address.zipcode}");
        }
    }

    public class Address
    {
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipcode { get; set; }
    }

}
