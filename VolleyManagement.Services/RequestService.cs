namespace VolleyManagement.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using VolleyManagement.Contracts;
    using VolleyManagement.Domain.RequestsAggregate;

    /// <summary>
    /// Defines an implementation of <see cref="IRequestService"/> contract.
    /// </summary>
    public class RequestService : IRequestService
    {
        /// <summary>
        /// Approve request by id
        /// </summary>
        /// <param name="requestId">The id of request to approve.</param>
        public void Approve(int requestId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create a new request.
        /// </summary>
        /// <param name="userId"> User's id to link.</param>
        /// <param name="playerId"> Player's id to link</param>
        public void Create(int userId, int playerId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Decline request by id.
        /// </summary>
        /// <param name="requestId">The id of request to decline.</param>
        public void Decline(int requestId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method for getting all requests.
        /// </summary>
        /// <returns>All requests.</returns>
        public List<Request> Get()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds a Request by id.
        /// </summary>
        /// <param name="id">id for search.</param>
        /// <returns>A found Request.</returns>
        public Request Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
