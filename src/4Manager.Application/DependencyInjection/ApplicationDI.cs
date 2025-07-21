using Microsoft.Extensions.DependencyInjection;
using MediatR;
using System.Reflection;


namespace _4Manager.Application.DependencyInjection
{
    public static class ApplicationDI
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ApplicationDI).Assembly);

            return services;
        }
    }
}
