namespace JwtWebApiApp.Services
{
    public interface IUserService
    {
        string GetName();
        string Get(string claim);
    }
}