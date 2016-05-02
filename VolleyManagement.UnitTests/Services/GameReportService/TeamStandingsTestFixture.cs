namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using VolleyManagement.Domain.GameReportsAggregate;

    /// <summary>
    /// Generates <see cref="TeamStandingsDto"/> test data for unit tests for <see cref="GameReportService"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class TeamStandingsTestFixture
    {
        private List<TeamStandingsDto> _teamStandings = new List<TeamStandingsDto>();

        /// <summary>
        /// Generates <see cref="TeamStandingsDto"/> objects filled with test data.
        /// </summary>
        /// <returns>Instance of <see cref="TeamStandingsTestFixture"/></returns>
        public TeamStandingsTestFixture TestTeamStandings()
        {
            _teamStandings.Add(new TeamStandingsDto
            {
                TeamId = 1,
                TeamName = "TeamNameA",
                Points = 5,
                SetsRatio = 6.0f / 3,
                BallsRatio = 234.0f / 214
            });
            _teamStandings.Add(new TeamStandingsDto
            {
                TeamId = 3,
                TeamName = "TeamNameC",
                Points = 3,
                SetsRatio = 4.0f / 3,
                BallsRatio = 166.0f / 105
            });
            _teamStandings.Add(new TeamStandingsDto
            {
                TeamId = 2,
                TeamName = "TeamNameB",
                Points = 1,
                SetsRatio = 2.0f / 6,
                BallsRatio = 123.0f / 204
            });

            return this;
        }

        /// <summary>
        /// Adds team standings that correspond game results with all possible scores
        /// to collection of <see cref="TeamStandingsDto"/> objects.
        /// </summary>
        /// <returns>Instance of <see cref="TeamStandingsTestFixture"/>.</returns>
        public TeamStandingsTestFixture WithTeamStandingsForAllPossibleScores()
        {
            _teamStandings.Clear();
            _teamStandings.Add(new TeamStandingsDto
            {
                TeamId = 1,
                TeamName = "TeamNameA",
                Points = 7,
                SetsRatio = 9.0f / 7
            });
            _teamStandings.Add(new TeamStandingsDto
            {
                TeamId = 2,
                TeamName = "TeamNameB",
                Points = 4,
                SetsRatio = 6.0f / 10,
            });
            _teamStandings.Add(new TeamStandingsDto
            {
                TeamId = 3,
                TeamName = "TeamNameC",
                Points = 7,
                SetsRatio = 9.0f / 7
            });

            return this;
        }

        /// <summary>
        /// Orders team standings by points in descending order.
        /// </summary>
        /// <returns>Instance of <see cref="TeamStandingsTestFixture"/>.</returns>
        public TeamStandingsTestFixture OrderByPoints()
        {
            _teamStandings = _teamStandings.OrderByDescending(ts => ts.Points).ToList();
            return this;
        }

        /// <summary>
        /// Orders standings by points, then by sets ratio in descending order.
        /// </summary>
        /// <returns>Instance of <see cref="TeamStandingsTestFixture"/>.</returns>
        public TeamStandingsTestFixture OrderByPointsAndSets()
        {
            _teamStandings = _teamStandings.OrderByDescending(ts => ts.Points).ThenByDescending(ts => ts.SetsRatio).ToList();
            return this;
        }

        /// <summary>
        /// Builds instance of <see cref="TeamStandingsTestFixture"/>.
        /// </summary>
        /// <returns>Collection of <see cref="TeamStandingsDto"/> objects filled with test data.</returns>
        public List<TeamStandingsDto> Build()
        {
            return _teamStandings;
        }
    }
}
