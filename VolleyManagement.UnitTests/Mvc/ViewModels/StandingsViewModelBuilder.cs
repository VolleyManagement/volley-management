namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.GameReports;

    /// <summary>
    /// Represents <see cref="StandingsViewModel"/> builder for unit tests for <see cref="GameReportsController"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class StandingsViewModelBuilder
    {
        private StandingsViewModel _standingsViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandingsViewModelBuilder"/> class.
        /// </summary>
        public StandingsViewModelBuilder()
        {
            _standingsViewModel = new StandingsViewModel
            {
                TournamentId = 1,
                TournamentName = "Name",
                Entries = new List<StandingsEntryViewModel>
                {
                    new StandingsEntryViewModel
                    {
                        TeamName = "TeamNameA",
                        Points = 5,
                        GamesTotal = 2,
                        GamesWon = 2,
                        GamesLost = 0,
                        GamesWithScoreThreeNil = 0,
                        GamesWithScoreThreeOne = 1,
                        GamesWithScoreThreeTwo = 1,
                        GamesWithScoreTwoThree = 0,
                        GamesWithScoreOneThree = 0,
                        GamesWithScoreNilThree = 0,
                        SetsWon = 6,
                        SetsLost = 3,
                        SetsRatio = 6.0f / 3,
                        BallsWon = 234,
                        BallsLost = 214,
                        BallsRatio = 234.0f / 214
                    },
                    new StandingsEntryViewModel
                    {
                        TeamName = "TeamNameC",
                        Points = 3,
                        GamesTotal = 2,
                        GamesWon = 1,
                        GamesLost = 1,
                        GamesWithScoreThreeNil = 1,
                        GamesWithScoreThreeOne = 0,
                        GamesWithScoreThreeTwo = 0,
                        GamesWithScoreTwoThree = 0,
                        GamesWithScoreOneThree = 1,
                        GamesWithScoreNilThree = 0,
                        SetsWon = 4,
                        SetsLost = 3,
                        SetsRatio = 4.0f / 3,
                        BallsWon = 166,
                        BallsLost = 105,
                        BallsRatio = 166.0f / 105
                    },
                    new StandingsEntryViewModel
                    {
                        TeamName = "TeamNameB",
                        Points = 1,
                        GamesTotal = 2,
                        GamesWon = 0,
                        GamesLost = 2,
                        GamesWithScoreThreeNil = 0,
                        GamesWithScoreThreeOne = 0,
                        GamesWithScoreThreeTwo = 0,
                        GamesWithScoreTwoThree = 1,
                        GamesWithScoreOneThree = 0,
                        GamesWithScoreNilThree = 1,
                        SetsWon = 2,
                        SetsLost = 6,
                        SetsRatio = 2.0f / 6,
                        BallsWon = 123,
                        BallsLost = 204,
                        BallsRatio = 123.0f / 204
                    }
                }
            };
        }

        /// <summary>
        /// Sets the tournament's identifier of the view model.
        /// </summary>
        /// <param name="id">Identifier of the tournament.</param>
        /// <returns>Instance of <see cref="StandingsViewModelBuilder"/>.</returns>
        public StandingsViewModelBuilder WithTournamentId(int id)
        {
            _standingsViewModel.TournamentId = id;
            return this;
        }

        /// <summary>
        /// Sets the tournament's name of the view model.
        /// </summary>
        /// <param name="name">Name of the tournament.</param>
        /// <returns>Instance of <see cref="StandingsViewModelBuilder"/>.</returns>
        public StandingsViewModelBuilder WithTournamentName(string name)
        {
            _standingsViewModel.TournamentName = name;
            return this;
        }

        /// <summary>
        /// Sets the standings entries of the view model.
        /// </summary>
        /// <param name="entries">Entries of the standings.</param>
        /// <returns>Instance of <see cref="StandingsViewModelBuilder"/>.</returns>
        public StandingsViewModelBuilder WithEntries(List<StandingsEntryViewModel> entries)
        {
            _standingsViewModel.Entries = entries;
            return this;
        }

        /// <summary>
        /// Builds instance of <see cref="StandingsViewModelBuilder"/>.
        /// </summary>
        /// <returns>Instance of <see cref="StandingsViewModel"/>.</returns>
        public StandingsViewModel Build()
        {
            return _standingsViewModel;
        }
    }
}
