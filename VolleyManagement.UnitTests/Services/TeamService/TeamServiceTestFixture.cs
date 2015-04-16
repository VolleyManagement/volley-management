namespace VolleyManagement.UnitTests.Services.TeamService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.Teams;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Players;

    /// <summary>
    /// Class for generating test data
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TeamServiceTestFixture
    {
        /// <summary>
        /// Holds collection of teams
        /// </summary>
        private IList<Team> _teams = new List<Team>();

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
                Coach = "TeamCoachA",
                Achievements = "TeamAchievementsA"
            });
            _teams.Add(new Team()
            {
                Id = 2,
                Name = "TeamNameB",
                Coach = "TeamCoachB",
                Achievements = "TeamAchievementsB"
            });
            _teams.Add(new Team()
            {
                Id = 3,
                Name = "TeamNameC",
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
        /// Builds test data
        /// </summary>
        /// <returns>Team collection</returns>
        public IList<Team> Build()
        {
            return _teams;
        }
    }
}