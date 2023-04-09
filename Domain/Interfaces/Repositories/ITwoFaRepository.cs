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
        Task<TwoFA?> GetByCustomerAsync(int customerId);
        Task<TwoFA?> GetCustomerKeyAsync(string key, int customerId);
    }
}
