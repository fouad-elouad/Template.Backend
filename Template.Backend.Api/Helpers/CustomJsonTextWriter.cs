using Newtonsoft.Json;
using System.IO;

namespace Template.Backend.Api.Helpers
{
    /// <summary>
    /// CustomJsonTextWriter class
    /// </summary>
    public class CustomJsonTextWriter : JsonTextWriter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomJsonTextWriter"/> class.
        /// </summary>
        /// <param name="textWriter">The <see cref="T:System.IO.TextWriter" /> to write to.</param>
        public CustomJsonTextWriter(TextWriter textWriter) : base(textWriter) { }

        /// <summary>
        /// Gets the current depth.
        /// </summary>
        /// <value>
        /// The current depth.
        /// </value>
        public int CurrentDepth { get; private set; }

        /// <summary>
        /// Writes the beginning of a JSON object.
        /// </summary>
        public override void WriteStartObject()
        {
            CurrentDepth++;
            base.WriteStartObject();
        }

        /// <summary>
        /// Writes the end of a JSON object.
        /// </summary>
        public override void WriteEndObject()
        {
            CurrentDepth--;
            base.WriteEndObject();
        }
    }
}