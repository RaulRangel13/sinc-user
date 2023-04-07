using Domain.DTOs.Requests;
using Domain.DTOs.Responses;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<CustomerResponse> GetCustomerByIdAsync(int id);
        Task<CustomerResponse> LoginNoPasswordAsync(int id);
        Task<bool> HasRegisteredCustomer(string email);
        Task<CustomerResponse> RecoverPassword(string email, string baseUrl);
        Task<CustomerResponse> SaveNewCustomer(CustomerSigUpRequestDto customer);
        Task<CustomerResponse> LoginCustomer(CustomerSigInRequestDto customer);
        Task<CustomerResponse> ChangePassword(int customerId, string password);
        void SetToken(CustomerResponse customerResponse, HttpRequest request);
    }
}
