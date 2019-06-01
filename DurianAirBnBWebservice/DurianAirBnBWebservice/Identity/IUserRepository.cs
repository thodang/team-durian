using System.Threading.Tasks;

namespace DurianAirBnBWebservice.Identity
{
    public interface IUserRepository
    {
        /// <summary>
        /// Get User by username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>User</returns>
        User GetUserByUsername(string userName);

        /// <summary>
        /// Validate user by comparing the entered password with the stored password
        /// Password is stored as Salt + Hash
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userExists"></param>
        /// <returns>Validated User</returns>
        User ValidateUser(User user, out bool userExists);

        /// <summary>
        /// Register a new user
        /// If user already exists, return HttpStatusCode.Forbidden
        /// </summary>
        /// <param name="user"></param>
        /// <returns>HttpStatusCode</returns>
        Task<string> RegisterUser(User user);

        /// <summary>
        /// Update User document
        /// Used mainly to manage WebSessions and Device collection
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task UpdateUserAsync(User user);

        /// <summary>
        /// Using the plaintext password, create a hash of the password
        /// with a secure randomly generated salt
        /// passwordHash[36] with 16-bit salt
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        string GeneratePasswordHash(string password);

        /// <summary>
        /// Get User by passwordResetToken
        /// There should only be one unique passwordResetToken in users collection
        /// </summary>
        /// <param name="token"></param>
        /// <returns>User</returns>
        User GetUserByPasswordResetToken(string token);

        /// <summary>
        /// Generate a secure random token using RNGCryptoServiceProvider
        /// Convert the token bytes to Base64String and store in DB
        /// Update the user object and store the password reset information
        /// in database
        /// </summary>
        /// <param name="user"></param>
        /// <returns>URLEncoded value for token value stored in DB</returns>
        Task<string> GeneratePasswordResetTokenUrl(User user);

        /// <summary>
        /// Check for duplicate active passwordResetToken
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        bool IsDuplicateResetToken(string token);

        /// <summary>
        /// Remove all the expired WebSessions for a user
        /// Check expiration data for Refresh Tokens
        /// If Refresh token is expired, remove the UserWebSession entry
        /// </summary>
        /// <param name="user"></param>
        User RemoveExpiredUserWebSessions(User user);

        /// <summary>
        /// Get User by Id
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        User GetUserById(string uid);
    }
}
