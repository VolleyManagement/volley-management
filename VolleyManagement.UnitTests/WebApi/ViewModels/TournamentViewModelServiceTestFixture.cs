namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using UI.Areas.WebApi.ViewModels.Tournaments;

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
                RegulationsLink = "www.Volleyball.dp.ua/Regulations/Tournaments('1')",
                ApplyingPeriodStart = new DateTime(2015, 02, 20),
                ApplyingPeriodEnd = new DateTime(2015, 06, 20),
                GamesStart = new DateTime(2015, 06, 30),
                GamesEnd = new DateTime(2015, 11, 30),
                TransferStart = new DateTime(2015, 08, 20),
                TransferEnd = new DateTime(2015, 09, 10)
            });
            _tournaments.Add(new TournamentViewModel()
            {
                Id = 2,
                Name = "Tournament 2",
                Description = "Tournament 2 description",
                Season = 2014,
                Scheme = "2",
                RegulationsLink = "www.Volleyball.dp.ua/Regulations/Tournaments('2')",
                ApplyingPeriodStart = new DateTime(2015, 02, 20),
                ApplyingPeriodEnd = new DateTime(2015, 06, 20),
                GamesStart = new DateTime(2015, 06, 30),
                GamesEnd = new DateTime(2015, 11, 30),
                TransferStart = new DateTime(2015, 08, 20),
                TransferEnd = new DateTime(2015, 09, 10)
            });
            _tournaments.Add(new TournamentViewModel()
            {
                Id = 3,
                Name = "Tournament 3",
                Description = "Tournament 3 description",
                Season = 2014,
                Scheme = "2.5",
                RegulationsLink = "www.Volleyball.dp.ua/Regulations/Tournaments('3')",
                ApplyingPeriodStart = new DateTime(2015, 02, 20),
                ApplyingPeriodEnd = new DateTime(2015, 06, 20),
                GamesStart = new DateTime(2015, 06, 30),
                GamesEnd = new DateTime(2015, 11, 30),
                TransferStart = new DateTime(2015, 08, 20),
                TransferEnd = new DateTime(2015, 09, 10)
            });
            _tournaments.Add(new TournamentViewModel()
            {
                Id = 4,
                Name = "Tournament 4",
                Description = "Tournament 4 description",
                Season = 2014,
                Scheme = "PlayOff",
                RegulationsLink = "www.Volleyball.dp.ua/Regulations/Tournaments('4')",
                ApplyingPeriodStart = new DateTime(2015, 02, 20),
                ApplyingPeriodEnd = new DateTime(2015, 06, 20),
                GamesStart = new DateTime(2015, 06, 30),
                GamesEnd = new DateTime(2015, 11, 30),
                TransferStart = new DateTime(2015, 08, 20),
                TransferEnd = new DateTime(2015, 09, 10)
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