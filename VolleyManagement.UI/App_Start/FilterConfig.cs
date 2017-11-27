namespace VolleyManagement.UI
{
    using System.Web.Mvc;
    using VolleyManagement.UI.Infrastructure;

    /// <summary>
    /// Filter configuration for ASP.NET application
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// Register application filters
        /// </summary>
        /// <param name="filters">Global filter collection</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new VolleyExceptionFilterAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
