// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TournamentsController.cs" company="SoftServe">
//   Copyright (c) SoftServe. All rights reserved.
// </copyright>
// <summary>
//   Defines controller for Tournaments.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace VolleyManagement.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Contracts;
    using Domain.Tournaments;

    /// <summary>
    /// Defines TournamentsController
    /// </summary>
    public class TournamentsController : ApiController
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
            this._tournamentService = tournamentService;
        }

        /// <summary>
        /// Gets all tournaments from TournamentService
        /// </summary>
        /// <returns>All tournaments</returns>
        public IEnumerable<Tournament> Get()
        {
            return this._tournamentService.GetAllTournaments();
        }
    }
}
