namespace VolleyManagement.UI.Areas.Admin.Controllers
{
    using System.Web.Mvc;
    using Contracts;
    using Contracts.Authorization;
    using Domain.RolesAggregate;
    using Models;

    /// <summary>
    /// Provides Feedback management
    /// </summary>
    [Authorize]
    public class RequestsController : Controller
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IAuthorizationService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestsController"/> class.
        /// </summary>
        /// <param name="feedbackService"> The feedbacks service. </param>
        /// <param name="authService">Authorization service</param>
        public RequestsController(
            IFeedbackService feedbackService,
                IAuthorizationService authService)
        {
            this._feedbackService = feedbackService;
            this._authService = authService;
        }

        /// <summary>
        /// Index view
        /// </summary>
        /// <returns>Action result</returns>
        public ActionResult Index()
        {
            this._authService.CheckAccess(AuthOperations.AdminDashboard.View);
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
            this._authService.CheckAccess(AuthOperations.AdminDashboard.View);
            var feedback = _feedbackService.GetDetails(id);
            return View(feedback);
        }

        /// <summary>
        /// Feedback reply.
        /// </summary>
        /// <param name="id"> Feedback Id. </param>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        public ActionResult Reply(int id)
        {
            this._authService.CheckAccess(AuthOperations.AdminDashboard.View);
            _feedbackService.Reply(id);
            return RedirectToAction("Reply");
        }

        /// <summary>
        /// Feedback Close.
        /// </summary>
        /// <param name="id"> Feedback Id. </param>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        public ActionResult Close(int id)
        {
            this._authService.CheckAccess(AuthOperations.AdminDashboard.View);
            _feedbackService.Close(id);
            //// var feedbacks = _feedbackService.Get().ConvertAll(f => new RequestsViewModel(f));
            //// return View("Index", feedbacks);
            return RedirectToAction("Index");
        }
    }
}