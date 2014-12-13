namespace VolleyManagement.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.OData;
    using System.Web.Http.OData.Routing;
    using Contracts;
    using Domain.Tournaments;
    using VolleyManagement.WebApi.Mappers;
    using VolleyManagement.WebApi.ViewModels.Tournaments;

    /// <summary>
    /// Defines TournamentsController
    /// </summary>
    public class TournamentsController : ODataController
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
        /// <returns>All tournaments</returns>
        public IQueryable<TournamentViewModel> Get()
        {
            var tounaments = _tournamentService.GetAll().ToList();
            var tvm = new List<TournamentViewModel>();
            foreach (var t in tounaments)
            {
                tvm.Add(DomainToViewModel.Map(t));
            }

            return tvm.AsQueryable<TournamentViewModel>();
        }

        /// <summary>
        /// Gets specific tournament from TournamentService
        /// </summary>
        /// <param name="key">Tournament id</param>
        /// <returns>Response with specific tournament</returns>
        public HttpResponseMessage Get([FromODataUri]int key)
        {
            try
            {
                var tournament = _tournamentService.FindById(key);
                return Request.CreateResponse(HttpStatusCode.OK, DomainToViewModel.Map(tournament));
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        /// <summary>
        /// Creates new Tournament.
        /// </summary>
        /// <param name="viewModel">Tournament to create.</param>
        /// <returns>HttpResponse with created tournament.</returns>
        [HttpPost]
        public HttpResponseMessage Post(TournamentViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            var response = new HttpResponseMessage();
            try
            {
                _tournamentService.Create(ViewModelToDomain.Map(viewModel));
                var tournamentToReturn = _tournamentService.GetAll().Single(t => t.Name == viewModel.Name);
                viewModel.Id = tournamentToReturn.Id;
                response = Request.CreateResponse<TournamentViewModel>(HttpStatusCode.Created, viewModel);
                return response;
            }
            catch (Exception)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                return response;
            }
        }

        /// <summary>
        /// Removes tournament from TournamentService
        /// </summary>
        /// <param name="id">tournament id</param>
        /// <returns>All tournaments</returns>
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                Tournament tournamentToDelete = _tournamentService.FindById(id);
                _tournamentService.Delete(id);
                return Request.CreateResponse(HttpStatusCode.Accepted);
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}
