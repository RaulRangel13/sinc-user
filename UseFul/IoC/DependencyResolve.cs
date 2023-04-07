using Domain.Interfaces.Repositories;
using Domain.Interfaces.Repositories.Base;
using Domain.Interfaces.Services;
using Domain.Services;
using Infrastructure.AutoMapper;
using Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseFul.IoC
{
    public class DependencyResolve
    {
        public static void Instance(IServiceCollection services)
        {
            RepositoriesResolve(services);
            ServicesResolve(services);
        }
        private static void RepositoriesResolve(IServiceCollection services)
        {
            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddTransient<ICustomerRepository, CustomerRepository>();
        }
        private static void ServicesResolve(IServiceCollection services)
        {
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddAutoMapper(typeof(Mapper));
        }
    }
}
