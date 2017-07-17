namespace VolleyManagement.UnitTests.Admin.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using UI.Areas.Admin.Models;

    /// <summary>
    /// Class for generating test data
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class TournamentRequestsViewModelsBuilder
    {
        /// <summary>
        /// Holds collection of tournaments requests
        /// </summary>
        private List<TournamentRequestViewModel> _tournamentsRequest = new List<TournamentRequestViewModel>();

        /// <summary>
        /// Adds tournaments to collection
        /// </summary>
        /// <returns>Builder object with collection of tournaments</returns>
        public TournamentRequestsViewModelsBuilder TestTournamentsRequests()
        {
            _tournamentsRequest.Add(new TournamentRequestViewModel()
            {
                Id = 1,
                TeamId = 4,
                PersonId = 1,
                PersonName = "Eugene",
                TeamTitle = "TestName",
                TournamentTitle = "Name"
            });

            return this;
        }

        /// <summary>
        /// Add tournament request to collection.
        /// </summary>
        /// <param name="newTournament">Tournament request to add.</param>
        /// <returns>Builder object with collection of tournaments requests.</returns>
        public TournamentRequestsViewModelsBuilder AddTournamentRequest(TournamentRequestViewModel newTournament)
        {
            _tournamentsRequest.Add(newTournament);
            return this;
        }

        /// <summary>
        /// Builds test data
        /// </summary>
        /// <returns>Tournament request collection</returns>
        public List<TournamentRequestViewModel> Build()
        {
            return _tournamentsRequest;
        }
    }
}
