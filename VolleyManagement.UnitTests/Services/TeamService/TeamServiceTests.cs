namespace VolleyManagement.UnitTests.Services.TeamService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using Data.Queries.Player;
    using Data.Queries.Team;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using VolleyManagement.Contracts.Authorization;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Exceptions;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Domain.PlayersAggregate;
    using VolleyManagement.Domain.Properties;
    using VolleyManagement.Domain.RolesAggregate;
    using VolleyManagement.Domain.TeamsAggregate;
    using VolleyManagement.Services;
    using VolleyManagement.UnitTests.Services.PlayerService;
    using TournamentResources = Domain.Properties.Resources;

    /// <summary>
    /// Tests for TournamentService class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TeamServiceTests
    {
        #region Fields and constants

        private const int SPECIFIC_PLAYER_ID = 2;
        private const int PLAYER_ID = 1;
        private const int SPECIFIC_TEAM_ID = 2;
        private const int UNASSIGNED_ID = 0;
        private const string TEAM_NAME_TO_VALIDATE = "empire";

        private const int ANOTHER_TEAM_ID = SPECIFIC_TEAM_ID + 1;

        private TeamServiceTestFixture _testFixture = new TeamServiceTestFixture();

        private Mock<ITeamRepository> _teamRepositoryMock;
        private Mock<IPlayerRepository> _playerRepositoryMock;
        private Mock<IAuthorizationService> _authServiceMock;
        private Mock<IQuery<Team, FindByIdCriteria>> _getTeamByIdQueryMock;
        private Mock<IQuery<Player, FindByIdCriteria>> _getPlayerByIdQueryMock;
        private Mock<IQuery<Team, FindByCaptainIdCriteria>> _getTeamByCaptainQueryMock;
        private Mock<IQuery<List<Team>, GetAllCriteria>> _getAllTeamsQueryMock;
        private Mock<IQuery<List<Player>, TeamPlayersCriteria>> _getTeamRosterQueryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;

        #endregion

        #region Constuctor

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _teamRepositoryMock = new Mock<ITeamRepository>();
            _playerRepositoryMock = new Mock<IPlayerRepository>();
            _authServiceMock = new Mock<IAuthorizationService>();
            _getTeamByIdQueryMock = new Mock<IQuery<Team, FindByIdCriteria>>();
            _getPlayerByIdQueryMock = new Mock<IQuery<Player, FindByIdCriteria>>();
            _getTeamByCaptainQueryMock = new Mock<IQuery<Team, FindByCaptainIdCriteria>>();
            _getAllTeamsQueryMock = new Mock<IQuery<List<Team>, GetAllCriteria>>();
            _getTeamRosterQueryMock = new Mock<IQuery<List<Player>, TeamPlayersCriteria>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _teamRepositoryMock.Setup(tr => tr.UnitOfWork).Returns(_unitOfWorkMock.Object);
            _playerRepositoryMock.Setup(pr => pr.UnitOfWork).Returns(_unitOfWorkMock.Object);
        }

        #endregion

        #region Team tests

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
            var sut = BuildSUT();
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
            var sut = BuildSUT();

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
            var sut = BuildSUT();

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

            var captain = new PlayerBuilder().WithId(SPECIFIC_PLAYER_ID).WithNoTeam().Build();
            _getPlayerByIdQueryMock.Setup(pr =>
                                          pr.Execute(It.Is<FindByIdCriteria>(cr =>
                                                                             cr.Id == SPECIFIC_PLAYER_ID)))
                                    .Returns(captain);
            MockGetAllTeamsQuery(CreateSeveralTeams());

            // Act
            var sut = BuildSUT();
            sut.Create(newTeam);

            // Assert
            Assert.AreNotEqual(newTeam.Id, UNASSIGNED_ID);
            VerifyCreateTeam(newTeam, Times.Once());
        }

        /// <summary>
        /// Test for Create() method. The method check case when team achievements are invalid.
        /// Should throw ArgumentException
        /// </summary>
        [TestMethod]
        public void Create_InvalidAchievements_ArgumentExceptionThrown()
        {
            string invalidAchievements = CreateInvalidTeamAchievements();
            string argExMessage = string.Format(
                    Resources.ValidationTeamAchievements,
                        VolleyManagement.Domain.Constants.Team.MAX_ACHIEVEMENTS_LENGTH);
            var testTeam = new TeamBuilder()
                                        .WithAchievements(invalidAchievements)
                                        .Build();
            Player testPlayer = new PlayerBuilder().WithId(PLAYER_ID).Build();
            _getPlayerByIdQueryMock.Setup(pr => pr.Execute(It.Is<FindByIdCriteria>(cr => cr.Id == testPlayer.Id))).Returns(testPlayer);
            MockGetAllTeamsQuery(CreateSeveralTeams());
            Exception exception = null;
            var sut = BuildSUT();

            // Act
            try
            {
                sut.Create(testTeam);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(
                exception,
                new ArgumentException(argExMessage, "Achievements"));
        }

        /// <summary>
        /// Test for Create() method. The method should create a new team with empty achievements.
        /// </summary>
        [TestMethod]
        public void Create_EmptyTeamAchievements_TeamCreated()
        {
            // Arrange
            MockGetPlayerByIdQuery(new PlayerBuilder().WithTeamId(SPECIFIC_TEAM_ID).Build());
            var newTeam = new TeamBuilder().WithId(SPECIFIC_TEAM_ID).WithAchievements(string.Empty).Build();
            MockGetAllTeamsQuery(CreateSeveralTeams());

            // Act
            var sut = BuildSUT();
            sut.Create(newTeam);

            // Assert
            VerifyCreateTeam(newTeam, Times.Once());
        }

        /// <summary>
        /// Test for Create() method. The method check case when team name is invalid.
        /// Should throw ArgumentException
        /// </summary>
        [TestMethod]
        public void Create_InvalidTeamName_ArgumentExceptionThrown()
        {
            string invalidName = CreateInvalidTeamName();
            string argExMessage = string.Format(
                    Resources.ValidationTeamName,
                        VolleyManagement.Domain.Constants.Team.MAX_NAME_LENGTH);
            var testTeam = new TeamBuilder()
                                        .WithName(invalidName)
                                        .Build();
            Player testPlayer = new PlayerBuilder().WithId(PLAYER_ID).Build();
            _getPlayerByIdQueryMock.Setup(pr => pr.Execute(It.Is<FindByIdCriteria>(cr => cr.Id == testPlayer.Id))).Returns(testPlayer);
            MockGetAllTeamsQuery(CreateSeveralTeams());
            Exception exception = null;
            var sut = BuildSUT();

            // Act
            try
            {
                sut.Create(testTeam);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(
                exception,
                new ArgumentException(argExMessage, "Name"));
        }

        /// <summary>
        /// Test for Create() method. The method check case when team name is invalid.
        /// Should throw ArgumentException
        /// </summary>
        [TestMethod]
        public void Create_EmptyTeamName_ArgumentExceptionThrown()
        {
            string invalidName = string.Empty;
            string argExMessage = string.Format(
                    Resources.ValidationTeamName,
                    Domain.Constants.Team.MAX_NAME_LENGTH);
            var testTeam = new TeamBuilder()
                                        .WithName(invalidName)
                                        .Build();
            Player testPlayer = new PlayerBuilder().WithId(PLAYER_ID).Build();
            _getPlayerByIdQueryMock.Setup(pr => pr.Execute(It.Is<FindByIdCriteria>(cr => cr.Id == testPlayer.Id))).Returns(testPlayer);
            MockGetAllTeamsQuery(CreateSeveralTeams());
            Exception exception = null;
            var sut = BuildSUT();

            // Act
            try
            {
                sut.Create(testTeam);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(
                exception,
                new ArgumentException(argExMessage, "Name"));
        }

        /// <summary>
        /// Test for Create() method. The method check case when team coach name is invalid.
        /// Should throw ArgumentException
        /// </summary>
        [TestMethod]
        public void Create_InvalidTeamCoachNameNotAllowedLength_ArgumentExceptionThrown()
        {
            string invalidCoachName = CreateInvalidTeamCoachName();
            string argExMessage = string.Format(
                    Resources.ValidationCoachName,
                        VolleyManagement.Domain.Constants.Team.MAX_COACH_NAME_LENGTH);
            var testTeam = new TeamBuilder()
                                        .WithCoach(invalidCoachName)
                                        .Build();
            Player testPlayer = new PlayerBuilder().WithId(PLAYER_ID).Build();
            _getPlayerByIdQueryMock.Setup(pr => pr.Execute(It.Is<FindByIdCriteria>(cr => cr.Id == testPlayer.Id))).Returns(testPlayer);
            MockGetAllTeamsQuery(CreateSeveralTeams());
            Exception exception = null;
            var sut = BuildSUT();

            // Act
            try
            {
                sut.Create(testTeam);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(
                exception,
                new ArgumentException(argExMessage, "Coach"));
        }

        /// <summary>
        /// Test for Create() method. The method check case when team coach name is invalid.
        /// Should throw ArgumentException
        /// </summary>
        [TestMethod]
        public void Create_InvalidTeamCoachNameNotAllowedSymbols_ArgumentExceptionThrown()
        {
            string invalidCoachName = "name%-)";
            string argExMessage = string.Format(
                    Resources.ValidationCoachName,
                        VolleyManagement.Domain.Constants.Team.MAX_COACH_NAME_LENGTH);
            var testTeam = new TeamBuilder()
                                        .WithCoach(invalidCoachName)
                                        .Build();
            Player testPlayer = new PlayerBuilder().WithId(PLAYER_ID).Build();
            _getPlayerByIdQueryMock.Setup(pr => pr.Execute(It.Is<FindByIdCriteria>(cr => cr.Id == testPlayer.Id))).Returns(testPlayer);
            MockGetAllTeamsQuery(CreateSeveralTeams());
            Exception exception = null;
            var sut = BuildSUT();

            // Act
            try
            {
                sut.Create(testTeam);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(
                exception,
                new ArgumentException(argExMessage, "Coach"));
        }

        /// <summary>
        /// Test for Create() method. The method should create a new team with empty coach name.
        /// </summary>
        [TestMethod]
        public void Create_EmptyTeamCoachName_TeamCreated()
        {
            // Arrange
            MockGetPlayerByIdQuery(new PlayerBuilder().WithTeamId(SPECIFIC_TEAM_ID).Build());
            var newTeam = new TeamBuilder().WithId(SPECIFIC_TEAM_ID).WithCoach(string.Empty).Build();
            MockGetAllTeamsQuery(CreateSeveralTeams());

            // Act
            var sut = BuildSUT();
            sut.Create(newTeam);

            // Assert
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
            var sut = BuildSUT();
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
            var captain = new PlayerBuilder().WithTeamId(SPECIFIC_TEAM_ID).Build();

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
            var sut = BuildSUT();
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
            var captain = new PlayerBuilder().WithId(SPECIFIC_PLAYER_ID).WithTeamId(SPECIFIC_TEAM_ID).Build();
            _getPlayerByIdQueryMock.Setup(pr => pr.Execute(It.IsAny<FindByIdCriteria>())).Returns(captain);
            _getTeamByCaptainQueryMock.Setup(tq => tq.Execute(It.IsAny<FindByCaptainIdCriteria>())).Returns(null as Team);
            MockGetAllTeamsQuery(CreateSeveralTeams());

            // Act
            var sut = BuildSUT();
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

            var captain = new PlayerBuilder()
                                        .WithId(SPECIFIC_PLAYER_ID)
                                        .WithNoTeam()
                                        .Build();

            _getPlayerByIdQueryMock.Setup(pr =>
                                          pr.Execute(It.Is<FindByIdCriteria>(
                                              cr =>
                                              cr.Id == captain.Id)))
                                    .Returns(captain);
            MockGetAllTeamsQuery(CreateSeveralTeams());

            // Act
            var sut = BuildSUT();
            sut.Create(newTeam);

            // Assert
            Assert.AreEqual(captain.TeamId, SPECIFIC_TEAM_ID);
            VerifyCreateTeam(newTeam, Times.Once());
        }

        /// <summary>
        /// Test for Create() method. The method check if team already exist
        /// Throw exeption
        /// </summary>
        [TestMethod]
        public void Create_TeamNameIsAlreadyExist_ValidationExceptionThrown()
        {
            // Arrange
            MockGetPlayerByIdQuery(new PlayerBuilder().WithTeamId(SPECIFIC_TEAM_ID).Build());
            var newTeam = new TeamBuilder().WithName(TEAM_NAME_TO_VALIDATE).WithId(SPECIFIC_TEAM_ID).Build();
            var teamWithSameName = new TeamBuilder().WithName(TEAM_NAME_TO_VALIDATE).Build();
            var existingTeams = CreateSeveralTeams();
            existingTeams.Add(teamWithSameName);
            MockGetAllTeamsQuery(existingTeams);

            var sut = BuildSUT();
            Exception exception = null;
            string argExMessage =
                   TournamentResources.TeamNameInTournamentNotUnique;

            // Act
            try
            {
                sut.Create(newTeam);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(
                exception,
                new ArgumentException(argExMessage));
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
            var ts = BuildSUT();
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
            var ts = BuildSUT();
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
            var ts = BuildSUT();
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
            var ts = BuildSUT();
            ts.Delete(SPECIFIC_TEAM_ID);

            // Assert
            _playerRepositoryMock.Verify(
                                         pr => pr.Update(It.Is<Player>(player => expectedRoster.Contains(player))),
                                         Times.Exactly(expectedCountOfPlayers));

            _unitOfWorkMock.Verify(tr => tr.Commit(), Times.Once());
        }

        /// <summary>
        /// Edit() method test. catch ConcurrencyException from DAL
        /// Throws MissingEntityException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(MissingEntityException))]
        public void Edit_CatchDalConcurrencyException_ThrowMissingEntityException()
        {
            // Arrange
            MockGetPlayerByIdQuery(new PlayerBuilder().WithId(SPECIFIC_PLAYER_ID).WithTeamId(UNASSIGNED_ID).Build());
            var teamWithWrongId = new TeamBuilder().WithId(UNASSIGNED_ID).WithCaptain(SPECIFIC_PLAYER_ID).Build();
            _teamRepositoryMock.Setup(pr => pr.Update(It.IsAny<Team>())).Throws(new ConcurrencyException());
            MockGetAllTeamsQuery(CreateSeveralTeams());

            // Act
            var sut = BuildSUT();
            sut.Edit(teamWithWrongId);
        }

        /// <summary>
        /// Test for Edit() method. The method check case when captain id is invalid.
        /// Should throw MissingEntityException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(MissingEntityException))]
        public void Edit_InvalidCaptainId_ThrowMissingEntityException()
        {
            // Arrange
            var teamWithWrongCaptainId = new TeamBuilder().Build();

            // Act
            var sut = BuildSUT();
            sut.Edit(teamWithWrongCaptainId);
        }

        /// <summary>
        /// Test for Edit() method. The method checks case when captain is captain of another team.
        /// Should throw ValidationException
        /// </summary>
        [TestMethod]
        public void Edit_PlayerIsCaptainOfExistingTeam_ValidationExceptionThrown()
        {
            // Arrange
            var newTeam = new TeamBuilder().Build();
            var captain = new PlayerBuilder().WithTeamId(SPECIFIC_TEAM_ID).Build();

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

            MockGetAllTeamsQuery(CreateSeveralTeams());

            // Act
            var sut = BuildSUT();
            bool gotException = false;

            try
            {
                sut.Edit(newTeam);
            }
            catch (ValidationException)
            {
                gotException = true;
            }

            // Assert
            Assert.IsTrue(gotException);
            VerifyEditTeam(newTeam, Times.Never());
        }

        /// <summary>
        /// Test for Edit() method. Existing team should be updated
        /// </summary>
        [TestMethod]
        public void Edit_TeamPassed_TeamUpdated()
        {
            // Arrange
            MockGetPlayerByIdQuery(new PlayerBuilder().WithId(SPECIFIC_PLAYER_ID).WithTeamId(SPECIFIC_TEAM_ID).Build());
            var teamToEdit = new TeamBuilder().WithId(SPECIFIC_TEAM_ID).Build();
            MockGetAllTeamsQuery(CreateSeveralTeams());

            // Act
            var sut = BuildSUT();
            sut.Edit(teamToEdit);

            // Assert
            VerifyEditTeam(teamToEdit, Times.Once());
        }

        /// <summary>
        /// Test for Edit() method. Existing team should be updated
        /// </summary>
        [TestMethod]
        public void Edit_TeamNameIsAlreadyExist_ValidationExceptionThrown()
        {
            // Arrange
            MockGetPlayerByIdQuery(new PlayerBuilder().WithId(SPECIFIC_PLAYER_ID).WithTeamId(SPECIFIC_TEAM_ID).Build());
            var teamToEdit = new TeamBuilder().WithName(TEAM_NAME_TO_VALIDATE).WithId(SPECIFIC_TEAM_ID).Build();
            var teamWithSameName = new TeamBuilder().WithName(TEAM_NAME_TO_VALIDATE).Build();
            var existingTeams = CreateSeveralTeams();
            existingTeams.Add(teamWithSameName);
            MockGetAllTeamsQuery(existingTeams);

            var sut = BuildSUT();
            Exception exception = null;
            string argExMessage =
                   TournamentResources.TeamNameInTournamentNotUnique;

            // Act
            try
            {
                sut.Edit(teamToEdit);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(
                exception,
                new ArgumentException(argExMessage));
        }

        /// <summary>
        /// Test for Edit() method. Any property except name
        /// </summary>
        [TestMethod]
        public void Edit_TeamNameAlreadyExist_TeamUpdated()
        {
            // Arrange
            MockGetPlayerByIdQuery(new PlayerBuilder().WithId(SPECIFIC_PLAYER_ID).WithTeamId(SPECIFIC_TEAM_ID).Build());
            var teamToEdit = new TeamBuilder().WithName(TEAM_NAME_TO_VALIDATE).WithId(SPECIFIC_TEAM_ID).Build();
            MockGetAllTeamsQuery(CreateSeveralTeams());
            MockGetTeamByIdQuery(teamToEdit);

            // Act
            var sut = BuildSUT();
            sut.Edit(teamToEdit);

            // Assert
            VerifyEditTeam(teamToEdit, Times.Once());
        }

        /// <summary>
        /// Test for UpdateRosterTeamId() method.
        /// Case when specified player isn't exist. Throw MissingEntityException
        /// </summary>
        [TestMethod]
        public void UpdateRosterTeamId_InvalidPlayerId_MissingEntityExceptionThrown()
        {
            // Arrange
            Player invalidPlayer = new PlayerBuilder().WithId(UNASSIGNED_ID).Build();
            List<Player> roster = new List<Player> { invalidPlayer };

            MockGetTeamByIdQuery(new TeamBuilder().WithId(SPECIFIC_TEAM_ID).Build());

            var testPlayer = new PlayerBuilder().WithTeamId(SPECIFIC_TEAM_ID).Build();
            MockGetTeamRosterQuery(new List<Player> { testPlayer });

            // Act
            var ts = BuildSUT();
            bool gotException = false;

            try
            {
                ts.UpdateRosterTeamId(roster, SPECIFIC_TEAM_ID);
            }
            catch (MissingEntityException)
            {
                gotException = true;
            }

            // Assert
            Assert.IsTrue(gotException);
            VerifyEditPlayer(UNASSIGNED_ID, SPECIFIC_TEAM_ID, Times.Never());
        }

        /// <summary>
        /// Test for UpdateRosterTeamId() method.
        /// Case when specified team isn't exist. Throw MissingEntityException
        /// </summary>
        [TestMethod]
        public void UpdateRosterTeamId_InvalidTeamId_MissingEntityExceptionThrown()
        {
            // Arrange
            Player testPlayer = new PlayerBuilder().WithId(SPECIFIC_PLAYER_ID).Build();
            List<Player> roster = new List<Player> { testPlayer };

            MockGetAllTeamsQuery(new TeamServiceTestFixture().TestTeams().Build());

            Player testData = new PlayerBuilder().WithId(SPECIFIC_PLAYER_ID).WithTeamId(UNASSIGNED_ID).Build();
            MockGetPlayerByIdQuery(testData);

            List<Player> rosterOfInvalidTeam = new List<Player> { testData };
            MockGetTeamRosterQuery(rosterOfInvalidTeam);

            // Act
            var ts = BuildSUT();
            bool gotException = false;

            try
            {
                ts.UpdateRosterTeamId(roster, UNASSIGNED_ID);
            }
            catch (MissingEntityException)
            {
                gotException = true;
            }

            // Assert
            Assert.IsTrue(gotException);
            VerifyEditPlayer(SPECIFIC_PLAYER_ID, UNASSIGNED_ID, Times.Never());
        }

        /// <summary>
        /// Test for UpdateRosterTeamId() method.
        /// Case when edited player's is captain of existing team and
        /// new teamId is null or not equal Id of existing team
        /// The method should throw InvalidOperationException.
        /// </summary>
        [TestMethod]
        public void UpdateRosterTeamId_PlayerIsCaptainOfExistingTeam_ValidationExceptionThrown()
        {
            // Arrange
            Player captain = new PlayerBuilder().WithId(SPECIFIC_PLAYER_ID).WithTeamId(SPECIFIC_TEAM_ID).Build();
            List<Player> roster = new List<Player> { captain };
            MockGetTeamRosterQuery(roster);

            MockGetPlayerByIdQuery(new PlayerBuilder()
                                                .WithId(SPECIFIC_PLAYER_ID)
                                                .WithTeamId(SPECIFIC_TEAM_ID)
                                                .Build());

            var existingTeam = new TeamBuilder().WithId(SPECIFIC_TEAM_ID).WithCaptain(SPECIFIC_PLAYER_ID).Build();

            var teamToSet = new TeamBuilder().WithId(ANOTHER_TEAM_ID).Build();

            _getTeamByCaptainQueryMock.Setup(tr => tr.Execute(It.IsAny<FindByCaptainIdCriteria>())).Returns(existingTeam);

            // Act
            var ts = BuildSUT();
            bool gotException = false;

            try
            {
                ts.UpdateRosterTeamId(roster, ANOTHER_TEAM_ID);
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
        /// Successful Test for UpdateRosterTeamId() method.
        /// Case when edited player's is player of existing team but
        /// player is not captain
        /// </summary>
        [TestMethod]
        public void UpdateRosterTeamId_PlayerIsNotCaptainOfExistingTeam_PlayerUpdated()
        {
            // Arrange
            Player testPlayer = new PlayerBuilder().WithId(PLAYER_ID).WithTeamId(SPECIFIC_TEAM_ID).Build();
            List<Player> testRoster = new List<Player> { testPlayer };
            MockGetTeamRosterQuery(testRoster);

            Player captain = new PlayerBuilder().WithId(SPECIFIC_PLAYER_ID).WithTeamId(ANOTHER_TEAM_ID).Build();
            List<Player> roster = new List<Player> { captain };
            MockGetPlayerByIdQuery(captain);

            var teamToSet = new TeamBuilder().WithId(SPECIFIC_TEAM_ID).Build();

            _getTeamByCaptainQueryMock.Setup(tr => tr.Execute(It.IsAny<FindByCaptainIdCriteria>())).Returns(null as Team);
            _getTeamByIdQueryMock.Setup(tr => tr.Execute(It.IsAny<FindByIdCriteria>())).Returns(teamToSet);

            // Act
            var ts = BuildSUT();
            ts.UpdateRosterTeamId(roster, SPECIFIC_TEAM_ID);

            // Assert
            VerifyEditPlayer(SPECIFIC_PLAYER_ID, SPECIFIC_TEAM_ID, Times.Once());
        }

        /// <summary>
        /// Successful Test for UpdateRosterTeamId() method.
        /// </summary>
        [TestMethod]
        public void UpdateRosterTeamId_PlayerAndTeamPassed_PlayerUpdated()
        {
            // Arrange
            Player testPlayer = new PlayerBuilder().WithId(PLAYER_ID).WithTeamId(SPECIFIC_TEAM_ID).Build();
            List<Player> testRoster = new List<Player> { testPlayer };
            MockGetTeamRosterQuery(testRoster);

            Player player = new PlayerBuilder().WithId(SPECIFIC_PLAYER_ID).WithTeamId(null).Build();
            List<Player> roster = new List<Player> { player };
            MockGetPlayerByIdQuery(player);

            var teamToSet = new TeamBuilder().WithId(SPECIFIC_TEAM_ID).Build();
            _getTeamByIdQueryMock.Setup(tr => tr.Execute(It.IsAny<FindByIdCriteria>())).Returns(teamToSet);

            // Act
            var ts = BuildSUT();
            ts.UpdateRosterTeamId(roster, SPECIFIC_TEAM_ID);

            // Assert
            VerifyEditPlayer(SPECIFIC_PLAYER_ID, SPECIFIC_TEAM_ID, Times.Once());
        }

        #endregion

        #region Authorization team tests

        /// <summary>
        /// Test for Create() method with no permission for such action. The method has to throw AuthorizationException,
        /// should invoke CheckAccess() and shouldn't invoke Commit() method of IUnitOfWork.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AuthorizationException))]
        public void Create_CreateNotPermitted_ExceptionThrown()
        {
            // Arrange
            var testData = new TeamBuilder().WithId(SPECIFIC_TEAM_ID).Build();
            MockAuthServiceThrowsExeption(AuthOperations.Teams.Create);

            var sut = BuildSUT();

            // Act
            sut.Create(testData);

            // Assert
            VerifyCreateTeam(testData, Times.Never());
            VerifyCheckAccess(AuthOperations.Teams.Create, Times.Once());
        }

        /// <summary>
        /// Test for Delete() method with no permission for such action. The method has to throw AuthorizationException,
        /// should invoke CheckAccess() and shouldn't invoke Commit() method of IUnitOfWork
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AuthorizationException))]
        public void Delete_DeleteNotPermitted_ExceptionThrown()
        {
            // Arrange
            var testData = new TeamBuilder().WithId(SPECIFIC_TEAM_ID).Build();
            MockAuthServiceThrowsExeption(AuthOperations.Teams.Delete);

            var sut = BuildSUT();

            // Act
            sut.Delete(testData.Id);

            // Assert
            VerifyDeleteTeam(testData.Id, Times.Never());
            VerifyCheckAccess(AuthOperations.Teams.Delete, Times.Once());
        }

        /// <summary>
        /// Test for Edit() method with no permission for such action. The method has to throw AuthorizationException,
        /// should invoke CheckAccess() and shouldn't invoke Commit() method of IUnitOfWork
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AuthorizationException))]
        public void Edit_EditNotPermitted_ExceptionThrown()
        {
            // Arrange
            var testData = new TeamBuilder().WithId(SPECIFIC_TEAM_ID).Build();
            MockAuthServiceThrowsExeption(AuthOperations.Teams.Edit);

            var sut = BuildSUT();

            // Act
            sut.Edit(testData);

            // Assert
            VerifyEditTeam(testData, Times.Never());
            VerifyCheckAccess(AuthOperations.Teams.Edit, Times.Once());
        }

        #endregion

        #region Private

        private TeamService BuildSUT()
        {
            return new TeamService(
                _teamRepositoryMock.Object,
                _playerRepositoryMock.Object,
                _getTeamByIdQueryMock.Object,
                _getPlayerByIdQueryMock.Object,
                _getTeamByCaptainQueryMock.Object,
                _getAllTeamsQueryMock.Object,
                _getTeamRosterQueryMock.Object,
                _authServiceMock.Object);
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

        private void VerifyEditTeam(Team team, Times times)
        {
            _teamRepositoryMock.Verify(tr => tr.Update(It.Is<Team>(t => TeamsAreEqual(t, team))), times);
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

        private List<Team> CreateSeveralTeams()
        {
            var existingTeams = new List<Team>();
            existingTeams.AddRange(new List<Team>
            {
                new TeamBuilder().WithId(SPECIFIC_TEAM_ID + 6).WithName("First").Build(),
                new TeamBuilder().WithId(SPECIFIC_TEAM_ID + 2).WithName("Second").Build(),
                new TeamBuilder().WithId(SPECIFIC_TEAM_ID + 4).WithName("Third").Build(),
            });
            return existingTeams;
        }

        /// <summary>
        /// Creates Achievements with more number of symbols than it is allowed
        /// </summary>
        /// <returns>Invalid Achievements</returns>
        private string CreateInvalidTeamAchievements()
        {
            StringBuilder invalidAchievements = new StringBuilder();
            for (int i = 0; i < VolleyManagement.Domain.Constants.Team.MAX_ACHIEVEMENTS_LENGTH + 1; i++)
            {
                invalidAchievements.Append("a");
            }

            return invalidAchievements.ToString();
        }

        /// <summary>
        /// Creates Team name with more number of symbols than it is allowed
        /// </summary>
        /// <returns>Invalid team name</returns>
        private string CreateInvalidTeamName()
        {
            StringBuilder invalidTeamName = new StringBuilder();
            for (int i = 0; i < VolleyManagement.Domain.Constants.Team.MAX_NAME_LENGTH + 1; i++)
            {
                invalidTeamName.Append("a");
            }

            return invalidTeamName.ToString();
        }

        /// <summary>
        /// Creates Team coach name with more number of symbols than it is allowed
        /// </summary>
        /// <returns>Invalid team coach name</returns>
        private string CreateInvalidTeamCoachName()
        {
            StringBuilder invalidTeamCoachName = new StringBuilder();
            for (int i = 0; i < VolleyManagement.Domain.Constants.Team.MAX_COACH_NAME_LENGTH + 1; i++)
            {
                invalidTeamCoachName.Append("a");
            }

            return invalidTeamCoachName.ToString();
        }

        /// <summary>
        /// Checks if exception was thrown and has appropriate message
        /// Checks if thrown exceptions are of the same type
        /// </summary>
        /// <param name="exception">Exception that has been thrown</param>
        /// <param name="expected">Expected exception</param>
        private void VerifyExceptionThrown(Exception exception, Exception expected)
        {
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception.GetType().Equals(expected.GetType()), "Exception is of the wrong type");
            Assert.IsTrue(exception.Message.Equals(expected.Message));
        }

        private void VerifyCheckAccess(AuthOperation operation, Times times)
        {
            _authServiceMock.Verify(tr => tr.CheckAccess(operation), times);
        }

        private void MockAuthServiceThrowsExeption(AuthOperation operation)
        {
            _authServiceMock.Setup(tr => tr.CheckAccess(operation)).Throws<AuthorizationException>();
        }
        #endregion
    }
}
