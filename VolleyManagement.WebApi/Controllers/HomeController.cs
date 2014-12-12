namespace VolleyManagement.WebApi.Controllers
{
    using System.Web.Mvc;

    /// <summary>
    /// Defines HomeController which responsible for returning Index for all routes
    /// except routes for OData endpoint
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Action returns Index view
        /// </summary>
        /// <returns>Index view</returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}
