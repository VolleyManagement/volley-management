﻿namespace VolleyManagement.Contracts
{
    using System.Linq;
    using Domain.Tournaments;

    /// <summary>
    /// Interface for TournamentService
    /// </summary>
    public interface ITournamentService
    {
        /// <summary>
        /// Gets list of all tournaments
        /// </summary>
        /// <returns>Return list of all tournaments.</returns>
        IQueryable<Tournament> GetAllTournaments();
    }
}
