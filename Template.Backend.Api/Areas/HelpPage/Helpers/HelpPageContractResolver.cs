using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Reflection;

namespace Template.Backend.Api.Areas.HelpPage.Helpers
{
    public class HelpPageContractResolver : DefaultContractResolver
    {
        /// <summary>
        /// boolean expression to determine included properties
        /// </summary>
        private readonly Func<bool> _includeProperty;

        /// <summary>
        /// Initializes a new instance of the HelpPageContractResolver class.
        /// </summary>
        /// <param name="includeProperty">boolean expression to determine included properties</param>
        public HelpPageContractResolver(Func<bool> includeProperty)
        {
            _includeProperty = includeProperty;
        }

        /// <summary>
        /// Creates a Newtonsoft.Json.Serialization.JsonProperty for the given System.Reflection.MemberInfo.
        /// </summary>
        /// <param name="member">The member to create a Newtonsoft.Json.Serialization.JsonProperty for.</param>
        /// <param name="memberSerialization">The member's parent Newtonsoft.Json.MemberSerialization.</param>
        /// <returns>A created Newtonsoft.Json.Serialization.JsonProperty for the given System.Reflection.MemberInfo.</returns>
        protected override JsonProperty CreateProperty(
            MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            var shouldSerialize = property.ShouldSerialize;

            property.ShouldSerialize = obj => _includeProperty() &&
                                              (shouldSerialize == null ||
                                               shouldSerialize(obj));

            return property;
        }
    }
}