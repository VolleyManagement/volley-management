namespace VolleyManagement.UI
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// The volley management application.
    /// </summary>
    public class VolleyManagementApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// The application start.
        /// </summary>
        [SuppressMessage("StyleCopPlus.StyleCopPlusRules", "SP0100:AdvancedNamingRules",
            Justification = "Sergii Diachenko: This is specific naming convention.")]
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
