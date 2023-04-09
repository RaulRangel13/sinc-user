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
        Task<bool> HasRegisteredCustomerAsync(string email);
        void SetToken(CustomerResponse customerResponse, HttpRequest request);
        Task<CustomerResponse> RecoverPasswordAsync(string email, string baseUrl);
        Task<CustomerResponse> LoginCustomerAsync(CustomerSigInRequestDto customer);
        Task<CustomerResponse> ChangePasswordAsync(int customerId, string password);
        Task<CustomerResponse> SaveNewCustomerAsync(CustomerSigUpRequestDto customer);

    }
}
