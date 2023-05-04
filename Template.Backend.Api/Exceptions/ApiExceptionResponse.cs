using Template.Backend.Model.Exceptions;
using System;
using System.Net;
using System.Net.Http;

namespace Template.Backend.Api.Exceptions
{
    public static class ApiExceptionResponse
    {
        /// <summary>
        /// build an Http response exception from system Exception
        /// with appropriate reason Phrase
        /// </summary>
        /// <param name="exception">The input exception.</param>
        /// <param name="request">The http request.</param>
        /// <returns>Http Error response</returns>
        public static HttpResponseMessage Throw(Exception exception, HttpRequestMessage request)
        {
            return BuildErrorResponse(exception, request);
        }

        public static HttpResponseMessage BuildErrorResponse(Exception exception, HttpRequestMessage request)
        {
            string message = "Unknown error";
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
            string reasonPhrase = "UnknownException";

            if (exception is IdNotFoundException)
            {
                message = exception.Message;
                httpStatusCode = HttpStatusCode.NotFound;
                reasonPhrase = exception.GetType().Name;
            }

            else if (exception is NoElementFoundException)
            {
                message = exception.Message;
                httpStatusCode = HttpStatusCode.NotFound;
                reasonPhrase = exception.GetType().Name;
            }

            else if (exception is CanNotBeDeletedException)
            {
                message = exception.Message;
                httpStatusCode = HttpStatusCode.InternalServerError;
                reasonPhrase = exception.GetType().Name;
            }

            else if (exception is DateTimeFormatException)
            {
                message = exception.Message;
                httpStatusCode = HttpStatusCode.BadRequest;
                reasonPhrase = exception.GetType().Name;
            }

            else if (exception is BadRequestException)
            {
                message = exception.Message;
                httpStatusCode = HttpStatusCode.BadRequest;
                reasonPhrase = exception.GetType().Name;
            }

            else if (exception is BusinessException)
            {
                message = exception.Message;
                httpStatusCode = HttpStatusCode.InternalServerError;
                reasonPhrase = exception.GetType().Name;
            }

            HttpResponseMessage response = request.CreateErrorResponse(httpStatusCode, message);
            response.ReasonPhrase = reasonPhrase;
            return response;
        }
    }
}