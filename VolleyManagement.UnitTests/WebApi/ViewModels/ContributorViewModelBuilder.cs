namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Contributors;

    /// <summary>
    /// Builder for test contributor view models
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class ContributorViewModelBuilder
    {
        /// <summary>
        /// Holds test contributor view model instance
        /// </summary>
        private ContributorViewModel _contributorViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContributorViewModelBuilder"/> class
        /// </summary>
        public ContributorViewModelBuilder()
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
        public ContributorViewModelBuilder WithId(int id)
        {
            _contributorViewModel.Id = id;
            return this;
        }

        /// <summary>
        /// Sets id of test contributor view model
        /// </summary>
        /// <param name="name">FirstName for test contributor view model</param>
        /// <returns>Contributor view model builder object</returns>
        public ContributorViewModelBuilder WithName(string name)
        {
            _contributorViewModel.Name = name;
            return this;
        }

        /// <summary>
        /// Sets id of test contributor view model
        /// </summary>
        /// <param name="contributorTeamId">contributorTeamId for test contributor view model</param>
        /// <returns>Contributor view model builder object</returns>
        public ContributorViewModelBuilder WithContributorTeamId(int contributorTeamId)
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
