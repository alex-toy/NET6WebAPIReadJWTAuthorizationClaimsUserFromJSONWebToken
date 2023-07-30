using CustomerAPI.Models;
using System.Linq;

namespace CustomerAPI.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _appDbContext;

        public UserService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void SaveUserInDb(TblUser user)
        {
            TblUser tblUser = new TblUser()
            {
                Name = user.Name,
                Email = user.Email,
                Userid = user.Userid,
                Role = string.Empty,
                Password = user.Password,
                IsActive = false
            };
            _appDbContext.TblUser.Add(tblUser);
            _appDbContext.SaveChanges();
        }

        public TblUser GetUserByCredentials(UserCredential user)
        {
            TblUser userDb = _appDbContext.TblUser.FirstOrDefault(o => o.Userid == user.Name && o.Password == user.Password && o.IsActive == true);
            return userDb;
        }

        public TblUser GetUserById(string userid)
        {
            return _appDbContext.TblUser.FirstOrDefault(o => o.Userid == userid);
        }
    }
}
