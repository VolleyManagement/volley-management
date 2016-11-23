namespace VolleyManagement.UnitTests.Services.TournamentRequestService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.TournamentRequestAggregate;

    /// <summary>
    /// Class for generating test data
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class TournamentRequestServiceTestFixture
    {
        /// <summary>
        /// Holds collection of tournaments requests
        /// </summary>
        private List<TournamentRequest> _tournamentsRequest = new List<TournamentRequest>();

        /// <summary>
        /// Adds tournaments to collection
        /// </summary>
        /// <returns>Builder object with collection of tournaments</returns>
        public TournamentRequestServiceTestFixture TestTournamentsRequests()
        {
            _tournamentsRequest.Add(new TournamentRequest()
            {
                Id = 1,
                TeamId = 4,
                UserId = 1,
                TournamentId = 1
            });
            return this;
        }

        /// <summary>
        /// Add tournament request to collection.
        /// </summary>
        /// <param name="newTournament">Tournament request to add.</param>
        /// <returns>Builder object with collection of tournaments requests.</returns>
        public TournamentRequestServiceTestFixture AddTournament(TournamentRequest newTournament)
        {
            _tournamentsRequest.Add(newTournament);
            return this;
        }

        /// <summary>
        /// Builds test data
        /// </summary>
        /// <returns>Tournament request collection</returns>
        public List<TournamentRequest> Build()
        {
            return _tournamentsRequest;
        }
    }
}
