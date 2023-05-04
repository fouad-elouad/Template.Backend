using System.Collections.Generic;

namespace Template.Backend.Service.Validation
{
    /// <summary>
    /// ValidationDictionary interface
    /// </summary>
    public interface IValidationDictionary
    {
        /// <summary>
        /// Adds the error to Dictionary.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="errorValues">The error values.</param>
        void AddError(string key, string errorMessage);


        /// <summary>
        /// Get Errors Dictionary from ValidationDictionary class
        /// </summary>
        /// <returns></returns>
        Dictionary<string, IList<string>> ToDictionary();


        /// <summary>
        /// test if the validation dictionary in valid
        /// </summary>
        /// <returns>
        ///   <c>true</c> if No validation error found; otherwise, <c>false</c>.
        /// </returns>
        bool IsValid();
    }
}
