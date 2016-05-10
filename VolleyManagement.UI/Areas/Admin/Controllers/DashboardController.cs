namespace VolleyManagement.UI.Areas.Admin.Controllers
{
    using System.Web.Mvc;

    /// <summary>
    /// Main view in Admin part of Volley Management web-site
    /// </summary>
    //[Authorize]
    public class DashboardController : Controller
    {
        /// <summary>
        /// Returns default view for Admin dashboard
        /// </summary>
        /// <returns>Action result</returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}