namespace VolleyManagement.UI.Areas.Admin.Controllers
{
    using System.Web.Mvc;
    using Contracts;
    using Models;

    /// <summary>
    /// Provides Feedback management
    /// </summary>
    [Authorize]
    public class RequestsController : Controller
    {
        private readonly IFeedbackService _feedbackService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestsController"/> class.
        /// </summary>
        /// <param name="feedbackService"> The feedbacks service. </param>
        public RequestsController(IFeedbackService feedbackService)
        {
            this._feedbackService = feedbackService;
        }

        /// <summary>
        /// Index view
        /// </summary>
        /// <returns>Action result</returns>
        public ActionResult Index()
        {
            var feedbacks = _feedbackService.Get().ConvertAll(f => new RequestsViewModel(f));
            return View(feedbacks);
        }

        /// <summary>
        /// Feedback details.
        /// </summary>
        /// <param name="id"> Feedback Id. </param>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        public ActionResult Details(int id)
        {
            var feedback = new RequestsViewModel(_feedbackService.GetDetails(id));
            return View(feedback);
        }

        /// <summary>
        /// Feedback reply.
        /// </summary>
        /// <param name="id"> Feedback Id. </param>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        public ActionResult Reply(int id)
        {
            _feedbackService.Reply(id);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Feedback Close.
        /// </summary>
        /// <param name="id"> Feedback Id. </param>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        public ActionResult Close(int id)
        {
            _feedbackService.Close(id);
            return RedirectToAction("Index");
        }
    }
}