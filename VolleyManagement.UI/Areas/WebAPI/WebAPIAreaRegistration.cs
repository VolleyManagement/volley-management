namespace VolleyManagement.UI.Areas.WebApi
{
    using System.Web.Http;
    using System.Web.Http.OData.Builder;
    using System.Web.Mvc;

    using VolleyManagement.UI.Areas.WebApi.ViewModels.Players;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Tournaments;

    /// <summary>
    /// The WebApi area registration.
    /// </summary>
    public class WebApiAreaRegistration : AreaRegistration
    {
        /// <summary>
        /// Gets the area name.
        /// </summary>
        public override string AreaName
        {
            get
            {
                return "WebApi";
            }
        }

        /// <summary>
        /// The register area.
        /// </summary>
        /// <param name="context"> The context. </param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "WebApi_default",
                "WebApi/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "VolleyManagement.UI.Areas.WebApi.Controllers" });

            RegisterOData(GlobalConfiguration.Configuration);
        }

        private void RegisterOData(HttpConfiguration config)
        {
            var builder = new ODataConventionModelBuilder();

            builder.EntitySet<TournamentViewModel>("Tournaments");
            builder.EntitySet<PlayerViewModel>("Players");

            config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
        }
    }
}