namespace VolleyManagement.UnitTests.Services.TournamentService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using VolleyManagement.Domain.TournamentsAggregate;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Tournaments;

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
                    Season = 2014,
                    Scheme = TournamentSchemeEnum.One,
                    RegulationsLink = "www.Volleyball.dp.ua/Regulations/Tournaments('1')",
                    ApplyingPeriodStart = new DateTime(2015, 06, 02),
                    ApplyingPeriodEnd = new DateTime(2015, 09, 02),
                    GamesStart = new DateTime(2015, 09, 03),
                    GamesEnd = new DateTime(2015, 12, 03),
                    TransferStart = new DateTime(2015, 10, 01),
                    TransferEnd = new DateTime(2015, 11, 01)
                });
            _tournaments.Add(new Tournament()
                {
                    Id = 2,
                    Name = "Tournament 2",
                    Description = "Tournament 2 description",
                    Season = 2014,
                    Scheme = TournamentSchemeEnum.Two,
                    RegulationsLink = "www.Volleyball.dp.ua/Regulations/Tournaments('2')",
                    ApplyingPeriodStart = new DateTime(2015, 06, 02),
                    ApplyingPeriodEnd = new DateTime(2015, 09, 02),
                    GamesStart = new DateTime(2015, 09, 03),
                    GamesEnd = new DateTime(2015, 12, 03),
                    TransferStart = new DateTime(2015, 10, 01),
                    TransferEnd = new DateTime(2015, 11, 01)
                });
            _tournaments.Add(new Tournament()
                {
                    Id = 3,
                    Name = "Tournament 3",
                    Description = "Tournament 3 description",
                    Season = 2014,
                    Scheme = TournamentSchemeEnum.TwoAndHalf,
                    RegulationsLink = "www.Volleyball.dp.ua/Regulations/Tournaments('3')",
                    ApplyingPeriodStart = new DateTime(2015, 06, 02),
                    ApplyingPeriodEnd = new DateTime(2015, 09, 02),
                    GamesStart = new DateTime(2015, 09, 03),
                    GamesEnd = new DateTime(2015, 12, 03),
                    TransferStart = new DateTime(2015, 10, 01),
                    TransferEnd = new DateTime(2015, 11, 01)
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
        /// Builds test data
        /// </summary>
        /// <returns>Tournament collection</returns>
        public List<Tournament> Build()
        {
            return _tournaments;
        }
    }
}
