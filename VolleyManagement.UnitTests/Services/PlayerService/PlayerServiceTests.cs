namespace VolleyManagement.UnitTests.Services.PlayerService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using Ninject;

    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Exceptions;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Data.Queries.Team;
    using VolleyManagement.Domain.PlayersAggregate;
    using VolleyManagement.Domain.TeamsAggregate;
    using VolleyManagement.Services;
    using VolleyManagement.UnitTests.Services.TeamService;

    /// <summary>
    /// Tests for TournamentService class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class PlayerServiceTests
    {
        private readonly PlayerServiceTestFixture _testFixture = new PlayerServiceTestFixture();

        private readonly Mock<IPlayerRepository> _playerRepositoryMock = new Mock<IPlayerRepository>();

        private readonly Mock<ITeamRepository> _teamRepositoryMock = new Mock<ITeamRepository>();

        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

        private readonly Mock<IPlayerService> _playerServiceMock = new Mock<IPlayerService>();

        private readonly Mock<IQuery<Player, FindByIdCriteria>> _getPlayerByIdQueryMock =
            new Mock<IQuery<Player, FindByIdCriteria>>();

        private readonly Mock<IQuery<Team, FindByIdCriteria>> _getTeamByIdQueryMock =
            new Mock<IQuery<Team, FindByIdCriteria>>();

        private readonly Mock<IQuery<IQueryable<Player>, GetAllCriteria>> _getAllPlayersQueryMock =
            new Mock<IQuery<IQueryable<Player>, GetAllCriteria>>();

        private readonly Mock<IQuery<Team, FindByCaptainIdCriteria>> _getTeamByCaptainQueryMock =
            new Mock<IQuery<Team, FindByCaptainIdCriteria>>();

        private IKernel _kernel;

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IPlayerRepository>().ToConstant(_playerRepositoryMock.Object);
            _kernel.Bind<ITeamRepository>().ToConstant(_teamRepositoryMock.Object);
            _kernel.Bind<IQuery<Player, FindByIdCriteria>>().ToConstant(_getPlayerByIdQueryMock.Object);
            _kernel.Bind<IQuery<Team, FindByIdCriteria>>().ToConstant(_getTeamByIdQueryMock.Object);
            _kernel.Bind<IQuery<IQueryable<Player>, GetAllCriteria>>().ToConstant(_getAllPlayersQueryMock.Object);
            _kernel.Bind<IQuery<Team, FindByCaptainIdCriteria>>().ToConstant(_getTeamByCaptainQueryMock.Object);
            _playerRepositoryMock.Setup(tr => tr.UnitOfWork).Returns(_unitOfWorkMock.Object);
            _teamRepositoryMock.Setup(tr => tr.UnitOfWork).Returns(_unitOfWorkMock.Object);
        }

        /// <summary>
        /// Test for Get() method. The method should return existing players
        /// (order is important).
        /// </summary>
        [TestMethod]
        public void GetAll_PlayersExist_PlayersReturned()
        {
            // Arrange
            var testData = _testFixture.TestPlayers()
                                       .Build();
            MockGetAllQuery(testData);
            var sut = _kernel.Get<PlayerService>();
            var expected = new PlayerServiceTestFixture()
                                            .TestPlayers()
                                            .Build()
                                            .ToList();

            // Act
            var actual = sut.Get().ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual, new PlayerComparer());
        }

        /// <summary>
        /// Test for Create() method. The method should create a new player.
        /// </summary>
        [TestMethod]
        public void Create_PlayerPassed_PlayerCreated()
        {
            // Arrange
            int teamId = 1;
            Team team = new TeamBuilder().WithId(teamId).Build();
            MockGetTeamByIdQuery(team);
            var newPlayer = new PlayerBuilder().WithTeamId(teamId).Build();

            // Act
            var sut = _kernel.Get<PlayerService>();
            sut.Create(newPlayer);

            // Assert
            _playerRepositoryMock.Verify(
                ur => ur.Add(It.Is<Player>(u => PlayersAreEqual(u, newPlayer))));
            _unitOfWorkMock.Verify(u => u.Commit());
        }

        /// <summary>
        /// Test for Create() method. The method should create a new player.
        /// </summary>
        [TestMethod]
        public void Create_InvalidTeamId_MissingEntityExceptionThrown()
        {
            // Arrange
            var newPlayer = new PlayerBuilder().Build();
            MockGetTeamByIdQuery(null);

            // Act
            var sut = _kernel.Get<PlayerService>();
            bool gotException = false;
            try
            {
                sut.Create(newPlayer);
            }
            catch (MissingEntityException)
            {
                gotException = true;
            }

            // Assert
            _playerRepositoryMock.Verify(pr => pr.Add(It.IsAny<Player>()), Times.Never());
            Assert.IsTrue(gotException);
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Never());
        }

        /// <summary>
        /// Test for Delete() method
        /// </summary>
        [TestMethod]
        public void Delete_PlayerId_CorrectIdPostedToDatabaseLayer()
        {
            // Arrange
            var expectedId = 10;
            MockGetTeamByIdQuery(null);

            // Act
            var ps = _kernel.Get<PlayerService>();
            ps.Delete(expectedId);

            // Assert
            _playerRepositoryMock.Verify(pr => pr.Remove(It.Is<int>(playerId => playerId == expectedId)), Times.Once());
            _unitOfWorkMock.Verify(pr => pr.Commit());
        }

        /// <summary>
        /// Test for Delete() method
        /// </summary>
        [TestMethod]
        public void Delete_InvalidPlayerId_MissingEntityExceptionThrown()
        {
            // Arrange
            var expectedId = 10;
            MockGetTeamByIdQuery(null);
            _playerRepositoryMock.Setup(tr => tr.Remove(It.IsAny<int>()))
                .Throws(new InvalidKeyValueException());

            // Act
            var ps = _kernel.Get<PlayerService>();
            bool gotException = false;
            string actualErrorMessage = string.Empty;
            try
            {
                ps.Delete(expectedId);
            }
            catch (MissingEntityException)
            {
                gotException = true;
            }

            // Assert
            _playerRepositoryMock.Verify(pr => pr.Remove(It.Is<int>(playerId => playerId == expectedId)), Times.Once());
            Assert.IsTrue(gotException);
            _unitOfWorkMock.Verify(uw => uw.Commit(), Times.Never());
        }

        /// <summary>
        /// Test for Delete() method
        /// </summary>
        [TestMethod]
        public void Delete_CaptainOfExistTeam_ValidationExceptionThrown()
        {
            // Arrange
            var expectedId = 10;
            Team existTeam = new TeamBuilder().WithCaptain(expectedId).Build();
            MockGetTeamByCapitanIdQuety(existTeam);

            // Act
            var ps = _kernel.Get<PlayerService>();
            bool gotException = false;
            try
            {
                ps.Delete(expectedId);
            }
            catch (ValidationException)
            {
                gotException = true;
            }

            // Assert
            _playerRepositoryMock.Verify(pr => pr.Remove(It.IsAny<int>()), Times.Never());
            Assert.IsTrue(gotException);
            _unitOfWorkMock.Verify(uw => uw.Commit(), Times.Never());
        }

        /// <summary>
        /// Edit() method test. catch InvalidKeyValueException from DAL
        /// Throws MissingEntityException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(MissingEntityException))]
        public void Edit_CatchDalInvalidKeyValueException_ThrowMissingEntityException()
        {
            // Arrange
            _playerRepositoryMock.Setup(pr => pr.Update(It.IsAny<Player>())).Throws(new InvalidKeyValueException());
            var sut = _kernel.Get<PlayerService>();
            var playerWithWrongId = new PlayerBuilder().WithTeamId(null).Build();

            // Act
            sut.Edit(playerWithWrongId);
        }

        /// <summary>
        /// Positive test for Edit() method.
        /// </summary>
        [TestMethod]
        public void Edit_PlayerPassed_PlayerUpdated()
        {
            // Arrange
            int? teamId = null;
            var expectedPlayer = new PlayerBuilder().WithTeamId(teamId).Build();

            // Act
            var playerToEdit = new PlayerBuilder().WithTeamId(teamId).Build();
            var sut = _kernel.Get<PlayerService>();
            sut.Edit(playerToEdit);

            // Assert
            _playerRepositoryMock.Verify(pr => pr.Update(It.Is<Player>(p => PlayersAreEqual(playerToEdit, expectedPlayer))), Times.Once());
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Once());
        }

        private void MockGetAllQuery(IEnumerable<Player> testData)
        {
            _getAllPlayersQueryMock.Setup(tr => tr.Execute(It.IsAny<GetAllCriteria>())).Returns(testData.AsQueryable());
        }

        private bool PlayersAreEqual(Player x, Player y)
        {
            return new PlayerComparer().Compare(x, y) == 0;
        }

        private void MockGetTeamByIdQuery(Team team)
        {
            _getTeamByIdQueryMock.Setup(tr => tr.Execute(It.IsAny<FindByIdCriteria>())).Returns(team);
        }

        private void MockGetTeamByCapitanIdQuety(Team team)
        {
            _getTeamByCaptainQueryMock.Setup(t => t.Execute(It.IsAny<FindByCaptainIdCriteria>())).Returns(team);
        }
    }
}
