using VolleyManagement.Domain.PlayersAggregate;
using VolleyManagement.UI.Areas.Mvc.ViewModels.Players;

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
                    var roster = teamViewModel.Roster.Select(t => t.ToCreatePlayerDto()).ToList();

                    var players = _playerService.CreateBulk(roster);

                    var team = teamViewModel.ToCreateTeamDto();
                    teamViewModel.Roster = players.Select(x =>
                        new PlayerNameViewModel {
                            Id = x.Id,
                            FirstName = x.FirstName,
                            LastName = x.LastName
                        })
                            .ToList();

                    var createdTeam = _teamService.Create(team);

                    _teamService.AddPlayers(new TeamId(createdTeam.Id), players.Select(x => new PlayerId(x.Id)));

                    teamViewModel.Id = createdTeam.Id;
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
                    var team = _teamService.Get(teamViewModel.Id);
                    var playersInTeamDb = team.Roster.Select(x => x.Id);
                    var playersInTeamViewModelWhichHaveId = teamViewModel.Roster.Select(x => x.Id);
                    var playersIdToAddToTeam = new List<PlayerId>();

                    CheckChangeCaptain(teamViewModel,team);

                    var playersStillSame = !playersInTeamDb.Except(playersInTeamViewModelWhichHaveId).Any()
                                          && !playersInTeamViewModelWhichHaveId.Except(playersInTeamDb).Any()
                                          && playersInTeamDb.Count() == playersInTeamViewModelWhichHaveId.Count()
                                          && playersInTeamDb.Intersect(playersInTeamViewModelWhichHaveId).Count()
                                          == playersInTeamViewModelWhichHaveId.Count();

                    if (teamViewModel.AddedPlayers.Count > 0)
                    {
                        playersIdToAddToTeam = _playerService.CreateBulk(teamViewModel.AddedPlayers)
                            .Select(x => new PlayerId(x.Id))
                            .ToList();
                    }
                    if (!playersStillSame)
                    {
                        playersIdToAddToTeam.AddRange(UpdateRoaster(teamViewModel));
                    }
                    _teamService.AddPlayers(new TeamId(teamViewModel.Id), playersIdToAddToTeam);
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

        private void CheckChangeCaptain(TeamViewModel teamViewModel,Team team)
        {
            if (teamViewModel.Captain.Id == 0)
            {
                var createdCaptain = _playerService.Create(teamViewModel.Roster.First().ToCreatePlayerDto());
                teamViewModel.Captain.Id = createdCaptain.Id;
                _teamService.ChangeCaptain(new TeamId(teamViewModel.Id), new PlayerId(createdCaptain.Id));
            }
            else if (teamViewModel.Roster.First().Id!=team.Roster.First().Id)
            {
                var captainId = teamViewModel.Roster.First().Id;
                teamViewModel.Captain.Id = captainId;
                _teamService.ChangeCaptain(new TeamId(teamViewModel.Id), new PlayerId(captainId));
            }
        }

        private List<PlayerId> UpdateRoaster(TeamViewModel teamViewModel)
        {
            //select players which are in Db
            var registeredPlayers = teamViewModel.Roster
                .Where(x => x.Id != 0)
                .ToList();
            //select teams which players returned from view
            var playersTeams = registeredPlayers
                .Select(x => _playerService.GetPlayerTeam(_playerService.Get(x.Id)))
                .ToList();
            //check if players play in another team
            if (playersTeams.Any(x => x != null && x.Id != teamViewModel.Id))
            {
                throw new ArgumentException("Player can not play in two teams");
            }
            //select new players which are not id Db and add them to Db
            var playersToAddToTeam = new List<PlayerId>();

            var limit = registeredPlayers.Count;
            for (var i = 0; i < limit; i++)
            {
                var player = new PlayerId(registeredPlayers[i].Id);

                if (playersTeams[i] == null)
                {
                    //add new players which are in Db and have no team
                    playersToAddToTeam.Add(player);
                }
            }

            return playersToAddToTeam;
        }
    }
}
