namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using UI.Areas.Mvc.ViewModels.Players;
    using UI.Areas.Mvc.ViewModels.Teams;

    /// <summary>
    /// Builder for test MVC tournament view models
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class TeamMvcViewModelBuilder
    {
        /// <summary>
        /// Holds test tournament view model instance
        /// </summary>
        private TeamViewModel _teamViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamMvcViewModelBuilder"/> class
        /// </summary>
        public TeamMvcViewModelBuilder()
        {
            _teamViewModel = new TeamViewModel() {
                Id = 1,
                Name = "Name",
                Coach = "Coach",
                Achievements = "Achievements",
                Captain = new PlayerNameViewModel() { Id = 1, FirstName = "First", LastName = "Player" },
                Roster = new List<PlayerNameViewModel>()
                {
                    new PlayerNameViewModel() { Id = 3, FirstName = "Third", LastName = "Player" },
                    new PlayerNameViewModel() { Id = 4, FirstName = "Fourth", LastName = "Player" }
                },
                AddedPlayers = new List<PlayerNameViewModel>
                {
                    new PlayerNameViewModel() { Id = 1, FirstName = "First", LastName = "Player" },
                    new PlayerNameViewModel() { Id = 2, FirstName = "Second", LastName = "Player" }
                }
            };
        }

        /// <summary>
        /// Sets id of test team view model
        /// </summary>
        /// <param name="id">Id for test team view model</param>
        /// <returns>Team view model builder object</returns>
        public TeamMvcViewModelBuilder WithId(int id)
        {
            _teamViewModel.Id = id;
            return this;
        }

        /// <summary>
        /// Sets name of test team view model
        /// </summary>
        /// <param name="name">Name for test team view model</param>
        /// <returns>Team view model builder object</returns>
        public TeamMvcViewModelBuilder WithName(string name)
        {
            _teamViewModel.Name = name;
            return this;
        }

        /// <summary>
        /// Sets coach of test team view model
        /// </summary>
        /// <param name="coach">Coach for test team view model</param>
        /// <returns>Team view model builder object</returns>
        public TeamMvcViewModelBuilder WithCoach(string coach)
        {
            _teamViewModel.Coach = coach;
            return this;
        }

        /// <summary>
        /// Sets achievements of test team view model
        /// </summary>
        /// <param name="achievements">Coach for test team view model</param>
        /// <returns>Team view model builder object</returns>
        public TeamMvcViewModelBuilder WithAchievements(string achievements)
        {
            _teamViewModel.Achievements = achievements;
            return this;
        }

        /// <summary>
        /// Sets captain of test team view model
        /// </summary>
        /// <param name="captain">Captain for test team view model</param>
        /// <returns>Team view model builder object</returns>
        public TeamMvcViewModelBuilder WithCaptain(PlayerNameViewModel captain)
        {
            _teamViewModel.Captain = captain;
            return this;
        }

        /// <summary>
        /// Sets roster of test team view model
        /// </summary>
        /// <param name="roster">Roster for test team view model</param>
        /// <returns>Team view model builder object</returns>
        public TeamMvcViewModelBuilder WithRoster(List<PlayerNameViewModel> roster)
        {
            _teamViewModel.Roster = roster;
            return this;
        }

        /// <summary>
        /// Builds test team view model
        /// </summary>
        /// <returns>test team view model</returns>
        public TeamViewModel Build()
        {
            return _teamViewModel;
        }
    }
}
