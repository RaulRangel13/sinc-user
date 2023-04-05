using Domain.DTOs.Requests;
using Domain.DTOs.Responses;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public Task<CustomerResponse> CustomerRegister(CustomerRequestDto customer)
        {
            throw new NotImplementedException();
        }
    }
}
