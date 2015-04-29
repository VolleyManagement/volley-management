namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.ContributorsAggregate;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.ContributorsTeam;



    /// <summary>
    /// Builder for test contributor view models
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class ContributorTeamViewModelBuilder
    {
        /// <summary>
        /// Holds test contributor view model instance
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
        /// Sets id of test contributor view model
        /// </summary>
        /// <param name="id">Id for test contributor view model</param>
        /// <returns>Contributor view model builder object</returns>
        public ContributorTeamViewModelBuilder WithId(int id)
        {
            _contributorTeamViewModel.Id = id;
            return this;
        }

        /// <summary>
        /// Sets id of test contributor view model
        /// </summary>
        /// <param name="name">FirstName for test contributor view model</param>
        /// <returns>Contributor view model builder object</returns>
        public ContributorTeamViewModelBuilder WithName(string name)
        {
            this._contributorTeamViewModel.Name = name;
            return this;
        }

        /// <summary>
        /// Sets id of test contributor view model
        /// </summary>
        /// <param name="contributorTeamId">ContributorTeamId for test contributor view model</param>
        /// <returns>Contributor view model builder object</returns>
        public ContributorTeamViewModelBuilder WithcourseDirection(string courseDirection)
        {
            this._contributorTeamViewModel.CourseDirection = courseDirection;
            return this;
        }

        /// <summary>
        /// Sets id of test contributor view model
        /// </summary>
        /// <param name="contributorTeamId">ContributorTeamId for test contributor view model</param>
        /// <returns>Contributor view model builder object</returns>
        public ContributorTeamViewModelBuilder Withcontributors(IList<Contributor> contributors)
        {
            this._contributorTeamViewModel.Contributors = contributors;
            return this;
        }

        /// <summary>
        /// Builds test contributor view model
        /// </summary>
        /// <returns>test contributor view model</returns>
        public ContributorsTeamViewModel Build()
        {
            return _contributorTeamViewModel;
        }
    }
}
