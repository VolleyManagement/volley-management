namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System;
    using System.Collections.Generic;
    using UI.Areas.Mvc.ViewModels.Teams;

    public class TeamNameViewModelBuilder
    {
        private TeamNameViewModel _teamNameViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamNameViewModelBuilder"/> class
        /// </summary>
        public TeamNameViewModelBuilder()
        {
            _teamNameViewModel = new TeamNameViewModel()
            {
                Id = 1,
                Name = "Name"      
            };
        }

        /// <summary>
        /// Sets id of test team view model
        /// </summary>
        /// <param name="id">Id for test team view model</param>
        /// <returns>Team view model builder object</returns>
        public TeamNameViewModelBuilder WithId(int id)
        {
            _teamNameViewModel.Id = id;
            return this;
        }

        /// <summary>
        /// Sets name of test team view model
        /// </summary>
        /// <param name="name">Name for test team view model</param>
        /// <returns>Team view model builder object</returns>
        public TeamNameViewModelBuilder WithName(string name)
        {
            _teamNameViewModel.Name = name;
            return this;
        }

        /// <summary>
        /// Builds test team view model
        /// </summary>
        /// <returns>test team view model</returns>
        public IEnumerable<TeamNameViewModel> GetList()
        {
            return new List<TeamNameViewModel>()
            {
                new TeamNameViewModel()
                {
                    Id = 1,
                    Name = "TeamNameA",
                },
                new TeamNameViewModel()
                {
                    Id = 2,
                    Name = "TeamNameB",
                },
                new TeamNameViewModel()
                {
                    Id = 3,
                    Name = "TeamNameC",                  
                },
            };
        }

        /// <summary>
        /// Builds test team view model
        /// </summary>
        /// <returns>test team view model</returns>
        public TeamNameViewModel Build()
        {
            return _teamNameViewModel;
        }
    }
}