using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;

namespace Template.Backend.Api.Areas.HelpPage.Helpers
{
    public static class Helper
    {

        /// <summary>
        /// Serializes an object with maximum depth.
        /// its ignore looping by default
        /// maxDepth = -1 ignore depth and preserve looping with Object reference
        /// </summary>
        /// <param name="obj">The object to Serialize.</param>
        /// <param name="maxDepth">The maximum level to achieve for navigation properties serialization.</param>
        /// <returns>Json representation of serialized object</returns>
        public static string SerializeDocumentationObjectDepth(object obj, int maxDepth)
        {
            try
            {
                ReferenceLoopHandling referenceLoopHandling = ReferenceLoopHandling.Ignore;

                // preserve looping with Object reference
                if (maxDepth == -1)
                {
                    var serializerSettings = new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = referenceLoopHandling,
                        PreserveReferencesHandling = PreserveReferencesHandling.Objects

                    };
                    serializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    return JsonConvert.SerializeObject(obj, serializerSettings);
                }

                using (var strWriter = new StringWriter())
                {
                    using (var jsonWriter = new HelpPageJsonTextWriter(strWriter))
                    {
                        Func<bool> include = () => jsonWriter.CurrentDepth <= maxDepth;
                        DefaultContractResolver resolver = null;
                        resolver = new HelpPageContractResolver(include);

                        var serializer = new JsonSerializer
                        {
                            ContractResolver = resolver,
                            ReferenceLoopHandling = referenceLoopHandling
                        };
                        serializer.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                        serializer.Serialize(jsonWriter, obj);
                    }
                    return strWriter.ToString();
                }
            }

            catch (Exception)
            {
                throw;
            }
        }
    }
}