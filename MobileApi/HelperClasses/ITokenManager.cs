using MobileApi.Models;

namespace MobileApi.HelperClasses
{
    public interface ITokenManager
    {
        bool Authenticate(string username, string pass);
        Token NewToken(string ticket);
        bool VerifyToken(string token);
    }
}