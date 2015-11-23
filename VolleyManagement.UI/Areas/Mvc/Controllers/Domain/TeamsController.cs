namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Players;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Teams;

    /// <summary>
    /// Defines teams controller
    /// </summary>
    public class TeamsController : Controller
    {
        private const string TEAM_DELETED_SUCCESSFULLY_DESCRIPTION = "Команда была успешно удалена.";

        /// <summary>
        /// Holds PlayerService instance
        /// </summary>
        private readonly ITeamService _teamService;

        private readonly IHttpContextService _httpContentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamsController"/> class
        /// </summary>
        /// <param name="teamService">Instance of the class that implements
        /// ITeamService.</param>
        public TeamsController(ITeamService teamService, IHttpContextService httpContentService)
        {
            this._teamService = teamService;
            this._httpContentService = httpContentService;
        }

        /// <summary>
        /// Gets teams from TeamService
        /// </summary>
        /// <returns>View with collection of teams.</returns>
        public ActionResult Index()
        {
            var teams = this._teamService.Get()
                                         .ToList()
                                         .Select(t => TeamViewModel.Map(t, null, null));
            return View(teams);
        }

        /// <summary>
        /// Create team action GET
        /// </summary>
        /// <returns>Empty team view model</returns>
        public ActionResult Create()
        {
            var teamViewModel = new TeamViewModel();
            return this.View(teamViewModel);
        }

        /// <summary>
        /// Create team action POST
        /// </summary>
        /// <param name="teamViewModel">Team view model</param>
        /// <returns>Redirect to team index page</returns>
        [HttpPost]
        public ActionResult Create(TeamViewModel teamViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(teamViewModel);
            }

            var domainTeam = teamViewModel.ToDomain();

            try
            {
                this._teamService.Create(domainTeam);
            }
            catch (MissingEntityException ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return this.View(teamViewModel);
            }
            catch (ValidationException ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return this.View(teamViewModel);
            }

            bool duringRosterUpdateErrors = false;

            if (teamViewModel.Roster != null)
            {
                duringRosterUpdateErrors = !UpdateRosterPlayersTeamId(teamViewModel.Roster, domainTeam.Id);
            }

            teamViewModel.Id = domainTeam.Id;

            if (duringRosterUpdateErrors)
            {
                return View("Edit", teamViewModel);
            }
            else
            {
                return this.RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Delete team action (POST)
        /// </summary>
        /// <param name="id">Team id</param>
        /// <returns>Result message</returns>
        [HttpPost]
        public JsonResult Delete(int id)
        {
            TeamOperationResultViewModel result;

            try
            {
                this._teamService.Delete(id);
                result = new TeamOperationResultViewModel
                {
                    Message = TEAM_DELETED_SUCCESSFULLY_DESCRIPTION,
                    OperationSuccessful = true
                };
            }
            catch (MissingEntityException ex)
            {
                result = new TeamOperationResultViewModel { Message = ex.Message, OperationSuccessful = false };
            }

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// Details action method for specific team.
        /// </summary>
        /// <param name="id">Team ID</param>
        /// <returns>View with specific team.</returns>
        public ActionResult Details(int id = 0)
        {
            var team = _teamService.Get(id);

            if (team == null)
            {
                return HttpNotFound();
            }

            ViewBag.ReturnUrl = this._httpContentService.Request.RawUrl;
            var viewModel = TeamViewModel.Map(team, _teamService.GetTeamCaptain(team), _teamService.GetTeamRoster(id));
            return View(viewModel);
        }

        private bool UpdateRosterPlayersTeamId(List<PlayerNameViewModel> roster, int teamId)
        {
            bool clearUpdate = true;

            List<PlayerNameViewModel> playersToRemoveFromViewModel = new List<PlayerNameViewModel>();
            foreach (var item in roster)
            {
                // Here possible the case when captain will be updated twice
                // First as captain in services, second - if user chose him as roster player
                try
                {
                    this._teamService.UpdatePlayerTeam(item.Id, teamId);
                }
                catch (MissingEntityException ex)
                {
                    clearUpdate = false;
                    string message = string.Format("{0} (id = {1}) : {2} \n", item.FullName, item.Id, ex.Message);
                    this.ModelState.AddModelError(string.Empty, message);
                    playersToRemoveFromViewModel.Add(item);
                }
                catch (ValidationException ex)
                {
                    clearUpdate = false;
                    string message = string.Format("{0} (id = {1}) : {2} \n", item.FullName, item.Id, ex.Message);
                    this.ModelState.AddModelError(string.Empty, message);
                    playersToRemoveFromViewModel.Add(item);
                }
            }

            if (playersToRemoveFromViewModel.Count > 0)
            {
                RemovePlayersFromRoster(roster, playersToRemoveFromViewModel);
            }

            return clearUpdate;
        }

        private void RemovePlayersFromRoster(List<PlayerNameViewModel> roster, List<PlayerNameViewModel> playersToRemove)
        {
            foreach (var item in playersToRemove)
            {
                roster.Remove(item);
            }
        }
    }
}
