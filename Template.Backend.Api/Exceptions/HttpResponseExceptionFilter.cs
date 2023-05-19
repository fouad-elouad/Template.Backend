using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Template.Backend.Model.Exceptions;

namespace Template.Backend.Api.Exceptions
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        readonly ILogger<HttpResponseExceptionFilter> _logger;

        public HttpResponseExceptionFilter(ILogger<HttpResponseExceptionFilter> logger)
        {
            _logger = logger;
        }

        public int Order => int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Exception occured
            if (context.Exception != null)
            {
                if (context.Exception is BusinessException businessException)
                {
                    Type type = context.Exception.GetType();

                    if (type == typeof(IdNotFoundException) || type == typeof(NoElementFoundException))
                    {
                        context.Result = new NotFoundObjectResult(
                            new HttpResponseException(type.Name, businessException.Message, StatusCodes.Status404NotFound));
                        _logger.LogWarning(businessException.Message);
                    }

                    else if (type == typeof(CanNotBeDeletedException) || type == typeof(DateTimeFormatException) || type == typeof(BadRequestException))
                    {
                        context.Result = new BadRequestObjectResult(
                            new HttpResponseException(type.Name, businessException.Message, StatusCodes.Status400BadRequest));
                        _logger.LogError(businessException.ToString());
                    }

                    else if (context.Exception is ModelStateException modelStateException)
                    {
                        context.Result = new BadRequestObjectResult(
                            new HttpResponseException(type.Name, businessException.Message, StatusCodes.Status400BadRequest, modelStateException.Errors));
                        _logger.LogError(businessException.ToString());
                    }

                    else if (type == typeof(TaskCanceledBusinessException))
                    {
                        context.Result = new ObjectResult(
                            new HttpResponseException(type.Name, businessException.Message, StatusCodes.Status500InternalServerError))
                        {
                            StatusCode = StatusCodes.Status500InternalServerError
                        };
                        _logger.LogError(businessException.ToString());
                    }

                    else
                    {
                        context.Result = new ObjectResult(
                            new HttpResponseException(type.Name, businessException.Message, StatusCodes.Status500InternalServerError))
                        {
                            StatusCode = StatusCodes.Status500InternalServerError
                        };
                        _logger.LogError(businessException.ToString());
                    }
                }
                else
                {
                    context.Result = new ObjectResult(
                        new HttpResponseException(nameof(UnknownException), "Unknown Error, Contact Administrator", StatusCodes.Status500InternalServerError))
                    {
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                    _logger.LogCritical(context.Exception?.ToString());
                }
                context.ExceptionHandled = true;
            }

            // return action result
        }
    }
}
