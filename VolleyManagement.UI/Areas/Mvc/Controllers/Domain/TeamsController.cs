namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Authorization;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Domain.RolesAggregate;
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
        private readonly IFileService _fileService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamsController"/> class
        /// </summary>
        /// <param name="teamService">Instance of the class that implements ITeamService.</param>
        /// <param name="authService">The authorization service</param>
        /// <param name="fileService">The interface reference of file service.</param>
        public TeamsController(
            ITeamService teamService,
            IAuthorizationService authService,
            IFileService fileService)
        {
            this._teamService = teamService;
            this._authService = authService;
            this._fileService = fileService;
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

            var referrerViewModel = new TeamCollectionReferrerViewModel(teams, this.HttpContext.Request.RawUrl);
            return View(referrerViewModel);
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
        /// <returns>Redirect to team index page</returns>
        public ActionResult Edit(int id)
        {
            var team = _teamService.Get(id);
            if (team == null)
            {
                return HttpNotFound();
            }

            var viewModel = TeamViewModel.Map(team, _teamService.GetTeamCaptain(team), _teamService.GetTeamRoster(id));

            viewModel.PhotoPath = photoPath(id);

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
                result = new TeamOperationResultViewModel
                {
                    Message = ex.Message,
                    OperationSuccessful = false
                };
            }
            catch (DataException)
            {
                result = new TeamOperationResultViewModel 
                { 
                    Message = App_GlobalResources.TournamentController.TeamDelete, 
                    OperationSuccessful = false 
                };
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

            var viewModel = TeamViewModel.Map(team, _teamService.GetTeamCaptain(team), _teamService.GetTeamRoster(id));
            var refererViewModel = new TeamRefererViewModel(viewModel, returnUrl, this.HttpContext.Request.RawUrl);
            refererViewModel.Model.PhotoPath = photoPath(id);
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

        /// <summary>
        /// Action method adds photo of the team.
        /// </summary>
        /// <param name="fileToUpload">The photo that is being uploaded.</param>
        /// <param name="id">Id of the photo.</param>
        /// <returns>Redirect to Edit page.</returns>
        [HttpPost]
        public ActionResult AddPhoto(HttpPostedFileBase fileToUpload, int id)
        {
            try
            {
                var photoPath = string.Format(Constants.TEAM_PHOTO_PATH, id);
                _fileService.Upload(fileToUpload, HttpContext.Request.MapPath(photoPath));
            }
            catch (System.IO.FileLoadException ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
            }

            return RedirectToAction("Edit", "Teams", new { id = id });
        }

        /// <summary>
        /// Action method deletes photo of the team.
        /// </summary>
        /// <param name="id">Id of the photo.</param>
        /// <returns>Redirect to Edit page.</returns>
        [HttpPost]
        public ActionResult DeletePhoto(int id)
        {
            try
            {
                var photoPath = string.Format(Constants.TEAM_PHOTO_PATH, id);
                _fileService.Delete(HttpContext.Request.MapPath(photoPath));
            }
            catch (System.IO.FileNotFoundException ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
            }

            return RedirectToAction("Edit", "Teams", new { id = id });
        }

        /// <summary>
        /// return photo path
        /// </summary>
        /// <param name="id">team id</param>
        /// <returns>photo path</returns>
        private string photoPath(int id)
        {
            var photoPath = string.Format(Constants.TEAM_PHOTO_PATH, id);
            return _fileService.FileExists(HttpContext.Request.MapPath(photoPath)) ? photoPath : string.Format(Constants.TEAM_PHOTO_PATH, 0);
        }
    }
}