using CustomerAPI.Models;

namespace CustomerAPI.Services
{
    public interface ICustomerService
    {
        void AddCustomer(TblCustomer customer);
        TblCustomer GetCustomerById(int customerId);
        string UpdateCustomer(TblCustomer customer, TblCustomer customerDb);
    }
}