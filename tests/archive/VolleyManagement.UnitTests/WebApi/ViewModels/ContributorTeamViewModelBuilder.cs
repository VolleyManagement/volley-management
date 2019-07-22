namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using UI.Areas.WebApi.ViewModels.ContributorsTeam;

    /// <summary>
    /// Builder for test contributors team view models
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class ContributorTeamViewModelBuilder
    {
        /// <summary>
        /// Holds test contributors team view model instance
        /// </summary>
        private ContributorsTeamViewModel _contributorTeamViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContributorTeamViewModelBuilder"/> class
        /// </summary>
        public ContributorTeamViewModelBuilder()
        {
            _contributorTeamViewModel = new ContributorsTeamViewModel()
            {
                Id = 1,
                Name = "FirstName",
                CourseDirection = "Course",
                Contributors = new List<string> { "FirstNameA", "FirstNameB", "FirstNameC" }
            };
        }

        /// <summary>
        /// Sets id of test contributors team view model
        /// </summary>
        /// <param name="id">Id for test contributors team view model</param>
        /// <returns>Contributors team view model builder object</returns>
        public ContributorTeamViewModelBuilder WithId(int id)
        {
            _contributorTeamViewModel.Id = id;
            return this;
        }

        /// <summary>
        /// Sets id of test contributor view model
        /// </summary>
        /// <param name="name">Name for test contributors team view model</param>
        /// <returns>Contributor view model builder object</returns>
        public ContributorTeamViewModelBuilder WithName(string name)
        {
            _contributorTeamViewModel.Name = name;
            return this;
        }

        /// <summary>
        /// Sets id of test contributor view model
        /// </summary>
        /// <param name="courseDirection">course direction for test contributor view model</param>
        /// <returns>Contributor view model builder object</returns>
        public ContributorTeamViewModelBuilder WithcourseDirection(string courseDirection)
        {
            _contributorTeamViewModel.CourseDirection = courseDirection;
            return this;
        }

        /// <summary>
        /// Sets id of test contributor view model
        /// </summary>
        /// <param name="contributors">Contributors in team for test contributor view model</param>
        /// <returns>Contributor view model builder object</returns>
        public ContributorTeamViewModelBuilder Withcontributors(IList<string> contributors)
        {
            foreach (var item in contributors)
            {
                _contributorTeamViewModel.Contributors.Add(item);
            }

            return this;
        }

        /// <summary>
        /// Builds test contributor team view model
        /// </summary>
        /// <returns>test contributor view model</returns>
        public ContributorsTeamViewModel Build()
        {
            return _contributorTeamViewModel;
        }
    }
}
