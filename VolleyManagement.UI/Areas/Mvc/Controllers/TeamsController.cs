namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Domain.Teams;
    using VolleyManagement.Domain.Players;
    using VolleyManagement.UI.Areas.Mvc.Mappers;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Teams;
    using System.Collections.Generic;

    /// <summary>
    /// Defines teams controller
    /// </summary>
    public class TeamsController : Controller
    {
        private const string TEAM_DELETED_SUCCESSFULLY_DESCRITPION = "Команда была успешно удалена.";

        /// <summary>
        /// Holds PlayerService instance
        /// </summary>
        private readonly ITeamService _teamService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamsController"/> class
        /// </summary>
        /// <param name="teamSerivce">Instance of the class that implements
        /// ITeamService.</param>
        public TeamsController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        /// <summary>
        /// Gets teams from TeamService
        /// </summary>
        /// <returns>View with collection of teams.</returns>
        public ActionResult Index()
        {
            var teams = this._teamService.Get().ToList();
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
        /// Delete team action (POST)
        /// </summary>
        /// <param name="id">Team id</param>
        /// <returns>Result message</returns>
        [HttpPost]
        public JsonResult Delete(int id)
        {
            TeamDeleteResultViewModel result;
            try
            {
                this._teamService.Delete(id);
                result = new TeamDeleteResultViewModel
                {
                    Message = TEAM_DELETED_SUCCESSFULLY_DESCRITPION,
                    HasDeleted = true
                };
            }
            catch (MissingEntityException ex)
            {
                result = new TeamDeleteResultViewModel { Message = ex.Message, HasDeleted = false };
            }

            return Json(result, JsonRequestBehavior.DenyGet);
        }
    }
}