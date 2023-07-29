using JwtWebApiApp.Dtos;
using JwtWebApiApp.Models;
using JwtWebApiApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JwtWebApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthController(IConfiguration configuration, IUserService userService, ITokenService tokenService)
        {
            _configuration = configuration;
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpGet, Authorize]
        public ActionResult<string> GetMe()
        {
            var userName = _userService.Get(ClaimTypes.Name);
            var role = _userService.Get(ClaimTypes.Role);
            var tokenExpires = _userService.Get("expires");
            return Ok(new { userName = userName, role = role, tokenExpires = tokenExpires });
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            _tokenService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            if (user.Username != request.Username) return BadRequest("User not found.");

            bool passwordOK = _tokenService.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt);
            if (!passwordOK) return BadRequest("Wrong password.");

            string token = _tokenService.CreateToken(user);

            var refreshToken = _tokenService.GenerateRefreshToken();
            SetRefreshToken(refreshToken);

            return Ok(token);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            bool isRefreshTokenOK = user.RefreshToken.Equals(refreshToken);
            if (!isRefreshTokenOK) return Unauthorized("Invalid Refresh Token.");

            bool tokenExpired = user.TokenExpires < DateTime.Now;
            if (tokenExpired) return Unauthorized("Token expired.");

            string token = _tokenService.CreateToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            SetRefreshToken(newRefreshToken);

            return Ok(token);
        }

        private void SetRefreshToken(RefreshToken newRefreshToken)
        {
            AddCookieToResponse(newRefreshToken);

            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;
        }

        private void AddCookieToResponse(RefreshToken newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
        }
    }
}
