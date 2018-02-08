namespace VolleyManagement.UI.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Contracts;
    using Contracts.Exceptions;
    using Models;
    using Mvc.ViewModels.Teams;
    using Mvc.ViewModels.Tournaments;

    /// <summary>
    /// Provides management for tournament's requests
    /// </summary>
    public class TournamentRequestController : Controller
    {
        private readonly ITournamentRequestService _requestService;
        private readonly IUserService _userService;
        private readonly ITeamService _teamService;
        private readonly ITournamentService _tournamentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentRequestController"/> class.
        /// </summary>
        /// <param name="requestService"> The request service.</param>
        /// <param name="userService"> The user service.</param>
        /// <param name="teamService">The team service.</param>
        /// <param name="tournamentService">The tournament service.</param>
        public TournamentRequestController(
            ITournamentRequestService requestService,
            IUserService userService,
            ITeamService teamService,
            ITournamentService tournamentService)
        {
            _teamService = teamService;
            _requestService = requestService;
            _userService = userService;
            _tournamentService = tournamentService;
        }

        /// <summary>
        /// Get list of all tournament requests
        /// </summary>
        /// <returns>Action result</returns>
        public ActionResult Index()
        {
            var requests = new TournamentRequestCollectionViewModel()
            {
                 Requests = _requestService.Get().Select(r => TournamentRequestViewModel.Map(
                     r,
                     _teamService.Get(r.TeamId),
                     _userService.GetUserDetails(r.UserId),
                     _tournamentService.GetTournamentByGroup(r.GroupId)))
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
            var user = _userService.GetUserDetails(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var result = UserViewModel.Map(user);
            return View(result);
        }

        /// <summary>
        /// Team details
        /// </summary>
        /// <param name="id"> Team id</param>
        /// <returns> The <see cref="ActionResult"/>.</returns>
        public ActionResult TeamDetails(int id)
        {
            var team = _teamService.Get(id);

            if (team == null)
            {
                return HttpNotFound();
            }

            var viewModel = TeamViewModel.Map(team, _teamService.GetTeamCaptain(team), _teamService.GetTeamRoster(id));
            return View(viewModel);
        }

        /// <summary>
        /// Tournament's details
        /// </summary>
        /// <param name="id"> Tournament id</param>
        /// <returns> The <see cref="ActionResult"/>.</returns>
        public ActionResult TournamentDetails(int id)
        {
            var tournament = _tournamentService.Get(id);

            if (tournament == null)
            {
                return HttpNotFound();
            }

            var tournamentViewModel = TournamentViewModel.Map(tournament);
            return View(tournamentViewModel);
        }

        /// <summary>
        /// Request decline action GET.
        /// </summary>
        /// <param name="id"> Request id</param>
        /// <returns> The <see cref="ActionResult"/>.</returns>
        public ActionResult Decline(int id)
        {
            var messageViewModel = new MessageViewModel
            {
                Id = id
            };
            return View(messageViewModel);
        }

        /// <summary>
        /// Request decline
        /// </summary>
        /// <param name="result"> Request id</param>
        /// <returns> The <see cref="ActionResult"/>.</returns>
        [HttpPost]
        public ActionResult Decline(MessageViewModel result)
        {
            try
            {
                _requestService.Decline(result.Id, result.Message);
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
                _requestService.Confirm(id);
            }
            catch (MissingEntityException)
            {
                return View("InvalidOperation");
            }

            return RedirectToAction("Index");
        }
    }
}