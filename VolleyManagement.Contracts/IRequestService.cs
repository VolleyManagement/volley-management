namespace VolleyManagement.Contracts
{
    using System.Collections.Generic;
    using Domain.RequestsAggregate;

    /// <summary>
    /// Defines a contract for request service.
    /// </summary>
    public interface IRequestService
    {
        /// <summary>
        /// Gets list of all requests.
        /// </summary>
        /// <returns>Return list of all requests.</returns>
        List<Request> Get();

        /// <summary>
        /// Find request by id.
        /// </summary>
        /// <param name="id">Request id.</param>
        /// <returns>Found request.</returns>
        Request Get(int id);

        /// <summary>
        /// Create a new request
        /// </summary>
        /// <param name="userId">Id of user that ask for link</param>
        /// <param name="playerId"> Player's id</param>
        void Create(int userId, int playerId);

        /// <summary>
        /// Approve the request
        /// </summary>
        /// <param name="requestId">Request's id</param>
        void Approve(int requestId);

        /// <summary>
        /// Decline the request
        /// </summary>
        /// <param name="requestId">Request's id</param>
        void Decline(int requestId);
    }
}
