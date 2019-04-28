using System;
using MongoDB.Bson;

namespace DurianMongoDbCRUD.Model
{
    public class Customer
    {
        public ObjectId Id { get; set; }
        public Guid customer_id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public Address address { get; set; }
        public override string ToString()
        {
            return string.Format($"Customer_Id: {customer_id}, Name: {name}, Email: {email}, Address: {address.street}, {address.city}, {address.state}, {address.zipcode}");
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
