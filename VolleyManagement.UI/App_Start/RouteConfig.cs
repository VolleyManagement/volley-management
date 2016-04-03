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
        /// Registers all routes needed to be Ignored.
        /// It should go before any route mappings(e.g. before RegisterAllAreas)
        /// </summary>
        /// <param name="routes"> The routes collection. </param>
        public static void RegisterIgnoreRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
        }

        /// <summary>
        /// The register routes.
        /// </summary>
        /// <param name="routes"> The routes collection. </param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "VolleyManagement.UI.Controllers" });
        }
    }
}
