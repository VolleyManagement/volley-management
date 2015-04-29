namespace VolleyManagement.UI.Areas.WebApi.ApiControllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Web.Http;
    using System.Web.OData;

    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Domain.Tournaments;
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
        [EnableQuery]
        [HttpGet]
        public SingleResult<TournamentViewModel> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_tournamentService.Get()
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
            var result = _tournamentService.GetActual()
                .Select(t => TournamentViewModel.Map(t));
                return Json(result);
        }

        /// <summary>
        /// Returns only finished tournaments
        /// </summary>
        /// <returns>The tournaments as json format</returns>
        [HttpGet]
        public IHttpActionResult GetFinished()
        {
            var result = _tournamentService.Get().ToList()
                .Select(t => TournamentViewModel.Map(t));
                return Json(result);
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
        [HttpDelete]
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
        [HttpPut]
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
