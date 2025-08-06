using Microsoft.Extensions.DependencyInjection;

namespace _4Manager.Application.DependencyInjection
{
    public static class ApplicationDI
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationDI).Assembly));

            return services;
        }
    }
}
