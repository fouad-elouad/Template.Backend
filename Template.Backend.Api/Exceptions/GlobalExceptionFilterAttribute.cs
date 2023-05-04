using System.Web.Http.Filters;


namespace Template.Backend.Api.Exceptions
{
    public class GlobalExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            context.Response = ApiExceptionResponse.BuildErrorResponse(context.Exception, context.Request);
        }
    }
}