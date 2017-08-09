namespace VolleyManagement.UnitTests.Services.TournamentRequestService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.TournamentRequestAggregate;

    /// <summary>
    /// Class for generation test data.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TournamentRequestServiceTestFixture
    {
        private List<TournamentRequest> _request
            = new List<TournamentRequest>();

        /// <summary>
        /// Return test collection of tournament requests
        /// </summary>
        /// <returns>Builder object with collection of tournament requests</returns>
        public TournamentRequestServiceTestFixture TestRequests()
        {
            _request.Add(new TournamentRequest()
            {
                Id = 1,
                UserId = 1,
                GroupId = 1,
                TeamId = 4
            });

            return this;
        }

        /// <summary>
        /// Add tournament request to collection.
        /// </summary>
        /// <param name="newTournamentRequest">Request to add</param>
        /// <returns>Builder object with collection of requests.</returns>
        public TournamentRequestServiceTestFixture AddTeam(
            TournamentRequest newTournamentRequest)
        {
            _request.Add(newTournamentRequest);
            return this;
        }

        /// <summary>
        /// Builds test data
        /// </summary>
        /// <returns>Request collection</returns>
        public List<TournamentRequest> Build()
        {
            return _request;
        }
    }
}
