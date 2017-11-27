namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using UI.Areas.WebApi.ViewModels.Teams;

    /// <summary>
    /// Class for generating test data
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class TeamViewModelServiceTestFixture
    {
        /// <summary>
        /// Holds collection of teams
        /// </summary>
        private IList<TeamViewModel> _teams = new List<TeamViewModel>();

        /// <summary>
        /// Return test collection of teams
        /// </summary>
        /// <returns>Builder object with collection of teams</returns>
        public TeamViewModelServiceTestFixture TestTeams()
        {
            _teams.Add(new TeamViewModel()
            {
                Id = 1,
                Name = "TeamNameA",
                CaptainId = 1,
                Coach = "TeamCoachA",
                Achievements = "TeamAchievementsA"
            });
            _teams.Add(new TeamViewModel()
            {
                Id = 2,
                Name = "TeamNameB",
                CaptainId = 2,
                Coach = "TeamCoachB",
                Achievements = "TeamAchievementsB"
            });
            _teams.Add(new TeamViewModel()
            {
                Id = 3,
                Name = "TeamNameC",
                CaptainId = 3,
                Coach = "TeamCoachC",
                Achievements = "TeamAchievementsC"
            });
            return this;
        }

        /// <summary>
        /// Builds test data
        /// </summary>
        /// <returns>Teams collection</returns>
        public IList<TeamViewModel> Build()
        {
            return _teams;
        }
    }
}
