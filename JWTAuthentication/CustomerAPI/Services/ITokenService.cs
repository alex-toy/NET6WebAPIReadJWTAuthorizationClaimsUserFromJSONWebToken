using CustomerAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace CustomerAPI.Services
{
    public interface ITokenService
    {
        ClaimsIdentity GenerateClaims(TblUser _user);
        string GenerateToken(TblUser _user);
        TokenResponse GetToken(string username, Claim[] claims);
        SecurityTokenDescriptor GetTokenDescriptor(TblUser _user, byte[] tokenkey);
    }
}