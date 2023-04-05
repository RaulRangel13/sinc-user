using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        private readonly DbContext _dbContext;
        public CustomerRepository(SintecProjectContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Customer?> GetByEmail(string email) =>
            await _dbContext.Set<Customer>().AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);

        public async Task<Customer?> GetByEmailPassword(string email, string password) =>
            await _dbContext.Set<Customer>().AsNoTracking().FirstOrDefaultAsync(x => x.Email == email && x.Password == password);

        public async Task<bool> HasRegisteredCustomer(string email) =>
            _dbContext.Set<Customer>().AsNoTracking().FirstOrDefaultAsync(x => x.Email == email).Result is null ? false : true;

    }
}
