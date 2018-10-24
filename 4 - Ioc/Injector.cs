using Microservice.DomainServices.Interfaces.Repositories;
using Microservice.Repository;
using Microservice.RepositoryServices.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Mircroservice.Ioc
{
    public static class Injector
    {
        public static void InitRepository(IServiceCollection services)
        {
            services.AddScoped<IValuesRepository, ValuesRepository>();
            services.AddScoped<MicroserviceContexto>();
        }
        
    }
}