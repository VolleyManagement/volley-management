namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System;
    using System.Web.Configuration;
    using System.Web.Mvc;
    using Contracts;
    using Contracts.Authorization;
    using Domain.UsersAggregate;
    using ViewModels.FeedbackViewModel;
    using System.Threading.Tasks;

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
        private ICaptchaManager _captchaManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbacksController"/> class.
        /// </summary>
        /// <param name="feedbackService">Instance of the class
        /// that implements IFeedbackService interface.</param>
        /// <param name="userService">Instance of the class
        /// that implements IUserService interface.</param>
        /// <param name="currentUserService">Instance of the class
        /// that implements ICurrentUserService interface.</param>
        /// <param name="captchaManager">Instance of the class
        /// that implements ICaptchaManager interface.</param>
        public FeedbacksController(
            IFeedbackService feedbackService,
            IUserService userService,
            ICurrentUserService currentUserService,
            ICaptchaManager captchaManager)
        {
            _feedbackService = feedbackService;
            _userService = userService;
            _currentUserService = currentUserService;
            _captchaManager = captchaManager;
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
            GetDataSiteKey(feedbackViewModel);
            return View("Create", feedbackViewModel);
        }

        /// <summary>
        /// Create feedback action POST.
        /// </summary>
        /// <param name="feedbackViewModel">Feedback view model.</param>
        /// <returns>Feedback creation view.</returns>
        [HttpPost]
#pragma warning disable S4261 // Methods should be named according to their synchronicities
        public async Task<JsonResult> Create(FeedbackViewModel feedbackViewModel)
#pragma warning restore S4261 // Methods should be named according to their synchronicities
        {
            FeedbackMessageViewModel result = new FeedbackMessageViewModel
            {
                ResultMessage = Resources.UI.TournamentController.CheckCaptcha,
                OperationSuccessful = false
            };

            try
            {
                var isCaptchaValid = await _captchaManager.ValidateUserCaptchaAsync(feedbackViewModel.CaptchaResponse);
                if (isCaptchaValid)
                {
                    if (ModelState.IsValid)
                    {
                        var domainFeedback = feedbackViewModel.ToDomain();
                        _feedbackService.Create(domainFeedback);
                        result.ResultMessage = Resources.UI.TournamentController.SuccessfulSent;
                        result.OperationSuccessful = true;
                    }
                    else
                    {
                        result.ResultMessage = Resources.UI.TournamentController.CheckData;
                    }
                }
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("ValidationMessage", ex.Message);
            }

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// Gets current user mail.
        /// </summary>
        /// <returns>User email.</returns>
        private string GetUserMail()
        {
            int userId = _currentUserService.GetCurrentUserId();
            User currentUser = new User
            {
                Email = string.Empty
            };

            if (userId != ANONYM)
            {
                currentUser = _userService.GetUser(userId);
            }

            return currentUser.Email;
        }

        private void GetDataSiteKey(FeedbackViewModel feedbackViewModel)
        {
            const string SECRET_KEY = "RecaptchaSiteKey";
            string secretKey = WebConfigurationManager.AppSettings[SECRET_KEY];
            feedbackViewModel.ReCapthaKey = secretKey;
        }
    }
}