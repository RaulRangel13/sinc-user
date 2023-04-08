using Domain.Entities;
using Domain.Interfaces.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface ITwoFaRepository : IBaseRepository<TwoFA>
    {
        Task<TwoFA?> GetCustomerKey(string key, int customerId);
        Task<TwoFA?> GetByCustomer(int customerId);
    }
}
