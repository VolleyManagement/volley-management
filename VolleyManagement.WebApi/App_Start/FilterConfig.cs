namespace VolleyManagement.WebApi
{
    using System.Web.Mvc;

    /// <summary>
    /// Defines FilterConfig
    /// </summary>
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}