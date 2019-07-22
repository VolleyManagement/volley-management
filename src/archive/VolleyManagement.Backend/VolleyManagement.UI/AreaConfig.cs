namespace VolleyManagement.UI
{
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Configure areas
    /// </summary>
    public static class AreaConfig
    {
        /// <summary>
        /// Manual register area
        /// </summary>
        /// <param name="areaRegistration">Area registration from Areas folder</param>
        public static void RegisterArea(AreaRegistration areaRegistration)
        {
            var areaContext = new AreaRegistrationContext(areaRegistration.AreaName, RouteTable.Routes);
            areaRegistration.RegisterArea(areaContext);
        }

        /// <summary>
        /// Register all areas in Areas folder.
        /// ORDER IS IMPORTANT!!!
        /// </summary>
        public static void RegisterAllAreas()
        {
            RegisterArea(new Areas.Admin.AdminAreaRegistration());
            RegisterArea(new Areas.Mvc.MvcAreaRegistration());
            RegisterArea(new Areas.WebApi.WebApiAreaRegistration());
        }
    }
}