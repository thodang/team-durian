using System;

namespace DurianBookstoreWebservice.Identity
{
    public class UserWebSession
    {
        public string RefreshToken { get; set; }
        public bool IsActive { get; set; }
        public DateTime ExpirationDateTime { get; set; }
    }
}