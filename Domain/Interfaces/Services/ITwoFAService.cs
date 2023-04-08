using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services
{
    public interface ITwoFAService
    {
        Task<bool> ValidateKeyAsync(string key, int customerId);
        Task<TwoFA> GenerateKeyAsync(int customerId);
    }
}
