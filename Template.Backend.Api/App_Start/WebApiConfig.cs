using Microsoft.Web.Http.Routing;
using Template.Backend.Api.Providers;
using Template.Backend.Data.Repositories;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Routing;
using Unity;
using Unity.Lifetime;
using Unity.RegistrationByConvention;
using Template.Backend.Api.Exceptions;
using System.Web.Http.ExceptionHandling;
using Template.Backend.Api.Areas.HelpPage;

namespace Template.Backend.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.Remove(config.Formatters.FormUrlEncodedFormatter);
            HelpPageAreaRegistration.RegisterAllAreas();

            config.Filters.Add(new GlobalExceptionFilterAttribute());
            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
            config.Services.Replace(typeof(IExceptionLogger), new GlobalExceptionLogger());

            // Web API configuration and services
            UnityContainer unityContainer = new UnityContainer();
            unityContainer.RegisterTypes(
                AllClasses.FromLoadedAssemblies(),
                WithMappings.FromMatchingInterface,
                WithName.Default);

            unityContainer.RegisterType<IDbFactory, DbFactory>(new PerResolveLifetimeManager());

            config.DependencyResolver = new UnityResolver(unityContainer);

            // AutoMapper;
            AutoMapperConfig.Configure();

            // API Version
            var constraintResolver = new DefaultInlineConstraintResolver()
            {
                ConstraintMap = { ["apiVersion"] = typeof(ApiVersionRouteConstraint) }
            };
            config.AddApiVersioning();
            config.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV"; options.SubstituteApiVersionInUrl = true;
            });

            //Web API Cors
            config.EnableCors();
            ICorsPolicyProviderFactory policyProvider = new CorsPolicyFactory();
            config.SetCorsPolicyProviderFactory(policyProvider);

            //Web API routes
            config.MapHttpAttributeRoutes(constraintResolver);
        }
    }
}
