namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using UI.Areas.WebApi.ViewModels.GameReports;

    /// <summary>
    /// Generates <see cref="StandingsEntryViewModel"/> test data for unit tests.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class StandingsEntryViewModelServiceTestFixture
    {
        /// <summary>
        /// Holds collection of entries
        /// </summary>
        private IList<StandingsEntryViewModel> _entries = new List<StandingsEntryViewModel>();

        /// <summary>
        /// Adds entries to collection
        /// </summary>
        /// <returns>Builder object with collection of entries</returns>
        public StandingsEntryViewModelServiceTestFixture TestEntries()
        {
            _entries.Add(
                new StandingsEntryViewModel
                {
                    TeamName = "TeamNameA",
                    Points = 5,
                    Position = 1,
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
                });
            _entries.Add(
                    new StandingsEntryViewModel
                    {
                        TeamName = "TeamNameC",
                        Points = 3,
                        Position = 2,
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
                    });
            _entries.Add(
                new StandingsEntryViewModel
                {
                    TeamName = "TeamNameB",
                    Points = 1,
                    Position = 3,
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
                });
            return this;
        }

        /// <summary>
        /// Builds test data
        /// </summary>
        /// <returns>Entries collection</returns>
        public IList<StandingsEntryViewModel> Build()
        {
            return _entries;
        }
    }
}
