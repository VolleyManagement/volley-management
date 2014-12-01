namespace SoftServe.VolleyManagement.Mvc
{
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// filter configuration class
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// register global filters
        /// </summary>
        /// <param name="filters">global filter collection</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}