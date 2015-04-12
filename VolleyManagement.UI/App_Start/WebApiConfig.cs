namespace VolleyManagement.UI
{
    using System.Web.Http;
    using System.Web.OData.Builder;
    using System.Web.OData.Extensions;

    using VolleyManagement.UI.Areas.WebApi.ViewModels.Players;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Tournaments;

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

            // Convention-based routing.
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            RegisterOData(config);
        }

        private static void RegisterOData(HttpConfiguration config)
        {
            var builder = new ODataConventionModelBuilder();
            builder.EnableLowerCamelCase();

            builder.EntitySet<TournamentViewModel>("Tournaments");
            builder.EntitySet<PlayerViewModel>("Players");

            config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
        }
    }
}
