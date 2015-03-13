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
                "Mvc/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "VolleyManagement.UI.Areas.Mvc.Controllers" });
        }
    }
}