using Newtonsoft.Json;
using System;
using System.IO;

namespace Template.Backend.CsharpClient.Helpers
{
    /// <summary>
    /// ClientHelper class group all most used static methods in Client Assemble
    /// </summary>
    public static class ClientHelper
    {
        /// <summary>
        /// Serializes an object with maximum depth.
        /// its ignore looping by default
        /// maxDepth = -1 ignore depth and preserve looping with Object reference
        /// </summary>
        /// <param name="obj">The object to Serialize.</param>
        /// <param name="maxDepth">The maximum level to achieve for navigation properties serialization.</param>
        /// <returns>Json representation of serialized object</returns>
        public static string SerializeObjectDepth(object obj, int maxDepth)
        {
            ReferenceLoopHandling referenceLoopHandling = ReferenceLoopHandling.Ignore;

            if (maxDepth == -1)
            {
                var serializerSettings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = referenceLoopHandling,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects

                };

                return JsonConvert.SerializeObject(obj, serializerSettings);
            }

            using (var strWriter = new StringWriter())
            {
                using (var jsonWriter = new CustomJsonTextWriter(strWriter))
                {
                    Func<bool> include = () => jsonWriter.CurrentDepth <= maxDepth;
                    var resolver = new CustomContractResolver(include);
                    var serializer = new JsonSerializer
                    {
                        ContractResolver = resolver,
                        ReferenceLoopHandling = referenceLoopHandling
                    };
                    serializer.Serialize(jsonWriter, obj);
                }
                return strWriter.ToString();
            }
        }
    }
}
