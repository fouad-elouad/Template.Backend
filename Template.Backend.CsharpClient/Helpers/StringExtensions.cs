using System.Collections.Specialized;
using System.Web;

namespace Template.Backend.CsharpClient.Helpers
{
    public static class StringExtensions
    {
        public static string AddQuery(this string str, string name, string value)
        {
            NameValueCollection httpValueCollection = HttpUtility.ParseQueryString(str);
            httpValueCollection.Remove(name);
            httpValueCollection.Add(name, value);
            return httpValueCollection.ToString();
        }
    }
}
