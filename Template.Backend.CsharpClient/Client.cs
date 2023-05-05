using Template.Backend.CsharpClient.Exceptions;
using Template.Backend.CsharpClient.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using Template.Backend.Model.Exceptions;

namespace Template.Backend.CsharpClient
{
    /// <summary>
    /// Client base Class
    /// Provides a base class for sending serialised objects HTTP requests and receiving objects HTTP responses
    /// from a resource identified by a URI.
    /// </summary>
    /// <typeparam name="Entity">The type of the entity.</typeparam>
    public abstract class Client<Entity, AuditEntity> where Entity : class where AuditEntity : class
    {
        /// <summary>
        /// The HTTP client
        /// </summary>
        internal protected static readonly HttpClient httpClient;

        /// <summary>
        /// The media type
        /// </summary>
        protected const string _mediaType = "application/json";

        /// <summary>
        /// The maximum depth for Json serialisation and deserialization
        /// </summary>
        protected const int _listDepth = 2;

        /// <summary>
        /// The date format
        /// </summary>
        protected const string _dateTimeFormat = "yyyyMMddTHHmmss";
        protected const string _dateFormat = "yyyyMMdd";

        /// <summary>
        /// Initializes a new instance of the <see cref="Client{Entity}"/> class.
        /// </summary>
        public Client()
        {
        }

        /// <summary>
        /// Initializes the <see cref="Client{Entity}"/> class.
        /// </summary>
        static Client()
        {
            httpClient = new HttpClient { BaseAddress = new Uri(ApiConfiguration.BaseAddress) };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
        }

