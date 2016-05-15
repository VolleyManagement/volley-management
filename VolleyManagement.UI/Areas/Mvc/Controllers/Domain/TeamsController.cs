namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Authorization;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Domain.RolesAggregate;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Players;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Teams;

    /// <summary>
    /// Defines teams controller
    /// </summary>
    public class TeamsController : Controller
    {
        private const string TEAM_DELETED_SUCCESSFULLY_DESCRIPTION = "Team has been deleted successfully.";

        /// <summary>
        /// Holds TeamService instance
        /// </summary>
        private readonly ITeamService _teamService;
        private readonly IAuthorizationService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamsController"/> class
        /// </summary>
        /// <param name="teamService">Instance of the class that implements ITeamService.</param>
        /// <param name="authService">The authorization service</param>
        public TeamsController(ITeamService teamService, IAuthorizationService authService)
        {
            this._teamService = teamService;
            this._authService = authService;
        }

        /// <summary>
        /// Gets teams from TeamService
        /// </summary>
        /// <returns>View with collection of teams.</returns>
        public ActionResult Index()
        {
            List<AuthOperation> requestedOperations = new List<AuthOperation>()
            {
                AuthOperations.Teams.Create,
                AuthOperations.Teams.Edit,
                AuthOperations.Teams.Delete
            };

            var teams = new TeamCollectionViewModel()
            {
                Teams = this._teamService.Get()
                                         .ToList()
                                         .Select(t => TeamViewModel.Map(t, null, null)),
                AllowedOperations = this._authService.GetAllowedOperations(new List<AuthOperation>()
                                                                          {
                                                                            AuthOperations.Teams.Create,
                                                                            AuthOperations.Teams.Edit,
                                                                            AuthOperations.Teams.Delete
                                                                          })
            };
            ViewBag.ReturnUrl = this.HttpContext.Request.RawUrl;

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
        public JsonResult Create(TeamViewModel teamViewModel)
        {
            JsonResult result = null;

            if (!this.ModelState.IsValid)
            {
                result = this.Json(this.ModelState);
            }
            else
            {
                var domainTeam = teamViewModel.ToDomain();

                try
                {
                    this._teamService.Create(domainTeam);
                    if (teamViewModel.Roster != null)
                    {
                        _teamService.UpdateRosterTeamId(teamViewModel.Roster.Select(t => t.ToDomain()).ToList(), domainTeam.Id);
                    }

                    teamViewModel.Id = domainTeam.Id;
                    result = this.Json(teamViewModel, JsonRequestBehavior.AllowGet);
                }
                catch (ArgumentException ex)
                {
                    this.ModelState.AddModelError(string.Empty, ex.Message);
                    result = this.Json(this.ModelState);
                }
                catch (MissingEntityException ex)
                {
                    this.ModelState.AddModelError(string.Empty, ex.Message);
                    result = this.Json(this.ModelState);
                }
                catch (ValidationException ex)
                {
                    this.ModelState.AddModelError(string.Empty, ex.Message);
                    result = this.Json(this.ModelState);
                }
            }

            return result;
        }

        /// <summary>
        /// Edit team action POST
        /// </summary>
        /// <param name="id">Id of the team which is needed to be edited</param>
        /// <param name="returnUrl">URL for back link</param>
        /// <returns>Redirect to team index page</returns>
        public ActionResult Edit(int id, string returnUrl = "")
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
        /// Edit team action POST
        /// </summary>
        /// <param name="teamViewModel">Team view model</param>
        /// <returns>Redirect to team index page</returns>
        [HttpPost]
        public JsonResult Edit(TeamViewModel teamViewModel)
        {
            JsonResult result = null;

            if (!this.ModelState.IsValid)
            {
                result = this.Json(this.ModelState);
            }
            else
            {
                var domainTeam = teamViewModel.ToDomain();

                try
                {
                    this._teamService.Edit(domainTeam);
                    if (teamViewModel.Roster != null)
                    {
                        _teamService.UpdateRosterTeamId(teamViewModel.Roster.Select(t => t.ToDomain()).ToList(), domainTeam.Id);
                    }

                    teamViewModel.Id = domainTeam.Id;
                    result = this.Json(teamViewModel, JsonRequestBehavior.AllowGet);
                }
                catch (ArgumentException ex)
                {
                    this.ModelState.AddModelError(string.Empty, ex.Message);
                    result = this.Json(this.ModelState);
                }
                catch (MissingEntityException ex)
                {
                    this.ModelState.AddModelError(string.Empty, ex.Message);
                    result = this.Json(this.ModelState);
                }
                catch (ValidationException ex)
                {
                    this.ModelState.AddModelError(string.Empty, ex.Message);
                    result = this.Json(this.ModelState);
                }
            }

            return result;
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
        /// <param name="returnUrl">URL for back link</param>
        /// <returns>View with specific team.</returns>
        public ActionResult Details(int id = 0, string returnUrl = "")
        {
            var team = _teamService.Get(id);

            if (team == null)
            {
                return HttpNotFound();
            }

            ViewBag.ReturnUrl = this.HttpContext.Request.RawUrl;
            var viewModel = TeamViewModel.Map(team, _teamService.GetTeamCaptain(team), _teamService.GetTeamRoster(id));
            var refererViewModel = new TeamRefererViewModel(viewModel, returnUrl);
            return View(refererViewModel);
        }

        /// <summary>
        /// Returns list of all teams
        /// </summary>
        /// <returns>Json list of teams</returns>
        public JsonResult GetAllTeams()
        {
            var teams = this._teamService.Get()
                                         .ToList()
                                         .Select(t => TeamNameViewModel.Map(t));
            return Json(teams, JsonRequestBehavior.AllowGet);
        }
    }
}
