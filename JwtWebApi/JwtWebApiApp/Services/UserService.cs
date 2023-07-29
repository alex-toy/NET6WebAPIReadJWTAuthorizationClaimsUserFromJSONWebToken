using System.Security.Claims;

namespace JwtWebApiApp.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetName()
        {
            if (_httpContextAccessor.HttpContext == null) return string.Empty;

            string? result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            return result;
        }

        public string Get(string claim)
        {
            if (_httpContextAccessor.HttpContext == null) return string.Empty;

            string? result = _httpContextAccessor.HttpContext.User.FindFirstValue(claim);
            return result;
        }
    }
}
