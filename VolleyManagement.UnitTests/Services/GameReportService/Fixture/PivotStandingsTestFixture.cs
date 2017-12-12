namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Domain.GameReportsAggregate;

    /// <summary>
    /// Generates <see cref="TeamStandingsDto"/> test data for unit tests for <see cref="GameReportService"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class PivotStandingsTestFixture
    {
        private readonly List<(int DivisionId, TeamStandingsDto Standings)> _teamStandings = new List<(int DivisionId, TeamStandingsDto Standings)>();

        private readonly List<(int DivisionId, ShortGameResultDto Result)> _gameResults = new List<(int DivisionId, ShortGameResultDto Result)>();

        private DateTime? _lastUpdateTime;

        public PivotStandingsTestFixture WithEmptyStandings()
        {
            // do nothing it will build itself with proper empty object
            return this;
        }

        public PivotStandingsTestFixture WithNoResults()
        {
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 1,
                TeamName = "TeamNameA",
                Points = 0,
                SetsRatio = null,
                BallsRatio = null
            });
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 2,
                TeamName = "TeamNameB",
                Points = 0,
                SetsRatio = null,
                BallsRatio = null
            });
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 3,
                TeamName = "TeamNameC",
                Points = 0,
                SetsRatio = null,
                BallsRatio = null
            });

            return this;
        }

        public PivotStandingsTestFixture WithStandingsForAllPossibleScores()
        {
            _teamStandings.Clear();
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 1,
                TeamName = "TeamNameA",
                Points = 7,
                SetsRatio = 9.0f / 7,
                BallsRatio = 363.0f / 355
            });
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 3,
                TeamName = "TeamNameC",
                Points = 7,
                SetsRatio = 9.0f / 7,
                BallsRatio = 350.0f / 362
            });
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 2,
                TeamName = "TeamNameB",
                Points = 4,
                SetsRatio = 6.0f / 10,
                BallsRatio = 349.0f / 345
            });

            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeGameScore = 3,
                AwayGameScore = 0,
                IsTechnicalDefeat = false
            });
            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 1,
                AwayTeamId = 3,
                HomeGameScore = 3,
                AwayGameScore = 1,
                IsTechnicalDefeat = false
            });
            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 2,
                AwayTeamId = 3,
                HomeGameScore = 3,
                AwayGameScore = 2,
                IsTechnicalDefeat = false
            });
            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeGameScore = 2,
                AwayGameScore = 3,
                IsTechnicalDefeat = false
            });
            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 1,
                AwayTeamId = 3,
                HomeGameScore = 1,
                AwayGameScore = 3,
                IsTechnicalDefeat = false
            });
            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 2,
                AwayTeamId = 3,
                HomeGameScore = 0,
                AwayGameScore = 3,
                IsTechnicalDefeat = false
            });

            return this;
        }

        public PivotStandingsTestFixture WithUniquePoints()
        {
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 3,
                TeamName = "TeamNameC",
                Points = 3,
                SetsRatio = (float)3 / 1,
                BallsRatio = (float)102 / 96
            });
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 2,
                TeamName = "TeamNameB",
                Points = 2,
                SetsRatio = (float)3 / 2,
                BallsRatio = (float)121 / 122
            });
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 1,
                TeamName = "TeamNameA",
                Points = 1,
                SetsRatio = (float)3 / 6,
                BallsRatio = (float)218 / 223
            });

            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 3,
                AwayTeamId = 1,
                HomeGameScore = 3,
                AwayGameScore = 1,
                IsTechnicalDefeat = false
            });
            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 2,
                AwayTeamId = 1,
                HomeGameScore = 3,
                AwayGameScore = 2,
                IsTechnicalDefeat = false
            });

            return this;
        }

        public PivotStandingsTestFixture WithSamePoints()
        {
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 3,
                TeamName = "TeamNameC",
                Points = 4,
                SetsRatio = (float)6 / 4,
                BallsRatio = (float)234 / 220
            });
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 1,
                TeamName = "TeamNameA",
                Points = 4,
                SetsRatio = (float)5 / 3,
                BallsRatio = (float)191 / 188
            });
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 2,
                TeamName = "TeamNameB",
                Points = 4,
                SetsRatio = (float)7 / 11,
                BallsRatio = (float)408 / 425
            });

            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeGameScore = 3,
                AwayGameScore = 0,
                IsTechnicalDefeat = false
            });
            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 3,
                AwayTeamId = 2,
                HomeGameScore = 3,
                AwayGameScore = 2,
                IsTechnicalDefeat = false
            });
            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 3,
                AwayTeamId = 2,
                HomeGameScore = 3,
                AwayGameScore = 2,
                IsTechnicalDefeat = false
            });
            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeGameScore = 2,
                AwayGameScore = 3,
                IsTechnicalDefeat = false
            });

            return this;
        }

        public PivotStandingsTestFixture WithSamePointsAndWonGames()
        {
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 3,
                TeamName = "TeamNameC",
                Points = 3,
                SetsRatio = (float)3 / 0,
                BallsRatio = (float)76 / 72
            });
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 1,
                TeamName = "TeamNameA",
                Points = 3,
                SetsRatio = (float)3 / 1,
                BallsRatio = (float)98 / 98
            });
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 2,
                TeamName = "TeamNameB",
                Points = 0,
                SetsRatio = (float)1 / 6,
                BallsRatio = (float)170 / 174
            });

            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeGameScore = 3,
                AwayGameScore = 1,
                IsTechnicalDefeat = false
            });
            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 3,
                AwayTeamId = 2,
                HomeGameScore = 3,
                AwayGameScore = 0,
                IsTechnicalDefeat = false
            });

            return this;
        }

        public PivotStandingsTestFixture WithSamePointsWonGamesAndSetsRatio()
        {
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 3,
                TeamName = "TeamNameC",
                Points = 3,
                SetsRatio = (float)3 / 1,
                BallsRatio = (float)101 / 82
            });
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 1,
                TeamName = "TeamNameA",
                Points = 3,
                SetsRatio = (float)3 / 1,
                BallsRatio = (float)104 / 98
            });
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 2,
                TeamName = "TeamNameB",
                Points = 0,
                SetsRatio = (float)2 / 6,
                BallsRatio = (float)180 / 205
            });

            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 3,
                AwayTeamId = 2,
                HomeGameScore = 3,
                AwayGameScore = 1,
                IsTechnicalDefeat = false
            });
            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeGameScore = 3,
                AwayGameScore = 1,
                IsTechnicalDefeat = false
            });

            return this;
        }

        public PivotStandingsTestFixture WithMaxSetsRatioForOneTeam()
        {
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 1,
                TeamName = "TeamNameA",
                Points = 3,
                SetsRatio = (float)3 / 0,
                BallsRatio = (float)81 / 72
            });
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 3,
                TeamName = "TeamNameC",
                Points = 3,
                SetsRatio = (float)3 / 1,
                BallsRatio = (float)102 / 82
            });
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 2,
                TeamName = "TeamNameB",
                Points = 0,
                SetsRatio = (float)1 / 6,
                BallsRatio = (float)154 / 183
            });

            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeGameScore = 3,
                AwayGameScore = 0,
                IsTechnicalDefeat = false
            });
            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 3,
                AwayTeamId = 2,
                HomeGameScore = 3,
                AwayGameScore = 1,
                IsTechnicalDefeat = false
            });

            return this;
        }

        public PivotStandingsTestFixture WithMaxBallsRatioForOneTeam()
        {
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 1,
                TeamName = "TeamNameA",
                Points = 3,
                SetsRatio = (float)3 / 0,
                BallsRatio = (float)75 / 0
            });
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 3,
                TeamName = "TeamNameC",
                Points = 3,
                SetsRatio = (float)3 / 0,
                BallsRatio = (float)81 / 72
            });
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 2,
                TeamName = "TeamNameB",
                Points = 0,
                SetsRatio = (float)0 / 6,
                BallsRatio = (float)72 / 156
            });

            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeGameScore = 3,
                AwayGameScore = 0,
                IsTechnicalDefeat = false
            });
            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 3,
                AwayTeamId = 2,
                HomeGameScore = 3,
                AwayGameScore = 0,
                IsTechnicalDefeat = false
            });

            return this;
        }

        public PivotStandingsTestFixture WithTeamAPenalty()
        {
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 1,
                TeamName = "TeamNameA",
                Points = 3,
                SetsRatio = (float)6 / 3,
                BallsRatio = (float)234 / 214
            });
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 3,
                TeamName = "TeamNameC",
                Points = 3,
                SetsRatio = (float)4 / 3,
                BallsRatio = (float)166 / 125
            });
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 2,
                TeamName = "TeamNameB",
                Points = 1,
                SetsRatio = (float)2 / 6,
                BallsRatio = (float)143 / 204
            });

            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeGameScore = 3,
                AwayGameScore = 2,
                IsTechnicalDefeat = false
            });
            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 1,
                AwayTeamId = 3,
                HomeGameScore = 3,
                AwayGameScore = 1,
                IsTechnicalDefeat = false
            });
            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 2,
                AwayTeamId = 3,
                HomeGameScore = 0,
                AwayGameScore = 3,
                IsTechnicalDefeat = false
            });
            return this;
        }

        public PivotStandingsTestFixture WithTeamCPenalty()
        {
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 1,
                TeamName = "TeamNameA",
                Points = 5,
                SetsRatio = (float)6 / 3,
                BallsRatio = (float)234 / 214
            });
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 3,
                TeamName = "TeamNameC",
                Points = 1,
                SetsRatio = (float)4 / 3,
                BallsRatio = (float)166 / 125
            });
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 2,
                TeamName = "TeamNameB",
                Points = 1,
                SetsRatio = (float)2 / 6,
                BallsRatio = (float)143 / 204
            });

            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeGameScore = 3,
                AwayGameScore = 2,
                IsTechnicalDefeat = false
            });
            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 1,
                AwayTeamId = 3,
                HomeGameScore = 3,
                AwayGameScore = 1,
                IsTechnicalDefeat = false
            });
            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 2,
                AwayTeamId = 3,
                HomeGameScore = 0,
                AwayGameScore = 3,
                IsTechnicalDefeat = false
            });
            return this;
        }

        public PivotStandingsTestFixture WithTechnicalDefeatInGame()
        {
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 1,
                TeamName = "TeamNameA",
                Points = 5,
                SetsRatio = (float)6 / 3,
                BallsRatio = (float)234 / 248
            });
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 3,
                TeamName = "TeamNameC",
                Points = 3,
                SetsRatio = (float)4 / 3,
                BallsRatio = (float)91 / 105
            });
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 2,
                TeamName = "TeamNameB",
                Points = 1,
                SetsRatio = (float)2 / 6,
                BallsRatio = (float)157 / 129
            });

            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeGameScore = 3,
                AwayGameScore = 2,
                IsTechnicalDefeat = false
            });
            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 1,
                AwayTeamId = 3,
                HomeGameScore = 3,
                AwayGameScore = 1,
                IsTechnicalDefeat = false
            });
            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 2,
                AwayTeamId = 3,
                HomeGameScore = 0,
                AwayGameScore = 3,
                IsTechnicalDefeat = true
            });
            return this;
        }

        public PivotStandingsTestFixture WithTechnicalDefeatInSet()
        {
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 1,
                TeamName = "TeamNameA",
                Points = 5,
                SetsRatio = (float)6 / 3,
                BallsRatio = (float)234 / 248
            });
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 3,
                TeamName = "TeamNameC",
                Points = 3,
                SetsRatio = (float)4 / 4,
                BallsRatio = (float)166 / 135
            });
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 2,
                TeamName = "TeamNameB",
                Points = 1,
                SetsRatio = (float)3 / 6,
                BallsRatio = (float)187 / 204
            });

            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeGameScore = 3,
                AwayGameScore = 2,
                IsTechnicalDefeat = false
            });
            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 1,
                AwayTeamId = 3,
                HomeGameScore = 3,
                AwayGameScore = 1,
                IsTechnicalDefeat = false
            });
            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 2,
                AwayTeamId = 3,
                HomeGameScore = 1,
                AwayGameScore = 3,
                IsTechnicalDefeat = false
            });
            return this;
        }

        public PivotStandingsTestFixture WithMultipleDivisionsAllPossibleScores()
        {
            AddTeamStandings(// A
                new TeamStandingsDto
                {
                    TeamId = 1,
                    TeamName = "TeamNameA",
                    Points = 7,
                    SetsRatio = (float)9 / 7,
                    BallsRatio = (float)363 / 355
                },
                divisionId: 1);
            AddTeamStandings(// E
                new TeamStandingsDto
                {
                    TeamId = 5,
                    TeamName = "TeamNameE",
                    Points = 7,
                    SetsRatio = (float)9 / 7,
                    BallsRatio = (float)363 / 355
                },
                divisionId: 1);
            AddTeamStandings(// C
                new TeamStandingsDto
                {
                    TeamId = 3,
                    TeamName = "TeamNameC",
                    Points = 7,
                    SetsRatio = (float)9 / 7,
                    BallsRatio = (float)350 / 362
                },
                divisionId: 1);
            AddTeamStandings(// D
                new TeamStandingsDto
                {
                    TeamId = 4,
                    TeamName = "TeamNameD",
                    Points = 7,
                    SetsRatio = (float)9 / 7,
                    BallsRatio = (float)350 / 362
                },
                divisionId: 1);
            AddTeamStandings(// B
                new TeamStandingsDto
                {
                    TeamId = 2,
                    TeamName = "TeamNameB",
                    Points = 4,
                    SetsRatio = (float)6 / 10,
                    BallsRatio = (float)349 / 345
                },
                divisionId: 1);
            AddTeamStandings(// F
                new TeamStandingsDto
                {
                    TeamId = 6,
                    TeamName = "TeamNameF",
                    Points = 4,
                    SetsRatio = (float)6 / 10,
                    BallsRatio = (float)349 / 345
                },
                divisionId: 1);
            AddTeamStandings(// G
                new TeamStandingsDto
                {
                    TeamId = 7,
                    TeamName = "TeamNameG",
                    Points = 7,
                    SetsRatio = (float)9 / 7,
                    BallsRatio = (float)363 / 355
                },
                divisionId: 2);
            AddTeamStandings(// I
                new TeamStandingsDto
                {
                    TeamId = 9,
                    TeamName = "TeamNameI",
                    Points = 7,
                    SetsRatio = (float)9 / 7,
                    BallsRatio = (float)350 / 362
                },
                divisionId: 2);
            AddTeamStandings(// H
                new TeamStandingsDto
                {
                    TeamId = 8,
                    TeamName = "TeamNameH",
                    Points = 4,
                    SetsRatio = (float)6 / 10,
                    BallsRatio = (float)349 / 345
                },
                divisionId: 2);
            AddTeamStandings(// J
                new TeamStandingsDto
                {
                    TeamId = 10,
                    TeamName = "TeamNameJ",
                    Points = 0,
                    SetsRatio = null,
                    BallsRatio = null
                },
                divisionId: 2);
            AddTeamStandings(// K
                new TeamStandingsDto
                {
                    TeamId = 11,
                    TeamName = "TeamNameK",
                    Points = 0,
                    SetsRatio = null,
                    BallsRatio = null
                },
                divisionId: 2);
            AddTeamStandings(// L
                new TeamStandingsDto
                {
                    TeamId = 12,
                    TeamName = "TeamNameL",
                    Points = 0,
                    SetsRatio = null,
                    BallsRatio = null
                },
                divisionId: 2);

            AddDivision1Group1Results();
            AddDivision1Group2Results();
            AddDivision2Group1Results();

            return this;
        }

        public PivotStandingsTestFixture WithMultipleDivisionsEmptyStandings()
        {
            AddTeamStandings(// A
                   new TeamStandingsDto
                   {
                       TeamId = 1,
                       TeamName = "TeamNameA",
                       Points = 0,
                       SetsRatio = null,
                       BallsRatio = null
                   },
                   divisionId: 1);
            AddTeamStandings(// B
                new TeamStandingsDto
                {
                    TeamId = 2,
                    TeamName = "TeamNameB",
                    Points = 0,
                    SetsRatio = null,
                    BallsRatio = null
                },
                divisionId: 1);
            AddTeamStandings(// C
                new TeamStandingsDto
                {
                    TeamId = 3,
                    TeamName = "TeamNameC",
                    Points = 0,
                    SetsRatio = null,
                    BallsRatio = null
                },
                divisionId: 1);
            AddTeamStandings(// D
                new TeamStandingsDto
                {
                    TeamId = 4,
                    TeamName = "TeamNameD",
                    Points = 0,
                    SetsRatio = null,
                    BallsRatio = null
                },
                divisionId: 1);
            AddTeamStandings(// E
                new TeamStandingsDto
                {
                    TeamId = 5,
                    TeamName = "TeamNameE",
                    Points = 0,
                    SetsRatio = null,
                    BallsRatio = null
                },
                divisionId: 1);
            AddTeamStandings(// F
                new TeamStandingsDto
                {
                    TeamId = 6,
                    TeamName = "TeamNameF",
                    Points = 0,
                    SetsRatio = null,
                    BallsRatio = null
                },
                divisionId: 1);
            AddTeamStandings(// G
                new TeamStandingsDto
                {
                    TeamId = 7,
                    TeamName = "TeamNameG",
                    Points = 0,
                    SetsRatio = null,
                    BallsRatio = null
                },
                divisionId: 2);
            AddTeamStandings(// H
                new TeamStandingsDto
                {
                    TeamId = 8,
                    TeamName = "TeamNameH",
                    Points = 0,
                    SetsRatio = null,
                    BallsRatio = null
                },
                divisionId: 2);
            AddTeamStandings(// I
                new TeamStandingsDto
                {
                    TeamId = 9,
                    TeamName = "TeamNameI",
                    Points = 0,
                    SetsRatio = null,
                    BallsRatio = null
                },
                divisionId: 2);
            AddTeamStandings(// J
                new TeamStandingsDto
                {
                    TeamId = 10,
                    TeamName = "TeamNameJ",
                    Points = 0,
                    SetsRatio = null,
                    BallsRatio = null
                },
                divisionId: 2);
            AddTeamStandings(// K
                new TeamStandingsDto
                {
                    TeamId = 11,
                    TeamName = "TeamNameK",
                    Points = 0,
                    SetsRatio = null,
                    BallsRatio = null
                },
                divisionId: 2);
            AddTeamStandings(// L
                new TeamStandingsDto
                {
                    TeamId = 12,
                    TeamName = "TeamNameL",
                    Points = 0,
                    SetsRatio = null,
                    BallsRatio = null
                },
                divisionId: 2);

            return this;
        }

        public PivotStandingsTestFixture WithLastUpdateTime(DateTime? lastUpdateTime)
        {
            _lastUpdateTime = lastUpdateTime;

            return this;
        }

        public TournamentStandings<PivotStandingsDto> Build()
        {
            var result = new TournamentStandings<PivotStandingsDto>();

            var uniqueDivisions = _teamStandings.Select(t => t.DivisionId)
                .Union(_gameResults.Select(gr => gr.DivisionId)).Distinct().ToList();

            result.Divisions = uniqueDivisions.Select(divId =>
                {
                    return new PivotStandingsDto(
                        _teamStandings.Where(t => t.DivisionId == divId).Select(t => t.Standings).ToList(),
                        _gameResults.Where(g => g.DivisionId == divId).Select(g => g.Result).ToList())
                    {
                        DivisionId = divId,
                        DivisionName = $"Division {divId}",
                        LastUpdateTime = _lastUpdateTime
                    };
                })
                .ToList();

            return result;
        }

        private void AddTeamStandings(TeamStandingsDto standing, int divisionId = 1)
        {
            _teamStandings.Add((divisionId, standing));
        }

        private void AddGameResult(ShortGameResultDto result, int divisionId = 1)
        {
            _gameResults.Add((divisionId, result));
        }

        private void AddDivision1Group1Results()
        {
            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeGameScore = 3,
                AwayGameScore = 0,
                IsTechnicalDefeat = false
            });
            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 1,
                AwayTeamId = 3,
                HomeGameScore = 3,
                AwayGameScore = 1,
                IsTechnicalDefeat = false
            });
            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 2,
                AwayTeamId = 3,
                HomeGameScore = 3,
                AwayGameScore = 2,
                IsTechnicalDefeat = false
            });
            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeGameScore = 2,
                AwayGameScore = 3,
                IsTechnicalDefeat = false
            });
            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 1,
                AwayTeamId = 3,
                HomeGameScore = 1,
                AwayGameScore = 3,
                IsTechnicalDefeat = false
            });
            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 2,
                AwayTeamId = 3,
                HomeGameScore = 0,
                AwayGameScore = 3,
                IsTechnicalDefeat = false
            });
        }

        private void AddDivision1Group2Results()
        {
            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 5,
                AwayTeamId = 6,
                HomeGameScore = 3,
                AwayGameScore = 0,
                IsTechnicalDefeat = false
            });
            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 5,
                AwayTeamId = 4,
                HomeGameScore = 3,
                AwayGameScore = 1,
                IsTechnicalDefeat = false
            });
            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 6,
                AwayTeamId = 4,
                HomeGameScore = 3,
                AwayGameScore = 2,
                IsTechnicalDefeat = false
            });
            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 5,
                AwayTeamId = 6,
                HomeGameScore = 2,
                AwayGameScore = 3,
                IsTechnicalDefeat = false
            });
            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 5,
                AwayTeamId = 4,
                HomeGameScore = 1,
                AwayGameScore = 3,
                IsTechnicalDefeat = false
            });
            AddGameResult(new ShortGameResultDto
            {
                HomeTeamId = 6,
                AwayTeamId = 4,
                HomeGameScore = 0,
                AwayGameScore = 3,
                IsTechnicalDefeat = false
            });
        }

        private void AddDivision2Group1Results()
        {
            AddGameResult(
                new ShortGameResultDto
                {
                    HomeTeamId = 7,
                    AwayTeamId = 8,
                    HomeGameScore = 3,
                    AwayGameScore = 0,
                    IsTechnicalDefeat = false
                },
                divisionId: 2);
            AddGameResult(
                new ShortGameResultDto
                {
                    HomeTeamId = 7,
                    AwayTeamId = 9,
                    HomeGameScore = 3,
                    AwayGameScore = 1,
                    IsTechnicalDefeat = false
                },
                divisionId: 2);
            AddGameResult(
                new ShortGameResultDto
                {
                    HomeTeamId = 8,
                    AwayTeamId = 9,
                    HomeGameScore = 3,
                    AwayGameScore = 2,
                    IsTechnicalDefeat = false
                },
                divisionId: 2);
            AddGameResult(
                new ShortGameResultDto
                {
                    HomeTeamId = 7,
                    AwayTeamId = 8,
                    HomeGameScore = 2,
                    AwayGameScore = 3,
                    IsTechnicalDefeat = false
                },
                divisionId: 2);
            AddGameResult(
                new ShortGameResultDto
                {
                    HomeTeamId = 7,
                    AwayTeamId = 9,
                    HomeGameScore = 1,
                    AwayGameScore = 3,
                    IsTechnicalDefeat = false
                },
                divisionId: 2);
            AddGameResult(
                new ShortGameResultDto
                {
                    HomeTeamId = 8,
                    AwayTeamId = 9,
                    HomeGameScore = 0,
                    AwayGameScore = 3,
                    IsTechnicalDefeat = false
                },
                divisionId: 2);
        }
    }
}