using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class TwoFaRepository : BaseRepository<TwoFA>, ITwoFaRepository
    {
        private readonly DbContext _dbContext;

        public TwoFaRepository(SintecProjectContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<TwoFA?> GetByCustomerAsync(int customerId) =>
            await _dbContext.Set<TwoFA>().AsNoTracking().FirstOrDefaultAsync(x => x.CustomerId == customerId);

        public async Task<TwoFA?> GetCustomerKeyAsync(string key, int customerId) =>
            await _dbContext.Set<TwoFA>().AsNoTracking().FirstOrDefaultAsync(x => x.key == key && x.CustomerId == customerId);
    }
}
