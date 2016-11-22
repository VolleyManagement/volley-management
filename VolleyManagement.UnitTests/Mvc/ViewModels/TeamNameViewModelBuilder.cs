using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyManagement.UI.Areas.Mvc.ViewModels.Teams;

namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
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
                _teamNameViewModel,
                _teamNameViewModel
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
