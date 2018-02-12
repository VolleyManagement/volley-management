namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System.Web.Mvc;

    /// <summary>
    /// Represents error controller.
    /// </summary>
    public class ErrorController : Controller
    {
        /// <summary>
        /// Handles 404 or Not Found error.
        /// </summary>
        /// <returns>View with error details.</returns>
        public ActionResult PageNotFound()
        {
            return View();
        }
    }
}
