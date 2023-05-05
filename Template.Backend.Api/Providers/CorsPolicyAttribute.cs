using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http.Cors;
using Template.Backend.Api.Helpers;

namespace Template.Backend.Api.Providers
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class CorsPolicyAttribute : Attribute, ICorsPolicyProvider
    {
        private CorsPolicy _policy;

        public CorsPolicyAttribute()
        {
            _policy = new CorsPolicy
            {
                AllowAnyMethod = true,
                AllowAnyHeader = true,
                AllowAnyOrigin = true
            };

            //Custom headers may not send back to client because of cors policies restrictions : we Add exception to it
            _policy.ExposedHeaders.Add(Constants.CustomHeader_Total_Count_Found);
            _policy.ExposedHeaders.Add(Constants.CustomHeader_Total_Count_Returned);
        }

        public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_policy);
        }
    }
}