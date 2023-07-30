using CustomerAPI.Models;
using CustomerAPI.Models.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CustomerAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly JWTSetting _setting;
        private readonly IRefreshTokenGenerator _tokenGenerator;

        public TokenService(IOptions<JWTSetting> options, IRefreshTokenGenerator tokenGenerator)
        {
            _setting = options.Value;
            _tokenGenerator = tokenGenerator;
        }

        public string GenerateToken(TblUser _user)
        {
            var tokenhandler = new JwtSecurityTokenHandler();
            var tokenkey = Encoding.UTF8.GetBytes(_setting.securitykey);
            var tokenDescriptor = GetTokenDescriptor(_user, tokenkey);
            var token = tokenhandler.CreateToken(tokenDescriptor);
            string finaltoken = tokenhandler.WriteToken(token);
            return finaltoken;
        }

        public SecurityTokenDescriptor GetTokenDescriptor(TblUser _user, byte[] tokenkey)
        {
            ClaimsIdentity claimsIdentity = GenerateClaims(_user);

            return new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.Now.AddMinutes(20),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)
            };
        }

        public ClaimsIdentity GenerateClaims(TblUser _user)
        {
            return new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, _user.Userid),
                new Claim(ClaimTypes.Role, _user.Role)
            });
        }

        public TokenResponse GetToken(string username, Claim[] claims)
        {
            TokenResponse tokenResponse = new TokenResponse();
            var tokenkey = Encoding.UTF8.GetBytes(_setting.securitykey);
            DateTime expires = DateTime.Now.AddMinutes(15);
            SigningCredentials signingCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256);
            var tokenhandler = new JwtSecurityToken(claims: claims, expires: expires, signingCredentials: signingCredentials);
            tokenResponse.JWTToken = new JwtSecurityTokenHandler().WriteToken(tokenhandler);
            tokenResponse.RefreshToken = _tokenGenerator.GenerateRefreshToken(username);

            return tokenResponse;
        }
    }
}
