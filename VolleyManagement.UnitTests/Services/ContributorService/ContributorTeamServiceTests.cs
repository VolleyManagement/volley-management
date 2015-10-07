namespace VolleyManagement.UnitTests.Services.ContributorService
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Contracts;
    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.Dal.MsSql.Infrastructure;
    using VolleyManagement.Domain.ContributorsAggregate;
    using VolleyManagement.Services;

    /// <summary>
    /// Tests for ContributorTeamServiceTests class.
    /// </summary>
    [TestClass]
    public class ContributorTeamServiceTests
    {
        /// <summary>
        /// Test Fixture.
        /// </summary>
        private readonly ContributorTeamServiceTestFixture _testFixture = new ContributorTeamServiceTestFixture();

        /// <summary>
        /// Players Repository Mock.
        /// </summary>
        private readonly Mock<IContributorTeamRepository> _contributorTeamRepositoryMock = new Mock<IContributorTeamRepository>();

        /// <summary>
        /// Unit of work mock.
        /// </summary>
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

        /// <summary>
        /// ITournament service mock
        /// </summary>
        private readonly Mock<IContributorTeamService> _contributorTeamServiceMock = new Mock<IContributorTeamService>();

        /// <summary>
        /// IoC for tests.
        /// </summary>
        private IKernel _kernel;

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IContributorTeamRepository>()
                   .ToConstant(_contributorTeamRepositoryMock.Object);
            _contributorTeamRepositoryMock.Setup(tr => tr.UnitOfWork).Returns(_unitOfWorkMock.Object);
        }

        /// <summary>
        /// Test for Get() method. The method should return existing contributors teams
        /// </summary>
        [TestMethod]
        public void GetAll_ContributorsTeamExist_ContributorsTeamReturned()
        {
            // Arrange
            var testData = _testFixture.TestContributors()
                                       .Build();
            MockRepositoryFindAll(testData);
            var sut = _kernel.Get<ContributorTeamService>();
            var expected = new ContributorTeamServiceTestFixture()
                                            .TestContributors()
                                            .Build()
                                            .ToList();

            // Act
            var actual = sut.Get().ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual, new ContributorTeamComparer());
        }

        /// <summary>
        /// Mocks Find method.
        /// </summary>
        /// <param name="testData">Test data to mock.</param>
        private void MockRepositoryFindAll(IEnumerable<ContributorTeam> testData)
        {
            _contributorTeamRepositoryMock.Setup(tr => tr.Find()).Returns(testData.AsQueryable());
        }

        /// <summary>
        /// Find out whether two contributor team objects have the same property values.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if the team have the same property values.</returns>
        private bool PlayersAreEqual(ContributorTeam x, ContributorTeam y)
        {
            return new ContributorTeamComparer().Compare(x, y) == 0;
        }

        [TestMethod]
        public void TestMethod1()
        {
            var kernel = new StandardKernel(new NinjectDataAccessModule(null));

            var repo = kernel.Get<IContributorTeamRepository>();

            var data = repo.Find().ToList();

            Assert.IsNotNull(data);
        }
    }
}
