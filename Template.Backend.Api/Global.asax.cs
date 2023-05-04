using System.Web.Http;

namespace Template.Backend.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Let OWIN STARTUP handle Setup to avoid
            // GlobalConfiguration.Configuration initialization problems

            GlobalConfiguration.Configuration.IncludeErrorDetailPolicy =
            IncludeErrorDetailPolicy.Always;
        }
    }
}
