﻿using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DurianBookstoreWebservice.Model
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string customer_id { get; set; }
        public List<string> book_id { get; set; }
        public bool is_fulfilled { get; set; }
    }
}
