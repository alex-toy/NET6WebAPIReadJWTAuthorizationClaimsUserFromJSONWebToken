using CustomerAPI.Models;
using CustomerAPI.Models.Authentication;
using CustomerAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace CustomerAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly IRefreshTokenGenerator _tokenGenerator;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public UserController(AppDbContext learn_DB, IRefreshTokenGenerator _refreshToken, ITokenService tokenService, IUserService userService)
        {
            _appDbContext = learn_DB;
            _tokenGenerator = _refreshToken;
            _tokenService = tokenService;
            _userService = userService;
        }

        [NonAction]
        public TokenResponse Authenticate(string username,Claim[] claims)
        {
            return _tokenService.GetToken(username, claims);
        }

        [Route("Authenticate")]
        [HttpPost]
        public IActionResult Authenticate([FromBody] UserCredential user)
        {
            TblUser _user = _userService.GetUserByCredentials(user);
            if (_user == null) return Unauthorized();

            TokenResponse tokenResponse = new TokenResponse();
            tokenResponse.JWTToken = _tokenService.GenerateToken(_user);
            tokenResponse.RefreshToken = _tokenGenerator.GenerateRefreshToken(user.Name);

            return Ok(tokenResponse);
        }

        [Route("Refresh")]
        [HttpPost]
        public IActionResult Refresh([FromBody] TokenResponse token)
        {
           
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token.JWTToken);
            var username = securityToken.Claims.FirstOrDefault(c => c.Type == "unique_name")?.Value;

            //var username = principal.Identity.Name;
            var _reftable = _appDbContext.TblRefreshtoken.FirstOrDefault(o => o.UserId == username && o.RefreshToken == token.RefreshToken);
            if (_reftable == null)
            {
                return Unauthorized();
            }
            TokenResponse _result = Authenticate(username, securityToken.Claims.ToArray());
            return Ok(_result);
        }

        [Route("GetMenubyRole/{role}")]
        [HttpGet]
        public IActionResult GetMenubyRole(string role)
        {
            var _result = (from q1 in _appDbContext.TblPermission.Where(item=>item.RoleId==role)
                          join q2 in _appDbContext.TblMenu
                          on q1.MenuId equals q2.Id
                          select new { q1.MenuId, q2.Name, q2.LinkName }).ToList();
           // var _result = context.TblPermission.Where(o => o.RoleId == role).ToList();
           
            return Ok(_result);
        }

        [Route("HaveAccess")]
        [HttpGet]
        public IActionResult HaveAccess(string role,string menu)
        {
            APIResponse result = new APIResponse();
            //var username = principal.Identity.Name;
            var _result = _appDbContext.TblPermission.Where(o => o.RoleId == role && o.MenuId == menu).FirstOrDefault();
            if (_result != null)
            {
                result.result = "pass";
            }
            return Ok(result);
        }

        [Route("GetAllRole")]
        [HttpGet]
        public IActionResult GetAllRole()
        {
            var _result = _appDbContext.TblRole.ToList();
            // var _result = context.TblPermission.Where(o => o.RoleId == role).ToList();

            return Ok(_result);
        }

        [HttpPost("Register")]
        public APIResponse Register([FromBody] TblUser value)
        {
            string result = string.Empty;
            try
            {
                TblUser _emp = _userService.GetUserById(value.Userid);
                if (_emp != null)
                {
                    result = string.Empty;
                }
                else
                {
                    _userService.SaveUserInDb(value);
                    result = "pass";
                }
            }
            catch (Exception ex)
            {
                result = string.Empty;
            }
            return new APIResponse { keycode = string.Empty, result = result };
        }
    }
}
