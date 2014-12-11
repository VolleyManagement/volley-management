namespace VolleyManagement.WebApi.Controllers
{
    using System;
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
        //public IQueryable<Tournament> Get()
        //{
        //    return _tournamentService.GetAll();
        //}

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
            _tournamentService.Create(ViewModelToDomain.Map(viewModel));
            return new HttpResponseMessage(HttpStatusCode.OK);

            ////HttpResponseMessage response;
            ////try
            ////{
            ////    //_tournamentService.Create(tournamentViewModel.Tournament);
            ////   // response = Request.CreateResponse(HttpStatusCode.Created, tournamentViewModel.Tournament);
            ////    response.Headers.Add("Location", Url.ODataLink(new EntitySetPathSegment("Tournaments")));
            ////    return response;
            ////}
            ////catch (Exception)
            ////{
            ////    response = Request.CreateResponse(HttpStatusCode.InternalServerError);
            ////    return response;
            ////}
        }
    }
}