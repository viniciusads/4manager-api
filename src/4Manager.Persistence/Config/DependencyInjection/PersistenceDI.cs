using _4Tech._4Manager.Application.Interfaces;
using _4Manager.Persistence.Context;
using _4Manager.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace _4Tech._4Manager.Persistence.Config.DependencyInjection
{
    public static class PersistenceDI
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
