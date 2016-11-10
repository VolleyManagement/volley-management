namespace VolleyManagement.UI.Areas.Admin.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Contracts;
    using Models;

    /// <summary>
    /// Provides Request Manager
    /// </summary>
    public class RequestController : Controller
    {
        private readonly IRequestService _requestService;
        private readonly IUserService _userService;
        private readonly IPlayerService _playerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestController"/> class.
        /// </summary>
        /// <param name="requestService"> The request service.</param>
        /// <param name="userService"> The user service.</param>
        /// <param name="playerService"> The player service.</param>
        public RequestController(IRequestService requestService, IUserService userService, IPlayerService playerService)
        {
            _playerService = playerService;
            _requestService = requestService;
            _userService = userService;
        }

        /// <summary>
        /// Index view
        /// </summary>
        /// <returns>Action result</returns>
        public ActionResult Index()
        {
            var requests = new RequestCollectionViewModel()
            {
                Requests = _requestService.Get()
                .ToList()
                .Select(r => RequestViewModel.Map(r, _playerService.Get(r.PlayerId), _userService.GetUser(r.UserId)))
            };

            return View(requests);
        }

        /// <summary>
        /// User details
        /// </summary>
        /// <param name="id"> User id</param>
        /// <returns> The <see cref="ActionResult"/>.</returns>
        public ActionResult UserDetails(int id)
        {
            var user = _userService.GetUser(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        /// <summary>
        /// Player details
        /// </summary>
        /// <param name="id"> Player id</param>
        /// <returns> The <see cref="ActionResult"/>.</returns>
        public ActionResult PlayerDetails(int id)
        {
            var player = _playerService.Get(id);

            if (player == null)
            {
                return HttpNotFound();
            }

            return View(player);
        }

        /// <summary>
        /// Request decline
        /// </summary>
        /// <param name="id"> Request id</param>
        /// <returns> The <see cref="ActionResult"/>.</returns>
        public ActionResult Decline(int id)
        {
            try
            {
                _requestService.Decline(id);
            }
            catch (MissingEntityException)
            {
                return View("InvalidOperation");
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Request approve
        /// </summary>
        /// <param name="id"> Request id</param>
        /// <returns> The <see cref="ActionResult"/>.</returns>
        public ActionResult Confirm(int id)
        {
            try
            {
                _requestService.Approve(id);
            }
            catch (MissingEntityException)
            {
                return View("InvalidOperation");
            }

            return RedirectToAction("Index");
        }
    }
}