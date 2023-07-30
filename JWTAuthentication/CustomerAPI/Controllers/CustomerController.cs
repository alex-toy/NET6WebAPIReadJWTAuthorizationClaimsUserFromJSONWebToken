using CustomerAPI.Models;
using CustomerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CustomerAPI.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly ICustomerService _customerService;

        public CustomerController(AppDbContext learn_DB, ICustomerService customerService)
        {
            _appDbContext = learn_DB;
            _customerService = customerService;
        }

        // GET: api/<CustomerController>
        [HttpGet("get")]
        public IEnumerable<TblCustomer> Get()
        {
            return _customerService.GetAll();
        }

        [HttpGet("getbyid/{id}")]
        public TblCustomer Get(int id)
        {
            return _appDbContext.TblCustomer.FirstOrDefault(o => o.Id == id);
        }

        // POST api/<CustomerController>
        [HttpPost]
        public APIResponse Post([FromBody] TblCustomer customer)
        {
            string result = string.Empty;
            try
            {
                TblCustomer customerDb = _customerService.GetCustomerById(customer.Id);

                bool customerExists = customerDb != null;
                if (customerExists)
                {
                    result = _customerService.UpdateCustomer(customer, customerDb);
                }
                else
                {
                    _customerService.AddCustomer(customer);
                    result = "pass";
                }
            }
            catch (Exception ex)
            {
                result = string.Empty;
            }
            return new APIResponse { keycode = string.Empty, result = result };
        }

        // PUT api/<CustomerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
