namespace VolleyManagement.UI.Areas.Mvc
{
    using System.Web.Mvc;

    /// <summary>
    /// The MVC area registration.
    /// </summary>
    public class MvcAreaRegistration : AreaRegistration
    {
        /// <summary>
        /// Gets the area name.
        /// </summary>
        public override string AreaName
        {
            get
            {
                return "Mvc";
            }
        }

        /// <summary>
        /// The register area.
        /// </summary>
        /// <param name="context"> The context. </param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Mvc_default",
                "{controller}/{action}/{id}",
                new { action = "Index", controller = "Tournaments", id = UrlParameter.Optional },
                new[] { "VolleyManagement.UI.Areas.Mvc.Controllers" });

            context.MapRoute(
                "Mvc_default_site",
                "site/{controller}/{action}/{id}",
                new { action = "Index", controller = "Tournaments", id = UrlParameter.Optional },
                new[] { "VolleyManagement.UI.Areas.Mvc.Controllers" });
        }
    }
}