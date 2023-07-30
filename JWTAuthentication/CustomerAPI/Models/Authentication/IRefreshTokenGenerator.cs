namespace CustomerAPI.Models.Authentication
{
    public interface IRefreshTokenGenerator
    {
        string GenerateRefreshToken(string username);
    }
}
