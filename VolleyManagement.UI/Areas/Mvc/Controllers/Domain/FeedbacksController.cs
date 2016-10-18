namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using Contracts;
    using Contracts.Authentication;
    using Contracts.Authentication.Models;
    using Microsoft.AspNet.Identity;
    using ViewModels.FeedbackViewModel;
    using ViewModels.Users;

    /// <summary>
    /// Defines feedback controller.
    /// </summary>
    public class FeedbacksController : Controller
    {
        /// <summary>
        /// Holds VolleyUserManager instance.
        /// </summary>
        private readonly IVolleyUserManager<UserModel> _userManager;

        /// <summary>
        /// Holds FeedbackService instance.
        /// </summary>
        private readonly IFeedbackService _feedbackService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbacksController"/> class.
        /// </summary>
        /// <param name="feedbackService">Instance of the class
        /// that implements <see cref="IFeedbackService"/></param>
        /// <param name="userManager">Instance of the class
        /// that implements <see cref="IVolleyUserManager{T}"/></param>
        public FeedbacksController(
            IFeedbackService feedbackService, IVolleyUserManager<UserModel> userManager)
        {
            this._feedbackService = feedbackService;
            this._userManager = userManager;
        }

        /// <summary>
        /// Create feedback action GET.
        /// </summary>
        /// <returns>Feedback creation view.</returns>
        public async Task<ActionResult> Create()
        {
            var feedbackViewModel = new FeedbackViewModel();

            if (System.Web.HttpContext.Current.User != null
                && System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                int currentUserId = int.Parse(System.Web.HttpContext.Current.User.Identity.GetUserId());
                UserModel user = await this._userManager.FindByIdAsync(currentUserId);
                UserViewModel userViewModel = UserViewModel.Map(user);
                feedbackViewModel.UsersEmail = userViewModel.Email;
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
    }
}