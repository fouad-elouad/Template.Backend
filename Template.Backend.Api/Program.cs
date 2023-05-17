using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Template.Backend.Api.Exceptions;
using Template.Backend.Api.Services;
using Template.Backend.Data.Audit;
using Template.Backend.Data.Repositories;
using Template.Backend.Data.SpecificRepositories;
using Template.Backend.Data.Utilities;
using Template.Backend.Data;
using Template.Backend.Service.Audit;
using Template.Backend.Service.Services;
using Template.Backend.Service.Validation;
using NLog;
using NLog.Web;
using System.Text.Json.Serialization;
using Template.Backend.Api.Utilities;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();
    builder.Host.UseNLog();

    builder.Services.AddApiVersioning(opt =>
    {
        opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
        opt.AssumeDefaultVersionWhenUnspecified = true;
        opt.ReportApiVersions = true;
        opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                        new HeaderApiVersionReader(),
                                                        new MediaTypeApiVersionReader(Constants.CustomHeader_api_version));
    });

    // Add ApiExplorer to discover versions
    builder.Services.AddVersionedApiExplorer(setup =>
    {
        setup.GroupNameFormat = "'v'VVV";
        setup.SubstituteApiVersionInUrl = true;
    });


    var allowedOrigins = builder.Configuration.GetValue<string>("AllowedOrigins");

    if (!string.IsNullOrEmpty(allowedOrigins))
    {
        var origins = allowedOrigins.Split(";");
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                policy =>
                {
                    //policy.AllowAnyOrigin() // uncomment for Development Environment
                    policy.WithOrigins(origins)
                    .AllowAnyMethod()
                    //.AllowAnyHeader()
                    //Custom headers may not send back to client because of cors policies restrictions : we Add exception to it
                    .WithExposedHeaders(Constants.CustomHeader_total_count_returned, Constants.CustomHeader_total_count_found);
                });
        });
    }

    // Add services to the container.

    builder.Services.AddAutoMapper(typeof(Program));

    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<HttpResponseExceptionFilter>();
    }).AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddTransient<ICompanyService, CompanyService>();
    builder.Services.AddTransient<ICompanyRepository, CompanyRepository>();

    builder.Services.AddTransient<ICompanyAuditService, CompanyAuditService>();
    builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
    builder.Services.AddTransient<IValidationDictionary, ValidationDictionary>();
    builder.Services.AddScoped(typeof(IAuditRepository<>), typeof(AuditRepository<>));

    builder.Services.AddScoped<AuditSaveChangesInterceptor>();
    builder.Services.AddSingleton<IDateTime, DateTimeService>();
    builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();

    builder.Services.AddHttpContextAccessor();

    builder.Services.AddDbContext<StarterDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        );

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<StarterDbContext>();
            dbContext.Database.Migrate();
            StarterSeedData.SeedData(dbContext);
        }
        catch (Exception ex)
        {
            logger.Error(ex, "An error occurred while migrating or seeding the database.");
            throw;
        }
    }

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseExceptionHandler("/error-development");
    }
    else
    {
        app.UseExceptionHandler("/error");
    }

    app.UseHttpsRedirection();

    app.UseCors();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception exception)
{
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    LogManager.Shutdown();
}
