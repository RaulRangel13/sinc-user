using Domain.DTOs.Requests;
using Domain.DTOs.Responses;
using Domain.Entities;
using Domain.Interfaces.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        Task<Customer?> GetByEmailAsync(string email);
        Task<bool> HasRegisteredCustomerAsync(string email);
        Task<Customer?> GetByEmailPasswordAsync(string email, string password);
    }
}
