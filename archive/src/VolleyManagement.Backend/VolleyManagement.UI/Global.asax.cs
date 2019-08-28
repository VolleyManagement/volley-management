using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Integration.WebApi;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using VolleyManagement.Crosscutting.IOC;
using VolleyManagement.Data.MsSql.Infrastructure;
using VolleyManagement.Services.Infrastructure;
using VolleyManagement.UI.Helpers;
using VolleyManagement.UI.Infrastructure;

namespace VolleyManagement.UI
{

#pragma warning disable SA1649 // File name must match first type name
#pragma warning disable S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)
    public class VolleyManagementApplication : System.Web.HttpApplication
#pragma warning restore S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)
#pragma warning restore SA1649 // File name must match first type name
    {
        /// <summary>
        /// Fix problem Entity framework
        /// </summary>
        public static void FixEfProviderServicesProblem()
        {
            ////The Entity Framework provider type 'System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer'
            ////for the 'System.Data.SqlClient' ADO.NET provider could not be loaded.
            ////Make sure the provider assembly is available to the running application.
            ////See http://go.microsoft.com/fwlink/?LinkId=260882 for more information.

#pragma warning disable S1481 // Unused local variables should be removed
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
#pragma warning restore S1481 // Unused local variables should be removed
        }

        /// <summary>
        /// The application start.
        /// </summary>
        [SuppressMessage(
            "StyleCopPlus.StyleCopPlusRules",
            "SP0100:AdvancedNamingRules",
            Justification = "Sergii Diachenko: This is specific naming convention.")]
#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
        protected void Application_Start()
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterIgnoreRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AreaConfig.RegisterAllAreas();
            ModelBinders.Binders.Add(typeof(DateTime), new DateTimeModelBinder());
            ModelBinders.Binders.Add(typeof(DateTime?), new DateTimeModelBinder());

            ConfigureIoc();
        }

        private static void ConfigureIoc()
        {
            var ioc = new SimpleInjectorContainer();

            // Need to explicitly specify default scope
            ioc.InternalContainer.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            ioc
                .Register(new IocDataAccessModule())
                .Register(new IocServicesModule())
                .Register(new IocUiModule());

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(ioc.InternalContainer));

            // Need for Simple Injector invoking on Web Api controller constructor call
            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(ioc.InternalContainer);
        }
    }
}
