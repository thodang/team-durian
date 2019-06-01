using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DurianAirBnBWebservice.Identity
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string Id { get; set; }
        [BsonElement("username")]
        public string UserName { get; set; }
        [BsonElement("password")]
        public string Password { get; set; }
        [BsonElement("web_sessions")]
        public List<UserWebSession> WebSessions { get; set; }
        [BsonElement("passwordResetToken")]
        public string PasswordResetToken { get; set; }
        [BsonElement("resetTokenExpiration")]
        public DateTime ResetTokenExpiration { get; set; }
        [BsonElement("resetTokenActive")]
        public bool ResetTokenActive { get; set; }
    }
}
