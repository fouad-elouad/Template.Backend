using System.Text.Json;
using System.Text.Json.Serialization;

namespace Template.Backend.CsharpClient.Helpers
{
    /// <summary>
    /// ClientHelper class group all most used static methods in Client Assemble
    /// </summary>
    public static class ClientHelper
    {
        public static JsonSerializerOptions globalJsonSerializerOptions
        {
            get
            {
                JsonSerializerOptions options = new(JsonSerializerOptions.Default) { PropertyNameCaseInsensitive = true };
                options.Converters.Add(new JsonStringEnumConverter());
                return options;
            }
        }
    }
}
