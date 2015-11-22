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

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            this._kernel = new StandardKernel();
            this._kernel.Bind<IContributorTeamService>().ToConstant(this._contributorTeamServiceMock.Object);
        }

        /// <summary>
        /// Test for Index method. All contributors are requested. All contributors are returned.
        /// </summary>
        [TestMethod]
        public void Index_GetAllContributors_AllContributorsAreReturned()
        {
            // Arrange
            var testData = MakeTestContributorTeams();
            var expected = MakeTestContributorTeamViewModels();
            var sut = GetSystemUnderTest();
            MockSetupGetAll(testData);

            // Act
            var actual = TestExtensions.GetModel<IEnumerable<ContributorsTeamViewModel>>(sut.Index()).ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual, new ContributorTeamViewModelComparer());
        }

        /// <summary>
        /// Makes contributor teams filled with test data.
        /// </summary>
        /// <returns>List of contributor teams with test data.</returns>
        private List<ContributorTeam> MakeTestContributorTeams()
        {
            return new ContributorTeamServiceTestFixture().TestContributors().Build();
        }

        /// <summary>
        /// Sets up a mock for Get method of ContributorTeam service to return specified contributor teams.
        /// </summary>
        /// <param name="teams">Contributor teams that will be returned by Get method of ContributorTeam service.</param>
        private void MockSetupGetAll(List<ContributorTeam> teams)
        {
            this._contributorTeamServiceMock.Setup(cts => cts.Get()).Returns(teams.AsQueryable());
        }

        /// <summary>
        /// Makes view models of contributor team filled with test data.
        /// </summary>
        /// <returns>List of view models of contributor team with test data.</returns>
        private List<ContributorsTeamViewModel> MakeTestContributorTeamViewModels()
        {
            return MakeTestContributorTeams().Select(ct => new ContributorTeamMvcViewModelBuilder()
                .WithId(ct.Id)
                .WithName(ct.Name)
                .WithCourseDirection(ct.CourseDirection)
                .WithContributors(ct.Contributors.ToList())
                .Build())
                .ToList();
        }

        /// <summary>
        /// Gets system being tested by a unit test.
        /// </summary>
        /// <returns>System being tested by a unit test.</returns>
        private ContributorsTeamController GetSystemUnderTest()
        {
            return this._kernel.Get<ContributorsTeamController>();
        }
    }
}
