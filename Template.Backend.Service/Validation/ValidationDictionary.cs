using System.Collections.ObjectModel;


namespace Template.Backend.Service.Validation
{
    /// <summary>
    /// ValidationDictionary class
    /// </summary>
    /// <seealso cref="IValidationDictionary" />
    public class ValidationDictionary : IValidationDictionary
    {
        private readonly Dictionary<string, IList<string>> ErrorsDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationDictionary"/> class.
        /// </summary>
        public ValidationDictionary()
        {
            ErrorsDictionary = new Dictionary<string, IList<string>>();
        }

        /// <summary>
        /// test if the validation dictionary in valid
        /// </summary>
        /// <returns>
        ///   <c>true</c> if No validation error found; otherwise, <c>false</c>.
        /// </returns>
        public bool IsValid()
        {
            return !ErrorsDictionary.Any();
        }

        /// <summary>
        /// Adds the error to Dictionary.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="errorValues">The error values.</param>
        public void AddError(string key, string errorMessage)
        {
            if (ErrorsDictionary.ContainsKey(key))
            {
                IList<string> list = ErrorsDictionary[key];
                list.Add(errorMessage);
            }
            else
            {
                // New key
                IList<string> list = new List<string>
                {
                    errorMessage
                };
                ErrorsDictionary.Add(key, list);
            }
        }

        /// <summary>
        /// Get Errors Dictionary from ValidationDictionary class
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, IList<string>> ToDictionary()
        {
            return ErrorsDictionary;
        }

        public IReadOnlyDictionary<string, IReadOnlyList<string>> ToReadOnlyDictionary()
        {
            return new ReadOnlyDictionary<string, IReadOnlyList<string>>
                (ErrorsDictionary.ToDictionary(k => k.Key, v => (IReadOnlyList<string>)v.Value.ToList().AsReadOnly()));
        }
    }
}