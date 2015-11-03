namespace VolleyManagement.UI.Areas.WebApi.ODataControllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Web.Http;
    using System.Web.OData;

    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Tournaments;

    /// <summary>
    /// The tournaments controller.
    /// </summary>
    public class TournamentsController : ODataController
    {
        private const string CONTROLLER_NAME = "tournaments";

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
        [EnableQuery]
        [HttpGet]
        public IQueryable<TournamentViewModel> GetTournaments()
        {
            return this._tournamentService.Get()
                                     .ToList()
                                     .Select(t => TournamentViewModel.Map(t))
                                     .AsQueryable();
        }

        /// <summary>
        /// The get tournament.
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <returns> The <see cref="SingleResult"/>. </returns>
        [EnableQuery]
        [HttpGet]
        public SingleResult<TournamentViewModel> Get([FromODataUri] int key)
        {
            return SingleResult.Create(this._tournamentService.Get()
                                                         .Where(t => t.Id == key)
                                                         .ToList()
                                                         .Select(t => TournamentViewModel.Map(t))
                                                         .AsQueryable());
        }

        /// <summary>
        /// Returns only upcoming and current tournaments
        /// </summary>
        /// <returns>The tournaments as json format</returns>
        [HttpGet]
        public IHttpActionResult GetActual()
        {
            var result = this._tournamentService.GetActual()
                .Select(t => TournamentViewModel.Map(t));
            return this.Json(result);
        }

        /// <summary>
        /// Returns only finished tournaments
        /// </summary>
        /// <returns>The tournaments as json format</returns>
        [HttpGet]
        public IHttpActionResult GetFinished()
        {
            var result = this._tournamentService.GetFinished().ToList()
                .Select(t => TournamentViewModel.Map(t));
            return this.Json(result);
        }

        /// <summary>
        /// Creates Tournament
        /// </summary>
        /// <param name="tournament"> The tournament as ViewModel. </param>
        /// <returns> Has been saved successfully - Created OData result
        /// unsuccessfully - Bad request </returns>
        [HttpPost]
        public IHttpActionResult Post(TournamentViewModel tournament)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            try
            {
                var tournamentToCreate = tournament.ToDomain();
                this._tournamentService.Create(tournamentToCreate);
                tournament.Id = tournamentToCreate.Id;
            }
            catch (ArgumentException ex)
            {
                this.ModelState.AddModelError(string.Format("{0}.{1}", CONTROLLER_NAME, ex.ParamName), ex.Message);
                return this.BadRequest(this.ModelState);
            }
            catch (TournamentValidationException ex)
            {
                this.ModelState.AddModelError(string.Format("{0}.{1}", CONTROLLER_NAME, ex.ParamName), ex.Message);
                return this.BadRequest(this.ModelState);
            }

            return this.Created(tournament);
        }

        /// <summary> Deletes tournament </summary>
        /// <param name="key"> The key. </param>
        /// <returns> The <see cref="IHttpActionResult"/>. </returns>
        [HttpDelete]
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            // if (tournament == null)
            // {
            //    return NotFound();
            // }
            this._tournamentService.Delete(key);

            return this.StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Updates Tournament
        /// </summary>
        /// <param name="tournament">The tournament to update</param>
        /// <returns>The <see cref="IHttpActionResult"/>.</returns>
        [HttpPut]
        public IHttpActionResult Put(TournamentViewModel tournament)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var tournamentToUpdate = tournament.ToDomain();

            try
            {
                this._tournamentService.Edit(tournamentToUpdate);
            }
            catch (TournamentValidationException ex)
            {
                return this.BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return this.InternalServerError();
            }

            return this.Updated(tournament);
        }
    }
}