        /// <summary>
        /// Send a PUT request to the specified Uri
        /// </summary>
        /// <param name="address">The Uri the request is sent to.</param>
        /// <param name="values">The HTTP request content sent to the server.
        /// Object values to update.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// response of type string
        /// </returns>
        /// <exception cref="ModelStateException"></exception>
        /// <exception cref="ApiResponseToException"></exception>
        /// <exception cref="ApiServerException">Impossible de se connecter au serveur Api distant
        /// or
        /// Problème Reseaux ou serveur Api distant</exception>
        /// <exception cref="System.Exception"></exception>
        internal void Update(string address, string values, AuthenticationHeaderValue authHeaderValue = null)
        {
            UpdateAsync(address, values, authHeaderValue).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send a PUT request to the specified Uri
        /// </summary>
        /// <param name="address">The Uri the request is sent to.</param>
        /// <param name="values">The HTTP request content sent to the server.
        /// Object values to update.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// response of type string
        /// </returns>
        /// <exception cref="ModelStateException"></exception>
        /// <exception cref="ApiResponseToException"></exception>
        /// <exception cref="ApiServerException">Impossible de se connecter au serveur Api distant
        /// or
        /// Problème Reseaux ou serveur Api distant</exception>
        /// <exception cref="System.Exception"></exception>
        internal async Task UpdateAsync(string address, string values, AuthenticationHeaderValue authHeaderValue = null)
        {
            try
            {
                HttpContent httpContent = new StringContent(values, Encoding.UTF8, _mediaType);
                HttpResponseMessage response = await httpClient.PutAsyncExtension(address, httpContent,
                             x => x.Headers.Authorization = authHeaderValue).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.ReasonPhrase == nameof(ModelStateException))
                    {
                        throw new ModelStateException(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                    }

                    throw new ApiResponseToException(response);
                }
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null && ex.InnerException.GetType() == typeof(WebException))
                {
                    throw new ApiServerException(Constants.ExceptionMessage_UnableToConnect);
                }
                throw new ApiServerException(Constants.ExceptionMessage_NetworkProblem);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Restores An object with existing one in Audit.
        /// </summary>
        /// <param name="address">The Uri the request is sent to.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// response of type string
        /// </returns>
        /// <exception cref="ApiResponseToException"></exception>
        /// <exception cref="ApiServerException">Impossible de se connecter au serveur Api distant
        /// or
        /// Problème Reseaux ou serveur Api distant</exception>
        /// <exception cref="System.Exception"></exception>
        internal void Restore(string address, AuthenticationHeaderValue authHeaderValue = null)
        {
            RestoreAsync(address, authHeaderValue).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Restores An object with existing one in Audit.
        /// </summary>
        /// <param name="address">The Uri the request is sent to.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// response of type string
        /// </returns>
        /// <exception cref="ApiResponseToException"></exception>
        /// <exception cref="ApiServerException">Impossible de se connecter au serveur Api distant
        /// or
        /// Problème Reseaux ou serveur Api distant</exception>
        /// <exception cref="System.Exception"></exception>
        internal async Task RestoreAsync(string address, AuthenticationHeaderValue authHeaderValue = null)
        {
            try
            {
                HttpContent httpContent = new StringContent("");
                HttpResponseMessage response = await httpClient.PutAsyncExtension(address, httpContent,
                        x => x.Headers.Authorization = authHeaderValue).ConfigureAwait(false); ;

                if (!response.IsSuccessStatusCode)
                {
                    throw new ApiResponseToException(response);
                }
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null && ex.InnerException.GetType() == typeof(WebException))
                {
                    throw new ApiServerException(Constants.ExceptionMessage_UnableToConnect);
                }
                throw new ApiServerException(Constants.ExceptionMessage_NetworkProblem);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Send a POST request to the specified Uri with the specified values.
        /// </summary>
        /// <param name="address">The Uri the request is sent to.</param>
        /// <param name="values">The HTTP request content sent to the server.
        /// object values to add.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// response of type string
        /// </returns>
        /// <exception cref="ModelStateException"></exception>
        /// <exception cref="ApiResponseToException"></exception>
        /// <exception cref="ApiServerException">Impossible de se connecter au serveur Api distant
        /// or
        /// Problème Reseaux ou serveur Api distant</exception>
        /// <exception cref="System.Exception"></exception>
        internal Entity Add(string address, string values, AuthenticationHeaderValue authHeaderValue = null)
        {
            return AddAsync(address, values, authHeaderValue).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send a POST request to the specified Uri with the specified values.
        /// </summary>
        /// <param name="address">The Uri the request is sent to.</param>
        /// <param name="values">The HTTP request content sent to the server.
        /// object values to add.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// response of type string
        /// </returns>
        /// <exception cref="ModelStateException"></exception>
        /// <exception cref="ApiResponseToException"></exception>
        /// <exception cref="ApiServerException">Impossible de se connecter au serveur Api distant
        /// or
        /// Problème Reseaux ou serveur Api distant</exception>
        /// <exception cref="System.Exception"></exception>
        internal async Task<Entity> AddAsync(string address, string values, AuthenticationHeaderValue authHeaderValue = null)
        {
            try
            {
                HttpContent httpContent = new StringContent(values, Encoding.UTF8, _mediaType);
                HttpResponseMessage response = await httpClient.PostAsyncExtension(address, httpContent,
                        x => x.Headers.Authorization = authHeaderValue).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.ReasonPhrase == nameof(ModelStateException))
                    {
                        throw new ModelStateException(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                    }

                    throw new ApiResponseToException(response);
                }
                string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<Entity>(json);
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null && ex.InnerException.GetType() == typeof(WebException))
                {
                    throw new ApiServerException(Constants.ExceptionMessage_UnableToConnect);
                }
                throw new ApiServerException(Constants.ExceptionMessage_NetworkProblem);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Send a DELETE request to the specified Uri
        /// </summary>
        /// <param name="address">The Uri the request is sent to.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// response of type string
        /// </returns>
        /// <exception cref="ApiResponseToException"></exception>
        /// <exception cref="ApiServerException">Impossible de se connecter au serveur Api distant
        /// or
        /// Problème Reseaux ou serveur Api distant</exception>
        /// <exception cref="System.Exception"></exception>
        internal void Delete(string address, AuthenticationHeaderValue authHeaderValue = null)
        {
            DeleteAsync(address, authHeaderValue).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send a DELETE request to the specified Uri
        /// </summary>
        /// <param name="address">The Uri the request is sent to.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// response of type string
        /// </returns>
        /// <exception cref="ApiResponseToException"></exception>
        /// <exception cref="ApiServerException">Impossible de se connecter au serveur Api distant
        /// or
        /// Problème Reseaux ou serveur Api distant</exception>
        /// <exception cref="System.Exception"></exception>
        internal async Task DeleteAsync(string address, AuthenticationHeaderValue authHeaderValue = null)
        {
            try
            {
                HttpResponseMessage response = await httpClient.DeleteAsyncExtension(address,
                            x => x.Headers.Authorization = authHeaderValue).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                    throw new ApiResponseToException(response);
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null && ex.InnerException.GetType() == typeof(WebException))
                {
                    throw new ApiServerException(Constants.ExceptionMessage_UnableToConnect);
                }
                throw new ApiServerException(Constants.ExceptionMessage_NetworkProblem);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Gets Entity object.
        /// </summary>
        /// <param name="address">The Uri the request is sent to.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// Entity object
        /// </returns>
        /// <exception cref="ApiResponseToException"></exception>
        /// <exception cref="ApiServerException">Impossible de se connecter au serveur Api distant
        /// or
        /// Problème Reseaux ou serveur Api distant</exception>
        /// <exception cref="System.Exception"></exception>
        internal Entity GetAsObject(string address, AuthenticationHeaderValue authHeaderValue = null)
        {
            return GetAsObjectAsync(address, authHeaderValue).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Gets Entity object async.
        /// </summary>
        /// <param name="address">The Uri the request is sent to.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// Entity object
        /// </returns>
        /// <exception cref="ApiResponseToException"></exception>
        /// <exception cref="ApiServerException">Impossible de se connecter au serveur Api distant
        /// or
        /// Problème Reseaux ou serveur Api distant</exception>
        /// <exception cref="System.Exception"></exception>
        internal async Task<Entity> GetAsObjectAsync(string address, AuthenticationHeaderValue authHeaderValue = null)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsyncExtension(address,
                           x => x.Headers.Authorization = authHeaderValue).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<Entity>(json);
                }

                throw new ApiResponseToException(response);
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null && ex.InnerException.GetType() == typeof(WebException))
                {
                    throw new ApiServerException(Constants.ExceptionMessage_UnableToConnect);
                }
                throw new ApiServerException(Constants.ExceptionMessage_NetworkProblem);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Gets an audit list for the parameters specified in address.
        /// </summary>
        /// <param name="address">The Uri the request is sent to.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// List of Audit entities
        /// </returns>
        /// <exception cref="ApiResponseToException"></exception>
        /// <exception cref="ApiServerException">Impossible de se connecter au serveur Api distant
        /// or
        /// Problème Reseaux ou serveur Api distant</exception>
        /// <exception cref="System.Exception"></exception>
        internal IEnumerable<AuditEntity> GetAuditObjects(string address, AuthenticationHeaderValue authHeaderValue = null)
        {
            return GetAuditObjectsAsync(address, authHeaderValue).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Gets an audit list for the parameters specified in address.
        /// </summary>
        /// <param name="address">The Uri the request is sent to.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// List of Audit entities
        /// </returns>
        /// <exception cref="ApiResponseToException"></exception>
        /// <exception cref="ApiServerException">Impossible de se connecter au serveur Api distant
        /// or
        /// Problème Reseaux ou serveur Api distant</exception>
        /// <exception cref="System.Exception"></exception>
        internal async Task<IEnumerable<AuditEntity>> GetAuditObjectsAsync(string address, AuthenticationHeaderValue authHeaderValue = null)
        {
            return await GetObjectsAsync<AuditEntity>(address, authHeaderValue).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets list of Entities.
        /// </summary>
        /// <param name="address">The Uri the request is sent to.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// list of Entities
        /// </returns>
        /// <exception cref="ApiResponseToException"></exception>
        /// <exception cref="ApiServerException">Impossible de se connecter au serveur Api distant
        /// or
        /// Problème Reseaux ou serveur Api distant</exception>
        /// <exception cref="System.Exception"></exception>
        internal IEnumerable<Entity> GetObjects(string address, AuthenticationHeaderValue authHeaderValue = null)
        {
            return GetObjectsAsync(address, authHeaderValue).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Gets list of Entities.
        /// </summary>
        /// <param name="address">The Uri the request is sent to.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// list of Entities
        /// </returns>
        /// <exception cref="ApiResponseToException"></exception>
        /// <exception cref="ApiServerException">Impossible de se connecter au serveur Api distant
        /// or
        /// Problème Reseaux ou serveur Api distant</exception>
        /// <exception cref="System.Exception"></exception>
        internal async Task<IEnumerable<Entity>> GetObjectsAsync(string address, AuthenticationHeaderValue authHeaderValue = null)
        {
            return await GetObjectsAsync<Entity>(address, authHeaderValue).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets list of Entities.
        /// </summary>
        /// <param name="address">The Uri the request is sent to.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// list of Entities
        /// </returns>
        /// <exception cref="ApiResponseToException"></exception>
        /// <exception cref="ApiServerException">Impossible de se connecter au serveur Api distant
        /// or
        /// Problème Reseaux ou serveur Api distant</exception>
        /// <exception cref="System.Exception"></exception>
        internal async Task<IEnumerable<T>> GetObjectsAsync<T>(string address, AuthenticationHeaderValue authHeaderValue = null) where T : class
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsyncExtension(address,
                            x => x.Headers.Authorization = authHeaderValue).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<IEnumerable<T>>(json);
                }
                throw new ApiResponseToException(response);
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null && ex.InnerException.GetType() == typeof(WebException))
                {
                    throw new ApiServerException(Constants.ExceptionMessage_UnableToConnect);
                }
                throw new ApiServerException(Constants.ExceptionMessage_NetworkProblem);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Gets the audit entity.
        /// </summary>
        /// <param name="address">The Uri the request is sent to.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// Audit Entity object
        /// </returns>
        /// <exception cref="ApiResponseToException"></exception>
        /// <exception cref="ApiServerException">Impossible de se connecter au serveur Api distant
        /// or
        /// Problème Reseaux ou serveur Api distant</exception>
        /// <exception cref="System.Exception"></exception>
        internal AuditEntity GetAudit(string address, AuthenticationHeaderValue authHeaderValue = null)
        {
            return GetAuditAsync(address, authHeaderValue).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Gets the audit entity.
        /// </summary>
        /// <param name="address">The Uri the request is sent to.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// Audit Entity object
        /// </returns>
        /// <exception cref="ApiResponseToException"></exception>
        /// <exception cref="ApiServerException">Impossible de se connecter au serveur Api distant
        /// or
        /// Problème Reseaux ou serveur Api distant</exception>
        /// <exception cref="System.Exception"></exception>
        internal async Task<AuditEntity> GetAuditAsync(string address, AuthenticationHeaderValue authHeaderValue = null)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsyncExtension(address,
                            x => x.Headers.Authorization = authHeaderValue).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<AuditEntity>(json);
                }
                throw new ApiResponseToException(response);
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null && ex.InnerException.GetType() == typeof(WebException))
                {
                    throw new ApiServerException(Constants.ExceptionMessage_UnableToConnect);
                }
                throw new ApiServerException(Constants.ExceptionMessage_NetworkProblem);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Send a POST request to the specified Uri with the specified values.
        /// </summary>
        /// <param name="address">The Uri the request is sent to.</param>
        /// <param name="values">The HTTP request content sent to the server.
        /// object values to add.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// response of type string
        /// </returns>
        /// <exception cref="ApiResponseToException"></exception>
        /// <exception cref="ApiServerException">Impossible de se connecter au serveur Api distant
        /// or
        /// Problème Reseaux ou serveur Api distant</exception>
        /// <exception cref="System.Exception"></exception>
        internal IEnumerable<Entity> PostList(string address, string values, AuthenticationHeaderValue authHeaderValue = null)
        {
            return PostListAsync(address, values, authHeaderValue).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send a POST request to the specified Uri with the specified values.
        /// </summary>
        /// <param name="address">The Uri the request is sent to.</param>
        /// <param name="values">The HTTP request content sent to the server.
        /// object values to add.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// response of type string
        /// </returns>
        /// <exception cref="ApiResponseToException"></exception>
        /// <exception cref="ApiServerException">Impossible de se connecter au serveur Api distant
        /// or
        /// Problème Reseaux ou serveur Api distant</exception>
        /// <exception cref="System.Exception"></exception>
        internal async Task<IEnumerable<Entity>> PostListAsync(string address, string values, AuthenticationHeaderValue authHeaderValue = null)
        {
            try
            {
                HttpContent httpContent = new StringContent(values, Encoding.UTF8, _mediaType);

                HttpResponseMessage response = await httpClient.PostAsyncExtension(address, httpContent,
                            x => x.Headers.Authorization = authHeaderValue).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new ApiResponseToException(response);
                }
                string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<IEnumerable<Entity>>(json);
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null && ex.InnerException.GetType() == typeof(WebException))
                {
                    throw new ApiServerException(Constants.ExceptionMessage_UnableToConnect);
                }
                throw new ApiServerException(Constants.ExceptionMessage_NetworkProblem);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Serializes an object with maximum depth.
        /// its ignore looping by default
        /// maxDepth = -1 ignore depth and preserve looping with Object reference
        /// </summary>
        /// <param name="obj">The object to Serialize.</param>
        /// <param name="depth">The maximum level to achieve for navigation properties serialization.</param>
        /// <returns>
        /// Json representation of serialized object
        /// </returns>
        public string ToJson(object obj, int depth = 1)
        {
            return ClientHelper.SerializeObjectDepth(obj, depth);
        }

        /// <summary>
        /// Adds the value of bearer authentication header for HTTP request.
        /// </summary>
        /// <param name="AuthHeaderValue">The authentication header value.</param>
        public void AddAuthenticationHeader(AuthenticationHeaderValue AuthHeaderValue)
        {
            httpClient.DefaultRequestHeaders.Authorization = AuthHeaderValue;
        }
    }
}