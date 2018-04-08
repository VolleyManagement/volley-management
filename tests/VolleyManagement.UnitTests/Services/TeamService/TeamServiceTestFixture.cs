namespace VolleyManagement.UnitTests.Services.TeamService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.TeamsAggregate;

    /// <summary>
    /// Class for generating test data
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TeamServiceTestFixture
    {
        /// <summary>
        /// Holds collection of teams
        /// </summary>
        private List<Team> _teams = new List<Team>();

        /// <summary>
        /// Holds collection of teams collections
        /// </summary>
        private List<List<Team>> _teamsByDivisions = new List<List<Team>>();

        /// <summary>
        /// Return test collection of teams
        /// </summary>
        /// <returns>Builder object with collection of teams</returns>
        public TeamServiceTestFixture TestTeams()
        {
            _teams.Add(new Team()
            {
                Id = 1,
                Name = "TeamNameA",
                Captain = 1,
                Coach = "TeamCoachA",
                Achievements = "TeamAchievementsA"
            });
            _teams.Add(new Team()
            {
                Id = 2,
                Name = "TeamNameB",
                Captain = 2,
                Coach = "TeamCoachB",
                Achievements = "TeamAchievementsB"
            });
            _teams.Add(
            new Team()
            {
                Id = 3,
                Name = "TeamNameC",
                Captain = 3,
                Coach = "TeamCoachC",
                Achievements = "TeamAchievementsC"
            });
            return this;
        }

        /// <summary>
        /// Add player to collection.
        /// </summary>
        /// <param name="newTeam">Team to add.</param>
        /// <returns>Builder object with collection of teams.</returns>
        public TeamServiceTestFixture AddTeam(Team newTeam)
        {
            _teams.Add(newTeam);
            return this;
        }

        /// <summary>
        /// Add collection of teams to collection.
        /// </summary>
        /// <param name="newTeams">Teams to add.</param>
        /// <returns>Builder object with collection of teams colections.</returns>
        public TeamServiceTestFixture AddTeams(List<Team> newTeams)
        {
            _teamsByDivisions.Add(newTeams);
            return this;
        }

        /// <summary>
        /// Builds test data
        /// </summary>
        /// <returns>Team collection</returns>
        public List<Team> Build()
        {
            return _teams;
        }

        /// <summary>
        /// Builds test data
        /// </summary>
        /// <returns>Collection of teams collections</returns>
        public List<List<Team>> BuildWithDivisions()
        {
            return _teamsByDivisions;
        }
    }
}
