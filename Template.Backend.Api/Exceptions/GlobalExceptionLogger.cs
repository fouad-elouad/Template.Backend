using NLog;
using System.Web.Http.ExceptionHandling;

namespace Template.Backend.Api.Exceptions
{
    public class GlobalExceptionLogger : ExceptionLogger
    {
        private Logger _logger = LogManager.GetCurrentClassLogger();

        public override void Log(ExceptionLoggerContext context)
        {
            
            _logger.Error($" {context.Request.RequestUri} |  {context.ExceptionContext.Exception} ");
        }
    }
}