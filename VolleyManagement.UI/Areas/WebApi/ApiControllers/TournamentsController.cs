namespace VolleyManagement.UI.Areas.WebApi.ApiControllers
{
    using System.Linq;
    using System.Net;
    using System.Web.Http;
    using System.Web.Http.OData;
    using System.Web.Mvc;

    using VolleyManagement.Contracts;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Tournaments;

    /// <summary>
    /// The tournaments controller.
    /// </summary>
    public class TournamentsController : ODataController
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
        /// Gets tournaments
        /// </summary>
        /// <returns> Tournament list. </returns>
        [Queryable]
        public IQueryable<TournamentViewModel> GetTournaments()
        {
            return _tournamentService.Get()
                                     .Select(t => TournamentViewModel.Map(t));
        }

        /// <summary>
        /// The get tournament.
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <returns> The <see cref="SingleResult"/>. </returns>
        [Queryable]
        public SingleResult<TournamentViewModel> GetTournament([FromODataUri] int key)
        {
            return SingleResult.Create(_tournamentService.Get()
                                                         .Where(t => t.Id == key)
                                                         .Select(t => TournamentViewModel.Map(t)));
        }

        /// <summary>
        /// Creates Tournament
        /// </summary>
        /// <param name="tournament"> The tournament. </param>
        /// <returns> The <see cref="IHttpActionResult"/>. </returns>
        public IHttpActionResult Post(TournamentViewModel tournament)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tournamentToCreate = tournament.ToDomain();
            _tournamentService.Create(tournamentToCreate);
            tournament.Id = tournamentToCreate.Id;

            return Created(tournament);
        }

        /// <summary> Deletes tournament </summary>
        /// <param name="key"> The key. </param>
        /// <returns> The <see cref="IHttpActionResult"/>. </returns>
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            // if (tournament == null)
            // {
            //    return NotFound();
            // }
            _tournamentService.Delete(key);

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Updates Tournament
        /// </summary>
        /// <param name="tournament">The tournament to update</param>
        /// <returns>The <see cref="IHttpActionResult"/>.</returns>
        public IHttpActionResult Put(int id, TournamentViewModel tournament)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //if (_tournamentService.Get().Count(t => t.Id == tournament.Id) == 0)
            //{
            //    this.ModelState.AddModelError(string.Empty, VolleyManagement.UI.App_GlobalResources.ViewModelResources.InvalidTournamentId);
            //    return BadRequest(ModelState);
            //}
            
            var tournamentToUpdate = tournament.ToDomain();

            try
            {
                _tournamentService.Edit(tournamentToUpdate);
            }
            catch (System.Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }

            return Updated(tournament);
        }

    }
}
