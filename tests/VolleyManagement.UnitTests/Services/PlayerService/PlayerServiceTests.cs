﻿namespace VolleyManagement.UnitTests.Services.PlayerService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Contracts.Authorization;
    using Contracts.Exceptions;
    using Data.Contracts;
    using Data.Exceptions;
    using Data.Queries.Common;
    using Data.Queries.Team;
    using Domain.PlayersAggregate;
    using Domain.RolesAggregate;
    using Domain.TeamsAggregate;
    using VolleyManagement.Services;
    using TeamService;

    /// <summary>
    /// Tests for TournamentService class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class PlayerServiceTests
    {
        private const int SPECIFIC_PLAYER_ID = 2;
        private const int SPECIFIC_TEAM_ID = 2;
        private const int NUMBER_OF_PLAYERS = 3;

        private PlayerServiceTestFixture _testFixture = new PlayerServiceTestFixture();

        private Mock<IAuthorizationService> _authServiceMock;
        private Mock<IPlayerRepository> _playerRepositoryMock;
        private Mock<IQuery<Player, FindByIdCriteria>> _getPlayerByIdQueryMock;
        private Mock<IQuery<IQueryable<Player>, GetAllCriteria>> _getAllPlayersQueryMock;
        private Mock<IQuery<ICollection<FreePlayerDto>, EmptyCriteria>> _getFreePlayersQueryMock;
        private Mock<ITeamRepository> _teamRepositoryMock;
        private Mock<IQuery<Team, FindByIdCriteria>> _getTeamByIdQueryMock;
        private Mock<IQuery<int, FindByPlayerCriteria>> _getTeamByPlayerQueryMock;
        private Mock<IQuery<Team, FindByCaptainIdCriteria>> _getTeamByCaptainQueryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _authServiceMock = new Mock<IAuthorizationService>();
            _playerRepositoryMock = new Mock<IPlayerRepository>();
            _getPlayerByIdQueryMock = new Mock<IQuery<Player, FindByIdCriteria>>();
            _getAllPlayersQueryMock = new Mock<IQuery<IQueryable<Player>, GetAllCriteria>>();
            _getFreePlayersQueryMock = new Mock<IQuery<ICollection<FreePlayerDto>, EmptyCriteria>>();
            _teamRepositoryMock = new Mock<ITeamRepository>();
            _getTeamByIdQueryMock = new Mock<IQuery<Team, FindByIdCriteria>>();
            _getTeamByPlayerQueryMock = new Mock<IQuery<int, FindByPlayerCriteria>>();
            _getTeamByCaptainQueryMock = new Mock<IQuery<Team, FindByCaptainIdCriteria>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
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
            var sut = BuildSUT();
            var expected = new PlayerServiceTestFixture()
                                            .TestPlayers()
                                            .Build();

            // Act
            var actual = sut.Get().ToList();

            // Assert
            TestHelper.AreEqual(expected, actual, new PlayerComparer());
        }

        /// <summary>
        /// Test for Get() method. The method should return existing player by id
        /// </summary>
        [TestMethod]
        public void Get_PlayerExist_PlayerReturned()
        {
            // Arrange
            var testData = new PlayerBuilder(SPECIFIC_PLAYER_ID).Build();
            MockGetByIdQuery(testData);
            var sut = BuildSUT();
            var expected = new PlayerBuilder(SPECIFIC_PLAYER_ID).Build();

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
            var testPlayer = new PlayerBuilder().Build();
            MockGetTeamByIdQuery(testTeam);
            MockGetTeamByPlayerQuery(testTeam.Id);
            var sut = BuildSUT();
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
            var testPlayer = new PlayerBuilder().Build();
            var sut = BuildSUT();

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
            var newPlayer = new CreatePlayerDtoBuilder().Build();

            // Act
            var sut = BuildSUT();
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
            var gotException = false;

            // Arrange
            CreatePlayerDto newPlayer = null;
            var sut = BuildSUT();

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
        }

        /// <summary>
        /// Test for Create() method with List. The method should create new players
        /// </summary>
        [TestMethod]
        public void Create_AllPlayersAreNew_PlayersCreated()
        {
            // Arrange
            var newPlayers = CreateSeveralPlayers();
           
            // Act
            var sut = BuildSUT();
            sut.CreateBulk(newPlayers);

            // Assert
            VerifyCreatePlayers(Times.Exactly(NUMBER_OF_PLAYERS));
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
            var testPlayer = new CreatePlayerDtoBuilder().Build();
            MockAuthServiceThrowsExeption(AuthOperations.Players.Create);
            var sut = BuildSUT();

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
            var testPlayer = new PlayerBuilder().Build();
            MockAuthServiceThrowsExeption(AuthOperations.Players.Edit);
            var sut = BuildSUT();

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
            var sut = BuildSUT();

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
            var ps = BuildSUT();
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
            var gotException = false;

            // Arrange
            _playerRepositoryMock.Setup(p => p.Remove(It.IsAny<int>()))
                .Throws<InvalidKeyValueException>();
            var sut = BuildSUT();

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
            var gotException = false;

            // Arrange
            var existTeam = new TeamBuilder().WithCaptain(new PlayerId(PLAYER_ID)).Build();
            MockGetTeamByCaptainIdQuery(existTeam);
            var sut = BuildSUT();

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
            var sut = BuildSUT();
            var playerWithWrongId = new PlayerBuilder().Build();

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
            var expectedPlayer = new PlayerBuilder().Build();

            // Act
            var playerToEdit = new PlayerBuilder().Build();
            var sut = BuildSUT();
            sut.Edit(playerToEdit);

            // Assert
            VerifyEditPlayer(playerToEdit, Times.Once());
        }

        private PlayerService BuildSUT()
        {
            return new PlayerService(
                _playerRepositoryMock.Object,
                _getTeamByIdQueryMock.Object,
                _getPlayerByIdQueryMock.Object,
                _getTeamByPlayerQueryMock.Object,
                _getFreePlayersQueryMock.Object,
                _getAllPlayersQueryMock.Object,
                _getTeamByCaptainQueryMock.Object,
                _authServiceMock.Object);
        }

        private List<CreatePlayerDto> CreateSeveralPlayers()
        {
            var newPlayers = new List<CreatePlayerDto>();
            newPlayers.AddRange(new List<CreatePlayerDto>
            {
                new CreatePlayerDtoBuilder().WithFirstName("First").WithLastName("Last").Build(),
                new CreatePlayerDtoBuilder().WithFirstName("Second").WithLastName("Last").Build(),
                new CreatePlayerDtoBuilder().WithFirstName("Name").WithLastName("Name").Build()
            });
            return newPlayers;
        }

        private List<Player> CreateListOfExistingPlayers()
        {
            var newPlayers = new List<Player>();
            newPlayers.AddRange(new List<Player>
            {
                new PlayerBuilder(1, "Ant", "Man").Build(),
                new PlayerBuilder(SPECIFIC_PLAYER_ID, "Van", "Van").Build(),
                new PlayerBuilder(SPECIFIC_PLAYER_ID + 2, "Hank", "Ripper").Build()
            });
            return newPlayers;
        }

        private List<Player> CreateTwoOfThreeNewPlayers()
        {
            var newPlayers = new List<Player>();
            newPlayers.AddRange(new List<Player>
            {
                new PlayerBuilder(1, "First", "Last").Build(),
                new PlayerBuilder(0, "NewSecond", "Last").Build(),
                new PlayerBuilder(0, "New Hank", "Ripper").Build()
            });
            return newPlayers;
        }

        private List<Player> CreateListOfExistingPlayersWithoutTeam()
        {
            var newPlayers = new List<Player>();
            newPlayers.AddRange(new List<Player>
            {
                new PlayerBuilder(0, "Ant", "Man").Build(),
                new PlayerBuilder(SPECIFIC_PLAYER_ID, "Van", "Van").Build(),
                new PlayerBuilder(SPECIFIC_PLAYER_ID + 2, "Hank", "Ripper").Build()
            });
            return newPlayers;
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

        private void MockGetTeamByPlayerQuery(int teamId)
        {
            _getTeamByPlayerQueryMock.SetupSequence(t => t.Execute(It.IsAny<FindByPlayerCriteria>()))
                .Returns(teamId);
        }
        private bool PlayersAreEqual(Player x, Player y)
        {
            return new PlayerComparer().Compare(x, y) == 0;
        }
        private void VerifyCreatePlayer(CreatePlayerDto playerDto, Times times)
        {
            _playerRepositoryMock.Verify(pr => pr.Add(playerDto), times);
        }
       
        private void VerifyCreatePlayers(Times times)
        {
            _playerRepositoryMock.Verify(pr => pr.Add(It.IsAny<CreatePlayerDto>()), times);
        }

        private void VerifyEditPlayer(Player player, Times times)
        {
            _playerRepositoryMock.Verify(pr => pr.Update(It.Is<Player>(p => PlayersAreEqual(p, player))), times);
        }

        private void VerifyDeletePlayer(int playerId, Times times)
        {
            _playerRepositoryMock.Verify(pr => pr.Remove(It.Is<int>(id => id == playerId)), times);
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