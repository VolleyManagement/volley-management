namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Domain.GameReportsAggregate;

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

            return this;
        }

        public StandingsTestFixture WithUniquePoints()
        {
            _standings.Clear();
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

            return this;
        }

        public StandingsTestFixture WithSamePoints()
        {
            _standings.Clear();
            _standings.Add(new StandingsEntry
            {
                TeamName = "TeamNameC",
                Points = 4,
                GamesTotal = 2,
                GamesWon = 2,
                GamesLost = 0,
                GamesWithScoreThreeNil = 0,
                GamesWithScoreThreeOne = 0,
                GamesWithScoreThreeTwo = 2,
                GamesWithScoreTwoThree = 0,
                GamesWithScoreOneThree = 0,
                GamesWithScoreNilThree = 0,
                SetsWon = 6,
                SetsLost = 4,
                SetsRatio = 6.0f / 4,
                BallsWon = 220,
                BallsLost = 234,
                BallsRatio = 220.0f / 234
            });
            _standings.Add(new StandingsEntry
            {
                TeamName = "TeamNameA",
                Points = 4,
                GamesTotal = 2,
                GamesWon = 1,
                GamesLost = 1,
                GamesWithScoreThreeNil = 1,
                GamesWithScoreThreeOne = 0,
                GamesWithScoreThreeTwo = 0,
                GamesWithScoreTwoThree = 1,
                GamesWithScoreOneThree = 0,
                GamesWithScoreNilThree = 0,
                SetsWon = 5,
                SetsLost = 3,
                SetsRatio = 5.0f / 3,
                BallsWon = 191,
                BallsLost = 188,
                BallsRatio = 191.0f / 188
            });
            _standings.Add(new StandingsEntry
            {
                TeamName = "TeamNameB",
                Points = 4,
                GamesTotal = 4,
                GamesWon = 1,
                GamesLost = 3,
                GamesWithScoreThreeNil = 0,
                GamesWithScoreThreeOne = 0,
                GamesWithScoreThreeTwo = 1,
                GamesWithScoreTwoThree = 2,
                GamesWithScoreOneThree = 0,
                GamesWithScoreNilThree = 1,
                SetsWon = 7,
                SetsLost = 11,
                SetsRatio = 7.0f / 11,
                BallsWon = 422,
                BallsLost = 411,
                BallsRatio = 422.0f / 411
            });

            return this;
        }

        public StandingsTestFixture WithSamePointsAndWonGames()
        {
            _standings.Clear();
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
            _standings.Add(new StandingsEntry
            {
                TeamName = "TeamNameA",
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
                TeamName = "TeamNameB",
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

            return this;
        }

        public StandingsTestFixture WithSamePointsWonGamesAndSetsRatio()
        {
            _standings.Clear();
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
                BallsWon = 101,
                BallsLost = 82,
                BallsRatio = 101.0f / 82
            });
            _standings.Add(new StandingsEntry
            {
                TeamName = "TeamNameA",
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
                BallsWon = 104,
                BallsLost = 98,
                BallsRatio = 104.0f / 98
            });
            _standings.Add(new StandingsEntry
            {
                TeamName = "TeamNameB",
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
                SetsWon = 2,
                SetsLost = 6,
                SetsRatio = 1.0f / 6,
                BallsWon = 180,
                BallsLost = 205,
                BallsRatio = 167.0f / 176
            });

            return this;
        }

        public StandingsTestFixture WithTeamStandingsForOneSetTechnicalDefeat()
        {
            _standings.Clear();
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
                SetsWon = 3,
                SetsLost = 1,
                SetsRatio = 6.0f / 3,
                BallsWon = 234,
                BallsLost = 214,
                BallsRatio = 234.0f / 214
            });
            _standings.Add(new StandingsEntry
            {
                TeamName = "TeamNameC",
                Points = 4,
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
                BallsWon = 141,
                BallsLost = 125,
                BallsRatio = 141.0f / 125
            });
            _standings.Add(new StandingsEntry
            {
                TeamName = "TeamNameB",
                Points = 2,
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
                BallsWon = 143,
                BallsLost = 179,
                BallsRatio = 143.0f / 179
            });

            return this;
        }

        public StandingsTestFixture WithNoResults()
        {
            _standings.Clear();
            _standings.Add(new StandingsEntry
            {
                TeamId = 1,
                TeamName = "TeamNameA",
                Points = 0,
                GamesTotal = 0,
                GamesWon = 0,
                GamesLost = 0,
                GamesWithScoreThreeNil = 0,
                GamesWithScoreThreeOne = 0,
                GamesWithScoreThreeTwo = 0,
                GamesWithScoreTwoThree = 0,
                GamesWithScoreOneThree = 0,
                GamesWithScoreNilThree = 0,
                SetsWon = 0,
                SetsLost = 0,
                SetsRatio = null,
                BallsWon = 0,
                BallsLost = 0,
                BallsRatio = null
            });
            _standings.Add(new StandingsEntry
            {
                TeamId = 2,
                TeamName = "TeamNameB",
                Points = 0,
                GamesTotal = 0,
                GamesWon = 0,
                GamesLost = 0,
                GamesWithScoreThreeNil = 0,
                GamesWithScoreThreeOne = 0,
                GamesWithScoreThreeTwo = 0,
                GamesWithScoreTwoThree = 0,
                GamesWithScoreOneThree = 0,
                GamesWithScoreNilThree = 0,
                SetsWon = 0,
                SetsLost = 0,
                SetsRatio = null,
                BallsWon = 0,
                BallsLost = 0,
                BallsRatio = null
            });
            _standings.Add(new StandingsEntry
            {
                TeamId = 3,
                TeamName = "TeamNameC",
                Points = 0,
                GamesTotal = 0,
                GamesWon = 0,
                GamesLost = 0,
                GamesWithScoreThreeNil = 0,
                GamesWithScoreThreeOne = 0,
                GamesWithScoreThreeTwo = 0,
                GamesWithScoreTwoThree = 0,
                GamesWithScoreOneThree = 0,
                GamesWithScoreNilThree = 0,
                SetsWon = 0,
                SetsLost = 0,
                SetsRatio = null,
                BallsWon = 0,
                BallsLost = 0,
                BallsRatio = null
            });

            return this;
        }

        /// <summary>
        /// Builds instance of <see cref="StandingsTestFixture"/>.
        /// </summary>
        /// <returns>Collection of <see cref="StandingsEntry"/> objects filled with test data.</returns>
        public List<StandingsEntry> Build()
        {
            return _standings.OrderByDescending(ts => ts.Points)
                .ThenByDescending(ts => ts.GamesWon)
                .ThenByDescending(ts => ts.SetsRatio)
                .ThenByDescending(s => s.BallsRatio)
                .ToList();
        }
    }
}
