using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DurianMongoDbCRUD.Model
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        //public Guid book_id { get; set; }
        public string isbn { get; set; }
        public string title { get; set; }
        public List<string> authors { get; set; }
        public double price { get; set; }
        public int total_inventory { get; set; }
        public int available_inventory { get; set; }
        public override string ToString()
        {
            return string.Format($"Id: {Id}, ISBN: {isbn}, Title: {title}, Authors: {authors}, Price: ${price}, Total_Inventory: {total_inventory}, Available_Inventory: {available_inventory}");
        }
    }
}
