using Domain.Interfaces.Repositories.Base;
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
        }
        private static void RepositoriesResolve(IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        }
    }
}
