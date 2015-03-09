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
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }
    }
}
