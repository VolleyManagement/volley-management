namespace VolleyManagement.UnitTests.Services.ContributorService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Domain.ContributorsAggregate;
    using VolleyManagement.Services;

    /// <summary>
    /// Tests for ContributorTeamServiceTests class.
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ContributorTeamServiceTests
    {
        private readonly ContributorTeamServiceTestFixture _testFixture = new ContributorTeamServiceTestFixture();
        private Mock<IContributorTeamRepository> _contributorTeamRepositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _contributorTeamRepositoryMock = new Mock<IContributorTeamRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _contributorTeamRepositoryMock.Setup(tr => tr.UnitOfWork).Returns(_unitOfWorkMock.Object);
        }

        /// <summary>
        /// Test for Get() method. The method should return existing contributors teams
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task GetAll_ContributorsTeamExist_ContributorsTeamReturned()
        {
            // Arrange
            var testData = _testFixture.TestContributors()
                                       .Build();

            MockRepositoryFindAll(testData);

            var sut = BuildSUT();
            var expected = new ContributorTeamServiceTestFixture()
                                            .TestContributors()
                                            .Build()
                                            .ToList();

            // Act
            var actual = await sut.Get();

            // Assert
            CollectionAssert.AreEqual(expected, actual, new ContributorTeamComparer());
        }

        private ContributorTeamService BuildSUT()
        {
            return new ContributorTeamService(_contributorTeamRepositoryMock.Object);
        }

        private void MockRepositoryFindAll(List<ContributorTeam> testData)
        {
            _contributorTeamRepositoryMock.Setup(tr => tr.Find()).Returns(Task.FromResult(testData));
        }

        private bool PlayersAreEqual(ContributorTeam x, ContributorTeam y)
        {
            return new ContributorTeamComparer().Compare(x, y) == 0;
        }
    }
}
