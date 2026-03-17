using _4Manager.Persistence.Context;
using _4Manager.Persistence.Repositories;
using _4Tech._4Manager.Application.Features.TimesheetReports.Services;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectRepo = _4Tech._4Manager.Persistence.Repositories;

namespace _4Tech._4Manager.Persistence.Config.DependencyInjection
{
    public static class PersistenceDI
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ITimesheetRepository, TimesheetRepository>();
            services.AddScoped<ITimesheetReportCalculator, TimesheetReportCalculator>();
            services.AddScoped<IActivityTypeRepository, ActivityTypeRepository>();
            return services;
        }
    }
}
