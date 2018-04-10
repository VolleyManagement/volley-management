using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using VolleyManagement.API.ViewModels.Tournaments;
using VolleyManagement.Contracts;

namespace VolleyManagement.API.Controllers
{
    [Produces("application/json")]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class TournamentsController : Controller
    {
        private readonly ITournamentService _tournamentService;

        public TournamentsController(ITournamentService tournamentService)
        {
            _tournamentService = tournamentService;
        }

        // GET: api/Tournaments
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IEnumerable<TournamentViewModel> Get()
        {
            return _tournamentService.Get().Select(TournamentViewModel.Map);
        }
    }
}
