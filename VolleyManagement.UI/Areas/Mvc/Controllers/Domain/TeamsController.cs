namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Contracts;
    using Contracts.Authorization;
    using Contracts.Exceptions;
    using Domain.RolesAggregate;
    using ViewModels.Teams;

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
            _teamService = teamService;
            _authService = authService;
            _fileService = fileService;
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
                Teams = _teamService.Get()
                                         .ToList()
                                         .Select(t => TeamViewModel.Map(t, null, null)),
                AllowedOperations = _authService.GetAllowedOperations(new List<AuthOperation>()
                                                                          {
                                                                            AuthOperations.Teams.Create,
                                                                            AuthOperations.Teams.Edit,
                                                                            AuthOperations.Teams.Delete
                                                                          })
            };

            var referrerViewModel = new TeamCollectionReferrerViewModel(teams, HttpContext.Request.RawUrl);
            return View(referrerViewModel);
        }

        /// <summary>
        /// Create team action GET
        /// </summary>
        /// <returns>Empty team view model</returns>
        public ActionResult Create()
        {
            var teamViewModel = new TeamViewModel();
            return View(teamViewModel);
        }

        /// <summary>
        /// Create team action POST
        /// </summary>
        /// <param name="teamViewModel">Team view model</param>
        /// <returns>Redirect to team index page</returns>
        [HttpPost]
        public JsonResult Create(TeamViewModel teamViewModel,string name)
        {
            JsonResult result = null;

            if (!ModelState.IsValid)
            {
                result = Json(ModelState);
            }
            else
            {
                var domainTeam = teamViewModel.ToDomain();

                try
                {
                    _teamService.Create(domainTeam);
                    if (teamViewModel.Roster != null)
                    {
                        _teamService.UpdateRosterTeamId(teamViewModel.Roster.Select(t => t.ToDomain()).ToList(), domainTeam.Id);
                    }

                    teamViewModel.Id = domainTeam.Id;
                    result = Json(teamViewModel, JsonRequestBehavior.AllowGet);
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    result = Json(ModelState);
                }
                catch (MissingEntityException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    result = Json(ModelState);
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    result = Json(ModelState);
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

            viewModel.PhotoPath = PhotoPath(id);

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

            if (!ModelState.IsValid)
            {
                result = Json(ModelState);
            }
            else
            {
                var domainTeam = teamViewModel.ToDomain();

                try
                {
                    _teamService.Edit(domainTeam);
                    if (teamViewModel.Roster != null)
                    {
                        _teamService.UpdateRosterTeamId(teamViewModel.Roster.Select(t => t.ToDomain()).ToList(), domainTeam.Id);
                    }

                    teamViewModel.Id = domainTeam.Id;
                    result = Json(teamViewModel, JsonRequestBehavior.AllowGet);
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    result = Json(ModelState);
                }
                catch (MissingEntityException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    result = Json(ModelState);
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    result = Json(ModelState);
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
                _teamService.Delete(id);
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
                    Message = Resources.UI.TournamentController.TeamDelete,
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
            var refererViewModel = new TeamRefererViewModel(viewModel, returnUrl, HttpContext.Request.RawUrl);
            refererViewModel.Model.PhotoPath = PhotoPath(id);
            return View(refererViewModel);
        }

        /// <summary>
        /// Returns list of all teams
        /// </summary>
        /// <returns>Json list of teams</returns>
        public JsonResult GetAllTeams()
        {
            var teams = _teamService.Get()
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
                ModelState.AddModelError(string.Empty, ex.Message);
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
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return RedirectToAction("Edit", "Teams", new { id = id });
        }

        /// <summary>
        /// return photo path
        /// </summary>
        /// <param name="id">team id</param>
        /// <returns>photo path</returns>
        private string PhotoPath(int id)
        {
            var photoPath = string.Format(Constants.TEAM_PHOTO_PATH, id);
            return _fileService.FileExists(HttpContext.Request.MapPath(photoPath)) ? photoPath : string.Format(Constants.TEAM_PHOTO_PATH, 0);
        }
    }
}