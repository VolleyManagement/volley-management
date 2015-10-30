namespace VolleyManagement.UI.Areas.WebApi.Infrastructure
{
    using Microsoft.OData.Core;
    using Microsoft.OData.Core.UriParser;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Http.Routing;
    using System.Web.Http;
    using System.Web.OData.Extensions;
    using System.Web.OData.Routing;

    /// <summary>
    /// A class that minimizes the effort incurred in the design, implementation of functionality in our application 
    /// </summary>
    public static class WebApiHelpers
    {
        public static TKey GetKeyFromUri<TKey>(HttpRequestMessage request, Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            var urlHelper = request.GetUrlHelper() ?? new UrlHelper(request);

            string serviceRoot = urlHelper.CreateODataLink(
                request.ODataProperties().RouteName,
                request.ODataProperties().PathHandler, new List<ODataPathSegment>());
            var odataPath = request.ODataProperties().PathHandler.Parse(
                request.ODataProperties().Model,
                serviceRoot, uri.LocalPath);

            var keySegment = odataPath.Segments.OfType<KeyValuePathSegment>().FirstOrDefault();
            if (keySegment == null)
            {
                throw new InvalidOperationException("The link does not contain a key.");
            }

            var value = ODataUriUtils.ConvertFromUriLiteral(keySegment.Value, ODataVersion.V4);
            return (TKey)value;
        }

        /// <summary>
        /// Returns the SingleResult of the passed object.
        /// </summary>
        /// <param name="obj">Instance that should be represented as the SingleResult.</param>
        /// <returns></returns>
        public static SingleResult<T> ObjectToSingleResult<T>(T obj)
        {
            return SingleResult.Create(new[] { obj }.AsQueryable());
        }
    }
}