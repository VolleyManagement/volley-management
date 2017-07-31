namespace VolleyManagement.UnitTests.Services.TeamService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.TeamsAggregate;
    using PlayerService;
    using VolleyManagement.Contracts;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Teams;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments;

    /// <summary>
    /// Class for generating test data
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class GroupTeamServiceTestFixture
    {
        /// <summary>
        /// Holds collection of teams and groups
        /// </summary>
        private List<TeamTournamentAssignmentDto> _groupteams = new List<TeamTournamentAssignmentDto>();

        /// <summary>
        /// Return test collection of teams and groups
        /// </summary>
        /// <returns>Builder object with collection of teams and groups</returns>
        public GroupTeamServiceTestFixture TestGroupsTeams()
        {
            _groupteams.Add(new TeamTournamentAssignmentDto()
            {
                GroupId = 1,
                TeamId = 1,
            });
            _groupteams.Add(new TeamTournamentAssignmentDto()
            {
                GroupId = 2,
                TeamId = 2,
            });
            _groupteams.Add(new TeamTournamentAssignmentDto()
            {
                GroupId = 1,
                TeamId = 3,
            });
            return this;
        }

        public GroupTeamServiceTestFixture TestGroupsTeamsWithAlreadyExistTeam()
        {
            _groupteams.Add(new TeamTournamentAssignmentDto()
            {
                GroupId = 1,
                TeamId = 4,
            });
            _groupteams.Add(new TeamTournamentAssignmentDto()
            {
                GroupId = 2,
                TeamId = 2,
            });
            return this;
        }

        public GroupTeamServiceTestFixture TestGroupsTeamsWithTeamInSecondDivisionSecondGroup()
        {
            _groupteams.Add(new TeamTournamentAssignmentDto()
            {
                GroupId = 4,
                TeamId = 1,
            });
            return this;
        }

        /// <summary>
        /// Add teams and groups to collection.
        /// </summary>
        /// <param name="newGroupTeam">Team to add.</param>
        /// <returns>Builder object with collection of teams and groups.</returns>
        public GroupTeamServiceTestFixture AddTeam(TeamTournamentAssignmentDto newGroupTeam)
        {
            _groupteams.Add(newGroupTeam);
            return this;
        }

        /// <summary>
        /// Builds test data
        /// </summary>
        /// <returns>Team and Group collection</returns>
        public List<TeamTournamentAssignmentDto> Build()
        {
            return _groupteams;
        }
    }
}
