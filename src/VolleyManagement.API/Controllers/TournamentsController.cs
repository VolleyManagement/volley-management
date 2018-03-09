using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VolleyManagement.API.ViewModels.Tournaments;
using VolleyManagement.Contracts;

namespace VolleyManagement.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Tournaments")]

    public class TournamentsController : Controller
    {
        private readonly ITournamentService _tournamentService;

        public TournamentsController(ITournamentService tournamentService)
        {
            _tournamentService = tournamentService;
        }

        // GET: api/Tournaments
        [HttpGet]
        public IEnumerable<TournamentViewModel> Get()
        {
            return _tournamentService.Get().Select(TournamentViewModel.Map);
        }
    }
}
