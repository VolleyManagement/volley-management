namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.ContributorsAggregate;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Contributors;

    /// <summary>
    /// Builder for test MVC contributor view models
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class ContributorMvcViewModelBuilder
    {
        /// <summary>
        /// Holds test contributor view model instance
        /// </summary>
        private ContributorViewModel _contributorViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContributorMvcViewModelBuilder"/> class
        /// </summary>
        public ContributorMvcViewModelBuilder()
        {
            _contributorViewModel = new ContributorViewModel()
            {
                Id = 1,
                Name = "FirstName",
                ContributorTeamId = 1
            };
        }

        /// <summary>
        /// Sets id of test contributor view model
        /// </summary>
        /// <param name="id">Id for test contributor view model</param>
        /// <returns>Contributor view model builder object</returns>
        public ContributorMvcViewModelBuilder WithId(int id)
        {
            _contributorViewModel.Id = id;
            return this;
        }

        /// <summary>
        /// Sets id of test contributor view model
        /// </summary>
        /// <param name="name">FirstName for test contributor view model</param>
        /// <returns>Contributor view model builder object</returns>
        public ContributorMvcViewModelBuilder WithName(string name)
        {
            _contributorViewModel.Name = name;
            return this;
        }

        /// <summary>
        /// Sets id of test contributor view model
        /// </summary>
        /// <param name="contributorTeamId">ContributorTeamId for test contributor view model</param>
        /// <returns>Contributor view model builder object</returns>
        public ContributorMvcViewModelBuilder WithContributorTeamId(int contributorTeamId)
        {
            _contributorViewModel.ContributorTeamId = contributorTeamId;
            return this;
        }

        /// <summary>
        /// Builds test contributor view model
        /// </summary>
        /// <returns>test contributor view model</returns>
        public ContributorViewModel Build()
        {
            return _contributorViewModel;
        }
    }
}
