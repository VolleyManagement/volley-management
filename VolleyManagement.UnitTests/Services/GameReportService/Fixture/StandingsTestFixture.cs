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
        private readonly List<(int DivisionId, StandingsEntry Standings)> _standings = new List<(int DivisionId, StandingsEntry Standings)>();
        private DateTime? _lastUpdateTime;

        public StandingsTestFixture DefaultStandings()
        {
            AddStandings(new StandingsEntry
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
                BallsWon = 234,
                BallsLost = 214
            });
            AddStandings(new StandingsEntry
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
                BallsWon = 166,
                BallsLost = 105
            });
            AddStandings(new StandingsEntry
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
                BallsWon = 123,
                BallsLost = 204
            });

            return this;
        }

        public StandingsTestFixture Add(StandingsEntry newStandingEntry)
        {
            AddStandings(newStandingEntry);
            return this;
        }

        public StandingsTestFixture WithStandingsForAllPossibleScores()
        {
            _standings.Clear();
            AddStandings(new StandingsEntry
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
                BallsWon = 363,
                BallsLost = 355
            });
            AddStandings(new StandingsEntry
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
                BallsWon = 350,
                BallsLost = 362
            });
            AddStandings(new StandingsEntry
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
                BallsWon = 349,
                BallsLost = 345
            });

            return this;
        }

        public StandingsTestFixture WithUniquePoints()
        {
            _standings.Clear();
            AddStandings(new StandingsEntry
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
                BallsWon = 102,
                BallsLost = 96
            });
            AddStandings(new StandingsEntry
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
                BallsWon = 121,
                BallsLost = 122
            });
            AddStandings(new StandingsEntry
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
                BallsWon = 218,
                BallsLost = 223
            });

            return this;
        }

        public StandingsTestFixture WithSamePoints()
        {
            _standings.Clear();
            AddStandings(new StandingsEntry
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
                BallsWon = 234,
                BallsLost = 220
            });
            AddStandings(new StandingsEntry
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
                BallsWon = 191,
                BallsLost = 188
            });
            AddStandings(new StandingsEntry
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
                BallsWon = 408,
                BallsLost = 425
            });

            return this;
        }

        public StandingsTestFixture WithSamePointsAndWonGames()
        {
            _standings.Clear();
            AddStandings(new StandingsEntry
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
                BallsWon = 76,
                BallsLost = 72
            });
            AddStandings(new StandingsEntry
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
                BallsWon = 98,
                BallsLost = 98
            });
            AddStandings(new StandingsEntry
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
                BallsWon = 170,
                BallsLost = 174
            });

            return this;
        }

        public StandingsTestFixture WithSamePointsWonGamesAndSetsRatio()
        {
            _standings.Clear();
            AddStandings(new StandingsEntry
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
                BallsWon = 101,
                BallsLost = 82
            });
            AddStandings(new StandingsEntry
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
                BallsWon = 104,
                BallsLost = 98
            });
            AddStandings(new StandingsEntry
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
                GamesWithScoreOneThree = 2,
                GamesWithScoreNilThree = 0,
                SetsWon = 2,
                SetsLost = 6,
                BallsWon = 180,
                BallsLost = 205
            });

            return this;
        }

        public StandingsTestFixture WithTechnicalDefeatInGame()
        {
            _standings.Clear();
            AddStandings(new StandingsEntry
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
                BallsWon = 234,
                BallsLost = 248
            });
            AddStandings(new StandingsEntry
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
                BallsWon = 91,
                BallsLost = 105
            });
            AddStandings(new StandingsEntry
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
                BallsWon = 157,
                BallsLost = 129
            });

            return this;
        }

        public StandingsTestFixture WithOneSetTechnicalDefeat()
        {
            _standings.Clear();
            AddStandings(new StandingsEntry
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
                BallsWon = 234,
                BallsLost = 248
            });
            AddStandings(new StandingsEntry
            {
                TeamName = "TeamNameC",
                Points = 3,
                GamesTotal = 2,
                GamesWon = 1,
                GamesLost = 1,
                GamesWithScoreThreeNil = 0,
                GamesWithScoreThreeOne = 1,
                GamesWithScoreThreeTwo = 0,
                GamesWithScoreTwoThree = 0,
                GamesWithScoreOneThree = 1,
                GamesWithScoreNilThree = 0,
                SetsWon = 4,
                SetsLost = 4,
                BallsWon = 166,
                BallsLost = 135
            });
            AddStandings(new StandingsEntry
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
                GamesWithScoreOneThree = 1,
                GamesWithScoreNilThree = 0,
                SetsWon = 3,
                SetsLost = 6,
                BallsWon = 187,
                BallsLost = 204
            });

            return this;
        }

        public StandingsTestFixture WithTeamAPenalty()
        {
            _standings.Clear();
            AddStandings(new StandingsEntry
            {
                TeamName = "TeamNameA",
                Points = 3,
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
                BallsWon = 234,
                BallsLost = 214
            });
            AddStandings(new StandingsEntry
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
                BallsWon = 166,
                BallsLost = 125
            });
            AddStandings(new StandingsEntry
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
                BallsWon = 143,
                BallsLost = 204
            });

            return this;
        }

        public StandingsTestFixture WithTeamCPenalty()
        {
            _standings.Clear();
            AddStandings(new StandingsEntry
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
                BallsWon = 234,
                BallsLost = 214
            });
            AddStandings(new StandingsEntry
            {
                TeamName = "TeamNameC",
                Points = 1,
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
                BallsWon = 166,
                BallsLost = 125
            });
            AddStandings(new StandingsEntry
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
                BallsWon = 143,
                BallsLost = 204
            });

            return this;
        }

        public StandingsTestFixture WithNoResults()
        {
            _standings.Clear();
            AddStandings(new StandingsEntry
            {
                TeamId = 1,
                TeamName = "TeamNameA"
            });
            AddStandings(new StandingsEntry
            {
                TeamId = 2,
                TeamName = "TeamNameB"
            });
            AddStandings(new StandingsEntry
            {
                TeamId = 3,
                TeamName = "TeamNameC"
            });

            return this;
        }

        public StandingsTestFixture WithMaxSetsRatioForOneTeam()
        {
            _standings.Clear();
            AddStandings(new StandingsEntry
            {
                TeamId = 1,
                TeamName = "TeamNameA",
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
                BallsWon = 81,
                BallsLost = 72
            });
            AddStandings(new StandingsEntry
            {
                TeamId = 3,
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
                BallsWon = 102,
                BallsLost = 82
            });
            AddStandings(new StandingsEntry
            {
                TeamId = 2,
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
                BallsWon = 154,
                BallsLost = 183
            });

            return this;
        }

        public StandingsTestFixture WithMaxBallsRatioForOneTeam()
        {
            _standings.Clear();
            AddStandings(new StandingsEntry
            {
                TeamId = 1,
                TeamName = "TeamNameA",
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
                BallsWon = 75,
                BallsLost = 0
            });
            AddStandings(new StandingsEntry
            {
                TeamId = 3,
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
                BallsWon = 81,
                BallsLost = 72
            });
            AddStandings(new StandingsEntry
            {
                TeamId = 2,
                TeamName = "TeamNameB",
                Points = 0,
                GamesTotal = 2,
                GamesWon = 0,
                GamesLost = 2,
                GamesWithScoreThreeNil = 0,
                GamesWithScoreThreeOne = 0,
                GamesWithScoreThreeTwo = 0,
                GamesWithScoreTwoThree = 0,
                GamesWithScoreOneThree = 0,
                GamesWithScoreNilThree = 2,
                SetsWon = 0,
                SetsLost = 6,
                BallsWon = 72,
                BallsLost = 156
            });

            return this;
        }

        public StandingsTestFixture WithMultipleDivisionsAllPosibleScores()
        {
            _standings.Clear();
            AddStandings(// A
                new StandingsEntry
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
                    BallsWon = 363,
                    BallsLost = 355
                },
                divisionId: 1);
            AddStandings(// E
                new StandingsEntry
                {
                    TeamName = "TeamNameE",
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
                    BallsWon = 363,
                    BallsLost = 355
                },
                divisionId: 1);
            AddStandings(// C
                new StandingsEntry
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
                    BallsWon = 350,
                    BallsLost = 362
                },
                divisionId: 1);
            AddStandings(// D
                new StandingsEntry
                {
                    TeamName = "TeamNameD",
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
                    BallsWon = 350,
                    BallsLost = 362
                },
                divisionId: 1);
            AddStandings(// B
                new StandingsEntry
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
                    BallsWon = 349,
                    BallsLost = 345
                },
                divisionId: 1);
            AddStandings(// F
                new StandingsEntry
                {
                    TeamName = "TeamNameF",
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
                    BallsWon = 349,
                    BallsLost = 345
                },
                divisionId: 1);
            AddStandings(// G
                new StandingsEntry
                {
                    TeamName = "TeamNameG",
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
                    BallsWon = 363,
                    BallsLost = 355
                },
                divisionId: 2);
            AddStandings(// I
                new StandingsEntry
                {
                    TeamName = "TeamNameI",
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
                    BallsWon = 350,
                    BallsLost = 362
                },
                divisionId: 2);
            AddStandings(// H
                new StandingsEntry
                {
                    TeamName = "TeamNameH",
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
                    BallsWon = 349,
                    BallsLost = 345
                },
                divisionId: 2);
            AddStandings(// J
                new StandingsEntry
                {
                    TeamName = "TeamNameJ",
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
                    BallsWon = 0,
                    BallsLost = 0
                },
                divisionId: 2);
            AddStandings(// K
                new StandingsEntry
                {
                    TeamName = "TeamNameK",
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
                    BallsWon = 0,
                    BallsLost = 0
                },
                divisionId: 2);
            AddStandings(// L
                new StandingsEntry
                {
                    TeamName = "TeamNameL",
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
                    BallsWon = 0,
                    BallsLost = 0
                },
                divisionId: 2);

            return this;
        }

        public StandingsTestFixture WithMultipleDivisionsEmptyStandings()
        {
            _standings.Clear();
            AddStandings(// A
                new StandingsEntry
                {
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
                    BallsWon = 0,
                    BallsLost = 0
                },
                divisionId: 1);
            AddStandings(// B
                new StandingsEntry
                {
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
                    BallsWon = 0,
                    BallsLost = 0
                },
                divisionId: 1);
            AddStandings(// C
                new StandingsEntry
                {
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
                    BallsWon = 0,
                    BallsLost = 0
                },
                divisionId: 1);
            AddStandings(// D
                new StandingsEntry
                {
                    TeamName = "TeamNameD",
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
                    BallsWon = 0,
                    BallsLost = 0
                },
                divisionId: 1);
            AddStandings(// E
                new StandingsEntry
                {
                    TeamName = "TeamNameE",
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
                    BallsWon = 0,
                    BallsLost = 0
                },
                divisionId: 1);
            AddStandings(// F
                new StandingsEntry
                {
                    TeamName = "TeamNameF",
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
                    BallsWon = 0,
                    BallsLost = 0
                },
                divisionId: 1);
            AddStandings(// G
                new StandingsEntry
                {
                    TeamName = "TeamNameG",
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
                    BallsWon = 0,
                    BallsLost = 0
                },
                divisionId: 2);
            AddStandings(// H
                new StandingsEntry
                {
                    TeamName = "TeamNameH",
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
                    BallsWon = 0,
                    BallsLost = 0
                },
                divisionId: 2);
            AddStandings(// I
                new StandingsEntry
                {
                    TeamName = "TeamNameI",
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
                    BallsWon = 0,
                    BallsLost = 0
                },
                divisionId: 2);
            AddStandings(// J
                new StandingsEntry
                {
                    TeamName = "TeamNameJ",
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
                    BallsWon = 0,
                    BallsLost = 0
                },
                divisionId: 2);
            AddStandings(// K
                new StandingsEntry
                {
                    TeamName = "TeamNameK",
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
                    BallsWon = 0,
                    BallsLost = 0
                },
                divisionId: 2);
            AddStandings(// L
                new StandingsEntry
                {
                    TeamName = "TeamNameL",
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
                    BallsWon = 0,
                    BallsLost = 0
                },
                divisionId: 2);

            return this;
        }

        public StandingsTestFixture WithLastUpdateTime(DateTime? lastUpdateTime)
        {
            _lastUpdateTime = lastUpdateTime;

            return this;
        }

        public TournamentStandings<StandingsDto> Build()
        {
            var result = new TournamentStandings<StandingsDto>();

            result.Divisions = _standings.Select(t => t.DivisionId)
                                         .Distinct()
                                         .Select(divId =>
                {
                    return new StandingsDto
                    {
                        DivisionId = divId,
                        DivisionName = $"Division {divId}",
                        LastUpdateTime = _lastUpdateTime,
                        Standings = _standings.Where(t => t.DivisionId == divId).Select(t => t.Standings).ToList()
                    };
                })
                .ToList();

            return result;
        }

        private void AddStandings(StandingsEntry standing, int divisionId = 1)
        {
            _standings.Add((divisionId, standing));
        }
    }
}
