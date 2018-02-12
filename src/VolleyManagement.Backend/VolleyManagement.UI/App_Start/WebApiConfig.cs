namespace VolleyManagement.UI
{
    using System.Web.Http;

    /// <summary>
    /// The WebApi config.
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// The registration of WebApi configuration
        /// </summary>
        /// <param name="config"> The config. </param>
        public static void Register(HttpConfiguration config)
        {
            // Attribute routing.
            config.MapHttpAttributeRoutes();
        }
    }
}
