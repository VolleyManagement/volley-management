namespace VolleyManagement.Contracts
{
    using System.Collections.Generic;
    using VolleyManagement.Domain.TournamentRequestAggregate;

    /// <summary>
    /// Interface for TournamentRequestService.
    /// </summary>
    public interface ITournamentRequestService
    {
        /// <summary>
        /// Gets list of all requests.
        /// </summary>
        /// <returns>Return list of all requests.</returns>
        List<TournamentRequest> Get();

        /// <summary>
        /// Find request by id.
        /// </summary>
        /// <param name="id">Request id.</param>
        /// <returns>Found request.</returns>
        TournamentRequest Get(int id);

        /// <summary>
        /// Create a new request
        /// </summary>
        /// <param name="userId">Id of user that ask for link</param>
        /// <param name="tournamentId"> Tournament's id</param>
        /// <param name="teamId"> Team's id</param>
        void Create(int userId, int tournamentId, int teamId);

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
