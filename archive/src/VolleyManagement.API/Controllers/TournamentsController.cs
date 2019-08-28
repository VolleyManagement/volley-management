namespace VolleyManagement.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using ViewModels.Tournaments;
    using Contracts;

    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TournamentsController : Controller
    {
        private readonly ITournamentService _tournamentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentsController" /> class
        /// </summary>
        /// <param name="tournamentService"> The tournament service. </param>
        public TournamentsController(ITournamentService tournamentService)
        {
            _tournamentService = tournamentService;
        }

        /// <summary>
        /// Gets all tournaments
        /// </summary>
        /// <returns>Collection of all tournaments</returns>
        /// GET: api/Tournaments
        [HttpGet]
        public IEnumerable<TournamentViewModel> Get()
        {
            return _tournamentService.Get().Select(TournamentViewModel.Map);
        }
    }
}
