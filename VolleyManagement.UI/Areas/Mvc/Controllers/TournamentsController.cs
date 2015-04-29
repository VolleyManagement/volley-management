namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Domain.Tournaments;
    using VolleyManagement.UI.Areas.Mvc.Mappers;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments;

    /// <summary>
    /// Defines TournamentsController
    /// </summary>
    public class TournamentsController : Controller
    {
        public const string UNIQUE_NAME_KEY = "uniqueName";

        public const string APPLYING_START_BEFORE_NOW = "ApplyingStartbeforeNow";
        public const string APPLYING_START_DATE_AFTER_END_DATE = "ApplyingStartAfterDate";
        public const string APPLYING_PERIOD_LESS_THREE_MONTH = "ApplyingThreeMonthRule";        
        public const string APPLYING_END_DATE_AFTER_START_GAMES = "ApplyingPeriodAfterStartGames";
        public const string START_GAMES_AFTER_END_GAMES = "StartGamesAfterEndGaes";
        public const string TRANSFER_PERIOD_BEFORE_GAMES_START = "TransferPeriodBeforeGamesStart";
        public const string TRANSFER_END_BEFORE_TRANSFER_START = "TransferEndBeforeStart";
        public const string TRANSFER_END_AFTER_GAMES_END = "TransferEndAfterGamesEnd";
        

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
        /// Gets all tournaments from TournamentService
        /// </summary>
        /// <returns>View with collection of tournaments</returns>
        public ActionResult Index()
        {
            try
            {
                var tournaments = this._tournamentService.Get().ToList();
                return View(tournaments);
            }
            catch (Exception)
            {
                return this.HttpNotFound();
            }
        }

        /// <summary>
        /// Gets details for specific tournament
        /// </summary>
        /// <param name="id">Tournament id</param>
        /// <returns>View with specific tournament</returns>
        public ActionResult Details(int id)
        {
            try
            {
                Tournament tournament = this._tournamentService.Get(id);
                return View(tournament);
            }
            catch (InvalidOperationException)
            {
                return this.HttpNotFound();
            }
        }

        /// <summary>
        /// Create tournament action (GET)
        /// </summary>
        /// <returns>View to create a tournament</returns>
        public ActionResult Create()
        {
            var tournamentViewModel = new TournamentViewModel();
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
                if (e.Message.Equals(VolleyManagement.Domain.Properties.Resources.TournamentNameMustBeUnique))
                {
                    this.ModelState.AddModelError(UNIQUE_NAME_KEY, App_GlobalResources.ViewModelResources.UniqueNameMessage);
                }
                else if (e.Message.Equals(VolleyManagement.Domain.Properties.Resources.LateRegistrationDates))
                {
                    // is
                    this.ModelState.AddModelError(APPLYING_START_BEFORE_NOW, App_GlobalResources.ViewModelResources.ApplyingStartBeforeNow);
                }
                else if (e.Message.Equals(VolleyManagement.Domain.Properties.Resources.WrongRegistrationDatesPeriod))
                {
                    // is
                    this.ModelState.AddModelError(APPLYING_START_DATE_AFTER_END_DATE, App_GlobalResources.ViewModelResources.ApplyingStartBeforeEnd);
                }
                else if (e.Message.Equals(VolleyManagement.Domain.Properties.Resources.WrongThreeMonthRule))
                {
                    this.ModelState.AddModelError(APPLYING_PERIOD_LESS_THREE_MONTH, App_GlobalResources.ViewModelResources.ApplyingDateThreeMonth);
                }
                else if (e.Message.Equals(VolleyManagement.Domain.Properties.Resources.WrongRegistrationGames))
                {
                    // is
                    this.ModelState.AddModelError(APPLYING_END_DATE_AFTER_START_GAMES, App_GlobalResources.ViewModelResources.ApplyingPeriodBeforeGamesStart);
                }
                else if (e.Message.Equals(VolleyManagement.Domain.Properties.Resources.WrongStartTournamentDates))
                {
                    // is 
                    this.ModelState.AddModelError(START_GAMES_AFTER_END_GAMES, App_GlobalResources.ViewModelResources.EndGamesBeforeStart);
                }
                else if (e.Message.Equals(VolleyManagement.Domain.Properties.Resources.WrongTransferStart))
                {
                    // is 
                    this.ModelState.AddModelError(TRANSFER_PERIOD_BEFORE_GAMES_START, App_GlobalResources.ViewModelResources.TransferStartBeforeGamesStart);
                }
                else if (e.Message.Equals(VolleyManagement.Domain.Properties.Resources.WrongTransferPeriod))
                {
                    // is 
                    this.ModelState.AddModelError(TRANSFER_END_BEFORE_TRANSFER_START, App_GlobalResources.ViewModelResources.TransferEndAfterStart);
                }
                else if (e.Message.Equals(VolleyManagement.Domain.Properties.Resources.InvalidTransferEndpoint))
                {
                    // is
                    this.ModelState.AddModelError(TRANSFER_END_AFTER_GAMES_END, App_GlobalResources.ViewModelResources.TransferEndBeforeGamesEnd);
                }

                return this.View(tournamentViewModel);
            }
            catch (Exception)
            {
                return this.HttpNotFound();
            }
        }

        /// <summary>
        /// Edit tournament action (GET)
        /// </summary>
        /// <param name="id">Tournament id</param>
        /// <returns>View to edit specific tournament</returns>
        public ActionResult Edit(int id)
        {
            try
            {
                var tournament = this._tournamentService.Get(id);
                TournamentViewModel tournamentViewModel = TournamentViewModel.Map(tournament);
                return this.View(tournamentViewModel);
            }
            catch (Exception)
            {
                return this.HttpNotFound();
            }
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
            catch (Exception)
            {
                return this.HttpNotFound();
            }
        }

        /// <summary>
        /// Delete tournament action (GET)
        /// </summary>
        /// <param name="id">Tournament id</param>
        /// <returns>View to delete specific tournament</returns>
        public ActionResult Delete(int id)
        {
            Tournament tournament = this._tournamentService.Get(id);
            TournamentViewModel tournamentViewModel = TournamentViewModel.Map(tournament);
            return View(tournament);
        }

        /// <summary>
        /// Delete tournament action (POST)
        /// </summary>
        /// <param name="id">Tournament id</param>
        /// <returns>Index view</returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            // This will return "An error occured in VolleyManagement application."
            // Please contact site administrator." if tournament doesnt exist
            // this._tournamentService.Delete(id);
            // return this.RedirectToAction("Index");

            // This will return 404
            ActionResult result;
            try
            {
                this._tournamentService.Delete(id);
                result = this.RedirectToAction("Index");
            }
            catch (Exception)
            {
                result = this.HttpNotFound();
            }

            return result;
        }
    }
}
