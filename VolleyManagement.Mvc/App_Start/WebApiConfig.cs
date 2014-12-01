namespace SoftServe.VolleyManagement.Mvc
{
    using System.Web.Http;

    /// <summary>
    /// Web application programming interface configuration class
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// register configuration
        /// </summary>
        /// <param name="config">http configuration</param>
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }
    }
}