// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TournamentsController.cs" company="SoftServe">
//   Copyright (c) SoftServe. All rights reserved.
// </copyright>
// <summary>
//   Controller for Tournaments
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SoftServe.VolleyManagement.Mvc.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using SoftServe.VolleyManagement.Contracts;

    /// <summary>
    /// Defines TournamentsController
    /// </summary>
    public class TournamentsController : Controller
    {
        /// <summary>
        /// Holds TournamentService instance
        /// </summary>
        private readonly ITournamentService tournamentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentsController"/> class
        /// </summary>
        /// <param name="tournamentService">The tournament service</param>
        public TournamentsController(ITournamentService tournamentService)
        {
            this.tournamentService = tournamentService;
        }

        /// <summary>
        /// Gets all tournaments from TournamentService
        /// </summary>
        /// <returns>View with collection of tournaments</returns>
        public ActionResult Index()
        {
            var tournaments = tournamentService.GetAllTournaments().ToList();
            return View(tournaments);
        }

    }
}
