namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using Contracts;
    using Contracts.Authentication;
    using Contracts.Authentication.Models;
    using Contracts.Authorization;
    using ViewModels.FeedbackViewModel;
    using ViewModels.Users;

    /// <summary>
    /// Defines feedback controller.
    /// </summary>
    public class FeedbacksController : Controller
    {
        /// <summary>
        /// User Id for anonym role.
        /// </summary>
        private const int ANONYM = -1;

        /// <summary>
        /// Holds CurrentUserService instance.
        /// </summary>
        private readonly ICurrentUserService _userService;

        /// <summary>
        /// Holds VolleyUserManager instance.
        /// </summary>
        private readonly IVolleyUserStore _userStore;

        /// <summary>
        /// Holds FeedbackService instance.
        /// </summary>
        private readonly IFeedbackService _feedbackService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbacksController"/> class.
        /// </summary>
        /// <param name="feedbackService">Instance of the class
        /// that implements <see cref="IFeedbackService"/></param>
        /// <param name="userStore">Instance of the class
        /// that implements <see cref="IVolleyUserStore"/></param>
        /// <param name="userService">Instance of the class
        /// that implements <see cref="ICurrentUserService"/></param>
        public FeedbacksController(
            IFeedbackService feedbackService,
            IVolleyUserStore userStore,
            ICurrentUserService userService)
        {
            this._feedbackService = feedbackService;
            this._userStore = userStore;
            this._userService = userService;
        }

        /// <summary>
        /// Create feedback action GET.
        /// </summary>
        /// <returns>Feedback creation view.</returns>
        public ActionResult Create()
        {
            var feedbackViewModel = new FeedbackViewModel();
            int currentUserId = this._userService.GetCurrentUserId();

            if (currentUserId != ANONYM)
            {
                feedbackViewModel.UsersEmail = GetUsersEmailById(currentUserId);
            }

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

        /// <summary>
        /// Returns authenticated user email.
        /// </summary>
        /// <param name="currentUserId">Authenticated user Id.</param>
        /// <returns>Authenticated user email.</returns>
        private string GetUsersEmailById(int currentUserId)
        {
            var userTask =
                    Task.Run(() => this._userStore.FindByIdAsync(currentUserId));
            UserModel user = userTask.Result;
            UserViewModel userViewModel = UserViewModel.Map(user);
            return userViewModel.Email;
        }
    }
}