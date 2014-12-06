namespace VolleyManagement.Mvc
{
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Defines FilterConfig
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// Register global filters
        /// </summary>
        /// <param name="filters">Global filter collection</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}