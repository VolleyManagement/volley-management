namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.TeamsAggregate;

    [ExcludeFromCodeCoverage]
    public class TeamInTournamentTestFixture
    {
        private readonly List<TeamTournamentDto> _teams = new List<TeamTournamentDto>();

        /// <summary>
        /// Return test collection of teams grouped by divisions
        /// </summary>
        /// <returns>Builder object with collection of teams collection</returns>
        public TeamInTournamentTestFixture TestTeamsByDivisions()
        {

            return this;
        }

        /// <summary>
        /// Return test collection of teams grouped by divisions
        /// </summary>
        /// <returns>Builder object with collection of teams collection</returns>
        public TeamInTournamentTestFixture WithTeamsInSingleDivisionSingleGroup()
        {
            _teams.Add(CreateTeam(1, "A"));
            _teams.Add(CreateTeam(2, "B"));
            _teams.Add(CreateTeam(3, "C"));
            return this;
        }

        public List<TeamTournamentDto> Build()
        {
            return _teams;
        }

        private static TeamTournamentDto CreateTeam(int teamId, string teamName, int divisionId = 1, int groupId = 1)
        {
            return new TeamTournamentDto()
            {
                TeamId = teamId,
                TeamName = $"TeamName{teamName}",
                DivisionId = divisionId,
                GroupId = groupId
            };
        }
    }
}