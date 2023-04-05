using Domain.DTOs.Requests;
using Domain.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<CustomerResponse> SaveNewCustomer(CustomerSigUpRequestDto customer);
        Task<CustomerResponse> LoginCustomer(CustomerSigInRequestDto customer);
        Task<CustomerResponse> RecoverPassword(string email);
        Task<bool> HasRegisteredCustomer(string email);
    }
}
