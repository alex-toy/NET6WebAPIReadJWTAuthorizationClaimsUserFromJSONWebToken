using CustomerAPI.Models;
using System.Collections.Generic;

namespace CustomerAPI.Services
{
    public interface ICustomerService
    {
        void AddCustomer(TblCustomer customer);
        IEnumerable<TblCustomer> GetAll();
        TblCustomer GetCustomerById(int customerId);
        string UpdateCustomer(TblCustomer customer, TblCustomer customerDb);
    }
}