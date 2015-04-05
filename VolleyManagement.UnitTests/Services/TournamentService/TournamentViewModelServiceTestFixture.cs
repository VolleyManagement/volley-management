namespace VolleyManagement.UnitTests.Services.TournamentService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.Tournaments;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Tournaments;

    /// <summary>
    /// Class for generating test data
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class TournamentViewModelServiceTestFixture
    {
        /// <summary>
        /// Holds collection of tournaments
        /// </summary>
        private IList<TournamentViewModel> _tournaments = new List<TournamentViewModel>();

        /// <summary>
        /// Adds tournaments to collection
        /// </summary>
        /// <returns>Builder object with collection of tournaments</returns>
        public TournamentViewModelServiceTestFixture TestTournaments()
        {
            _tournaments.Add(new TournamentViewModel()
            {
                Id = 1,
                Name = "Tournament 1",
                Description = "Tournament 1 description",
                Season = 2014,
                Scheme = "1",
                RegulationsLink = "www.Volleyball.dp.ua/Regulations/Tournaments('1')"
            });
            _tournaments.Add(new TournamentViewModel()
            {
                Id = 2,
                Name = "Tournament 2",
                Description = "Tournament 2 description",
                Season = 2014,
                Scheme = "2",
                RegulationsLink = "www.Volleyball.dp.ua/Regulations/Tournaments('2')"
            });
            _tournaments.Add(new TournamentViewModel()
            {
                Id = 3,
                Name = "Tournament 3",
                Description = "Tournament 3 description",
                Season = 2014,
                Scheme = "2.5",
                RegulationsLink = "www.Volleyball.dp.ua/Regulations/Tournaments('3')"
            });
            return this;
        }

        /// <summary>
        /// Add tournament to collection.
        /// </summary>
        /// <param name="newTournament">Tournament to add.</param>
        /// <returns>Builder object with collection of tournaments.</returns>
        public TournamentViewModelServiceTestFixture AddTournament(TournamentViewModel newTournament)
        {
            _tournaments.Add(newTournament);
            return this;
        }

        /// <summary>
        /// Builds test data
        /// </summary>
        /// <returns>Tournament collection</returns>
        public IList<TournamentViewModel> Build()
        {
            return _tournaments;
        }
    }
}