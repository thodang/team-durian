using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace DurianMongoDbCRUD.Model
{
    public class Book
    {
        public ObjectId Id { get; set; }
        public Guid book_id { get; set; }
        public string isbn { get; set; }
        public string title { get; set; }
        public List<string> authors { get; set; }
        public double price { get; set; }
        public int total_inventory { get; set; }
        public int available_inventory { get; set; }
        public override string ToString()
        {
            return string.Format($"Book_Id: {book_id}, ISBN: {isbn}, Title: {title}, Authors: {authors}, Price: ${price}, Total_Inventory: {total_inventory}, Available_Inventory: {available_inventory}");
        }
    }
}
