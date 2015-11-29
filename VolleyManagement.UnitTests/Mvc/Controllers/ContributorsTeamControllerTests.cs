namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Contracts;
    using Domain.ContributorsAggregate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.UI.Areas.Mvc.Controllers;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.ContributorsTeam;
    using VolleyManagement.UnitTests.Mvc.ViewModels;
    using VolleyManagement.UnitTests.Services.ContributorService;

    /// <summary>
    /// Tests for MVC ContributorTeamController class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ContributorsTeamControllerTests
    {
        private readonly Mock<IContributorTeamService> _contributorTeamServiceMock = new Mock<IContributorTeamService>();

        private IKernel _kernel;
        private ContributorsTeamController _sut;

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            this._kernel = new StandardKernel();
            this._kernel.Bind<IContributorTeamService>().ToConstant(this._contributorTeamServiceMock.Object);
            this._sut = this._kernel.Get<ContributorsTeamController>();
        }

        /// <summary>
        /// Test for Index method. All contributors are requested. All contributors are returned.
        /// </summary>
        [TestMethod]
        public void Index_GetAllContributors_AllContributorsAreReturned()
        {
            // Arrange
            var testData = MakeTestContributorTeams();
            var expected = MakeTestContributorTeamViewModels(testData);
            SetupGetAll(testData);

            // Act
            var actual = TestExtensions.GetModel<IEnumerable<ContributorsTeamViewModel>>(this._sut.Index()).ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual, new ContributorTeamViewModelComparer());
        }

        private List<ContributorTeam> MakeTestContributorTeams()
        {
            return new ContributorTeamServiceTestFixture().TestContributors().Build();
        }

        private List<ContributorsTeamViewModel> MakeTestContributorTeamViewModels(List<ContributorTeam> contributorTeams)
        {
            return contributorTeams.Select(ct => new ContributorTeamMvcViewModelBuilder()
                .WithId(ct.Id)
                .WithName(ct.Name)
                .WithCourseDirection(ct.CourseDirection)
                .WithContributors(ct.Contributors.ToList())
                .Build())
                .ToList();
        }

        private void SetupGetAll(List<ContributorTeam> teams)
        {
            this._contributorTeamServiceMock.Setup(cts => cts.Get()).Returns(teams.AsQueryable());
        }
    }
}
