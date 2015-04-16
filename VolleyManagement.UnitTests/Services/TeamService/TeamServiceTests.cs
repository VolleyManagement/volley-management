namespace VolleyManagement.UnitTests.Services.TeamService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.Dal.Exceptions;
    using VolleyManagement.Domain.Teams;
    using VolleyManagement.Services;

    /// <summary>
    /// Tests for TournamentService class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TeamServiceTests
    {
        /// <summary>
        /// Test Fixture.
        /// </summary>
        private readonly TeamServiceTestFixture _testFixture = new TeamServiceTestFixture();

        /// <summary>
        /// Team Repository Mock.
        /// </summary>
        private readonly Mock<ITeamRepository> _teamRepositoryMock = new Mock<ITeamRepository>();

        /// <summary>
        /// Unit of work mock.
        /// </summary>
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

        /// <summary>
        /// ITeamService mock
        /// </summary>
        private readonly Mock<ITeamService> _teamServiceMock = new Mock<ITeamService>();

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
            _kernel.Bind<ITeamRepository>()
                   .ToConstant(_teamRepositoryMock.Object);
            _teamRepositoryMock.Setup(tr => tr.UnitOfWork).Returns(_unitOfWorkMock.Object);
        }

        /// <summary>
        /// Mocks Find method.
        /// </summary>
        /// <param name="testData">Test data to mock.</param>
        private void MockRepositoryFindAll(IEnumerable<Team> testData)
        {
            _teamRepositoryMock.Setup(tr => tr.Find()).Returns(testData.AsQueryable());
        }
    }
}
