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
        }
    }
}
