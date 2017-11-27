namespace VolleyManagement.UnitTests.Services.RequestsService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.RequestsAggregate;

    /// <summary>
    /// Class for generating test data
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RequestServiceTestFixture
    {
        private List<Request> _requests = new List<Request>();

        /// <summary>
        /// Return test collection of teams
        /// </summary>
        /// <returns>Builder object with collection of teams</returns>
        public RequestServiceTestFixture TestRequests()
        {
            _requests.Add(new Request()
            {
                Id = 1,
                UserId = 1,
                PlayerId = 1
            });

            _requests.Add(new Request()
            {
                Id = 2,
                UserId = 2,
                PlayerId = 2
            });

            _requests.Add(new Request()
            {
                Id = 3,
                UserId = 3,
                PlayerId = 3
            });

            return this;
        }

        /// <summary>
        /// Add request to collection.
        /// </summary>
        /// <param name="newRequest">Request to add.</param>
        /// <returns>Builder object with collection of requests.</returns>
        public RequestServiceTestFixture AddTeam(Request newRequest)
        {
            _requests.Add(newRequest);
            return this;
        }

        /// <summary>
        /// Builds test data
        /// </summary>
        /// <returns>Request collection</returns>
        public List<Request> Build()
        {
            return _requests;
        }
    }
}