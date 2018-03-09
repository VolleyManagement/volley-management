namespace VolleyManagement.UI.Areas.WebApi.Infrastructure
{
    using System;
    using System.Net.Http;
    using System.Web.Http.Routing;

    /// <summary>
    /// A class that minimizes the effort incurred in the design, implementation of functionality in our application
    /// </summary>
    public static class WebApiHelpers
    {
        /// <summary>
        /// Gets a key (id) from the Uri
        /// </summary>
        /// <typeparam name="TKey">Type of the key</typeparam>
        /// <param name="request">Http request</param>
        /// <param name="uri">Uri where the key should be found</param>
        /// <returns>Found key</returns>
        public static TKey GetKeyFromUri<TKey>(HttpRequestMessage request, Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            throw new NotSupportedException();
        }
    }
}