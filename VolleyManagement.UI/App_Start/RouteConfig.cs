namespace VolleyManagement.UI
{
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// The route config.
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// The register routes.
        /// </summary>
        /// <param name="routes"> The routes collection. </param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
            name: "UpcomingPlayers",                               // Route name
            url: "Mvc/Players/Index/{id}",                           // URL with params
            defaults: new { controller = "Players", action = "Index"}, // Param defaults
            namespaces: new[] { "VolleyManagement.UI.Areas.Mvc.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "VolleyManagement.UI.Controllers" });
        }
    }
}
