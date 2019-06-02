using System.Security.Claims;
using DurianAirBnBWebservice.Identity;

namespace DurianAirBnBWebservice.Model
{
    public interface ITokenManager
    {
        string GenerateToken(string username, string userId);
        string ValidateToken(string token);
        ClaimsPrincipal GetPrincipal(string token, bool validateExpiration = true);
        string GenerateRefreshToken();
        UserWebSession GenerateWebSession();
    }
}
