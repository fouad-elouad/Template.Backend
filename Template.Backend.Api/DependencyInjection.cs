using Microsoft.EntityFrameworkCore;
using Template.Backend.Api.Services;
using Template.Backend.Data.Audit;
using Template.Backend.Data.Repositories;
using Template.Backend.Data.SpecificRepositories;
using Template.Backend.Data;
using Template.Backend.Data.Utilities;
using Template.Backend.Service.Audit;
using Template.Backend.Service.Services;
using Template.Backend.Service.Validation;

namespace Template.Backend.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAllServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ICompanyService, CompanyService>();
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<ICompanyAuditService, CompanyAuditService>();

            services.AddTransient<IDepartmentService, DepartmentService>();
            services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            services.AddTransient<IDepartmentAuditService, DepartmentAuditService>();

            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IEmployeeRepository, EmployeeRepository>();
            services.AddTransient<IEmployeeAuditService, EmployeeAuditService>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IValidationDictionary, ValidationDictionary>();
            services.AddScoped(typeof(IAuditRepository<>), typeof(AuditRepository<>));

            services.AddScoped<AuditSaveChangesInterceptor>();
            services.AddSingleton<IDateTime, DateTimeService>();
            services.AddSingleton<ICurrentUserService, CurrentUserService>();

            services.AddHttpContextAccessor();

            services.AddDbContext<StarterDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                );

            return services;
        }
    }
}
