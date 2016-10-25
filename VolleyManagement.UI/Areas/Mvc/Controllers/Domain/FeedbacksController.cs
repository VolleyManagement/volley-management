namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System;
    using System.Web.Mvc;

    using Contracts;
    using Domain.UsersAggregate;
    using ViewModels.FeedbackViewModel;

    /// <summary>
    /// Defines feedback controller.
    /// </summary>
    public class FeedbacksController : Controller
    {
        /// <summary>
        /// Holds UserService instance.
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Holds FeedbackService instance.
        /// </summary>
        private readonly IFeedbackService _feedbackService;

       /// <summary>
       /// Initializes a new instance of the <see cref="FeedbacksController"/> class.
       /// </summary>
       /// <param name="feedbackService">Instance of the class
       /// that implements <see cref="IFeedbackService"/></param>
       /// <param name="userService">Instance of the class
       /// that implements <see cref="IUserService"/></param>
        public FeedbacksController(
            IFeedbackService feedbackService,
            IUserService userService)
        {
           this._feedbackService = feedbackService;
            this._userService = userService;
        }

        /// <summary>
        /// Create feedback action GET.
        /// </summary>
        /// <returns>Feedback creation view.</returns>
        public ActionResult Create()
        {
            var feedbackViewModel = new FeedbackViewModel
            {
                UsersEmail = GetCurrentUserMail()
            };

            return View("Create", feedbackViewModel);
        }

        /// <summary>
        /// Create feedback action POST.
        /// </summary>
        /// <param name="feedbackViewModel">Feedback view model.</param>
        /// <returns>Feedback creation view.</returns>
        [HttpPost]
        public ActionResult Create(FeedbackViewModel feedbackViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", feedbackViewModel);
            }

            try
            {
                var domainFeedback = feedbackViewModel.ToDomain();
                this._feedbackService.Create(domainFeedback);
                return View("FeedbackSentMessage");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("ValidationMessage", ex.Message);
                return View("Create", feedbackViewModel);
            }
        }

        private string GetCurrentUserMail()
        {
            User user = this._userService.GetCurrentUserInstance();
            return user.Email;
        }
    }
}