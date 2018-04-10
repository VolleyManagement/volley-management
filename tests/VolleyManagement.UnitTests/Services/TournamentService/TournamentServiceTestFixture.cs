namespace VolleyManagement.UnitTests.Services.TournamentService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.TournamentsAggregate;

    /// <summary>
    /// Class for generating test data
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class TournamentServiceTestFixture
    {
        /// <summary>
        /// Holds collection of tournaments
        /// </summary>
        private List<Tournament> _tournaments = new List<Tournament>();

        /// <summary>
        /// Adds tournaments to collection
        /// </summary>
        /// <returns>Builder object with collection of tournaments</returns>
        public TournamentServiceTestFixture TestTournaments()
        {
            _tournaments.Add(new Tournament()
            {
                Id = 1,
                Name = "Tournament 1",
                Description = "Tournament 1 description",
                Location = "Location 1",
                Season = 2014,
                Scheme = TournamentSchemeEnum.One,
                RegulationsLink = "www.Volleyball.dp.ua/Regulations/Tournaments('1')",
                ApplyingPeriodStart = new DateTime(2015, 02, 20),
                ApplyingPeriodEnd = new DateTime(2015, 06, 20),
                GamesStart = new DateTime(2015, 06, 30),
                GamesEnd = new DateTime(2015, 11, 30),
                TransferStart = new DateTime(2015, 08, 20),
                TransferEnd = new DateTime(2015, 09, 10)
            });
            _tournaments.Add(new Tournament()
            {
                Id = 2,
                Name = "Tournament 2",
                Description = "Tournament 2 description",
                Location = "Location 2",
                Season = 2014,
                Scheme = TournamentSchemeEnum.Two,
                RegulationsLink = "www.Volleyball.dp.ua/Regulations/Tournaments('2')",
                ApplyingPeriodStart = new DateTime(2015, 02, 20),
                ApplyingPeriodEnd = new DateTime(2015, 06, 20),
                GamesStart = new DateTime(2015, 06, 30),
                GamesEnd = new DateTime(2015, 11, 30),
                TransferStart = new DateTime(2015, 08, 20),
                TransferEnd = new DateTime(2015, 09, 10)
            });
            _tournaments.Add(new Tournament()
            {
                Id = 3,
                Name = "Tournament 3",
                Description = "Tournament 3 description",
                Location = "Location 3",
                Season = 2014,
                Scheme = TournamentSchemeEnum.TwoAndHalf,
                RegulationsLink = "www.Volleyball.dp.ua/Regulations/Tournaments('3')",
                ApplyingPeriodStart = new DateTime(2015, 02, 20),
                ApplyingPeriodEnd = new DateTime(2015, 06, 20),
                GamesStart = new DateTime(2015, 06, 30),
                GamesEnd = new DateTime(2015, 11, 30),
                TransferStart = new DateTime(2015, 08, 20),
                TransferEnd = new DateTime(2015, 09, 10)
            });
            _tournaments.Add(new Tournament()
            {
                Id = 4,
                Name = "Tournament 4",
                Description = "Tournament 4 description",
                Location = "Location 4",
                Season = 2014,
                Scheme = TournamentSchemeEnum.PlayOff,
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
        public TournamentServiceTestFixture AddTournament(Tournament newTournament)
        {
            _tournaments.Add(newTournament);
            return this;
        }

        /// <summary>
        /// Add archived tournaments to collection.
        /// </summary>
        /// <returns>Builder object with collection of tournaments.</returns>
        public TournamentServiceTestFixture WithArchivedTournaments()
        {
            _tournaments.Add(new Tournament()
            {
                Id = 5,
                Name = "Tournament 5",
                Description = "Tournament 5 description",
                Location = "Location 5",
                Season = 2014,
                Scheme = TournamentSchemeEnum.TwoAndHalf,
                RegulationsLink = "www.Volleyball.dp.ua/Regulations/Tournaments('5')",
                ApplyingPeriodStart = new DateTime(2015, 02, 20),
                ApplyingPeriodEnd = new DateTime(2015, 06, 20),
                GamesStart = new DateTime(2015, 06, 30),
                GamesEnd = new DateTime(2015, 11, 30),
                TransferStart = new DateTime(2015, 08, 20),
                TransferEnd = new DateTime(2015, 09, 10),
                IsArchived = true
            });
            _tournaments.Add(new Tournament()
            {
                Id = 6,
                Name = "Tournament 6",
                Description = "Tournament 6 description",
                Location = "Location 6",
                Season = 2014,
                Scheme = TournamentSchemeEnum.PlayOff,
                RegulationsLink = "www.Volleyball.dp.ua/Regulations/Tournaments('6')",
                ApplyingPeriodStart = new DateTime(2015, 02, 20),
                ApplyingPeriodEnd = new DateTime(2015, 06, 20),
                GamesStart = new DateTime(2015, 06, 30),
                GamesEnd = new DateTime(2015, 11, 30),
                TransferStart = new DateTime(2015, 08, 20),
                TransferEnd = new DateTime(2015, 09, 10),
                IsArchived = true
            });
            return this;
        }

        /// <summary>
        /// Add tournament to collection.
        /// </summary>
        /// <returns>Builder object with collection of tournaments.</returns>
        public TournamentServiceTestFixture WithFinishedTournaments()
        {
            _tournaments.Add(new Tournament()
            {
                Id = 1,
                Name = "Tournament 1",
                Description = "Tournament 1 description",
                Location = "Location 1",
                Season = 2014,
                Scheme = TournamentSchemeEnum.One,
                RegulationsLink = "www.Volleyball.dp.ua/Regulations/Tournaments('1')",
                ApplyingPeriodStart = new DateTime(2014, 02, 20),
                ApplyingPeriodEnd = new DateTime(2014, 06, 20),
                GamesStart = new DateTime(2014, 06, 30),
                GamesEnd = new DateTime(2014, 11, 30),
                TransferStart = new DateTime(2014, 08, 20),
                TransferEnd = new DateTime(2014, 09, 10)
            });

            _tournaments.Add(new Tournament()
            {
                Id = 2,
                Name = "Tournament 2",
                Description = "Tournament 2 description",
                Location = "Location 2",
                Season = 2014,
                Scheme = TournamentSchemeEnum.Two,
                RegulationsLink = "www.Volleyball.dp.ua/Regulations/Tournaments('2')",
                ApplyingPeriodStart = new DateTime(2014, 02, 20),
                ApplyingPeriodEnd = new DateTime(2014, 06, 20),
                GamesStart = new DateTime(2014, 06, 30),
                GamesEnd = new DateTime(2014, 11, 30),
                TransferStart = new DateTime(2014, 08, 20),
                TransferEnd = new DateTime(2014, 09, 10)
            });
            return this;
        }

        /// <summary>
        /// Builds test data
        /// </summary>
        /// <returns>Tournament collection</returns>
        public List<Tournament> Build()
        {
            return _tournaments;
        }
    }
}
