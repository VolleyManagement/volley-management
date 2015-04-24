namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using VolleyManagement.Domain.Contributors;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Contributors;
    using VolleyManagement.UnitTests.Services.ContributorService;

    /// <summary>
    /// View model contributor class test
    /// </summary>
    [TestClass]
    public class ContributorDomainToViewModel
    {
        /// <summary>
        /// Map() method test.
        /// Does correct a contributor domain model mapped to a view model.
        /// </summary>
        [TestMethod]
        public void Map_DomainContributorAsParam_MappedToViewModel()
        {
            // Arrange
            var testViewModel = new ContributorMvcViewModelBuilder()
                .WithId(1)
                .WithName("FirstName")
                .WithContributorTeamId(1)
                .Build();

            var testDomainModel = new ContributorBuilder()
                .WithId(1)
                .WithName("FirstName")
                .WithContributorTeamId(1)
                .Build();

            // Act
            var actual = ContributorViewModel.Map(testDomainModel);

            // Assert
            AssertExtensions.AreEqual<ContributorViewModel>(testViewModel, actual, new ContributorViewModelComparer());
        }
    }
}
