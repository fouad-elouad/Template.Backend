using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;


namespace Template.Backend.Api.Controllers
{
    [ApiController]
    public class ErrorApiController : ControllerBase
    {

        private readonly ILogger<ErrorApiController> _logger;

        public ErrorApiController(ILogger<ErrorApiController> logger)
        {
            _logger = logger;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("/error")]
        public IActionResult HandleError() => Problem();

        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("/error-development")]
        public IActionResult HandleErrorDevelopment([FromServices] IHostEnvironment hostEnvironment)
        {
            var exceptionHandlerFeature =
                HttpContext.Features.Get<IExceptionHandlerFeature>()!;
            if (exceptionHandlerFeature != null)
            {
                _logger.LogError("exception occured {exception}", exceptionHandlerFeature?.Error?.ToString());
            }

            if (!hostEnvironment.IsDevelopment())
            {
                return NotFound();
            }

            return Problem(
                detail: exceptionHandlerFeature?.Error?.StackTrace,
                title: exceptionHandlerFeature?.Error?.Message);
        }
    }
}