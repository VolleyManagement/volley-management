namespace VolleyManagement.UnitTests.Services.TeamService
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Data.Queries.Player;
    using Data.Queries.Team;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Exceptions;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Domain.PlayersAggregate;
    using VolleyManagement.Domain.TeamsAggregate;
    using VolleyManagement.Services;
    using VolleyManagement.UnitTests.Services.PlayerService;

    /// <summary>
    /// Tests for TournamentService class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TeamServiceTests
    {
        private const int SPECIFIC_PLAYER_ID = 2;
        private const int SPECIFIC_TEAM_ID = 2;
        private const int UNASSIGNED_ID = 0;

        private const int ANOTHER_TEAM_ID = SPECIFIC_TEAM_ID + 1;

        private readonly TeamServiceTestFixture _testFixture = new TeamServiceTestFixture();
        private readonly Mock<ITeamRepository> _teamRepositoryMock = new Mock<ITeamRepository>();
        private readonly Mock<IPlayerRepository> _playerRepositoryMock = new Mock<IPlayerRepository>();
        private readonly Mock<IQuery<Team, FindByIdCriteria>> _getTeamByIdQueryMock =
            new Mock<IQuery<Team, FindByIdCriteria>>();

        private readonly Mock<IQuery<Player, FindByIdCriteria>> _getPlayerByIdQueryMock =
            new Mock<IQuery<Player, FindByIdCriteria>>();

        private readonly Mock<IQuery<Team, FindByCaptainIdCriteria>> _getTeamByCaptainQueryMock =
            new Mock<IQuery<Team, FindByCaptainIdCriteria>>();

        private readonly Mock<IQuery<List<Team>, GetAllCriteria>> _getAllTeamsQueryMock =
            new Mock<IQuery<List<Team>, GetAllCriteria>>();

        private readonly Mock<IQuery<List<Player>, TeamPlayersCriteria>> _getTeamRosterQueryMock =
            new Mock<IQuery<List<Player>, TeamPlayersCriteria>>();

        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

        private IKernel _kernel;

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<ITeamRepository>().ToConstant(_teamRepositoryMock.Object);
            _kernel.Bind<IPlayerRepository>().ToConstant(_playerRepositoryMock.Object);
            _kernel.Bind<IQuery<Team, FindByIdCriteria>>().ToConstant(_getTeamByIdQueryMock.Object);
            _kernel.Bind<IQuery<Player, FindByIdCriteria>>().ToConstant(_getPlayerByIdQueryMock.Object);
            _kernel.Bind<IQuery<Team, FindByCaptainIdCriteria>>().ToConstant(_getTeamByCaptainQueryMock.Object);
            _kernel.Bind<IQuery<List<Team>, GetAllCriteria>>().ToConstant(_getAllTeamsQueryMock.Object);
            _kernel.Bind<IQuery<List<Player>, TeamPlayersCriteria>>().ToConstant(_getTeamRosterQueryMock.Object);
            _teamRepositoryMock.Setup(tr => tr.UnitOfWork).Returns(_unitOfWorkMock.Object);
            _playerRepositoryMock.Setup(pr => pr.UnitOfWork).Returns(_unitOfWorkMock.Object);
        }

        /// <summary>
        /// Test for Get() method. The method should return existing teams
        /// </summary>
        [TestMethod]
        public void GetAll_TeamsExist_TeamsReturned()
        {
            // Arrange
            var testData = _testFixture.TestTeams().Build();
            MockGetAllTeamsQuery(testData);

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
        /// Test for Get() method. The method should return existing team
        /// </summary>
        [TestMethod]
        public void GetById_TeamExist_TeamReturned()
        {
            // Arrange
            var testData = new TeamBuilder().WithId(SPECIFIC_TEAM_ID).Build();
            MockGetTeamByIdQuery(testData);
            var expected = new TeamBuilder().WithId(SPECIFIC_TEAM_ID).Build();
            var sut = _kernel.Get<TeamService>();

            // Act
            var actual = sut.Get(SPECIFIC_TEAM_ID);

            // Assert
            TestHelper.AreEqual<Team>(expected, actual, new TeamComparer());            
        }

        /// <summary>
        /// Test for GetTeamCaptain() method.
        /// The method should return existing captain
        /// </summary>
        [TestMethod]
        public void GetTeamCaptain_CaptainExist_PlayerReturned()
        {
            // Arrange
            var captain = new PlayerBuilder().WithId(SPECIFIC_PLAYER_ID).Build();
            var team = new TeamBuilder().WithCaptain(SPECIFIC_PLAYER_ID).Build();
            _getPlayerByIdQueryMock.Setup(pr =>
                                          pr.Execute(It.Is<FindByIdCriteria>(cr =>
                                                                             cr.Id == SPECIFIC_PLAYER_ID)))
                                    .Returns(captain);
            var expected = new PlayerBuilder().WithId(SPECIFIC_PLAYER_ID).Build();
            var sut = _kernel.Get<TeamService>();

            // Act
            var actual = sut.GetTeamCaptain(team);

            // Assert
            TestHelper.AreEqual<Player>(expected, actual, new PlayerComparer());
        }

        /// <summary>
        /// Test for Create() method. The method should create a new team.
        /// </summary>
        [TestMethod]
        public void Create_TeamPassed_TeamCreated()
        {
            // Arrange
            var newTeam = new TeamBuilder().WithCaptain(SPECIFIC_PLAYER_ID).Build();
            _teamRepositoryMock.Setup(tr => tr.Add(It.IsAny<Team>()))
                .Callback<Team>(t => t.Id = SPECIFIC_TEAM_ID);

            var captain = (new PlayerBuilder()).WithId(SPECIFIC_PLAYER_ID).WithNoTeam().Build();
            _getPlayerByIdQueryMock.Setup(pr =>
                                          pr.Execute(It.Is<FindByIdCriteria>(cr =>
                                                                             cr.Id == SPECIFIC_PLAYER_ID)))
                                    .Returns(captain);

            // Act
            var sut = _kernel.Get<TeamService>();
            sut.Create(newTeam);

            // Assert
            Assert.AreNotEqual(newTeam.Id, UNASSIGNED_ID);
            VerifyCreateTeam(newTeam, Times.Once());
        }

        /// <summary>
        /// Test for Create() method. The method check case when captain id is invalid.
        /// Should throw MissingEntityException
        /// </summary>
        [TestMethod]
        public void Create_InvalidCaptainId_MissingEntityExceptionThrown()
        {
            // Arrange
            var newTeam = new TeamBuilder().Build();
            Player testPlayer = new PlayerBuilder().WithId(SPECIFIC_PLAYER_ID).Build();
            _getPlayerByIdQueryMock.Setup(pr => pr.Execute(It.Is<FindByIdCriteria>(cr => cr.Id == testPlayer.Id))).Returns(testPlayer);

            // Act
            var sut = _kernel.Get<TeamService>();
            bool gotException = false;

            try
            {
                sut.Create(newTeam);
            }
            catch (MissingEntityException)
            {
                gotException = true;
            }

            // Assert
            Assert.IsTrue(gotException);
            VerifyCreateTeam(newTeam, Times.Never());
        }

        /// <summary>
        /// Test for Create() method. The method check case when captain is captain of another team.
        /// Should throw ValidationException
        /// </summary>
        [TestMethod]
        public void Create_PlayerIsCaptainOfExistingTeam_ValidationExceptionThrown()
        {
            // Arrange
            var newTeam = new TeamBuilder().Build();
            var captain = (new PlayerBuilder()).WithTeamId(SPECIFIC_TEAM_ID).Build();

            var captainLeadTeam = new TeamBuilder().WithId(SPECIFIC_TEAM_ID).Build();
            var testTeams = new TeamServiceTestFixture().AddTeam(captainLeadTeam).Build();
            _getPlayerByIdQueryMock.Setup(pr =>
                            pr.Execute(It.Is<FindByIdCriteria>(cr =>
                                            cr.Id == captain.Id)))
                                            .Returns(captain);
            _getTeamByCaptainQueryMock.Setup(tm =>
                            tm.Execute(It.Is<FindByCaptainIdCriteria>(cr =>
                                                                    cr.CaptainId == captain.Id)))
                            .Returns(testTeams.Where(tm => tm.Id == captain.TeamId).FirstOrDefault());

            // Act
            var sut = _kernel.Get<TeamService>();
            bool gotException = false;

            try
            {
                sut.Create(newTeam);
            }
            catch (ValidationException)
            {
                gotException = true;
            }

            // Assert
            Assert.IsTrue(gotException);
            VerifyCreateTeam(newTeam, Times.Never());
        }

        /// <summary>
        /// Test for Create() method. 
        /// The method check case when captain is player of another team.
        /// The method should create a new team.
        /// </summary>
        [TestMethod]
        public void Create_PlayerIsNotCaptainOfExistingTeam_TeamCreated()
        {
            // Arrange
            var newTeam = new TeamBuilder().WithCaptain(SPECIFIC_PLAYER_ID).Build();            
            var captain = (new PlayerBuilder()).WithId(SPECIFIC_PLAYER_ID).WithTeamId(SPECIFIC_TEAM_ID).Build();
            _getPlayerByIdQueryMock.Setup(pr => pr.Execute(It.IsAny<FindByIdCriteria>())).Returns(captain);
            _getTeamByCaptainQueryMock.Setup(tq => tq.Execute(It.IsAny<FindByCaptainIdCriteria>())).Returns(null as Team);

            // Act
            var sut = _kernel.Get<TeamService>();
            sut.Create(newTeam);

            // Assert
            Assert.AreEqual(newTeam.Id, captain.TeamId);
            VerifyCreateTeam(newTeam, Times.Once());
        }

        /// <summary>
        /// Test for Create() method. The method check that captain's teamId
        /// was updated after creating team in DB
        /// </summary>
        [TestMethod]
        public void Create_TeamPassed_CaptainUpdated()
        {
            // Arrange
            var newTeam = new TeamBuilder().WithCaptain(SPECIFIC_PLAYER_ID).Build();
            _teamRepositoryMock.Setup(tr => tr.Add(It.IsAny<Team>()))
                .Callback<Team>(t => t.Id = SPECIFIC_TEAM_ID);

            var captain = (new PlayerBuilder())
                                        .WithId(SPECIFIC_PLAYER_ID)
                                        .WithNoTeam()
                                        .Build();

            _getPlayerByIdQueryMock.Setup(pr =>
                                          pr.Execute(It.Is<FindByIdCriteria>(
                                              cr =>
                                              cr.Id == captain.Id)))
                                    .Returns(captain);

            // Act
            var sut = _kernel.Get<TeamService>();
            sut.Create(newTeam);

            // Assert
            Assert.AreEqual(captain.TeamId, SPECIFIC_TEAM_ID);
            VerifyCreateTeam(newTeam, Times.Once());
        }

        /// <summary>
        /// Test for Delete() method. The method should delete specified team.
        /// </summary>
        [TestMethod]
        public void Delete_TeamPassed_CorrectIdPostedToDatabaseLayer()
        {
            // Arrange
            var testData = new PlayerServiceTestFixture()
                .Build();
            MockGetTeamRosterQuery(testData.ToList());

            // Act
            var ts = _kernel.Get<TeamService>();
            ts.Delete(SPECIFIC_TEAM_ID);

            // Assert
            VerifyDeleteTeam(SPECIFIC_TEAM_ID, Times.Once());
        }

        /// <summary>
        /// Test for Delete() method. The method check case when team id is invalid.
        /// Should throw MissingEntityException
        /// </summary>
        [TestMethod]
        public void Delete_InvalidTeamId_MissingEntityExceptionThrown()
        {
            // Arrange
            _teamRepositoryMock.Setup(tr => tr.Remove(It.IsAny<int>()))
                .Throws(new InvalidKeyValueException());

            // Act
            var ts = _kernel.Get<TeamService>();
            bool gotException = false;

            try
            {
                ts.Delete(SPECIFIC_TEAM_ID);
            }
            catch (MissingEntityException)
            {
                gotException = true;
            }

            // Assert
            Assert.IsTrue(gotException);
            VerifyDeleteTeam(SPECIFIC_TEAM_ID, Times.Once(), Times.Never());
        }

        /// <summary>
        /// Successful test for Delete() method.
        /// </summary>
        [TestMethod]
        public void Delete_TeamPassed_TeamDeleted()
        {
            // Arrange
            var testData = new PlayerServiceTestFixture()
                    .Build();
            MockGetTeamRosterQuery(testData.ToList());

            // Act
            var ts = _kernel.Get<TeamService>();
            ts.Delete(SPECIFIC_TEAM_ID);

            // Assert
            VerifyDeleteTeam(SPECIFIC_TEAM_ID, Times.Once());
        }

        /// <summary>
        /// Successful test for Delete() method.
        /// </summary>
        [TestMethod]
        public void Delete_TeamPassed_RosterPlayersUpdated()
        {
            // Arrange
            IQueryable<Player> expectedRoster = new PlayerServiceTestFixture().TestPlayers().Build().AsQueryable<Player>();
            int expectedCountOfPlayers = expectedRoster.Count();
            MockGetTeamRosterQuery(expectedRoster.ToList());

            // Act
            var ts = _kernel.Get<TeamService>();
            ts.Delete(SPECIFIC_TEAM_ID);

            // Assert
            _playerRepositoryMock.Verify(
                                         pr => pr.Update(It.Is<Player>(player => expectedRoster.Contains(player))),
                                         Times.Exactly(expectedCountOfPlayers));

            _unitOfWorkMock.Verify(tr => tr.Commit(), Times.Once());
        }

        /// <summary>
        /// Test for SetPlayerTeam() method.
        /// Case when specified player isn't exist. Throw MissingEntityException
        /// </summary>
        [TestMethod]
        public void UpdatePlayerTeam_InvalidPlayerId_MissingEntityExceptionThrown()
        {
            // Arrange
            MockGetPlayerByIdQuery(null);

            // Act
            var ts = _kernel.Get<TeamService>();
            bool gotException = false;

            try
            {
                ts.UpdatePlayerTeam(SPECIFIC_PLAYER_ID, SPECIFIC_TEAM_ID);
            }
            catch (MissingEntityException)
            {
                gotException = true;
            }

            // Assert
            Assert.IsTrue(gotException);
            VerifyEditPlayer(SPECIFIC_PLAYER_ID, SPECIFIC_TEAM_ID, Times.Never());
        }

        /// <summary>
        /// Test for SetPlayerTeam() method.
        /// Case when specified team isn't exist. Throw MissingEntityException
        /// </summary>
        [TestMethod]
        public void UpdatePlayerTeam_InvalidTeamId_MissingEntityExceptionThrown()
        {
            // Arrange
            MockGetPlayerByIdQuery(new PlayerBuilder().WithNoTeam().Build());
            var emptyTeamList = new TeamServiceTestFixture().Build().AsQueryable<Team>();

            // Act
            var ts = _kernel.Get<TeamService>();
            bool gotException = false;

            try
            {
                ts.UpdatePlayerTeam(SPECIFIC_PLAYER_ID, SPECIFIC_TEAM_ID);
            }
            catch (MissingEntityException)
            {
                gotException = true;
            }

            // Assert
            Assert.IsTrue(gotException);
            VerifyEditPlayer(SPECIFIC_PLAYER_ID, SPECIFIC_TEAM_ID, Times.Never());
        }

        /// <summary>
        /// Test for SetPlayerTeam() method.
        /// Case when edited player's is captain of existing team and
        /// new teamId is null or not equal Id of existing team
        /// The method should throw InvalidOperationException.
        /// </summary>
        [TestMethod]
        public void UpdatePlayerTeam_PlayerIsCaptainOfExistingTeam_ValidationExceptionThrown()
        {
            // Arrange
            MockGetPlayerByIdQuery(new PlayerBuilder()
                                                .WithId(SPECIFIC_PLAYER_ID)
                                                .WithTeamId(SPECIFIC_TEAM_ID)
                                                .Build());

            var existingTeam = new TeamBuilder().WithId(SPECIFIC_TEAM_ID).WithCaptain(SPECIFIC_PLAYER_ID).Build();

            var teamToSet = new TeamBuilder().WithId(ANOTHER_TEAM_ID).Build();

            _getTeamByCaptainQueryMock.Setup(tr => tr.Execute(It.IsAny<FindByCaptainIdCriteria>())).Returns(existingTeam);

            // Act
            var ts = _kernel.Get<TeamService>();
            bool gotException = false;

            try
            {
                ts.UpdatePlayerTeam(SPECIFIC_PLAYER_ID, ANOTHER_TEAM_ID);
            }
            catch (ValidationException)
            {
                gotException = true;
            }

            // Assert
            Assert.IsTrue(gotException);
            VerifyEditPlayer(SPECIFIC_PLAYER_ID, ANOTHER_TEAM_ID, Times.Never());
        }

        /// <summary>
        /// Successful Test for SetPlayerTeam() method.
        /// Case when edited player's is player of existing team but
        /// player is not captain
        /// </summary>
        [TestMethod]
        public void UpdatePlayerTeam_PlayerIsNotCaptainOfExistingTeam_PlayerUpdated()
        {
            // Arrange
            MockGetPlayerByIdQuery(new PlayerBuilder().WithId(SPECIFIC_PLAYER_ID).WithTeamId(ANOTHER_TEAM_ID).Build());
            var teamToSet = new TeamBuilder().WithId(SPECIFIC_TEAM_ID).Build();

            _getTeamByCaptainQueryMock.Setup(tr => tr.Execute(It.IsAny<FindByCaptainIdCriteria>())).Returns(null as Team);
            _getTeamByIdQueryMock.Setup(tr => tr.Execute(It.IsAny<FindByIdCriteria>())).Returns(teamToSet);

            // Act
            var ts = _kernel.Get<TeamService>();
            ts.UpdatePlayerTeam(SPECIFIC_PLAYER_ID, SPECIFIC_TEAM_ID);

            // Assert
            VerifyEditPlayer(SPECIFIC_PLAYER_ID, SPECIFIC_TEAM_ID, Times.Once());
        }

        /// <summary>
        /// Successful Test for SetPlayerTeam() method.
        /// </summary>
        [TestMethod]
        public void UpdatePlayerTeam_PlayerAndTeamPassed_PlayerUpdated()
        {
            // Arrange
            MockGetPlayerByIdQuery(new PlayerBuilder().WithId(SPECIFIC_PLAYER_ID).WithTeamId(null).Build());
            var teamToSet = new TeamBuilder().WithId(SPECIFIC_TEAM_ID).Build();

            _getTeamByIdQueryMock.Setup(tr => tr.Execute(It.IsAny<FindByIdCriteria>())).Returns(teamToSet);

            // Act
            var ts = _kernel.Get<TeamService>();
            ts.UpdatePlayerTeam(SPECIFIC_PLAYER_ID, SPECIFIC_TEAM_ID);

            // Assert
            VerifyEditPlayer(SPECIFIC_PLAYER_ID, SPECIFIC_TEAM_ID, Times.Once());
        }

        private bool TeamsAreEqual(Team x, Team y)
        {
            return new TeamComparer().Compare(x, y) == 0;
        }

        private void MockGetAllTeamsQuery(IEnumerable<Team> testData)
        {
            _getAllTeamsQueryMock.Setup(tr => tr.Execute(It.IsAny<GetAllCriteria>())).Returns(testData.ToList());
        }

        private void MockGetTeamByIdQuery(Team testData)
        {
            _getTeamByIdQueryMock.Setup(tr => tr.Execute(It.IsAny<FindByIdCriteria>())).Returns(testData);
        }

        private void MockGetPlayerByIdQuery(Player player)
        {
            _getPlayerByIdQueryMock.Setup(tr => tr.Execute(It.IsAny<FindByIdCriteria>())).Returns(player);
        }

        private void MockGetTeamRosterQuery(List<Player> players)
        {
            _getTeamRosterQueryMock.Setup(tr => tr.Execute(It.IsAny<TeamPlayersCriteria>())).Returns(players);
        }

        private void VerifyCreateTeam(Team team, Times times)
        {
            _teamRepositoryMock.Verify(tr => tr.Add(It.Is<Team>(t => TeamsAreEqual(t, team))), times);
            _unitOfWorkMock.Verify(uow => uow.Commit(), times);
        }

        private void VerifyEditPlayer(int playerId, int teamId, Times times)
        {
            _playerRepositoryMock.Verify(pr => pr.Update(It.Is<Player>(p => p.Id == playerId && p.TeamId == teamId)), times);
            _unitOfWorkMock.Verify(uow => uow.Commit(), times);
        }

        private void VerifyDeleteTeam(int teamId, Times times)
        {
            _teamRepositoryMock.Verify(tr => tr.Remove(It.Is<int>(id => id == teamId)), times);
            _unitOfWorkMock.Verify(uow => uow.Commit(), times);
        }

        private void VerifyDeleteTeam(int teamId, Times repositoryTimes, Times unitOfWorkTimes)
        {
            _teamRepositoryMock.Verify(tr => tr.Remove(It.Is<int>(id => id == teamId)), repositoryTimes);
            _unitOfWorkMock.Verify(uow => uow.Commit(), unitOfWorkTimes);
        }
    }
}
