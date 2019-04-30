using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DurianBookstoreWebservice.Model
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string isbn { get; set; }
        public string title { get; set; }
        public List<string> authors { get; set; }
        public double price { get; set; }
        public int total_inventory { get; set; }
        public int available_inventory { get; set; }
     }
}
