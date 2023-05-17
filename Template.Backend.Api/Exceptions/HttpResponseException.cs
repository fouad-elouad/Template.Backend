
namespace Template.Backend.Api.Exceptions
{
    public class HttpResponseException
    {
        public HttpResponseException(string exceptionName, string title) =>
            (ExceptionName, Title) = (exceptionName, title);

        public HttpResponseException(string exceptionName, string title, object? errors) =>
            (ExceptionName, Title, Errors) = (exceptionName, title, errors);

        public HttpResponseException(string exceptionName, string title, int? status) =>
            (ExceptionName, Title, Status) = (exceptionName, title, status);

        public HttpResponseException(string exceptionName, string title, int? status, object? errors) =>
            (ExceptionName, Title, Status, Errors) = (exceptionName, title, status, errors);

        public string ExceptionName { get; }
        public string Title { get; }
        public int? Status { get; }
        public object? Errors { get; }
    }
}
