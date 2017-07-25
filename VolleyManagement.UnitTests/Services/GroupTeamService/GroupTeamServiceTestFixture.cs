namespace VolleyManagement.UnitTests.Services.TeamService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.TeamsAggregate;
    using PlayerService;
    using VolleyManagement.Domain.GroupTeamAggregate;

    /// <summary>
    /// Class for generating test data
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class GroupTeamServiceTestFixture
    {
        /// <summary>
        /// Holds collection of teams
        /// </summary>
        private List<GroupTeam> _groupteams = new List<GroupTeam>();

        /// <summary>
        /// Holds collection of teams
        /// </summary>
        private PlayerBuilder _playerBuilder;

        /// <summary>
        /// Return test collection of teams
        /// </summary>
        /// <returns>Builder object with collection of teams</returns>
        public GroupTeamServiceTestFixture TestGroupsTeams()
        {
            _playerBuilder = new PlayerBuilder();

            _groupteams.Add(new GroupTeam()
            {
                GroupId = 1,
                TeamId = 1,
                TournamentId = 1,
            });
            _groupteams.Add(new GroupTeam()
            {
                GroupId = 2,
                TeamId = 2,
                TournamentId = 1,
            });
            _groupteams.Add(new GroupTeam()
            {
                GroupId = 1,
                TeamId = 3,
                TournamentId = 1,
            });
            return this;
        }

        /// <summary>
        /// Add player to collection.
        /// </summary>
        /// <param name="newGroupTeam">Team to add.</param>
        /// <returns>Builder object with collection of teams.</returns>
        public GroupTeamServiceTestFixture AddTeam(GroupTeam newGroupTeam)
        {
            _groupteams.Add(newGroupTeam);
            return this;
        }

        /// <summary>
        /// Builds test data
        /// </summary>
        /// <returns>Team collection</returns>
        public List<GroupTeam> Build()
        {
            return _groupteams;
        }
    }
}
