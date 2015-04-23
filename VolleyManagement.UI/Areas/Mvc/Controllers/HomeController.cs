namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System.Web.Mvc;

    /// <summary>
    /// The home controller.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// The index action.
        /// </summary>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}