namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using UI.Areas.WebApi.ViewModels.Teams;

    /// <summary>
    /// Builder for test team view models
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class TeamViewModelBuilder
    {
        /// <summary>
        /// Holds test team view model instance
        /// </summary>
        private TeamViewModel _teamViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamViewModelBuilder" /> class
        /// </summary>
        public TeamViewModelBuilder()
        {
            _teamViewModel = new TeamViewModel()
            {
                Id = 1,
                Name = "TeamNameA",
                CaptainId = 1,
                Coach = "TeamCoachA",
                Achievements = "TeamAchievementsA"
            };
        }

        /// <summary>
        /// Sets id of test team view model
        /// </summary>
        /// <param name="id">Id for test team view model</param>
        /// <returns>Team view model builder object</returns>
        public TeamViewModelBuilder WithId(int id)
        {
            _teamViewModel.Id = id;
            return this;
        }

        /// <summary>
        /// Sets the captain id of test team view model
        /// </summary>
        /// <param name="captainId">Captain id for test team view model</param>
        /// <returns>Team view model builder object</returns>
        public TeamViewModelBuilder WithCaptainId(int captainId)
        {
            _teamViewModel.CaptainId = captainId;
            return this;
        }

        /// <summary>
        /// Sets the name of test team view model
        /// </summary>
        /// <param name="name">Name for test team view model</param>
        /// <returns>Team view model builder object</returns>
        public TeamViewModelBuilder WithName(string name)
        {
            _teamViewModel.Name = name;
            return this;
        }

        /// <summary>
        /// Sets the coach of test team view model
        /// </summary>
        /// <param name="coach">Coach for test team view model</param>
        /// <returns>Team view model builder object</returns>
        public TeamViewModelBuilder WithCoach(string coach)
        {
            _teamViewModel.Coach = coach;
            return this;
        }

        /// <summary>
        /// Sets the achievements of test team view model
        /// </summary>
        /// <param name="achievements">Achievements for test team view model</param>
        /// <returns>Team view model builder object</returns>
        public TeamViewModelBuilder WithAchievements(string achievements)
        {
            _teamViewModel.Achievements = achievements;
            return this;
        }

        /// <summary>
        /// Builds test team view model
        /// </summary>
        /// <returns>Test team view model</returns>
        public TeamViewModel Build()
        {
            return _teamViewModel;
        }
    }
}
