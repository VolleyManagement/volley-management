namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.ContributorsAggregate;
    using UI.Areas.Mvc.ViewModels.ContributorsTeam;

    /// <summary>
    /// Builder for test MVC contributor team view models
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class ContributorTeamMvcViewModelBuilder
    {
        /// <summary>
        /// Holds test contributor team view model instance
        /// </summary>
        private ContributorsTeamViewModel _contributorTeamViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContributorTeamMvcViewModelBuilder"/> class
        /// </summary>
        public ContributorTeamMvcViewModelBuilder()
        {
            _contributorTeamViewModel = new ContributorsTeamViewModel()
            {
                Id = 1,
                Name = "FirstName",
                CourseDirection = "Course",
                Contributors = new List<Contributor>
                {
                    new Contributor
                    {
                        Id = 1,
                        Name = "FirstNameA",
                        ContributorTeamId = 1
                    },
                    new Contributor
                    {
                        Id = 2,
                        Name = "FirstNameB",
                        ContributorTeamId = 1
                    },
                    new Contributor
                    {
                        Id = 3,
                        Name = "FirstNameC",
                        ContributorTeamId = 1
                    }
                }
            };
        }

        /// <summary>
        /// Sets id of test contributor team view model
        /// </summary>
        /// <param name="id">Id for test contributor team view model</param>
        /// <returns>Contributor view model builder object</returns>
        public ContributorTeamMvcViewModelBuilder WithId(int id)
        {
            _contributorTeamViewModel.Id = id;
            return this;
        }

        /// <summary>
        /// Sets id of test contributor team view model
        /// </summary>
        /// <param name="name">Name for test contributor team view model</param>
        /// <returns>Contributor view model builder object</returns>
        public ContributorTeamMvcViewModelBuilder WithName(string name)
        {
            _contributorTeamViewModel.Name = name;
            return this;
        }

        /// <summary>
        /// Sets id of test contributor view model
        /// </summary>
        /// <param name="courseDirection">CourseDirection for test contributor team view model</param>
        /// <returns>Contributor view model builder object</returns>
        public ContributorTeamMvcViewModelBuilder WithCourseDirection(string courseDirection)
        {
            _contributorTeamViewModel.CourseDirection = courseDirection;
            return this;
        }

        /// <summary>
        /// Sets id of test contributor view model
        /// </summary>
        /// <param name="contributors">Collection of contributors for test contributor team view model</param>
        /// <returns>Contributor view model builder object</returns>
        public ContributorTeamMvcViewModelBuilder WithContributors(List<Contributor> contributors)
        {
            _contributorTeamViewModel.Contributors = contributors;
            return this;
        }

        /// <summary>
        /// Builds test contributor team view model
        /// </summary>
        /// <returns>test contributor team view model</returns>
        public ContributorsTeamViewModel Build()
        {
            return _contributorTeamViewModel;
        }
    }
}
