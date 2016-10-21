namespace VolleyManagement.UI
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Areas.Admin;
    using Areas.Mvc;
    using VolleyManagement.UI.Helpers;
    
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
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AreaConfig.RegisterAreas();
            ModelBinders.Binders.Add(typeof(DateTime), new DateTimeModelBinder());
            ModelBinders.Binders.Add(typeof(DateTime?), new DateTimeModelBinder());
        }

        /// <summary>
        /// Configure areas
        /// </summary>
        public static class AreaConfig
        {
            /// <summary>
            /// Register all areas
            /// </summary>
            public static void RegisterAreas()
            {
                var adminArea = new AdminAreaRegistration();
                var adminAreaContext = new AreaRegistrationContext(adminArea.AreaName, RouteTable.Routes);
                adminArea.RegisterArea(adminAreaContext);
                var defaultArea = new MvcAreaRegistration();
                var defaultAreaContext = new AreaRegistrationContext(defaultArea.AreaName, RouteTable.Routes);
                defaultArea.RegisterArea(defaultAreaContext);
            }
        }
    }
}
