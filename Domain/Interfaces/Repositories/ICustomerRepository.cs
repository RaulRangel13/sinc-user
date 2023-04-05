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
        Task<bool> HasRegisteredCustomer(string email);
        Task<Customer?> GetByEmailPassword(string email, string password);
        Task<Customer?> GetByEmail(string email);
    }
}
