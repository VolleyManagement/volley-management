namespace VolleyManagement.WebApi
{
    using System.Web.Http;

    /// <summary>
    /// Defines WebAPIConfig
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Register configuration
        /// </summary>
        /// <param name="config">Http configuration</param>
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }
    }
}
