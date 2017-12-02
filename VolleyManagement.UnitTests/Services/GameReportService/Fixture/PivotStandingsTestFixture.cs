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
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Adds team standings that correspond game results with all possible scores
        /// to collection of <see cref="TeamStandingsDto"/> objects.
        /// </summary>
        /// <returns>Instance of <see cref="PivotStandingsTestFixture"/>.</returns>
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
                TeamId = 2,
                TeamName = "TeamNameB",
                Points = 4,
                SetsRatio = 6.0f / 10,
                BallsRatio = 349.0f / 345
            });
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 3,
                TeamName = "TeamNameC",
                Points = 7,
                SetsRatio = 9.0f / 7,
                BallsRatio = 350.0f / 362
            });

            return this;
        }

        public PivotStandingsTestFixture WithTeamStandingsTwoTeamsScoresCompletelyEqual()
        {
            _teamStandings.Clear();
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 2,
                TeamName = "TeamNameB",
                Points = 3,
                SetsRatio = 3.0f / 1,
                BallsRatio = 102.0f / 96
            });
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 3,
                TeamName = "TeamNameC",
                Points = 3,
                SetsRatio = 3.0f / 1,
                BallsRatio = 102.0f / 96
            });
            AddTeamStandings(new TeamStandingsDto
            {
                TeamId = 1,
                TeamName = "TeamNameA",
                Points = 0,
                SetsRatio = 2.0f / 6,
                BallsRatio = 192.0f / 204
            });
            return this;
        }

        /// <summary>
        /// Adds team standings that correspond game results with one game with technical defeat
        /// to collection of <see cref="TeamStandingsDto"/> objects.
        /// </summary>
        /// <returns>Instance of <see cref="PivotStandingsTestFixture"/>.</returns>
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

        /// <summary>
        /// Builds instance of <see cref="PivotStandingsTestFixture"/>.
        /// </summary>
        /// <returns>Collection of <see cref="TeamStandingsDto"/> objects filled with test data.</returns>
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