using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace DurianMongoDbCRUD.Model
{
    public class Order
    {
        public ObjectId Id { get; set; }
        public Guid order_id { get; set; }
        public Guid customer_id { get; set; }
        public List<Guid> book_id { get; set; }
    }
}
