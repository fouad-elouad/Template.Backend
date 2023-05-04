using Microsoft.Owin;
using Owin;
using System.Web.Http;

[assembly: OwinStartup(typeof(Template.Backend.Api.Startup))]
namespace Template.Backend.Api
{
    /// <summary>
    /// Startup class with Owin
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Gets the HTTP configuration.
        /// </summary>
        /// <value>
        /// The HTTP configuration.
        /// </value>
        public static HttpConfiguration HttpConfiguration { get; private set; }


        /// <summary>
        /// Configures application.
        /// </summary>
        /// <param name="app">The application.</param>
        public void Configuration(IAppBuilder app)
        {
            ConfigureOAuth(app);
            HttpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(HttpConfiguration);
            app.UseWebApi(HttpConfiguration);
        }

        /// <summary>
        /// Configures the authentication.
        /// </summary>
        /// <param name="app">The application.</param>
        private void ConfigureOAuth(IAppBuilder app)
        {
            // Configure the authentication HERE.
            // Token Consumption
            // Use OAuthBearer Authentication or other
        }
    }
}