// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TournamentService.cs" company="SoftServe">
//   Copyright (c) SoftServe. All rights reserved.
// </copyright>
// <summary>
//   Defines the implementation of ITournamentService.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SoftServe.VolleyManagement.Services
{
    using System.Linq;

    using SoftServe.VolleyManagement.Contracts;
    using SoftServe.VolleyManagement.Dal.Contracts;
    using SoftServe.VolleyManagement.Domain.Tournaments;

    /// <summary>
    /// Defines TournamentService
    /// </summary>
    public class TournamentService : ITournamentService
    {
        /// <summary>
        /// Holds UnitOfWork instance to access to repository
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentService"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public TournamentService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Method to get all tournaments
        /// </summary>
        /// <returns>IQueryable of all tournaments</returns>
        public IQueryable<Tournament> GetAllTournaments()
        {
            return this.unitOfWork.Tournaments.FindAll();
        }
    }
}
