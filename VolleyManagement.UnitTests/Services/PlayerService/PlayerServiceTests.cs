namespace VolleyManagement.UnitTests.Services.PlayerService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Contracts.Authorization;
    using Contracts.Exceptions;
    using Data.Contracts;
    using Data.Exceptions;
    using Data.Queries.Common;
    using Data.Queries.Team;
    using Domain.PlayersAggregate;
    using Domain.RolesAggregate;
    using Domain.TeamsAggregate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using TeamService;
    using VolleyManagement.Services;

    /// <summary>
    /// Tests for TournamentService class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class PlayerServiceTests
    {
        private const int SPECIFIC_PLAYER_ID = 2;

        private const int SPECIFIC_TEAM_ID = 2;

        private readonly PlayerServiceTestFixture _testFixture = new PlayerServiceTestFixture();
        private readonly Mock<IAuthorizationService> _authServiceMock = new Mock<IAuthorizationService>();

        private readonly Mock<IPlayerRepository> _playerRepositoryMock = new Mock<IPlayerRepository>();

        private readonly Mock<IQuery<Player, FindByIdCriteria>> _getPlayerByIdQueryMock =
            new Mock<IQuery<Player, FindByIdCriteria>>();

        private readonly Mock<IQuery<IQueryable<Player>, GetAllCriteria>> _getAllPlayersQueryMock =
            new Mock<IQuery<IQueryable<Player>, GetAllCriteria>>();

        private readonly Mock<ITeamRepository> _teamRepositoryMock = new Mock<ITeamRepository>();

        private readonly Mock<IQuery<Team, FindByIdCriteria>> _getTeamByIdQueryMock =
            new Mock<IQuery<Team, FindByIdCriteria>>();

        private readonly Mock<IQuery<Team, FindByCaptainIdCriteria>> _getTeamByCaptainQueryMock =
            new Mock<IQuery<Team, FindByCaptainIdCriteria>>();

        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

        private IKernel _kernel;

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IPlayerRepository>().ToConstant(_playerRepositoryMock.Object);
            _kernel.Bind<IQuery<Player, FindByIdCriteria>>().ToConstant(_getPlayerByIdQueryMock.Object);
            _kernel.Bind<IQuery<IQueryable<Player>, GetAllCriteria>>().ToConstant(_getAllPlayersQueryMock.Object);
            _kernel.Bind<ITeamRepository>().ToConstant(_teamRepositoryMock.Object);
            _kernel.Bind<IQuery<Team, FindByIdCriteria>>().ToConstant(_getTeamByIdQueryMock.Object);
            _kernel.Bind<IQuery<Team, FindByCaptainIdCriteria>>().ToConstant(_getTeamByCaptainQueryMock.Object);
            _kernel.Bind<IAuthorizationService>().ToConstant(_authServiceMock.Object);
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
        /// Test for Get() method. The method should return existing player by id
        /// </summary>
        [TestMethod]
        public void Get_PlayerExist_PlayerReturned()
        {
            // Arrange
            var testData = new PlayerBuilder().WithId(SPECIFIC_PLAYER_ID).Build();
            MockGetByIdQuery(testData);
            var sut = _kernel.Get<PlayerService>();
            var expected = new PlayerBuilder().WithId(SPECIFIC_PLAYER_ID).Build();

            // Act
            var actual = sut.Get(SPECIFIC_PLAYER_ID);

            // Assert
            TestHelper.AreEqual<Player>(expected, actual, new PlayerComparer());
        }

        /// <summary>
        /// Test for GetPlayerTeam() method. The method should return existing player team by player object
        /// </summary>
        [TestMethod]
        public void GetPlayerTeam_TeamExist_TeamReturned()
        {
            // Arrange
            var testTeam = new TeamBuilder().WithId(SPECIFIC_TEAM_ID).Build();
            var testPlayer = new PlayerBuilder().WithTeamId(SPECIFIC_TEAM_ID).Build();
            MockGetTeamByIdQuery(testTeam);
            var sut = _kernel.Get<PlayerService>();
            var expected = new TeamBuilder().WithId(SPECIFIC_TEAM_ID).Build();

            // Act
            var actual = sut.GetPlayerTeam(testPlayer);

            // Assert
            TestHelper.AreEqual<Team>(expected, actual, new TeamComparer());
        }

        /// <summary>
        /// Test for GetPlayerTeam() method. The method should return null by player object with no team
        /// </summary>
        [TestMethod]
        public void GetPlayerTeam_PlayerWithNoTeam_NullReturned()
        {
            // Arrange
            var testPlayer = new PlayerBuilder().WithNoTeam().Build();
            var sut = _kernel.Get<PlayerService>();

            // Act
            var actual = sut.GetPlayerTeam(testPlayer);

            // Assert
            Assert.IsNull(actual);
        }

        /// <summary>
        /// Test for Create() method. The method should create a new player.
        /// </summary>
        [TestMethod]
        public void Create_PlayerPassed_PlayerCreated()
        {
            // Arrange
            var newPlayer = new PlayerBuilder().WithTeamId(SPECIFIC_TEAM_ID).Build();

            // Act
            var sut = _kernel.Get<PlayerService>();
            sut.Create(newPlayer);

            // Assert
            VerifyCreatePlayer(newPlayer, Times.Once());
        }

        /// <summary>
        /// Test for Create() method. The method should throw ArgumentNullException.
        /// Player must not be created
        /// </summary>
        [TestMethod]
        public void Create_InvalidNullPlayer_ArgumentNullExceptionIsThrown()
        {
            bool gotException = false;

            // Arrange
            Player newPlayer = null;
            var sut = _kernel.Get<PlayerService>();

            // Act
            try
            {
                sut.Create(newPlayer);
            }
            catch (ArgumentNullException)
            {
                gotException = true;
            }

            // Assert
            Assert.IsTrue(gotException);
            VerifyCreatePlayer(newPlayer, Times.Never());
        }

        /// <summary>
        /// Test for Create() method with no rights for such action. The method should throw AuthorizationException
        /// and shouldn't invoke Commit() method of IUnitOfWork.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AuthorizationException))]
        public void Create_NoCreateRights_ExceptionThrown()
        {
            // Arrange
            Player testPlayer = new PlayerBuilder().Build();
            MockAuthServiceThrowsExeption(AuthOperations.Players.Create);
            var sut = _kernel.Get<PlayerService>();

            // Act
            sut.Create(testPlayer);

            // Assert
            VerifyCreatePlayer(testPlayer, Times.Never());
            VerifyCheckAccess(AuthOperations.Players.Create, Times.Once());
        }

        /// <summary>
        /// Test for Edit() method with no rights for such action. The method should throw AuthorizationException
        /// and shouldn't invoke Commit() method of IUnitOfWork.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AuthorizationException))]
        public void Edit_NoEditRights_ExceptionThrown()
        {
            // Arrange
            Player testPlayer = new PlayerBuilder().Build();
            MockAuthServiceThrowsExeption(AuthOperations.Players.Edit);
            var sut = _kernel.Get<PlayerService>();

            // Act
            sut.Edit(testPlayer);

            // Assert
            VerifyEditPlayer(testPlayer, Times.Never());
            VerifyCheckAccess(AuthOperations.Players.Create, Times.Once());
        }

        /// <summary>
        /// Test for Delete() method with no rights for such action. The method should throw AuthorizationException
        /// and shouldn't invoke Commit() method of IUnitOfWork.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AuthorizationException))]
        public void Delete_NoDeleteRights_ExceptionThrown()
        {
            // Arrange
            MockAuthServiceThrowsExeption(AuthOperations.Players.Delete);
            var sut = _kernel.Get<PlayerService>();

            // Act
            sut.Delete(SPECIFIC_PLAYER_ID);

            // Assert
            VerifyDeletePlayer(SPECIFIC_PLAYER_ID, Times.Never());
            VerifyCheckAccess(AuthOperations.Players.Delete, Times.Once());
        }

        /// <summary>
        /// Test for Delete() method
        /// </summary>
        [TestMethod]
        public void Delete_PlayerId_CorrectIdPostedToDatabaseLayer()
        {
            // Arrange
            _getTeamByCaptainQueryMock.Setup(tm => tm.Execute(It.IsAny<FindByCaptainIdCriteria>())).Returns((Team)null);

            // Act
            var ps = _kernel.Get<PlayerService>();
            ps.Delete(SPECIFIC_PLAYER_ID);

            // Assert
            VerifyDeletePlayer(SPECIFIC_PLAYER_ID, Times.Once());
        }

        /// <summary>
        /// Test for Delete() method. Player we want to delete does not exist.
        /// </summary>
        [TestMethod]
        public void Delete_InvalidPlayerId_MissingEntityExceptionThrown()
        {
            const int PLAYER_ID = 1;
            bool gotException = false;

            // Arrange
            _playerRepositoryMock.Setup(p => p.Remove(It.IsAny<int>()))
                .Throws<InvalidKeyValueException>();
            var sut = _kernel.Get<PlayerService>();

            // Act
            try
            {
                sut.Delete(PLAYER_ID);
            }
            catch (MissingEntityException)
            {
                gotException = true;
            }

            // Assert
            Assert.IsTrue(gotException);
            VerifyDeletePlayer(PLAYER_ID, Times.Once(), Times.Never());
        }

        /// <summary>
        /// Test for Delete() method. Player we want to delete is captain of existing team.
        /// </summary>
        [TestMethod]
        public void Delete_CaptainOfExistTeam_ValidationExceptionThrown()
        {
            const int PLAYER_ID = 1;
            bool gotException = false;

            // Arrange
            var existTeam = new TeamBuilder().WithCaptain(PLAYER_ID).Build();
            MockGetTeamByCaptainIdQuery(existTeam);
            var sut = _kernel.Get<PlayerService>();

            // Act
            try
            {
                sut.Delete(PLAYER_ID);
            }
            catch (ValidationException)
            {
                gotException = true;
            }

            // Assert
            Assert.IsTrue(gotException);
            VerifyDeletePlayer(PLAYER_ID, Times.Never());
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
            VerifyEditPlayer(playerToEdit, Times.Once());
        }

        private void MockGetByIdQuery(Player player)
        {
            _getPlayerByIdQueryMock.Setup(pq => pq.Execute(It.IsAny<FindByIdCriteria>())).Returns(player);
        }

        private void MockGetTeamByIdQuery(Team team)
        {
            _getTeamByIdQueryMock.Setup(pq => pq.Execute(It.IsAny<FindByIdCriteria>())).Returns(team);
        }

        private void MockGetAllQuery(IEnumerable<Player> testData)
        {
            _getAllPlayersQueryMock.Setup(tr => tr.Execute(It.IsAny<GetAllCriteria>())).Returns(testData.AsQueryable());
        }

        private void MockGetTeamByCaptainIdQuery(Team team)
        {
            _getTeamByCaptainQueryMock.Setup(t => t.Execute(It.IsAny<FindByCaptainIdCriteria>())).Returns(team);
        }

        private bool PlayersAreEqual(Player x, Player y)
        {
            return new PlayerComparer().Compare(x, y) == 0;
        }

        private void VerifyCreatePlayer(Player player, Times times)
        {
            _playerRepositoryMock.Verify(pr => pr.Add(It.Is<Player>(p => PlayersAreEqual(p, player))), times);
            _unitOfWorkMock.Verify(uow => uow.Commit(), times);
        }

        private void VerifyEditPlayer(Player player, Times times)
        {
            _playerRepositoryMock.Verify(pr => pr.Update(It.Is<Player>(p => PlayersAreEqual(p, player))), times);
            _unitOfWorkMock.Verify(uow => uow.Commit(), times);
        }

        private void VerifyDeletePlayer(int playerId, Times times)
        {
            _playerRepositoryMock.Verify(pr => pr.Remove(It.Is<int>(id => id == playerId)), times);
            _unitOfWorkMock.Verify(uow => uow.Commit(), times);
        }

        private void VerifyDeletePlayer(int playerId, Times repositoryTimes, Times unitOfWorkTimes)
        {
            _playerRepositoryMock.Verify(pr => pr.Remove(It.Is<int>(id => id == playerId)), repositoryTimes);
            _unitOfWorkMock.Verify(uow => uow.Commit(), unitOfWorkTimes);
        }

        private void MockAuthServiceThrowsExeption(AuthOperation operation)
        {
            _authServiceMock.Setup(tr => tr.CheckAccess(operation)).Throws<AuthorizationException>();
        }

        private void VerifyCheckAccess(AuthOperation operation, Times times)
        {
            _authServiceMock.Verify(tr => tr.CheckAccess(operation), times);
        }
    }
}
