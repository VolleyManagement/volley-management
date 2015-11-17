namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Domain.Tournaments;
    using VolleyManagement.Domain.TournamentsAggregate;
    using VolleyManagement.UI.Areas.Mvc.Mappers;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments;
    using ErrorMessages = VolleyManagement.Domain.Properties.Resources;
    using ValidationMessages = App_GlobalResources.ViewModelResources;

    /// <summary>
    /// Defines TournamentsController
    /// </summary>
    public class TournamentsController : Controller
    {
        /// <summary>
        /// Holds TournamentService instance
        /// </summary>
        private readonly ITournamentService _tournamentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentsController"/> class
        /// </summary>
        /// <param name="tournamentService">The tournament service</param>
        public TournamentsController(ITournamentService tournamentService)
        {
            this._tournamentService = tournamentService;
        }

        /// <summary>
        /// Gets current and upcoming tournaments from TournamentService
        /// </summary>
        /// <returns>View with collection of tournaments</returns>
        public ActionResult Index()
        {
            var tournamentsCollections = new TournamentsCollectionsViewModel();

            var actualTournaments = this._tournamentService.GetActual().ToArray();

            tournamentsCollections.CurrentTournaments = actualTournaments
                .Where(tr => tr.State == TournamentStateEnum.Current);

            tournamentsCollections.UpcomingTournaments = actualTournaments
                .Where(tr => tr.State == TournamentStateEnum.Upcoming);

            return View(tournamentsCollections);
        }

        /// <summary>
        /// Get finished tournaments
        /// </summary>
        /// <returns>Json result</returns>
        public JsonResult GetFinished()
        {
            var result = _tournamentService.GetFinished().ToList()
                 .Select(t => TournamentViewModel.Map(t));
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets details for specific tournament
        /// </summary>
        /// <param name="id">Tournament id</param>
        /// <returns>View with specific tournament</returns>
        public ActionResult Details(int id)
        {
            return GetTournamentView(id);
        }

        /// <summary>
        /// Create tournament action (GET)
        /// </summary>
        /// <returns>View to create a tournament</returns>
        public ActionResult Create()
        {
            var tournamentViewModel = new TournamentViewModel()
                {
                    ApplyingPeriodStart = DateTime.Now.AddDays(1),
                    ApplyingPeriodEnd = DateTime.Now.AddDays(1),
                    GamesStart = DateTime.Now.AddDays(1),
                    GamesEnd = DateTime.Now.AddDays(1),
                    TransferStart = DateTime.Now.AddDays(1),
                    TransferEnd = DateTime.Now.AddDays(1)
                };
            return this.View(tournamentViewModel);
        }

        /// <summary>
        /// Create tournament action (POST)
        /// </summary>
        /// <param name="tournamentViewModel">Tournament, which the user wants to create</param>
        /// <returns>Index view if tournament was valid, else - create view</returns>
        [HttpPost]
        public ActionResult Create(TournamentViewModel tournamentViewModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var tournament = tournamentViewModel.ToDomain();
                    this._tournamentService.Create(tournament);
                    return this.RedirectToAction("Index");
                }

                return this.View(tournamentViewModel);
            }
            catch (TournamentValidationException e)
            {
                this.ModelState.AddModelError(e.ValidationKey, e.Message);
                return this.View(tournamentViewModel);
            }
        }

        /// <summary>
        /// Edit tournament action (GET)
        /// </summary>
        /// <param name="id">Tournament id</param>
        /// <returns>View to edit specific tournament</returns>
        public ActionResult Edit(int id)
        {
            return GetTournamentView(id);
        }

        /// <summary>
        /// Edit tournament action (POST)
        /// </summary>
        /// <param name="tournamentViewModel">Tournament after editing</param>
        /// <returns>Index view if tournament was valid, else - edit view</returns>
        [HttpPost]
        public ActionResult Edit(TournamentViewModel tournamentViewModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var tournament = tournamentViewModel.ToDomain();
                    this._tournamentService.Edit(tournament);
                    return this.RedirectToAction("Index");
                }

                return this.View(tournamentViewModel);
            }
            catch (TournamentValidationException ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return this.View(tournamentViewModel);
            }
        }

        /// <summary>
        /// Delete tournament action (GET)
        /// </summary>
        /// <param name="id">Tournament id</param>
        /// <returns>View to delete specific tournament</returns>
        public ActionResult Delete(int id)
        {
            return GetTournamentView(id);
        }

        /// <summary>
        /// Delete tournament action (POST)
        /// </summary>
        /// <param name="id">Tournament id</param>
        /// <returns>Index view</returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var tournament = _tournamentService.Get(id);

            if (tournament == null)
            {
                return View("PageNotFound");
            }

            this._tournamentService.Delete(id);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Gets the view for view model of the tournament with specified identifier.
        /// </summary>
        /// <param name="id">Identifier of the tournament.</param>
        /// <returns>View for view model of the tournament with specified identifier.</returns>
        private ViewResult GetTournamentView(int id)
        {
            var tournament = _tournamentService.Get(id);

            if (tournament == null)
            {
                return View("PageNotFound");
            }

            var tournamentViewModel = TournamentViewModel.Map(tournament);
            return View(tournamentViewModel);
        }
    }
}
