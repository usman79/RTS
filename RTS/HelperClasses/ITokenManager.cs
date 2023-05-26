using RTS.Models;

namespace RTS.HelperClasses
{
    public interface ITokenManager
    {
        bool Authenticate(string username, string pass);
        Token NewToken();
        bool VerifyToken(string token);
    }
}