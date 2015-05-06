﻿namespace VolleyManagement.UnitTests.Services.TeamService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.Dal.Exceptions;
    using VolleyManagement.Domain.Players;
    using VolleyManagement.Domain.Teams;
    using VolleyManagement.Services;
    using VolleyManagement.UnitTests.Services.PlayerService;
    using IsolationLevel = System.Data.IsolationLevel;

    /// <summary>
    /// Tests for TournamentService class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TeamServiceTests
    {
        private const string EXCEPTION_MESSAGE = "Test exception message.";

        private readonly TeamServiceTestFixture _testFixture = new TeamServiceTestFixture();

        private readonly Mock<ITeamRepository> _teamRepositoryMock = new Mock<ITeamRepository>();

        private readonly Mock<IPlayerRepository> _playerRepositoryMock = new Mock<IPlayerRepository>();

        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

        private readonly Mock<ITeamService> _teamServiceMock = new Mock<ITeamService>();

        private readonly Mock<IDbTransaction> _dbTransaction = new Mock<IDbTransaction>();

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
            _kernel.Bind<IPlayerRepository>()
                   .ToConstant(_playerRepositoryMock.Object);

            _teamRepositoryMock.Setup(tr => tr.UnitOfWork).Returns(_unitOfWorkMock.Object);

            _unitOfWorkMock.Setup(u => u.BeginTransaction(It.IsAny<IsolationLevel>())).Returns(_dbTransaction.Object);
        }

        /// <summary>
        /// Test for Get() method. The method should return existing teams
        /// </summary>
        [TestMethod]
        public void GetAll_TeamsExist_TeamsReturned()
        {
            // Arrange
            var testData = _testFixture.TestTeams().Build();
            MockRepositoryFindAll(testData);

            var expected = new TeamServiceTestFixture()
                                            .TestTeams()
                                            .Build()
                                            .ToList();

            // Act
            var sut = _kernel.Get<TeamService>();
            var actual = sut.Get().ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual, new TeamComparer());
        }

        /// <summary>
        /// Test for Create() method. The method should create a new team.
        /// </summary>
        [TestMethod]
        public void Create_TeamPassed_TeamCreated()
        {
            // Arrange
            var newTeam = new TeamBuilder().Build();
            int captainId = 1;

            var captain = (new PlayerBuilder()).WithId(captainId).Build();
            IQueryable<Player> listOfPlayers = (new PlayerServiceTestFixture())
                                                .AddPlayer(captain).Build()
                                                .AsQueryable();

            IQueryable<Player> queriableCaptain = listOfPlayers.Where(p => p.Id == captainId);

            _playerRepositoryMock.Setup(pr => pr.FindWhere(It.IsAny<Expression<Func<Player, bool>>>()))
                                                .Returns(queriableCaptain);

            // Act
            var sut = _kernel.Get<TeamService>();
            sut.Create(newTeam);

            // Assert
            _teamRepositoryMock.Verify(
                tr => tr.Add(It.Is<Team>(t => TeamsAreEqual(t, newTeam))), Times.Once());
            _unitOfWorkMock.Verify(u => u.Commit());
        }

        /// <summary>
        /// Test for Create() method. The method check case when captain id
        /// or some of roster player id is invalid. Should throw MissingEntityException
        /// </summary>
        [TestMethod]
        public void Create_InvalidPlayerId_MissingEntityExceptionThrown()
        {
            // Arrange
            var newTeam = new TeamBuilder().Build();
            _teamRepositoryMock.Setup(tr => tr.Add(It.IsAny<Team>()))
                .Throws(new InvalidKeyValueException(EXCEPTION_MESSAGE));

            // Act
            var sut = _kernel.Get<TeamService>();
            bool gotException = false;
            string actualErrorMessage = string.Empty;
            try
            {
                sut.Create(newTeam);
            }
            catch (MissingEntityException ex)
            {
                gotException = true;
                actualErrorMessage = ex.Message;
            }

            // Assert
            _teamRepositoryMock.Verify(tr => tr.Add(It.IsAny<Team>()), Times.Once());
            Assert.IsTrue(gotException);
            Assert.AreEqual(actualErrorMessage, EXCEPTION_MESSAGE);
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Never());
        }

        /// <summary>
        /// Test for Delete() method. The method should delete specified team.
        /// </summary>
        [TestMethod]
        public void Delete_TeamId_CorrectIdPostedToDatabaseLayer()
        {
            // Arrange
            var expectedId = 10;

            // Act
            var ts = _kernel.Get<TeamService>();
            ts.Delete(expectedId);

            // Assert
            _teamRepositoryMock.Verify(tr => tr.Remove(It.Is<int>(teamId => teamId == expectedId)), Times.Once());
            _unitOfWorkMock.Verify(tr => tr.Commit());
        }

        /// <summary>
        /// Test for Delete() method. The method check case when team id is invalid.
        /// Should throw MissingEntityException
        /// </summary>
        [TestMethod]
        public void Delete_InvalidTeamId_MissingEntityExceptionThrown()
        {
            // Arrange
            var expectedId = 10;
            _teamRepositoryMock.Setup(tr => tr.Remove(It.IsAny<int>()))
                .Throws(new InvalidKeyValueException(EXCEPTION_MESSAGE));

            // Act
            var ts = _kernel.Get<TeamService>();
            bool gotException = false;
            string actualErrorMessage = string.Empty;
            try
            {
                ts.Delete(expectedId);
            }
            catch (MissingEntityException)
            {
                gotException = true;
            }

            // Assert
            _teamRepositoryMock.Verify(tr => tr.Remove(It.Is<int>(teamId => teamId == expectedId)), Times.Once());
            Assert.IsTrue(gotException);
            _unitOfWorkMock.Verify(tr => tr.Commit(), Times.Never());
        }

        /// <summary>
        /// Find out whether two teams objects have the same property values.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if the teams have the same property values.</returns>
        private bool TeamsAreEqual(Team x, Team y)
        {
            return new TeamComparer().Compare(x, y) == 0;
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
