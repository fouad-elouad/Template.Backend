using System.Net.Http.Headers;

namespace Template.Backend.CsharpClient
{
    /// <summary>
    /// Client base interface
    /// Provides a base class for sending serialised objects HTTP requests and receiving objects HTTP responses
    /// from a resource identified by a URI.
    /// </summary>
    /// <typeparam name="Entity">The type of the Base Entity</typeparam>
    public interface IClient<Entity, AuditEntity> where Entity : class where AuditEntity : class
    {
        /// <summary>
        /// Adds the value of bearer authentication header for HTTP request.
        /// </summary>
        /// <param name="AuthHeaderValue">The authentication header value.</param>
        void AddAuthenticationHeader(AuthenticationHeaderValue AuthHeaderValue);
    }
}