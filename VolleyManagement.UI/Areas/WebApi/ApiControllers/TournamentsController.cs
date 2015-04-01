namespace VolleyManagement.UI.Areas.WebApi.ApiControllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Web.Http;
    using System.Web.Http.OData;
    using System.Web.Mvc;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Tournaments;

    /// <summary>
    /// The tournaments controller.
    /// </summary>
    public class TournamentsController : ODataController
    {
        private readonly ITournamentService _tournamentService;

        private const string CONTROLLER_NAME = "tournaments";

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
                                     .ToList()
                                     .Select(t => TournamentViewModel.Map(t))
                                     .AsQueryable();
        }

        /// <summary>
        /// The get tournament.
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <returns> The <see cref="SingleResult"/>. </returns>
        [Queryable]
        public SingleResult<TournamentViewModel> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_tournamentService.Get()
                                                         .Where(t => t.Id == key)
                                                         .ToList()
                                                         .Select(t => TournamentViewModel.Map(t))
                                                         .AsQueryable());
        }

        /// <summary>
        /// Creates Tournament
        /// </summary>
        /// <param name="tournament"> The tournament as ViewModel. </param>
        /// <returns> Has been saved successfully - Created OData result
        /// unsuccessfully - Bad request </returns>
        public IHttpActionResult Post(TournamentViewModel tournament)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var tournamentToCreate = tournament.ToDomain();
                _tournamentService.Create(tournamentToCreate);
                tournament.Id = tournamentToCreate.Id;
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Format("{0}.{1}", CONTROLLER_NAME, ex.ParamName), ex.Message);
                return BadRequest(ModelState);
            }

            catch (TournamentValidationException ex)
            {
                ModelState.AddModelError(string.Format("{0}.{1}", CONTROLLER_NAME, ex.ParamName), ex.Message); 
                return BadRequest(ModelState);
            }
            
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
        public IHttpActionResult Put(TournamentViewModel tournament)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var tournamentToUpdate = tournament.ToDomain();

            try
            {
                _tournamentService.Edit(tournamentToUpdate);
            }
            catch(TournamentValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return InternalServerError();                
            }

            return Updated(tournament);
        }

    }
}
