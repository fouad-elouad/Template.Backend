using Newtonsoft.Json;
using System.IO;


namespace Template.Backend.Api.Areas.HelpPage.Helpers
{
    public class HelpPageJsonTextWriter : JsonTextWriter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HelpPageJsonTextWriter"/> class.
        /// </summary>
        /// <param name="textWriter">The <see cref="T:System.IO.TextWriter" /> to write to.</param>
        public HelpPageJsonTextWriter(TextWriter textWriter) : base(textWriter) { }

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