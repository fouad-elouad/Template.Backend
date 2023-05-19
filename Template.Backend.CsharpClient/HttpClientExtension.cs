
namespace Template.Backend.CsharpClient
{
    /// <summary>
    /// System.Net.Http.HttpClient extension methods
    /// </summary>
    public static class HttpClientExtension
    {
        /// <summary>
        /// Send a GET request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="uri">The Uri the request is sent to.</param>
        /// <param name="preAction">The HttpRequestMessage pre action.</param>
        /// <returns>task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> GetAsyncExtension
                    (this HttpClient httpClient, string uri, Action<HttpRequestMessage> preAction)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            preAction(httpRequestMessage);
            return httpClient.SendAsync(httpRequestMessage);
        }

        /// <summary>
        /// Send a POST request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="uri">The Uri the request is sent to.</param>
        /// <param name="httpContent">The HTTP request content sent to the server.</param>
        /// <param name="preAction">The HttpRequestMessage pre action.</param>
        /// <returns>task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PostAsyncExtension
                    (this HttpClient httpClient, string uri, HttpContent httpContent, Action<HttpRequestMessage> preAction)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = httpContent
            };
            preAction(httpRequestMessage);
            return httpClient.SendAsync(httpRequestMessage);
        }

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="uri">The Uri the request is sent to.</param>
        /// <param name="httpContent">The HTTP request content sent to the server.</param>
        /// <param name="preAction">The HttpRequestMessage pre action.</param>
        /// <returns>task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PutAsyncExtension
                    (this HttpClient httpClient, string uri, HttpContent httpContent, Action<HttpRequestMessage> preAction)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, uri)
            {
                Content = httpContent
            };
            preAction(httpRequestMessage);
            return httpClient.SendAsync(httpRequestMessage);
        }

        /// <summary>
        /// Send a DELETE request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="uri">The Uri the request is sent to.</param>
        /// <param name="preAction">The HttpRequestMessage pre action.</param>
        /// <returns>task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> DeleteAsyncExtension
                    (this HttpClient httpClient, string uri, Action<HttpRequestMessage> preAction)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);
            preAction(httpRequestMessage);
            return httpClient.SendAsync(httpRequestMessage);
        }
    }
}