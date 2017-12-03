namespace VolleyManagement.UnitTests.Services.GameReportService
{
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

        public PivotStandingsTestFixture WithEmptyStandings()
        {
            // do nothing it will build itself with proper empty object
            return this;
        }

        /// <summary>
        /// Generates <see cref="TeamStandingsDto"/> objects filled with test data.
        /// </summary>
        /// <returns>Instance of <see cref="PivotStandingsTestFixture"/></returns>
        public PivotStandingsTestFixture DefaultStandings(int divisionId = 1)
        {
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 1,
                TeamName = "TeamNameA",
                Points = 5,
                SetsRatio = 6.0f / 3,
                BallsRatio = 234.0f / 214
            });
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 3,
                TeamName = "TeamNameC",
                Points = 3,
                SetsRatio = 4.0f / 3,
                BallsRatio = 166.0f / 105
            });
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 2,
                TeamName = "TeamNameB",
                Points = 1,
                SetsRatio = 2.0f / 6,
                BallsRatio = 123.0f / 204
            });

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

        public PivotStandingsTestFixture WithTeamStandingsForOneGameTechnicalDefeat()
        {
            _teamStandings.Clear();
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 1,
                TeamName = "TeamNameA",
                Points = 5,
                SetsRatio = 6.0f / 3,
                BallsRatio = 234.0f / 214
            });
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 3,
                TeamName = "TeamNameC",
                Points = 3,
                SetsRatio = 4.0f / 3,
                BallsRatio = 91.0f / 105
            });
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 2,
                TeamName = "TeamNameB",
                Points = 1,
                SetsRatio = 2.0f / 6,
                BallsRatio = 123.0f / 129
            });

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
                        DivisionName = $"Division {divId}"
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
    }
}