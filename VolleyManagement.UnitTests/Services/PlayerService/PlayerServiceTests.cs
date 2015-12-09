namespace VolleyManagement.UnitTests.Services.PlayerService
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
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
        private const int SPECIFIC_PLAYER_ID = 2;

        private const int SPECIFIC_TEAM_ID = 2;

        private readonly PlayerServiceTestFixture _testFixture = new PlayerServiceTestFixture();

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
            var newPlayer = new PlayerBuilder().WithTeamId(SPECIFIC_TEAM_ID).Build();

            // Act
            var sut = _kernel.Get<PlayerService>();
            sut.Create(newPlayer);

            // Assert
            VerifyCreatePlayer(newPlayer, Times.Once());
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

        private void MockGetAllQuery(IEnumerable<Player> testData)
        {
            _getAllPlayersQueryMock.Setup(tr => tr.Execute(It.IsAny<GetAllCriteria>())).Returns(testData.AsQueryable());
        }

        private bool PlayersAreEqual(Player x, Player y)
        {
            return new PlayerComparer().Compare(x, y) == 0;
        }

        private void MockGetTeamByCaptainIdQuery(Team team)
        {
            _getTeamByCaptainQueryMock.Setup(t => t.Execute(It.IsAny<FindByCaptainIdCriteria>())).Returns(team);
        }

        private void VerifyCreatePlayer(Player player, Times times)
        {
            _playerRepositoryMock.Verify(pr => pr.Add(It.Is<Player>(p => PlayersAreEqual(p, player))), times);
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
    }
}
