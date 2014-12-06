namespace VolleyManagement.Mvc.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using global::VolleyManagement.Contracts;
    using VolleyManagement.Domain.Tournaments;

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
            _tournamentService = tournamentService;
        }

        /// <summary>
        /// Gets all tournaments from TournamentService
        /// </summary>
        /// <returns>View with collection of tournaments</returns>
        public ActionResult Index()
        {
            var tournaments = _tournamentService.GetAll().ToList();
            return View(tournaments);
        }

        /// <summary>
        /// Gets details for specific tournament
        /// </summary>
        /// <param name="id">Tournament id</param>
        /// <returns>View with specific tournament</returns>
        public ViewResult Details(int id)
        {
            Tournament tournament = _tournamentService.FindById(id);
            return View(tournament);
        }

        /// <summary>
        /// Create tournament action (GET)
        /// </summary>
        /// <returns>View to create a tournament</returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create tournament action (POST)
        /// </summary>
        /// <param name="tournament">Tournament, which the user wants to create</param>
        /// <returns>Index view if tournament was valid, else - create view</returns>
        [HttpPost]
        public ActionResult Create(Tournament tournament)
        {
            if (ModelState.IsValid)
            {
                _tournamentService.Create(tournament);
                return RedirectToAction("Index");
            }

            return View(tournament);
        }

        /// <summary>
        /// Edit tournament action (GET)
        /// </summary>
        /// <param name="id">Tournament id</param>
        /// <returns>View to edit specific tournament</returns>
        public ActionResult Edit(int id)
        {
            Tournament tournament = _tournamentService.FindById(id);
            return View(tournament);
        }

        /// <summary>
        /// Edit tournament action (POST)
        /// </summary>
        /// <param name="tournament">Tournament after editing</param>
        /// <returns>Index view if tournament was valid, else - edit view</returns>
        [HttpPost]
        public ActionResult Edit(Tournament tournament)
        {
            if (ModelState.IsValid)
            {
                _tournamentService.Edit(tournament);
                return RedirectToAction("Index");
            }

            return View(tournament);
        }

        /// <summary>
        /// Delete tournament action (GET)
        /// </summary>
        /// <param name="id">Tournament id</param>
        /// <returns>View to delete specific tournament</returns>
        public ActionResult Delete(int id)
        {
            Tournament tournament = _tournamentService.FindById(id);
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
            _tournamentService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
