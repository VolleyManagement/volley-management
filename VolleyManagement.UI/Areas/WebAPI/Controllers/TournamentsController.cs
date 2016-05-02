namespace VolleyManagement.UI.Areas.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Web.Http;
    using System.Xml.Serialization;
    using ViewModels.Tournaments;
    using VolleyManagement.Contracts;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Games;

    /// <summary>
    /// The tournaments controller.
    /// </summary>
    public class TournamentsController : ApiController
    {
        private readonly ITournamentService _tournamentService;

        private readonly IGameService _gameService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentsController"/> class.
        /// </summary>
        /// <param name="tournamentService"> The tournament service. </param>
        /// <param name="gameService"> The game service. </param>
        public TournamentsController(
                ITournamentService tournamentService,
                IGameService gameService)
        {
            this._tournamentService = tournamentService;
            this._gameService = gameService;
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
        
        /// <summary>
        /// Gets games by tournament id.
        /// </summary>
        /// <param name="tournamentId">Id of tournament.</param>
        /// <returns>Information about games with specified tournament id.</returns>
        [Route("api/Tournament/{tournamentId}/Schedule")]
        public HttpResponseMessage GetSchedule(int tournamentId)
        {
            var games = this._gameService.GetTournamentResults(tournamentId)
                                                        .Select(t => GameViewModel.Map(t));

            XmlSerializer xsSubmit = new XmlSerializer(typeof(List<GameViewModel>));
            using (StringWriter sww = new StringWriter())
            {
                xsSubmit.Serialize(sww, games.ToList());
                return new HttpResponseMessage()
                {
                    Content = new StringContent(sww.ToString(), Encoding.UTF8, "application/xml")
                };
            }
        }
    }
}
