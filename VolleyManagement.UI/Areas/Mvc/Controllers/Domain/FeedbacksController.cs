namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System;
    using System.Web.Mvc;

    using Contracts;
    using Contracts.Authorization;
    using Domain.UsersAggregate;
    using ViewModels.FeedbackViewModel;
    
    /// <summary>
    /// Defines feedback controller.
    /// </summary>
    public class FeedbacksController : Controller
    {
        /// <summary>
        /// User Id for anonym role.
        /// </summary>
        private const int ANONYM = -1;

        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IFeedbackService _feedbackService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbacksController"/> class.
        /// </summary>
        /// <param name="feedbackService">Instance of the class
        /// that implements IFeedbackService interface.</param>
        /// <param name="userService">Instance of the class
        /// that implements IUserService interface.</param>
        /// <param name="currentUserService">Instance of the class
        /// that implements ICurrentUserService interface.</param>
        public FeedbacksController(
            IFeedbackService feedbackService,
            IUserService userService,
            ICurrentUserService currentUserService)
        {
           this._feedbackService = feedbackService;
            this._userService = userService;
            this._currentUserService = currentUserService;
        }

        /// <summary>
        /// Create feedback action GET.
        /// </summary>
        /// <returns>Feedback creation view.</returns>
        public ActionResult Create()
        {
            var feedbackViewModel = new FeedbackViewModel
            {
                UsersEmail = GetUserMail()
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
            try
            {
                if (ModelState.IsValid)
                {
                    var domainFeedback = feedbackViewModel.ToDomain();
                    this._feedbackService.Create(domainFeedback);
                    return View("FeedbackSentMessage");
                }
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("ValidationMessage", ex.Message);
            }

            return View("Create", feedbackViewModel);
        }

        /// <summary>
        /// Gets current user mail.
        /// </summary>
        /// <returns>User email.</returns>
        private string GetUserMail()
        {
            int userId = this._currentUserService.GetCurrentUserId();
            User currentUser = new User
            {
                Email = string.Empty
            };

            if (userId != ANONYM)
            {
                currentUser = this._userService.GetUser(userId);
            }

            return currentUser.Email;
        }
    }
}