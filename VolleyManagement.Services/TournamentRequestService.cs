namespace VolleyManagement.Services
{
    using System;
    using System.Collections.Generic;
    using VolleyManagement.Contracts;
    using VolleyManagement.Domain.TournamentRequestAggregate;

    /// <summary>
    /// Defines an implementation of <see cref="ITournamentRequestService"/> contract.
    /// </summary>
    public class TournamentRequestService : ITournamentRequestService
    {
        /// <summary>
        /// Confirm the request
        /// </summary>
        /// <param name="requestId">Request's id</param>
        public void Confirm(int requestId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create a new request
        /// </summary>
        /// <param name="userId">Id of user that ask for link</param>
        /// <param name="tournamentId"> Tournament's id</param>
        /// <param name="teamId"> Team's id</param>
        public void Create(int userId, int tournamentId, int teamId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Decline the request
        /// </summary>
        /// <param name="requestId">Request's id</param>
        /// <param name="messsage">Message about reason for decline</param>
        public void Decline(int requestId, string messsage)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets list of all requests.
        /// </summary>
        /// <returns>Return list of all requests.</returns>
        public List<TournamentRequest> Get()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Find request by id.
        /// </summary>
        /// <param name="id">Request id.</param>
        /// <returns>Found request.</returns>
        public TournamentRequest Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
