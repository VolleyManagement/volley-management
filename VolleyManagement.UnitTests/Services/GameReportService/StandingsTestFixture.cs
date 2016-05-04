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
                BallsWon = 363,
                BallsLost = 355,
                BallsRatio = 363.0f / 355
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
                BallsLost = 345,
                BallsRatio = 349.0f / 345
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
                BallsWon = 350,
                BallsLost = 362,
                BallsRatio = 350.0f / 362
            });

            return this;
        }

        /// <summary>
        /// Adds standings entries where teams have unique points to collection of <see cref="StandingsEntry"/> objects.
        /// </summary>
        /// <returns>Instance of <see cref="StandingsTestFixture"/>.</returns>
        public StandingsTestFixture WithUniquePoints()
        {
            _standings.Clear();
            _standings.Add(new StandingsEntry
            {
                TeamName = "TeamNameA",
                Points = 1,
                GamesTotal = 2,
                GamesWon = 0,
                GamesLost = 2,
                GamesWithScoreThreeNil = 0,
                GamesWithScoreThreeOne = 0,
                GamesWithScoreThreeTwo = 0,
                GamesWithScoreTwoThree = 1,
                GamesWithScoreOneThree = 1,
                GamesWithScoreNilThree = 0,
                SetsWon = 3,
                SetsLost = 6,
                SetsRatio = 3.0f / 6,
                BallsWon = 218,
                BallsLost = 223,
                BallsRatio = 218.0f / 223
            });
            _standings.Add(new StandingsEntry
            {
                TeamName = "TeamNameB",
                Points = 2,
                GamesTotal = 1,
                GamesWon = 1,
                GamesLost = 0,
                GamesWithScoreThreeNil = 0,
                GamesWithScoreThreeOne = 0,
                GamesWithScoreThreeTwo = 1,
                GamesWithScoreTwoThree = 0,
                GamesWithScoreOneThree = 0,
                GamesWithScoreNilThree = 0,
                SetsWon = 3,
                SetsLost = 2,
                SetsRatio = 3.0f / 2,
                BallsWon = 121,
                BallsLost = 122,
                BallsRatio = 121.0f / 122
            });
            _standings.Add(new StandingsEntry
            {
                TeamName = "TeamNameC",
                Points = 3,
                GamesTotal = 1,
                GamesWon = 1,
                GamesLost = 0,
                GamesWithScoreThreeNil = 0,
                GamesWithScoreThreeOne = 1,
                GamesWithScoreThreeTwo = 0,
                GamesWithScoreTwoThree = 0,
                GamesWithScoreOneThree = 0,
                GamesWithScoreNilThree = 0,
                SetsWon = 3,
                SetsLost = 1,
                SetsRatio = 3.0f / 1,
                BallsWon = 102,
                BallsLost = 96,
                BallsRatio = 102.0f / 96
            });

            return this;
        }

        /// <summary>
        /// Adds standings entries where teams have repetitive points to collection of <see cref="StandingsEntry"/> objects.
        /// </summary>
        /// <returns>Instance of <see cref="StandingsTestFixture"/>.</returns>
        public StandingsTestFixture WithRepetitivePoints()
        {
            _standings.Clear();
            _standings.Add(new StandingsEntry
            {
                TeamName = "TeamNameA",
                Points = 0,
                GamesTotal = 2,
                GamesWon = 0,
                GamesLost = 2,
                GamesWithScoreThreeNil = 0,
                GamesWithScoreThreeOne = 0,
                GamesWithScoreThreeTwo = 0,
                GamesWithScoreTwoThree = 0,
                GamesWithScoreOneThree = 1,
                GamesWithScoreNilThree = 1,
                SetsWon = 1,
                SetsLost = 6,
                SetsRatio = 1.0f / 6,
                BallsWon = 167,
                BallsLost = 176,
                BallsRatio = 167.0f / 176
            });
            _standings.Add(new StandingsEntry
            {
                TeamName = "TeamNameB",
                Points = 3,
                GamesTotal = 1,
                GamesWon = 1,
                GamesLost = 0,
                GamesWithScoreThreeNil = 0,
                GamesWithScoreThreeOne = 1,
                GamesWithScoreThreeTwo = 0,
                GamesWithScoreTwoThree = 0,
                GamesWithScoreOneThree = 0,
                GamesWithScoreNilThree = 0,
                SetsWon = 3,
                SetsLost = 1,
                SetsRatio = 3.0f / 1,
                BallsWon = 98,
                BallsLost = 97,
                BallsRatio = 98.0f / 87
            });
            _standings.Add(new StandingsEntry
            {
                TeamName = "TeamNameC",
                Points = 3,
                GamesTotal = 1,
                GamesWon = 1,
                GamesLost = 0,
                GamesWithScoreThreeNil = 1,
                GamesWithScoreThreeOne = 0,
                GamesWithScoreThreeTwo = 0,
                GamesWithScoreTwoThree = 0,
                GamesWithScoreOneThree = 0,
                GamesWithScoreNilThree = 0,
                SetsWon = 3,
                SetsLost = 0,
                SetsRatio = 3.0f / 0,
                BallsWon = 78,
                BallsLost = 78,
                BallsRatio = 78.0f / 70
            });

            return this;
        }

        /// <summary>
        /// Adds standings entries where teams have repetitive points and sets ratio to collection of <see cref="StandingsEntry"/> objects.
        /// </summary>
        /// <returns>Instance of <see cref="StandingsTestFixture"/>.</returns>
        public StandingsTestFixture WithRepetitivePointsAndSetsRatio()
        {
            _standings.Clear();
            _standings.Add(new StandingsEntry
            {
                TeamName = "TeamNameA",
                Points = 0,
                GamesTotal = 2,
                GamesWon = 0,
                GamesLost = 2,
                GamesWithScoreThreeNil = 0,
                GamesWithScoreThreeOne = 0,
                GamesWithScoreThreeTwo = 0,
                GamesWithScoreTwoThree = 0,
                GamesWithScoreOneThree = 2,
                GamesWithScoreNilThree = 0,
                SetsWon = 2,
                SetsLost = 6,
                SetsRatio = 2.0f / 6,
                BallsWon = 193,
                BallsLost = 200,
                BallsRatio = 193.0f / 200
            });
            _standings.Add(new StandingsEntry
            {
                TeamName = "TeamNameB",
                Points = 3,
                GamesTotal = 1,
                GamesWon = 1,
                GamesLost = 0,
                GamesWithScoreThreeNil = 0,
                GamesWithScoreThreeOne = 1,
                GamesWithScoreThreeTwo = 0,
                GamesWithScoreTwoThree = 0,
                GamesWithScoreOneThree = 0,
                GamesWithScoreNilThree = 0,
                SetsWon = 3,
                SetsLost = 1,
                SetsRatio = 3.0f / 1,
                BallsWon = 98,
                BallsLost = 97,
                BallsRatio = 98.0f / 97
            });
            _standings.Add(new StandingsEntry
            {
                TeamName = "TeamNameC",
                Points = 3,
                GamesTotal = 1,
                GamesWon = 1,
                GamesLost = 0,
                GamesWithScoreThreeNil = 0,
                GamesWithScoreThreeOne = 1,
                GamesWithScoreThreeTwo = 0,
                GamesWithScoreTwoThree = 0,
                GamesWithScoreOneThree = 0,
                GamesWithScoreNilThree = 0,
                SetsWon = 3,
                SetsLost = 1,
                SetsRatio = 3.0f / 1,
                BallsWon = 102,
                BallsLost = 96,
                BallsRatio = 102.0f / 96
            });

            return this;
        }

        /// <summary>
        /// Adds standings entries where teams have repetitive points and sets ratio and balls ratio to collection of <see cref="StandingsEntry"/> objects.
        /// </summary>
        /// <returns>Instance of <see cref="StandingsTestFixture"/>.</returns>
        public StandingsTestFixture WithRepetitivePointsAndSetsRatioAndBallsRatio()
        {
            _standings.Clear();
            _standings.Add(new StandingsEntry
            {
                TeamName = "TeamNameA",
                Points = 3,
                GamesTotal = 2,
                GamesWon = 0,
                GamesLost = 2,
                GamesWithScoreThreeNil = 0,
                GamesWithScoreThreeOne = 0,
                GamesWithScoreThreeTwo = 0,
                GamesWithScoreTwoThree = 0,
                GamesWithScoreOneThree = 2,
                GamesWithScoreNilThree = 0,
                SetsWon = 2,
                SetsLost = 6,
                SetsRatio = 3.0f / 6,
                BallsWon = 193,
                BallsLost = 200,
                BallsRatio = 193.0f / 200
            });
            _standings.Add(new StandingsEntry
            {
                TeamName = "TeamNameB",
                Points = 3,
                GamesTotal = 1,
                GamesWon = 1,
                GamesLost = 0,
                GamesWithScoreThreeNil = 0,
                GamesWithScoreThreeOne = 1,
                GamesWithScoreThreeTwo = 0,
                GamesWithScoreTwoThree = 0,
                GamesWithScoreOneThree = 0,
                GamesWithScoreNilThree = 0,
                SetsWon = 3,
                SetsLost = 1,
                SetsRatio = 3.0f / 6,
                BallsWon = 98,
                BallsLost = 97,
                BallsRatio = 193.0f / 200
            });
            return this;
        }

        /// <summary>
        /// Orders standings by points in descending order.
        /// </summary>
        /// <returns>Instance of <see cref="StandingsTestFixture"/>.</returns>
        public StandingsTestFixture OrderByPoints()
        {
            _standings = _standings.OrderByDescending(ts => ts.Points).ToList();
            return this;
        }

        /// <summary>
        /// Orders standings by points, then by sets ratio in descending order.
        /// </summary>
        /// <returns>Instance of <see cref="StandingsTestFixture"/>.</returns>
        public StandingsTestFixture OrderByPointsAndSets()
        {
            _standings = _standings.OrderByDescending(ts => ts.Points).ThenByDescending(ts => ts.SetsRatio).ToList();
            return this;
        }

        /// <summary>
        /// Orders standings by points, then by sets ratio and then by balls ratio in descending order.
        /// </summary>
        /// <returns>Instance of <see cref="StandingsTestFixture"/>.</returns>
        public StandingsTestFixture OrderByPointsAndSetsAndBallsAndName()
        {
            _standings = _standings.OrderByDescending(ts => ts.Points)
                .ThenByDescending(ts => ts.SetsRatio)
                .ThenByDescending(ts => ts.BallsRatio)
                .ThenBy(ts => ts.TeamName)
                .ToList();

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
