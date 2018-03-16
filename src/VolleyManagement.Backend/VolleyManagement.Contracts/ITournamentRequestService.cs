﻿namespace VolleyManagement.Contracts
{
    using System.Collections.Generic;
    using Domain.TournamentRequestAggregate;

    /// <summary>
    /// Interface for TournamentRequestService.
    /// </summary>
    public interface ITournamentRequestService
    {
        /// <summary>
        /// Gets list of all requests.
        /// </summary>
        /// <returns>Return list of all requests.</returns>
        ICollection<TournamentRequest> Get();

        /// <summary>
        /// Find request by id.
        /// </summary>
        /// <param name="id">Request id.</param>
        /// <returns>Found request.</returns>
        TournamentRequest Get(int id);

        /// <summary>
        /// Create a new request
        /// </summary>
        /// <param name="tournamentRequest">Contains Team Id, Group Id, User Id</param>
        void Create(TournamentRequest tournamentRequest);

        /// <summary>
        /// Confirm the request
        /// </summary>
        /// <param name="requestId">Request's id</param>
        void Confirm(int requestId);

        /// <summary>
        /// Decline the request
        /// </summary>
        /// <param name="requestId">Request's id</param>
        /// <param name="message">Message about reason for decline</param>
        void Decline(int requestId, string message);
    }
}
