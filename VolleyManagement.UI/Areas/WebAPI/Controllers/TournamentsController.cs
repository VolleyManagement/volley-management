namespace VolleyManagement.UI.Areas.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using ViewModels.Tournaments;
    using VolleyManagement.Contracts;

    /// <summary>
    /// The tournaments controller.
    /// </summary>
    public class TournamentsController : ApiController
    {
        private readonly ITournamentService _tournamentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentsController"/> class.
        /// </summary>
        /// <param name="tournamentService"> The tournament service. </param>
        public TournamentsController(ITournamentService tournamentService)
        {
            this._tournamentService = tournamentService;
        }

        /// <summary>
        /// Gets all tournaments
        /// </summary>
        /// <returns>Collection of all tournaments</returns>
        public IEnumerable<TournamentViewModel> GetAllTournaments()
        {
            return this._tournamentService.Get().Select(t => TournamentViewModel.Map(t));
        }

        /// <summary>
        /// Gets tournament by id
        /// </summary>
        /// <param name="id">Id of tournament</param>
        /// <returns>Information about the tournament with specified id</returns>
        public IHttpActionResult GetTournament(int id)
        {
            var tournament = this._tournamentService.Get(id);
            if (tournament == null)
            {
                return NotFound();
            }

            return Ok(TournamentViewModel.Map(tournament));
        }
    }
}
