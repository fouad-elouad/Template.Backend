using System.Net;
using System.Text.Json;
using Template.Backend.CsharpClient.Helpers;
using Template.Backend.Model.Exceptions;

namespace Template.Backend.CsharpClient.Exceptions
{
    /// <summary>
    /// Throw the apropriate exception from the Http response
    /// </summary>
    /// <seealso cref="MarketDataConnector.Model.BusinessLogic.Exceptions.BusinessException" />
    [Serializable]
    public class ApiResponseToException : BusinessException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponseToException"/> class.
        /// And Throw exception
        /// </summary>
        /// <param name="responseMessage">The response message.</param>
        /// <exception cref="IdNotFoundException">Id not Found</exception>
        /// <exception cref="NoElementFoundException">No element found</exception>
        /// <exception cref="CanNotBeDeletedException">Can't be deleted, This item is still referenced from one or many entities</exception>
        /// <exception cref="BusinessException">Unknown error occurred while processing your request.</exception>
        public ApiResponseToException(HttpResponseMessage responseMessage)
        {
            if (responseMessage.ReasonPhrase == nameof(IdNotFoundException))
            {
                string exceptionMessage = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                throw new IdNotFoundException(exceptionMessage);
            }

            else if (responseMessage.ReasonPhrase == nameof(BadRequestException))
            {
                throw new BadRequestException(responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult());
            }

            else if (responseMessage.ReasonPhrase == nameof(NoElementFoundException))
            {
                throw new NoElementFoundException("No element found");
            }

            else if (responseMessage.ReasonPhrase == nameof(TaskCanceledBusinessException))
            {
                throw new TaskCanceledBusinessException("Operation has been canceled");
            }

            else if (responseMessage.ReasonPhrase == nameof(ApiKeyNotFoundException))
            {
                throw new ApiKeyNotFoundException(responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult());
            }

            else if (responseMessage.ReasonPhrase == nameof(ApiServerException))
            {
                throw new ApiServerException(responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult());
            }

            else if (responseMessage.ReasonPhrase == nameof(ConfigFileNotFoundException))
            {
                throw new ConfigFileNotFoundException(responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult());
            }

            else if (responseMessage.ReasonPhrase == nameof(DateTimeFormatException))
            {
                throw new DateTimeFormatException(responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult());
            }

            else if (responseMessage.ReasonPhrase == nameof(CanNotBeDeletedException))
            {
                try
                {
                    string exceptionMessage = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    Dictionary<string, string> dicMessage = JsonSerializer.Deserialize<Dictionary<string, string>>(exceptionMessage, ClientHelper.globalJsonSerializerOptions);
                    if (dicMessage.ContainsKey("Message"))
                        throw new CanNotBeDeletedException(dicMessage["Message"]);
                }
                catch (CanNotBeDeletedException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw new CanNotBeDeletedException("Can't be deleted, This item is still referenced from one or many entities");
                }
            }

            else if (responseMessage.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedException("Unauthorized"); ;
            }

            else if (responseMessage.ReasonPhrase == nameof(BusinessException))
            {
                throw new BusinessException(responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult());
            }

            throw new BusinessException("Unknown error occurred while processing your request.");
        }
    }
}