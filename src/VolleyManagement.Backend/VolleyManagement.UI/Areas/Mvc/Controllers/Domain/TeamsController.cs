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
    using Domain.TeamsAggregate;
    using ViewModels.Teams;
    using ViewModels.Players;

#pragma warning disable S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)
    /// <summary>
    /// Defines teams controller
    /// </summary>
    public class TeamsController : Controller
#pragma warning restore S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)
    {
        private const string TEAM_DELETED_SUCCESSFULLY_DESCRIPTION = "Team has been deleted successfully.";

        /// <summary>
        /// Holds TeamService instance
        /// </summary>
        private readonly ITeamService _teamService;
        private readonly IAuthorizationService _authService;
        private readonly IFileService _fileService;
        private readonly IPlayerService _playerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamsController"/> class
        /// </summary>
        /// <param name="teamService">Instance of the class that implements ITeamService.</param>
        /// <param name="authService">The authorization service</param>
        /// <param name="fileService">The interface reference of file service.</param>
        /// <param name="playerService">The interface reference of player service.</param>
        public TeamsController(
            ITeamService teamService,
            IAuthorizationService authService,
            IFileService fileService,
            IPlayerService playerService)
        {
            _teamService = teamService;
            _authService = authService;
            _fileService = fileService;
            _playerService = playerService;
        }

        /// <summary>
        /// Gets teams from TeamService
        /// </summary>
        /// <returns>View with collection of teams.</returns>
        public ActionResult Index()
        {
            var teams = new TeamCollectionViewModel {
                Teams = _teamService.Get()
                                          .Select(t => TeamViewModel.Map(t, null, null)),
                AllowedOperations = _authService.GetAllowedOperations(new List<AuthOperation>
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
        public JsonResult Create(TeamViewModel teamViewModel)
        {
            JsonResult result = null;

            if (!ModelState.IsValid)
            {
                result = Json(ModelState);
            }
            else
            {
                try
                {
                    if (teamViewModel.Captain.Id == 0)
                    {
                        var newAddedCaptain = _playerService.Create(teamViewModel.Captain.ToCreatePlayerDto());
                        teamViewModel.Captain = new PlayerNameViewModel {
                            Id = newAddedCaptain.Id,
                            FirstName = newAddedCaptain.FirstName,
                            LastName = newAddedCaptain.LastName
                        };
                        var team = teamViewModel.ToCreateTeamDto();
                        var createdTeam = _teamService.Create(team);
                        teamViewModel.Id = createdTeam.Id;
                    }
                    else
                    {
                        var team = teamViewModel.ToCreateTeamDto();
                        var createdTeam = _teamService.Create(team);
                        teamViewModel.Id = createdTeam.Id;
                        createdTeam.SetCaptain(new PlayerId(teamViewModel.Captain.Id));
                    }
                    
                    if (teamViewModel.AddedPlayers.Count > 0)
                    {
                        var playersIdToAddToTeam = _playerService.CreateBulk(teamViewModel.AddedPlayers
                                 .Where(x => x.Id == 0)
                                 .Select(x => x.ToCreatePlayerDto())
                                 .ToList())
                             .Select(x => new PlayerId(x.Id))
                             .ToList();

                        playersIdToAddToTeam.AddRange(teamViewModel.AddedPlayers.Where(x => x.Id > 0).Select(x => new PlayerId(x.Id)));
                        _teamService.AddPlayers(new TeamId(teamViewModel.Id), playersIdToAddToTeam);
                    }

                    result = Json(teamViewModel, JsonRequestBehavior.AllowGet);
                }
                catch (ArgumentException ex)
                {
                    result = Json(new { Success = "False", ex.Message });
                }
                catch (MissingEntityException ex)
                {
                    result = Json(new { Success = "False", ex.Message });
                }
                catch (ValidationException ex)
                {
                    result = Json(new { Success = "False", ex.Message });
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

            var viewModel = TeamViewModel.Map(team, _teamService.GetTeamCaptain(team), _teamService.GetTeamRoster(new TeamId(id)));

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
                try
                {
                    var playersIdToAddToTeam = new List<PlayerId>();
                    if (teamViewModel.AddedPlayers.Count > 0)
                    {
                        playersIdToAddToTeam = _playerService.CreateBulk(teamViewModel.AddedPlayers
                                .Where(x => x.Id == 0)
                                .Select(x => x.ToCreatePlayerDto())
                                .ToList())
                                .Select(x => new PlayerId(x.Id))
                                .ToList();

                        playersIdToAddToTeam.AddRange(teamViewModel.AddedPlayers.Where(x => x.Id > 0).Select(x => new PlayerId(x.Id)));
                        _teamService.AddPlayers(new TeamId(teamViewModel.Id), playersIdToAddToTeam);
                    }

                    ChangeCapitain(teamViewModel, playersIdToAddToTeam);



                    if (teamViewModel.DeletedPlayers.Count > 0)
                    {
                        _teamService.RemovePlayers(new TeamId(teamViewModel.Id), teamViewModel.DeletedPlayers.Select(x => new PlayerId(x)));
                    }

                    var domainTeam = teamViewModel.ToDomain();

                    _teamService.Edit(domainTeam);

                    result = Json(teamViewModel, JsonRequestBehavior.AllowGet);
                }
                catch (ArgumentException ex)
                {
                    result = Json(new { Success = "False", ex.Message });
                }
                catch (MissingEntityException ex)
                {
                    result = Json(new { Success = "False", ex.Message });
                }
                catch (ValidationException ex)
                {
                    result = Json(new { Success = "False", ex.Message });
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
                _teamService.Delete(new TeamId(id));
                result = new TeamOperationResultViewModel {
                    Message = TEAM_DELETED_SUCCESSFULLY_DESCRIPTION,
                    OperationSuccessful = true
                };
            }
            catch (MissingEntityException ex)
            {
                result = new TeamOperationResultViewModel {
                    Message = ex.Message,
                    OperationSuccessful = false
                };
            }
            catch (DataException)
            {
                result = new TeamOperationResultViewModel {
                    Message = Resources.UI.TournamentController.TeamDelete,
                    OperationSuccessful = false
                };
            }

            return Json(result, JsonRequestBehavior.DenyGet);
        }

#pragma warning disable S2360 // Optional parameters should not be used
        /// <summary>
        /// Details action method for specific team.
        /// </summary>
        /// <param name="id">Team ID</param>
        /// <param name="returnUrl">URL for back link</param>
        /// <returns>View with specific team.</returns>
        public ActionResult Details(int id = 0, string returnUrl = "")
#pragma warning restore S2360 // Optional parameters should not be used
        {
            var team = _teamService.Get(id);

            if (team == null)
            {
                return HttpNotFound();
            }

            var viewModel = TeamViewModel.Map(team, _teamService.GetTeamCaptain(team), _teamService.GetTeamRoster(new TeamId(id)));
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

            return RedirectToAction("Edit", "Teams", new { id });
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

            return RedirectToAction("Edit", "Teams", new { id });
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

        private void ChangeCapitain(TeamViewModel teamViewModel, IEnumerable<PlayerId> playersIdToAddToTeam)
        {
            if (teamViewModel.IsCaptainChanged)
            {
                var captainId = teamViewModel.Captain.Id;
                if (teamViewModel.Captain.Id == 0)
                {
                    captainId = playersIdToAddToTeam.Select(x => x.Id).First();
                }

                _teamService.ChangeCaptain(new TeamId(teamViewModel.Id), new PlayerId(captainId));
            }
        }
    }
}
