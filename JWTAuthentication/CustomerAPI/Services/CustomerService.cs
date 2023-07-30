using CustomerAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace CustomerAPI.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _appDbContext;

        public CustomerService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void AddCustomer(TblCustomer customer)
        {
            TblCustomer tblEmployee = new TblCustomer()
            {
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone,
                CreditLimit = customer.CreditLimit,
            };
            _appDbContext.TblCustomer.Add(tblEmployee);
            _appDbContext.SaveChanges();
        }

        public string UpdateCustomer(TblCustomer customer, TblCustomer customerDb)
        {
            string result;
            customerDb.Email = customer.Email;
            customerDb.Name = customer.Name;
            customerDb.Phone = customer.Phone;
            customerDb.CreditLimit = customer.CreditLimit;
            _appDbContext.SaveChanges();
            result = "pass";
            return result;
        }

        public TblCustomer GetCustomerById(int customerId)
        {
            return _appDbContext.TblCustomer.FirstOrDefault(o => o.Id == customerId);
        }

        public IEnumerable<TblCustomer> GetAll()
        {
            return _appDbContext.TblCustomer.ToList();
        }
    }
}
