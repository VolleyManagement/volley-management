namespace VolleyManagement.WebApi
{
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Defines RouteConfig
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="routes">Route collection</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("odata/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{*anything}",
                defaults: new { controller = "Home", action = "Index" });
        }
    }
}