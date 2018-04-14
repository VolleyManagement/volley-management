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
    using Contracts.Authorization;
    using Contracts.Exceptions;
    using Data.Contracts;
    using Data.Exceptions;
    using Data.Queries.Common;
    using Domain.PlayersAggregate;
    using Domain.Properties;
    using Domain.RolesAggregate;
    using Domain.TeamsAggregate;
    using VolleyManagement.Services;
    using PlayerService;
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
        private const string TEAM_NOT_FOUND = "A team with specified identifier was not found";

        private const int ANOTHER_TEAM_ID = SPECIFIC_TEAM_ID + 1;

        private TeamServiceTestFixture _testFixture = new TeamServiceTestFixture();

        private Mock<ITeamRepository> _teamRepositoryMock;
        private Mock<IPlayerRepository> _playerRepositoryMock;
        private Mock<IAuthorizationService> _authServiceMock;
        private Mock<IQuery<Team, FindByIdCriteria>> _getTeamByIdQueryMock;
        private Mock<IQuery<Player, FindByIdCriteria>> _getPlayerByIdQueryMock;
        private Mock<IQuery<Player, FindByFullNameCriteria>> _getPlayerByFullNameQueryMock;
        private Mock<IQuery<Team, FindByCaptainIdCriteria>> _getTeamByCaptainQueryMock;
        private Mock<IQuery<int, FindByPlayerCriteria>> _getTeamByPlayerMock;
        private Mock<IQuery<ICollection<Team>, GetAllCriteria>> _getAllTeamsQueryMock;
        private Mock<IQuery<ICollection<Player>, TeamPlayersCriteria>> _getTeamRosterQueryMock;
        private Mock<IQuery<Team, FindByNameCriteria>> _getTeamByNameQueryMock;
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
            _getPlayerByFullNameQueryMock = new Mock<IQuery<Player, FindByFullNameCriteria>>();
            _getTeamByCaptainQueryMock = new Mock<IQuery<Team, FindByCaptainIdCriteria>>();
            _getTeamByPlayerMock = new Mock<IQuery<int, FindByPlayerCriteria>>();
            _getAllTeamsQueryMock = new Mock<IQuery<ICollection<Team>, GetAllCriteria>>();
            _getTeamRosterQueryMock = new Mock<IQuery<ICollection<Player>, TeamPlayersCriteria>>();
            _getTeamByNameQueryMock = new Mock<IQuery<Team, FindByNameCriteria>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
        }

        #endregion

        #region Team tests

        #region Get

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
                                            .Build();

            // Act
            var sut = BuildSUT();
            var actual = sut.Get();

            // Assert
            TestHelper.AreEqual(expected, actual, new TeamComparer());
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

        #endregion

        #region GetTeamCaptain

        /// <summary>
        /// Test for GetTeamCaptain() method.
        /// The method should return existing captain
        /// </summary>
        [TestMethod]
        public void GetTeamCaptain_CaptainExist_PlayerReturned()
        {
            // Arrange
            var captain = new PlayerBuilder(SPECIFIC_PLAYER_ID).Build();
            var team = new TeamBuilder().WithCaptain(new PlayerId(SPECIFIC_PLAYER_ID)).Build();
            _getPlayerByIdQueryMock.Setup(pr =>
                                          pr.Execute(It.Is<FindByIdCriteria>(cr =>
                                                                             cr.Id == SPECIFIC_PLAYER_ID)))
                                    .Returns(captain);
            var expected = new PlayerBuilder(SPECIFIC_PLAYER_ID).Build();
            var sut = BuildSUT();

            // Act
            var actual = sut.GetTeamCaptain(team);

            // Assert
            TestHelper.AreEqual(expected, actual, new PlayerComparer());
        }

        #endregion

        #region Create

        /// <summary>
        /// Test for Create() method. The method should create a new team.
        /// </summary>
        [TestMethod]
        public void Create_CreateTeamDtoPassed_DtoAddedByRepository()
        {
            // Arrange
            var newTeamDto = new CreateTeamDtoBuilder().WithCaptain(new PlayerId(SPECIFIC_PLAYER_ID)).Build();
            _teamRepositoryMock.Setup(tr => tr.Add(It.IsAny<CreateTeamDto>()));

            var captain = new PlayerBuilder(SPECIFIC_PLAYER_ID).Build();
            MockGetPlayerBySpecificIdQuery(SPECIFIC_PLAYER_ID, captain);
            MockGetAllTeamsQuery(CreateSeveralTeams());

            var sut = BuildSUT();

            // Act
            sut.Create(newTeamDto);

            // Assert
            VerifyCreateTeam(newTeamDto, Times.Once());
        }

        /// <summary>
        /// Test for Create() method. The method check case when team achievements are invalid.
        /// Should throw ArgumentException
        /// </summary>
        [TestMethod]
        public void Create_InvalidAchievements_ArgumentExceptionThrown()
        {
            var invalidAchievements = CreateInvalidTeamAchievements();
            var argExMessage = Resources.ValidationTeamAchievements;
            var testTeam = new CreateTeamDtoBuilder()
                                        .WithAchievements(invalidAchievements)
                                        .Build();
            var testPlayer = new PlayerBuilder(PLAYER_ID).Build();

            MockGetPlayerBySpecificIdQuery(PLAYER_ID, testPlayer);
            MockGetAllTeamsQuery(CreateSeveralTeams());
            MockTeamRepositoryAddToThrow(testTeam, 
                new ArgumentException(argExMessage));

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
                new ArgumentException(argExMessage));
        }

        /// <summary>
        /// Test for Create() method. The method should create a new team with empty achievements.
        /// </summary>
        [TestMethod]
        public void Create_EmptyTeamAchievements_TeamCreated()
        {
            // Arrange
            MockGetPlayerByIdQuery(new PlayerBuilder().Build());
            var newTeam = new CreateTeamDtoBuilder().WithAchievements(string.Empty).Build();
            MockGetAllTeamsQuery(CreateSeveralTeams());
            var sut = BuildSUT();

            // Act
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
            var invalidName = CreateInvalidTeamName();
            var argExMessage = Resources.ValidationTeamName;
            var testTeam = new CreateTeamDtoBuilder()
                                        .WithName(invalidName)
                                        .Build();
            var testPlayer = new PlayerBuilder(PLAYER_ID).Build();

            MockGetPlayerBySpecificIdQuery(PLAYER_ID, testPlayer);
            MockGetAllTeamsQuery(CreateSeveralTeams());
            MockTeamRepositoryAddToThrow(testTeam,
                new ArgumentException(argExMessage));

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
                new ArgumentException(argExMessage));
        }

        /// <summary>
        /// Test for Create() method. The method check case when team name is invalid.
        /// Should throw ArgumentException
        /// </summary>
        [TestMethod]
        public void Create_EmptyTeamName_ArgumentExceptionThrown()
        {
            var invalidName = string.Empty;
            var argExMessage = TournamentResources.ValidationTeamName;
            var testTeam = new CreateTeamDtoBuilder()
                                        .WithName(invalidName)
                                        .Build();
            var testPlayer = new PlayerBuilder(PLAYER_ID).Build();
            _getPlayerByIdQueryMock.Setup(pr => 
                                pr.Execute(It.Is<FindByIdCriteria>(cr => cr.Id == testPlayer.Id))).Returns(testPlayer);
            MockGetAllTeamsQuery(CreateSeveralTeams());
            MockTeamRepositoryAddToThrow(testTeam, 
                new ArgumentException(argExMessage));
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
                new ArgumentException(argExMessage));
        }

        /// <summary>
        /// Test for Create() method. The method check case when team coach name is invalid.
        /// Should throw ArgumentException
        /// </summary>
        [TestMethod]
        public void Create_InvalidTeamCoachNameNotAllowedLength_ArgumentExceptionThrown()
        {
            var invalidCoachName = CreateInvalidTeamCoachName();
            var argExMessage = Resources.ValidationCoachName;
            var testTeam = new CreateTeamDtoBuilder()
                                        .WithCoach(invalidCoachName)
                                        .Build();
            var testPlayer = new PlayerBuilder(PLAYER_ID).Build();

            MockGetPlayerBySpecificIdQuery(PLAYER_ID, testPlayer);
            MockGetAllTeamsQuery(CreateSeveralTeams());
            MockTeamRepositoryAddToThrow(testTeam, 
                new ArgumentException(argExMessage));
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
                new ArgumentException(argExMessage));
        }

        /// <summary>
        /// Test for Create() method. The method check case when team coach name is invalid.
        /// Should throw ArgumentException
        /// </summary>
        [TestMethod]
        public void Create_InvalidTeamCoachNameNotAllowedSymbols_ArgumentExceptionThrown()
        {
            var invalidCoachName = "name%-)";
            var argExMessage = Resources.ValidationCoachName;
            var testTeam = new CreateTeamDtoBuilder()
                                        .WithCoach(invalidCoachName)
                                        .Build();
            var testPlayer = new PlayerBuilder(PLAYER_ID).Build();

            MockGetPlayerBySpecificIdQuery(PLAYER_ID, testPlayer);
            MockGetAllTeamsQuery(CreateSeveralTeams());
            MockTeamRepositoryAddToThrow(testTeam, 
                new ArgumentException(argExMessage));
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
                new ArgumentException(argExMessage));
        }

        /// <summary>
        /// Test for Create() method. The method should create a new team with empty coach name.
        /// </summary>
        [TestMethod]
        public void Create_EmptyTeamCoachName_TeamCreated()
        {
            // Arrange
            MockGetPlayerByIdQuery(new PlayerBuilder().Build());
            var newTeam = new CreateTeamDtoBuilder().WithCoach(string.Empty).Build();
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
            var newTeam = new CreateTeamDtoBuilder().Build();
            var testPlayer = new PlayerBuilder(SPECIFIC_PLAYER_ID).Build();
            _getPlayerByIdQueryMock.Setup(pr => pr.Execute(It.Is<FindByIdCriteria>(cr => cr.Id == testPlayer.Id))).Returns(testPlayer);

            // Act
            var sut = BuildSUT();
            var gotException = false;

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
            var newTeam = new CreateTeamDtoBuilder().Build();
            var captain = new PlayerBuilder().Build();

            var captainLeadTeam = new TeamBuilder().WithId(SPECIFIC_TEAM_ID).Build();
            var testTeams = new TeamServiceTestFixture().AddTeam(captainLeadTeam).Build();
            _getPlayerByIdQueryMock.Setup(pr =>
                            pr.Execute(It.Is<FindByIdCriteria>(cr =>
                                            cr.Id == captain.Id)))
                                            .Returns(captain);
            _getTeamByCaptainQueryMock.Setup(tm =>
                            tm.Execute(It.Is<FindByCaptainIdCriteria>(cr =>
                                                                    cr.CaptainId == captain.Id)))
                            .Returns(testTeams.Where(tm => tm.Captain.Id == captain.Id).FirstOrDefault());

            // Act
            var sut = BuildSUT();
            var gotException = false;

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
            var newTeamDto = new CreateTeamDtoBuilder().WithCaptain(new PlayerId(SPECIFIC_PLAYER_ID)).Build();
            var newTeam = new TeamBuilder().WithCaptain(new PlayerId(SPECIFIC_PLAYER_ID)).Build();
            var captain = new PlayerBuilder(SPECIFIC_PLAYER_ID).Build();

            MockGetPlayerByIdQuery(captain);
            MockGetTeamByCaptainId(captain.Id, null);
            MockGetAllTeamsQuery(CreateSeveralTeams());
            MockTeamRepositoryAddToReturn(newTeam);

            // Act
            var sut = BuildSUT();
            var createdTeam = sut.Create(newTeamDto);

            // Assert
            Assert.AreEqual(createdTeam.Captain.Id, captain.Id);
            VerifyCreateTeam(newTeamDto, Times.Once());
        }

        /// <summary>
        /// Test for Create() method. The method check that captain's teamId
        /// was updated after creating team in DB
        /// </summary>
        [TestMethod]
        public void Create_CreateDtoTeamPassed_CaptainUpdated()
        {
            // Arrange
            var newTeamDto = new CreateTeamDtoBuilder().WithCaptain(new PlayerId(SPECIFIC_PLAYER_ID)).Build();
            var team = new TeamBuilder().WithCaptain(new PlayerId(SPECIFIC_PLAYER_ID)).Build();
            var captain = new PlayerBuilder(SPECIFIC_PLAYER_ID).Build();

            MockGetPlayerBySpecificIdQuery(SPECIFIC_PLAYER_ID, captain);
            MockTeamRepositoryAddToReturn(team);

            var sut = BuildSUT();

            // Act
            var newTeam = sut.Create(newTeamDto);

            // Assert
            Assert.AreEqual(newTeam.Captain.Id, captain.Id);
            VerifyCreateTeam(newTeamDto, Times.Once());
        }

        /// <summary>
        /// Test for Create() method. The method check if team already exist
        /// Throw exeption
        /// </summary>
        [TestMethod]
        public void Create_TeamWithGivenNameAlreadyExists_ValidationExceptionThrown()
        {
            // Arrange
            MockGetPlayerByIdQuery(new PlayerBuilder().Build());
            var newTeam = new CreateTeamDtoBuilder().WithName(TEAM_NAME_TO_VALIDATE).Build();
            var teamWithSameName = new TeamBuilder().WithName(TEAM_NAME_TO_VALIDATE).Build();

            MockGetTeamByNameQuery(teamWithSameName);

            var sut = BuildSUT();
            Exception exception = null;
            var argExMessage =
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

        #endregion

        #region Delete

        /// <summary>
        /// Test for Delete() method. The method should delete specified team.
        /// </summary>
        [TestMethod]
        public void Delete_TeamPassed_CorrectIdPostedToDatabaseLayer()
        {
            // Arrange
            var testData = new PlayerServiceTestFixture()
                .Build();
            MockGetTeamRosterQuery(testData);

            // Act
            var ts = BuildSUT();
            ts.Delete(new TeamId(SPECIFIC_TEAM_ID));

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
            _teamRepositoryMock.Setup(tr => tr.Remove(It.IsAny<TeamId>()))
                .Throws(new InvalidKeyValueException());

            // Act
            var ts = BuildSUT();
            var gotException = false;

            try
            {
                ts.Delete(new TeamId(SPECIFIC_TEAM_ID));
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
            MockGetTeamRosterQuery(testData);

            // Act
            var ts = BuildSUT();
            ts.Delete(new TeamId(SPECIFIC_TEAM_ID));

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
            var expectedRoster = new PlayerServiceTestFixture().TestPlayers().Build().AsQueryable<Player>();
            var expectedCountOfPlayers = expectedRoster.Count();
            MockGetTeamRosterQuery(expectedRoster.ToList());

            // Act
            var ts = BuildSUT();
            ts.Delete(new TeamId(SPECIFIC_TEAM_ID));

            // Assert
            _playerRepositoryMock.Verify(
                                         pr => pr.UpdateTeam(It.Is<Player>(player => expectedRoster.Contains(player)), null),
                                         Times.Exactly(expectedCountOfPlayers));
        }

        #endregion

        #region Edit

        /// <summary>
        /// Edit() method test. catch ConcurrencyException from DAL
        /// Throws MissingEntityException
        /// </summary>
        public void Edit_CatchDalConcurrencyException_ThrowMissingEntityException()
        {
            // Arrange
            var exceptionMessage = TEAM_NOT_FOUND;
            var teamWithWrongId = new TeamBuilder().WithCaptain(new PlayerId(SPECIFIC_PLAYER_ID)).Build();

            MockGetPlayerByIdQuery(new PlayerBuilder(SPECIFIC_PLAYER_ID).Build());
            MockGetTeamByIdQuery(new TeamBuilder().Build());
            MockTeamRepositoryEditToThrow(new ConcurrencyException());

            var exception = null as Exception;
            var sut = BuildSUT();

            // Act
            try
            {
                sut.Edit(teamWithWrongId);
            }
            catch(Exception ex)
            {
                exception = ex;
            }

            // Arrange
            VerifyExceptionThrown(exception,
                new MissingEntityException(exceptionMessage));
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
            var captain = new PlayerBuilder().Build();

            var captainLeadTeam = new TeamBuilder().WithId(SPECIFIC_TEAM_ID).Build();
            var testTeams = new TeamServiceTestFixture().AddTeam(captainLeadTeam).Build();
            _getPlayerByIdQueryMock.Setup(pr =>
                            pr.Execute(It.Is<FindByIdCriteria>(cr =>
                                            cr.Id == captain.Id)))
                                            .Returns(captain);
            _getTeamByCaptainQueryMock.Setup(tm =>
                            tm.Execute(It.Is<FindByCaptainIdCriteria>(cr =>
                                                                    cr.CaptainId == captain.Id)))
                            .Returns(testTeams.Where(tm => tm.Captain.Id == captain.Id).FirstOrDefault());

            MockGetAllTeamsQuery(CreateSeveralTeams());
            MockGetTeamByPlayerQuery(SPECIFIC_PLAYER_ID);

            // Act
            var sut = BuildSUT();
            var gotException = false;

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
            MockGetPlayerByIdQuery(new PlayerBuilder(SPECIFIC_PLAYER_ID).Build());
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
        public void Edit_TeamNameAlreadyExists_ValidationExceptionThrown()
        {
            // Arrange
            var teamToEdit = new TeamBuilder().WithName(TEAM_NAME_TO_VALIDATE).WithId(SPECIFIC_TEAM_ID).Build();
            var teamWithTheSameName = new TeamBuilder().Build();
            var argExMessage =
                   TournamentResources.TeamNameInTournamentNotUnique;

            MockGetTeamByNameQuery(teamWithTheSameName);

            var sut = BuildSUT();
            Exception exception = null;

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
            MockGetPlayerByIdQuery(new PlayerBuilder(SPECIFIC_PLAYER_ID).Build());
            var teamToEdit = new TeamBuilder().WithName(TEAM_NAME_TO_VALIDATE).WithId(SPECIFIC_TEAM_ID).Build();
            MockGetAllTeamsQuery(CreateSeveralTeams());
            MockGetTeamByIdQuery(teamToEdit);

            // Act
            var sut = BuildSUT();
            sut.Edit(teamToEdit);

            // Assert
            VerifyEditTeam(teamToEdit, Times.Once());
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
            var testData = new CreateTeamDtoBuilder().Build();
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
            sut.Delete(new TeamId(testData.Id));

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

        #endregion

        #region Private

        private TeamService BuildSUT()
        {
            return new TeamService(
                _teamRepositoryMock.Object,
                _playerRepositoryMock.Object,
                _getTeamByIdQueryMock.Object,
                _getPlayerByIdQueryMock.Object,
                _getPlayerByFullNameQueryMock.Object,
                _getTeamByCaptainQueryMock.Object,
                _getTeamByPlayerMock.Object,
                _getAllTeamsQueryMock.Object,
                _getTeamRosterQueryMock.Object,
                _getTeamByNameQueryMock.Object,
                _authServiceMock.Object);
        }

        private bool TeamsAreEqual(Team x, Team y)
        {
            return new TeamComparer().Compare(x, y) == 0;
        }

        private bool CreateTeamDtosAreEqual(CreateTeamDto x, CreateTeamDto y)
        {
            return new CreateTeamDtoComparer().Compare(x, y) == 0;
        }

        private void MockGetAllTeamsQuery(IEnumerable<Team> testData)
        {
            _getAllTeamsQueryMock.Setup(tr => tr.Execute(It.IsAny<GetAllCriteria>())).Returns(testData.ToList());
        }

        private void MockGetTeamByCaptainId(int captainId, Team result) =>
            _getTeamByCaptainQueryMock
                .Setup(tr => tr.Execute(It.Is<FindByCaptainIdCriteria>(c => c.CaptainId == captainId)))
                .Returns(result);

        private void MockGetTeamByIdQuery(Team testData)
        {
            _getTeamByIdQueryMock.Setup(tr => tr.Execute(It.IsAny<FindByIdCriteria>())).Returns(testData);
        }

        private void MockGetTeamByPlayerQuery(int teamId)
        {
            _getTeamByPlayerMock.SetupSequence(t => t.Execute(It.IsAny<FindByPlayerCriteria>()))
                .Returns(teamId)
                .Returns(0)
                .Returns(0);
        }

        private void MockGetPlayerBySpecificIdQuery(int id, Player player) =>
            _getPlayerByIdQueryMock
                .Setup(pq => pq.Execute(It.Is<FindByIdCriteria>(c => c.Id == id)))
                .Returns(player);

        private void MockGetPlayerByIdQuery(Player player)
        {
            _getPlayerByIdQueryMock.Setup(tr => tr.Execute(It.IsAny<FindByIdCriteria>())).Returns(player);
        }

        private void MockGetPlayerByFullNameQuery(Player player)
        {
            _getPlayerByFullNameQueryMock.Setup(tr => tr.Execute(It.IsAny<FindByFullNameCriteria>())).Returns(player);
        }

        private void MockGetTeamRosterQuery(List<Player> players)
        {
            _getTeamRosterQueryMock.Setup(tr => tr.Execute(It.IsAny<TeamPlayersCriteria>())).Returns(players);
        }

        private void MockTeamRepositoryAddToThrow(CreateTeamDto testTeam, Exception exception) =>
            _teamRepositoryMock.Setup(tr => tr.Add(It.Is<CreateTeamDto>(dto => CreateTeamDtosAreEqual(dto, testTeam))))
                .Throws(exception);

        private void MockTeamRepositoryAddToReturn(Team team) =>
            _teamRepositoryMock.Setup(tr => tr.Add(It.IsAny<CreateTeamDto>()))
                .Returns(team);

        private void MockTeamRepositoryEditToThrow(Exception exception) =>
            _teamRepositoryMock.Setup(tr => tr.Update(It.IsAny<Team>()))
                .Throws(exception);

        private void MockGetTeamByNameQuery(Team team) =>
            _getTeamByNameQueryMock.Setup(tq => tq.Execute(It.IsAny<FindByNameCriteria>()))
                .Returns(team);

        private void VerifyCreateTeam(CreateTeamDto team, Times times)
        {
            _teamRepositoryMock.Verify(tr => tr.Add(It.Is<CreateTeamDto>(t => CreateTeamDtosAreEqual(t, team))), times);
        }

        private void VerifyEditTeam(Team team, Times times)
        {
            _teamRepositoryMock.Verify(tr => tr.Update(It.Is<Team>(t => TeamsAreEqual(t, team))), times);
        }

        private void VerifyEditPlayer(int playerId, int teamId, Times times)
        {
            _playerRepositoryMock.Verify(pr => pr.Update(It.Is<Player>(p => p.Id == playerId)), times);
        }

        private void VerifyDeleteTeam(int teamId, Times times)
        {
            _teamRepositoryMock.Verify(tr => tr.Remove(It.Is<TeamId>(id => id.Id == teamId)), times);
        }

        private void VerifyDeleteTeam(int teamId, Times repositoryTimes, Times unitOfWorkTimes)
        {
            _teamRepositoryMock.Verify(tr => tr.Remove(It.Is<TeamId>(id => id.Id == teamId)), repositoryTimes);
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
            var invalidAchievements = new StringBuilder();
            for (var i = 0; i < VolleyManagement.Domain.Constants.Team.MAX_ACHIEVEMENTS_LENGTH + 1; i++)
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
            var invalidTeamName = new StringBuilder();
            for (var i = 0; i < VolleyManagement.Domain.Constants.Team.MAX_NAME_LENGTH + 1; i++)
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
            var invalidTeamCoachName = new StringBuilder();
            for (var i = 0; i < VolleyManagement.Domain.Constants.Team.MAX_COACH_NAME_LENGTH + 1; i++)
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
