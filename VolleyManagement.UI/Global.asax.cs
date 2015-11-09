namespace VolleyManagement.UI
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    /// <summary>
    /// The volley management application.
    /// </summary>
    public class VolleyManagementApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Fix problem Entity framework
        /// </summary>
        public void FixEfProviderServicesProblem()
        {
            ////The Entity Framework provider type 'System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer'
            ////for the 'System.Data.SqlClient' ADO.NET provider could not be loaded. 
            ////Make sure the provider assembly is available to the running application. 
            ////See http://go.microsoft.com/fwlink/?LinkId=260882 for more information.

            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

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
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
