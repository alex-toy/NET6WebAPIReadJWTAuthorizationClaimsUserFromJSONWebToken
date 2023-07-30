using CustomerAPI.Models;

namespace CustomerAPI.Services
{
    public interface IUserService
    {
        TblUser GetUserById(string userid);
        TblUser GetUserByCredentials(UserCredential user);
        void SaveUserInDb(TblUser value);
    }
}