namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using VolleyManagement.Domain.GameReportsAggregate;

    /// <summary>
    /// Generates <see cref="StandingsEntry"/> test data for unit tests for <see cref="GameReportService"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class StandingsTestFixture
    {
        private List<StandingsEntry> _standings = new List<StandingsEntry>();

        /// <summary>
        /// Generates <see cref="StandingsEntry"/> objects filled with test data.
        /// </summary>
        /// <returns>Instance of <see cref="StandingsTestFixture"/>.</returns>
        public StandingsTestFixture TestStandings()
        {
            _standings.Add(new StandingsEntry
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
            });
            _standings.Add(new StandingsEntry
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
            });
            _standings.Add(new StandingsEntry
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
            });

            return this;
        }

        /// <summary>
        /// Adds <see cref="StandingsEntry"/> object to collection.
        /// </summary>
        /// <param name="newStandingEntry"><see cref="StandingsEntry"/> object to add.</param>
        /// <returns>Instance of <see cref="StandingsTestFixture"/>.</returns>
        public StandingsTestFixture Add(StandingsEntry newStandingEntry)
        {
            _standings.Add(newStandingEntry);
            return this;
        }

        /// <summary>
        /// Adds standings entries that correspond game results with all possible scores
        /// to collection of <see cref="StandingsEntry"/> objects.
        /// </summary>
        /// <returns>Instance of <see cref="StandingsTestFixture"/>.</returns>
        public StandingsTestFixture WithStandingsForAllPossibleScores()
        {
            _standings.Clear();
            _standings.Add(new StandingsEntry
            {
                TeamName = "TeamNameA",
                Points = 7,
                GamesTotal = 4,
                GamesWon = 2,
                GamesLost = 2,
                GamesWithScoreThreeNil = 1,
                GamesWithScoreThreeOne = 1,
                GamesWithScoreThreeTwo = 0,
                GamesWithScoreTwoThree = 1,
                GamesWithScoreOneThree = 1,
                GamesWithScoreNilThree = 0,
                SetsWon = 9,
                SetsLost = 7,
                SetsRatio = 9.0f / 7,
                BallsWon = 373,
                BallsLost = 355,
                BallsRatio = 373.0f / 355
            });
            _standings.Add(new StandingsEntry
            {
                TeamName = "TeamNameB",
                Points = 4,
                GamesTotal = 4,
                GamesWon = 2,
                GamesLost = 2,
                GamesWithScoreThreeNil = 0,
                GamesWithScoreThreeOne = 0,
                GamesWithScoreThreeTwo = 2,
                GamesWithScoreTwoThree = 0,
                GamesWithScoreOneThree = 0,
                GamesWithScoreNilThree = 2,
                SetsWon = 6,
                SetsLost = 10,
                SetsRatio = 6.0f / 10,
                BallsWon = 349,
                BallsLost = 355,
                BallsRatio = 349.0f / 355
            });
            _standings.Add(new StandingsEntry
            {
                TeamName = "TeamNameC",
                Points = 7,
                GamesTotal = 4,
                GamesWon = 2,
                GamesLost = 2,
                GamesWithScoreThreeNil = 1,
                GamesWithScoreThreeOne = 1,
                GamesWithScoreThreeTwo = 0,
                GamesWithScoreTwoThree = 1,
                GamesWithScoreOneThree = 1,
                GamesWithScoreNilThree = 0,
                SetsWon = 9,
                SetsLost = 7,
                SetsRatio = 9.0f / 7,
                BallsWon = 360,
                BallsLost = 372,
                BallsRatio = 360.0f / 372
            });

            _standings = _standings.OrderByDescending(ts => ts.Points)
                .ThenByDescending(ts => ts.SetsRatio)
                .ThenByDescending(ts => ts.BallsRatio)
                .ToList();

            return this;
        }

        /// <summary>
        /// Adds standings entries where teams have repetitive points to collection of <see cref="StandingsEntry"/> objects.
        /// Standings entries are ordered only by points.
        /// </summary>
        /// <returns>Instance of <see cref="StandingsTestFixture"/>.</returns>
        public StandingsTestFixture WithRepetitivePoints()
        {
            _standings.Clear();
            _standings.Add(new StandingsEntry
            {
                TeamName = "TeamNameA",
                Points = 3,
                GamesTotal = 2,
                GamesWon = 1,
                GamesLost = 1,
                GamesWithScoreThreeNil = 0,
                GamesWithScoreThreeOne = 1,
                GamesWithScoreThreeTwo = 0,
                GamesWithScoreTwoThree = 0,
                GamesWithScoreOneThree = 0,
                GamesWithScoreNilThree = 1,
                SetsWon = 3,
                SetsLost = 4,
                SetsRatio = 3.0f / 4,
                BallsWon = 151,
                BallsLost = 152,
                BallsRatio = 151.0f / 152
            });
            _standings.Add(new StandingsEntry
            {
                TeamName = "TeamNameB",
                Points = 3,
                GamesTotal = 2,
                GamesWon = 1,
                GamesLost = 1,
                GamesWithScoreThreeNil = 1,
                GamesWithScoreThreeOne = 0,
                GamesWithScoreThreeTwo = 2,
                GamesWithScoreTwoThree = 0,
                GamesWithScoreOneThree = 1,
                GamesWithScoreNilThree = 0,
                SetsWon = 4,
                SetsLost = 3,
                SetsRatio = 4.0f / 3,
                BallsWon = 152,
                BallsLost = 151,
                BallsRatio = 152.0f / 152
            });

            _standings = _standings.OrderByDescending(ts => ts.Points).ToList();
            return this;
        }

        /// <summary>
        /// Adds standings entries where teams have repetitive points and sets ratio to collection of <see cref="StandingsEntry"/> objects.
        /// Standings entries are ordered by points and by sets ratio.
        /// </summary>
        /// <returns>Instance of <see cref="StandingsTestFixture"/>.</returns>
        public StandingsTestFixture WithRepetitivePointsAndSetsRatio()
        {
            _standings.Clear();
            _standings.Add(new StandingsEntry
            {
                TeamName = "TeamNameA",
                Points = 3,
                GamesTotal = 2,
                GamesWon = 1,
                GamesLost = 1,
                GamesWithScoreThreeNil = 1,
                GamesWithScoreThreeOne = 0,
                GamesWithScoreThreeTwo = 0,
                GamesWithScoreTwoThree = 0,
                GamesWithScoreOneThree = 0,
                GamesWithScoreNilThree = 1,
                SetsWon = 3,
                SetsLost = 3,
                SetsRatio = 3.0f / 3,
                BallsWon = 117,
                BallsLost = 125,
                BallsRatio = 117.0f / 125
            });
            _standings.Add(new StandingsEntry
            {
                TeamName = "TeamNameB",
                Points = 3,
                GamesTotal = 2,
                GamesWon = 1,
                GamesLost = 1,
                GamesWithScoreThreeNil = 1,
                GamesWithScoreThreeOne = 0,
                GamesWithScoreThreeTwo = 0,
                GamesWithScoreTwoThree = 0,
                GamesWithScoreOneThree = 0,
                GamesWithScoreNilThree = 1,
                SetsWon = 3,
                SetsLost = 3,
                SetsRatio = 3.0f / 3,
                BallsWon = 125,
                BallsLost = 117,
                BallsRatio = 125.0f / 117
            });

            _standings = _standings.OrderByDescending(ts => ts.Points).ThenByDescending(ts => ts.SetsRatio).ToList();
            return this;
        }

        /// <summary>
        /// Builds instance of <see cref="StandingsTestFixture"/>.
        /// </summary>
        /// <returns>Collection of <see cref="StandingsEntry"/> objects filled with test data.</returns>
        public List<StandingsEntry> Build()
        {
            return _standings;
        }
    }
}
