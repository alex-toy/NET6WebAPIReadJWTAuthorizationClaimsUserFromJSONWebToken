using JwtWebApiApp.Models;

namespace JwtWebApiApp.Services
{
    public interface ITokenService
    {
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        string CreateToken(User user);
        RefreshToken GenerateRefreshToken();
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}