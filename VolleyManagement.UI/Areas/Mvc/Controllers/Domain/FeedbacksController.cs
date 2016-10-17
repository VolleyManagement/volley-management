namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System.Web.Mvc;
    using ViewModels.FeedbackViewModel;

    /// <summary>
    /// Defines feedback controller.
    /// </summary>
    public class FeedbacksController : Controller
    {
        /// <summary>
        /// Create feedback action GET.
        /// </summary>
        /// <returns>Feedback creation view</returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create feedback action POST.
        /// </summary>
        /// <param name="feedbackViewModel">Feedback view model</param>
        /// <returns>Feedback creation view</returns>
        [HttpPost]
        public ActionResult Create(FeedbackViewModel feedbackViewModel)
        {
            return View();
        }
    }
}