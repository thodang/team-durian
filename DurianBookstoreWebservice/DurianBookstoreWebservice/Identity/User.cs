using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DurianBookstoreWebservice.Identity
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public List<UserWebSession> web_sessions { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
    }
}
