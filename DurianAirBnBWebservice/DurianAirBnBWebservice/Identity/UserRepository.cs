using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DurianAirBnBWebservice.Identity
{
    /// <summary>
    /// This class handle all user related functions used for
    /// Login, signup, password generation
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly IMongoDatabase _database;
        private const string UsersCollection = "users";

        public UserRepository(IConfiguration configuration)
        {
            var mongoClient = new MongoClient(configuration.GetValue<string>("MongoDb:ConnectionString"));
            _database = mongoClient.GetDatabase(configuration.GetValue<string>("MongoDb:Database"));
        }

        public User GetUserByUsername(string userName)
        {
            try
            {

                var users = _database.GetCollection<User>(UsersCollection);
                var filter = new FilterDefinitionBuilder<User>().Eq(u => u.UserName, userName);

                try
                {
                    var result = users.Find(filter).FirstOrDefault();
                    return result;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public User ValidateUser(User user, out bool userExists)
        {
            try
            {
                var dbUser = GetUserByUsername(user.UserName);

                if (dbUser == null)
                {
                    userExists = false;
                    return null;
                }

                userExists = true;

                //Retrieve the user’s salt and hash from the DB.
                var dbHashedSaltedBytes = Convert.FromBase64String(dbUser.Password);

                //Take the salt out of the db stored value
                var salt = new byte[16];
                Array.Copy(dbHashedSaltedBytes, 0, salt, 0, 16);

                //hash the user entered password with the salt
                var pbkdf2 = new Rfc2898DeriveBytes(user.Password, salt, 10000);

                //put the hashed input in a byte array to compare it byte-by-byte with stored password
                var hashedPassword = pbkdf2.GetBytes(20);

                //compare results byte-by-byte
                //starting from byte 16 in the stored array 
                for (var i = 0; i < 20; i++)
                {
                    if (dbHashedSaltedBytes[i + 16] != hashedPassword[i])
                    {
                        return null;
                    }
                }

                return dbUser;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<string> RegisterUser(User user)
        {
            var existingUser = GetUserByUsername(user.UserName);
            //Cannot create duplicate users
            if (existingUser != null)
                return string.Empty;

            var hashedPassword = GeneratePasswordHash(user.Password);
            user.Password = hashedPassword;

            var users = _database.GetCollection<User>(UsersCollection);

            try
            {
                await users.InsertOneAsync(user);
                var objectId = user.Id;

                return objectId;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            var users = _database.GetCollection<User>(UsersCollection);

            try
            {
                var userUdateFilter = Builders<User>.Filter.Eq("Id", ObjectId.Parse(user.Id));
                var updateUser = Builders<User>.Update.Set(x => x.WebSessions, user.WebSessions);
                var userUpdateResult = await users.UpdateOneAsync(userUdateFilter, updateUser);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public string GeneratePasswordHash(string password)
        {
            //Create Salt using CSPRNG (cryptographically secure pseudorandom number generator)
            //new salt for every password - salt is 16 bytes
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            //hash and salt the password using PBKDF2 (slow algorith, good for password)
            //hash the hash, repeat 10000 times
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);

            //place the string in the byte array - has is 20 bytes
            var hash = pbkdf2.GetBytes(20);

            //make a new byte array to stored the hashed password+salt
            var hashBytes = new byte[36];

            //place the has and salt in their respective places
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            //convert the byte array to string
            return Convert.ToBase64String(hashBytes);
        }

        public User RemoveExpiredUserWebSessions(User user)
        {
            var expiredSessions =
                user.WebSessions.Where(x => DateTime.Compare(x.ExpirationDateTime, DateTime.UtcNow) < 0);

            foreach (var session in expiredSessions.ToArray())
            {
                user.WebSessions.Remove(session);
            }

            return user;
        }

        public User GetUserById(string uid)
        {
            var users = _database.GetCollection<User>(UsersCollection);
            var filter = new FilterDefinitionBuilder<User>().Eq("Id", ObjectId.Parse(uid));

            try
            {
                var result = users.Find(filter).FirstOrDefault();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }

}
