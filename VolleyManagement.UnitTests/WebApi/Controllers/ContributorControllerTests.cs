namespace VolleyManagement.UnitTests.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using Ninject;

    using VolleyManagement.Contracts;
    using VolleyManagement.Domain.ContributorsAggregate;
    using VolleyManagement.UI.Areas.WebApi.ODataControllers;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.ContributorsTeam;
    using VolleyManagement.UnitTests.Services.ContributorService;
    using VolleyManagement.UnitTests.WebApi.ViewModels;

    /// <summary>
    /// Tests for ContributorsTeamController class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ContributorsControllerTests
    {
        /// <summary>
        /// Test Fixture
        /// </summary>
        private readonly ContributorTeamServiceTestFixture _testFixture = new ContributorTeamServiceTestFixture();

        /// <summary>
        /// Contributors team Service Mock
        /// </summary>
        private readonly Mock<IContributorTeamService> _contributorTeamServiceMock = new Mock<IContributorTeamService>();

        /// <summary>
        /// IoC for tests
        /// </summary>
        private IKernel _kernel;

        /// <summary>
        /// Initializes test data
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IContributorTeamService>()
                   .ToConstant(_contributorTeamServiceMock.Object);
        }

        /// <summary>
        /// Test for Get() method. The method should return existing contributors team
        /// </summary>
        [TestMethod]
        public void Get_ContributorsTeamExist_ContributorsTeamReturned()
        {
            // Arrange
            var testData = _testFixture.TestContributors()
                                            .Build();
            MockContributors(testData);
            var sut = _kernel.Get<ContributorsTeamController>();

            // Expected result
            var expected = new ContributorTeamViewModelServiceTestFixture()
                                            .TestContributor()
                                            .Build()
                                            .ToList();

            // Actual result
            var actual = sut.GetContributorsTeam().ToList();

            // Assert
            _contributorTeamServiceMock.Verify(cn => cn.Get(), Times.Once());
            CollectionAssert.AreEqual(expected, actual, new ContributorTeamViewModelComparer());
        }

        /// <summary>
        /// Test for Map() method.
        /// The method should map contributors team domain model to view model.
        /// </summary>
        [TestMethod]
        public void Map_ContributorAsParam_MappedToViewModelWebApi()
        {
            var contributors = CreateContributorsMock();

            // Arrange
            var contributor =
                new ContributorTeamBuilder().WithId(1)
                    .WithName("FirstName")
                    .WithcourseDirection("Course")
                    .Withcontributors(contributors)
                    .Build();
            var expected =
                new ContributorTeamViewModelBuilder().WithId(1)
                    .WithName("FirstName")
                    .WithcourseDirection("Course")
                    .Build();

            // Act
            var actual = ContributorsTeamViewModel.Map(contributor);

            // Assert
            AssertExtensions.AreEqual<ContributorsTeamViewModel>(
                expected,
                actual,
                new ContributorTeamViewModelComparer());
        }

        /// <summary>
        /// Create new list of tests contributors
        /// </summary>
        /// <returns>List of contributors</returns>
        private IList<Contributor> CreateContributorsMock()
        {
            return new List<Contributor>
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
                                   };
        }

        /// <summary>
        /// Mock the Contributors
        /// </summary>
        /// <param name="testData">Data what will be returned</param>
        private void MockContributors(IList<ContributorTeam> testData)
        {
            _contributorTeamServiceMock.Setup(cn => cn.Get())
                                            .Returns(testData.AsQueryable());
        }
    }
}
