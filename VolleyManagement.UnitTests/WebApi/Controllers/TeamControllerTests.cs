namespace VolleyManagement.UnitTests.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Http.Results;
    using System.Web.OData.Results;

    using Contracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;

    using VolleyManagement.Domain.TeamsAggregate;
    using VolleyManagement.UI.Areas.WebApi.ApiControllers;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Teams;
    using VolleyManagement.UnitTests.Services.TeamService;
    using VolleyManagement.UnitTests.WebApi.ViewModels;

    /// <summary>
    /// Tests for TeamController class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TeamControllerTests
    {
        /// <summary>
        /// Not valid name.
        /// </summary>
        private const string NOT_VALID_NAME = "TESTNAMETESTNAMETESTNAMETESTNAMETESTNAME";

        /// <summary>
        /// Test Fixture
        /// </summary>
        private readonly TeamServiceTestFixture _testFixture = new TeamServiceTestFixture();

        /// <summary>
        /// Team Service Mock
        /// </summary>
        private readonly Mock<ITeamService> _teamServiceMock = new Mock<ITeamService>();

        /// <summary>
        /// IoC for tests
        /// </summary>
        private IKernel _kernel;

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<ITeamService>()
                   .ToConstant(_teamServiceMock.Object);
        }

        /// <summary>
        /// Test for Get() method. The method should return existing teams
        /// </summary>
        [TestMethod]
        public void Get_TeamsExist_TeamsReturned()
        {
            // Arrange
            var testData = _testFixture.TestTeams()
                                            .Build();
            MockTeams(testData);
            var sut = _kernel.Get<TeamsController>();

            //// Expected result
            var expected = new TeamViewModelServiceTestFixture()
                                            .TestTeams()
                                            .Build()
                                            .ToList();

            //// Actual result
            var actual = sut.GetTeams().ToList();

            //// Assert
            _teamServiceMock.Verify(ts => ts.Get(), Times.Once());
            CollectionAssert.AreEqual(expected, actual, new TeamViewModelComparer());
        }

        /// <summary>
        /// Test Post method. Does a valid ViewModel return after Team has been created.
        /// </summary>
        [TestMethod]
        public void Post_ValidViewModelTeam_ReturnedAfterCreatedWebApi()
        {
            // Arrange
            var controller = _kernel.Get<TeamsController>();
            var input = new TeamViewModelBuilder().Build();

            var expected = new TeamViewModelBuilder().Build();

            // Act
            var response = controller.Post(input);
            var actual = ((CreatedODataResult<TeamViewModel>)response).Entity;

            // Assert
            AssertExtensions.AreEqual<TeamViewModel>(expected, actual, new TeamViewModelComparer());
        }

        /// <summary>
        /// Test Post method(). Returns InvalidModelStateResult
        /// if the ModelState has some errors
        /// </summary>
        [TestMethod]
        public void Post_NotValidTeamViewModel_ReturnBadRequestWebApi()
        {
            // Arrange
            var controller = _kernel.Get<TeamsController>();
            controller.ModelState.Clear();
            var notValidViewModel = new TeamViewModelBuilder().WithName(NOT_VALID_NAME).Build();
            controller.ModelState.AddModelError("NotValidName", "Name field isn't valid");

            // Act
            var result = controller.Post(notValidViewModel) as InvalidModelStateResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ModelState.Count);
            Assert.IsTrue(result.ModelState.Keys.Contains("NotValidName"));
        }

        /// <summary>
        /// Mock the teams.
        /// </summary>
        /// <param name="testData">Data what will be returned</param>
        private void MockTeams(IList<Team> testData)
        {
            _teamServiceMock.Setup(tr => tr.Get())
                                            .Returns(testData.AsQueryable());
        }
    }
}
