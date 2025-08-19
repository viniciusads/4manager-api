using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace _4Tech._4Manager.Infrastructure.Config.DependencyInjection
{
    public static class InfrastructureDI
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            return services;
        }
    }
}
