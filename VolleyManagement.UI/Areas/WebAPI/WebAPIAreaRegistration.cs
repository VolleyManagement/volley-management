namespace VolleyManagement.UI.Areas.WebApi
{
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.OData.Builder;
    using System.Web.OData.Extensions;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.ContributorsTeam;
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
        /// Registers OData routing
        /// </summary>
        /// <param name="config">The config.</param>
        public static void RegisterOData(HttpConfiguration config)
        {
            var builder = new ODataConventionModelBuilder();
            builder.EnableLowerCamelCase();

            builder.EntitySet<TournamentViewModel>("Tournaments");
            builder.EntitySet<PlayerViewModel>("Players");
            builder.EntitySet<ContributorsTeamViewModel>("ContributorsTeam");

            config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
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
        }
    }
}