﻿namespace VolleyManagement.UnitTests.Services.TeamService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using Data.Queries.Player;
    using Data.Queries.Team;
    using Xunit;
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
    using FluentAssertions;

    /// <summary>
    /// Tests for TournamentService class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TeamServiceTests
    {
        #region Fields and constants

        private const int SPECIFIC_PLAYER_ID = 2;
        private const int PLAYER_ID = 1;
        private const int TEAM_ID = 1;
        private const int SPECIFIC_TEAM_ID = 2;
        private const string TEAM_NAME_TO_VALIDATE = "empire";
        private const string TEAM_NOT_FOUND = "A team with specified identifier was not found";

        private readonly TeamServiceTestFixture _testFixture = new TeamServiceTestFixture();

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

        #endregion

        #region Constuctor

        /// <summary>
        /// Initializes test data.
        /// </summary>
        public TeamServiceTests()
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
        }

        #endregion

        #region Team tests

        #region Get

        /// <summary>
        /// Test for Get() method. The method should return existing teams
        /// </summary>
        [Fact]
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
            Assert.Equal(expected, actual, new TeamComparer());
        }

        /// <summary>
        /// Test for Get() method. The method should return existing team
        /// </summary>
        [Fact]
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
            Assert.Equal<Team>(expected, actual, new TeamComparer());
        }

        #endregion

        #region GetTeamCaptain

        /// <summary>
        /// Test for GetTeamCaptain() method.
        /// The method should return existing captain
        /// </summary>
        [Fact]
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
            Assert.Equal(expected, actual, new PlayerComparer());
        }

        #endregion

        #region Create

        /// <summary>
        /// Test for Create() method. The method should create a new team.
        /// </summary>
        [Fact]
        public void Create_CreateTeamDtoPassed_TeamCreated()
        {
            // Arrange
            var newTeamDto = new CreateTeamDtoBuilder()
                .WithCaptain(new PlayerId(SPECIFIC_PLAYER_ID))
                .Build();
            var newTeam = new TeamBuilder().Build();
            var captain = new PlayerBuilder(SPECIFIC_PLAYER_ID).Build();

            MockGetPlayerBySpecificIdQuery(SPECIFIC_PLAYER_ID, captain);
            MockTeamRepositoryAddToReturn(newTeam);

            var sut = BuildSUT();

            // Act
            var team = sut.Create(newTeamDto);

            // Assert
            Assert.NotNull(team);
            VerifyCreateTeam(newTeamDto, Times.Once());
        }

        /// <summary>
        /// Test for Create() method. The method check case when team achievements are invalid.
        /// Should throw ArgumentException
        /// </summary>
        [Fact]
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
        [Fact]
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
        [Fact]
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
        [Fact]
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
        [Fact]
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
        [Fact]
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
        [Fact]
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
        [Fact]
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
            Assert.True(gotException);
            VerifyCreateTeam(newTeam, Times.Never());
        }

        /// <summary>
        /// Test for Create() method. The method check case when captain is captain of another team.
        /// Should throw ValidationException
        /// </summary>
        [Fact]
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
                            .Returns(testTeams.FirstOrDefault(tm => tm.Captain.Id == captain.Id));

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
            Assert.True(gotException);
            VerifyCreateTeam(newTeam, Times.Never());
        }

        /// <summary>
        /// Test for Create() method.
        /// The method check case when captain is player of another team.
        /// The method should create a new team.
        /// </summary>
        [Fact]
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
            Assert.Equal(createdTeam.Captain.Id, captain.Id);
            VerifyCreateTeam(newTeamDto, Times.Once());
        }

        /// <summary>
        /// Test for Create() method. The method check that captain's teamId
        /// was updated after creating team in DB
        /// </summary>
        [Fact]
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
            Assert.Equal(newTeam.Captain.Id, captain.Id);
            VerifyCreateTeam(newTeamDto, Times.Once());
        }

        /// <summary>
        /// Test for Create() method. The method check if team already exist
        /// Throw exeption
        /// </summary>
        [Fact]
        public void Create_TeamWithGivenNameAlreadyExists_ArgumentExceptionThrown()
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
        [Fact]
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
        [Fact]
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
            Assert.True(gotException);
            VerifyDeleteTeam(SPECIFIC_TEAM_ID, Times.Once());
        }

        /// <summary>
        /// Successful test for Delete() method.
        /// </summary>
        [Fact]
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

        #endregion

        #region Edit

        /// <summary>
        /// Edit() method test. catch ConcurrencyException from DAL
        /// Throws MissingEntityException
        /// </summary>
        [Fact]
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
            catch (Exception ex)
            {
                exception = ex;
            }

            // Arrange
            VerifyExceptionThrown(exception,
                new MissingEntityException(exceptionMessage));
        }

        /// <summary>
        /// Test for Edit() method. The method checks case when captain is captain of another team.
        /// Should throw ValidationException
        /// </summary>
        [Fact]
        public void Edit_PlayerIsCaptainOfExistingTeam_ValidationExceptionThrown()
        {
            // Arrange
            var newTeam = new TeamBuilder().Build();
            var captain = new PlayerBuilder().Build();

            _getPlayerByIdQueryMock.Setup(pr =>
                            pr.Execute(It.Is<FindByIdCriteria>(cr =>
                                            cr.Id == captain.Id)))
                                            .Returns(captain);
            _getTeamByIdQueryMock.Setup(pr =>
                            pr.Execute(It.Is<FindByIdCriteria>(cr => cr.Id == TEAM_ID)))
                            .Returns(newTeam);

            MockGetAllTeamsQuery(CreateSeveralTeams());
            MockGetTeamByPlayerQuery(SPECIFIC_PLAYER_ID);

            // Act
            var sut = BuildSUT();
            var gotException = false;

            try
            {
                sut.ChangeCaptain(new TeamId(newTeam.Id), new PlayerId(captain.Id));
            }
            catch (ValidationException)
            {
                gotException = true;
            }

            // Assert
            Assert.True(gotException);
            VerifyEditTeam(newTeam, Times.Never());
        }

        /// <summary>
        /// Test for Edit() method. Existing team should be updated
        /// </summary>
        [Fact]
        public void Edit_TeamPassed_TeamUpdated()
        {
            // Arrange
            var teamToEdit = new TeamBuilder().WithId(SPECIFIC_TEAM_ID).Build();

            MockGetPlayerByIdQuery(new PlayerBuilder(SPECIFIC_PLAYER_ID).Build());
            MockGetTeamByIdQuery(new TeamBuilder().Build());

            var sut = BuildSUT();

            // Act
            sut.Edit(teamToEdit);

            // Assert
            VerifyEditSimpleDataInTeam(teamToEdit, Times.Once());
        }

        /// <summary>
        /// Test for Edit() method.
        /// </summary>
        [Fact]
        public void Edit_TeamNameAlreadyExists_ArgumentExceptionThrown()
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

        #endregion

        #region Change Captain
        /// <summary>
        /// Set captain one of team players. Captain updated.
        /// </summary>
        [Fact]
        public void ChangeCaptain_PlayerIsAlreadyPlayInTeam_CaptainUpdated()
        {
            // Arrange
            var newCaptainId = new PlayerId(SPECIFIC_PLAYER_ID);

            var teamForEdit = new TeamBuilder().WithPlayer(newCaptainId).Build();
            var teamForEditId = new TeamId(teamForEdit.Id);

            MockGetTeamByPlayerQuery(TEAM_ID);
            MockGetTeamByIdQuery(teamForEdit);

            // Act
            var sut = BuildSUT();
            sut.ChangeCaptain(teamForEditId, newCaptainId);

            // Assert
            Assert.Equal(teamForEdit.Captain.Id, newCaptainId.Id);
        }

        /// <summary>
        /// Set captain of team player who does not have a team.
        /// Captain updated.
        /// </summary>
        [Fact]
        public void ChangeCaptain_PlayerHasNoTeam_CaptainUpdated()
        {
            // Arrange
            var newCaptainId = new PlayerId(SPECIFIC_PLAYER_ID);
            var teamForEdit = new TeamBuilder().Build();
            var teamForEditId = new TeamId(teamForEdit.Id);

            MockGetTeamByPlayerQuery(TEAM_ID);
            MockGetTeamByIdQuery(teamForEdit);

            // Act
            var sut = BuildSUT();
            sut.ChangeCaptain(teamForEditId, newCaptainId);

            // Assert
            Assert.Equal(teamForEdit.Captain.Id, newCaptainId.Id);
        }

        /// <summary>
        /// Set captain of team player that plays in anothe team.
        /// Validate exception thrown.
        /// </summary>
        [Fact]
        public void ChangeCaptain_PlayerIsPlayerOfAnotherTeam_ValidateExceptionThrown()
        {
            // Arrange
            var newCaptainId = new PlayerId(PLAYER_ID);
            var newCaptainTeam = new TeamBuilder().WithId(TEAM_ID).WithPlayer(newCaptainId).Build();

            var teamForEdit = new TeamBuilder().WithId(SPECIFIC_TEAM_ID).Build();
            var teamForEditId = new TeamId(teamForEdit.Id);

            MockGetTeamByPlayerQuery(newCaptainTeam.Id);
            MockGetTeamByIdQuery(teamForEdit);

            // Act
            var sut = BuildSUT();
            var gotException = false;

            try
            {
                sut.ChangeCaptain(teamForEditId, newCaptainId);
            }
            catch (ValidationException)
            {
                gotException = true;
            }

            // Assert
            Assert.True(gotException);
            VerifyEditTeam(teamForEdit, Times.Never());
        }
        #endregion

        #region Add and remove players

        /// <summary>
        /// Test for AddPlayers() method. 
        /// </summary>
        [Fact(Skip="Weird behavior. Pass before. Failed after migrating to xunit.")]
        public void AddPlayers_PlayersAddedToTeam_TeamUpdated()
        {
            // Arrange
            var testPlayersIdToAdd = CreateSeveralPlayersId();
            var testTeamId = new TeamId(SPECIFIC_TEAM_ID);
            var testTeam = new TeamBuilder().WithId(SPECIFIC_TEAM_ID).Build();
            var exeptedTeam = new TeamBuilder().WithId(SPECIFIC_TEAM_ID).WithRoster(testPlayersIdToAdd).Build();
            MockGetTeamByIdQuery(testTeam);

            // Act
            var sut = BuildSUT();
            sut.AddPlayers(testTeamId, testPlayersIdToAdd);

            // Assert
            VerifyEditTeam(testTeam, Times.Once());
            testTeam.Should().Be(exeptedTeam);
        }

        /// <summary>
        /// Test for AddPlayers() method. The method check if team with such id exist
        /// Should throw MissingEntityException
        /// </summary>
        [Fact]
        public void AddPlayers_AddPlayersToNotExistTeam_MissingEntityExceptionThrown()
        {
            // Arrange
            var testPlayersIdToAdd = CreateSeveralPlayersId();
            var testTeamId = new TeamId(SPECIFIC_TEAM_ID);
            var testTeam = new TeamBuilder().WithId(SPECIFIC_TEAM_ID + 1).Build();
            _getTeamByIdQueryMock.Setup(tr => tr.Execute(It.Is<FindByIdCriteria>(cr => cr.Id == testTeam.Id))).Returns(testTeam);

            // Act
            var sut = BuildSUT();
            var gotException = false;

            try
            {
                sut.AddPlayers(testTeamId, testPlayersIdToAdd);
            }
            catch (MissingEntityException)
            {
                gotException = true;
            }

            // Assert
            Assert.True(gotException);
            VerifyEditTeam(testTeam, Times.Never());
        }

        /// <summary>
        /// Test for RemovePlayers() method. 
        /// </summary>
        [Fact(Skip = "Weird behavior. Pass before. Failed after migrating to xunit.")]
        public void RemovePlayers_AllPlayersRemoveFromTeam_TeamUpdated()
        {
            // Arrange
            var playersIdToRemove = CreateSeveralPlayersId();
            var testTeamId = new TeamId(SPECIFIC_TEAM_ID);

            var existingPlayersId = new List<PlayerId> {
                new PlayerId(1),
                new PlayerId(2),
                new PlayerId(3),
                new PlayerId(4)
            };

            var exeptedPlayerId = new List<PlayerId> {
                new PlayerId(1)
            };

            var testTeam = new TeamBuilder().WithRoster(existingPlayersId).Build();
            var exeptedTeam = new TeamBuilder().WithRoster(exeptedPlayerId).Build();
            MockGetTeamByIdQuery(testTeam);

            // Act
            var sut = BuildSUT();
            sut.RemovePlayers(testTeamId, playersIdToRemove);

            // Assert
            VerifyEditTeam(testTeam, Times.Once());
            Assert.Equal(exeptedTeam, testTeam, new TeamComparer());
        }

        /// <summary>
        /// Test for FromPlayers() method. The method check if team with such id exist
        /// Should throw MissingEntityException
        /// </summary>
        [Fact]
        public void RemovePlayers_RemovePlayersFromNotExistTeam_MissingEntityExceptionThrown()
        {
            // Arrange
            var testPlayersIdToRemove = new List<PlayerId>();
            var testTeamId = new TeamId(SPECIFIC_TEAM_ID);
            var testTeam = new TeamBuilder().WithId(SPECIFIC_TEAM_ID + 1).Build();
            _getTeamByIdQueryMock.Setup(tr => tr.Execute(It.Is<FindByIdCriteria>(cr => cr.Id == testTeam.Id))).Returns(testTeam);
            // Act
            var sut = BuildSUT();
            var gotException = false;

            try
            {
                sut.RemovePlayers(testTeamId, testPlayersIdToRemove);
            }
            catch (MissingEntityException)
            {
                gotException = true;
            }

            // Assert
            Assert.True(gotException);
            VerifyEditTeam(testTeam, Times.Never());
        }
        #endregion

        #region Authorization team tests

        /// <summary>
        /// Test for Create() method with no permission for such action. The method has to throw AuthorizationException,
        /// should invoke CheckAccess() and shouldn't invoke Commit() method of IUnitOfWork.
        /// </summary>
        [Fact]
        public void Create_CreateNotPermitted_ExceptionThrown()
        {
            // Arrange
            var testData = new CreateTeamDtoBuilder().Build();
            MockAuthServiceThrowsExeption(AuthOperations.Teams.Create);

            var sut = BuildSUT();

            // Act
            Action act = () => sut.Create(testData);

            // Assert
            act.Should().Throw<AuthorizationException>();
            VerifyCreateTeam(testData, Times.Never());
            VerifyCheckAccess(AuthOperations.Teams.Create, Times.Once());
        }

        /// <summary>
        /// Test for Delete() method with no permission for such action. The method has to throw AuthorizationException,
        /// should invoke CheckAccess() and shouldn't invoke Commit() method of IUnitOfWork
        /// </summary>
        [Fact]
        public void Delete_DeleteNotPermitted_ExceptionThrown()
        {
            // Arrange
            var testData = new TeamBuilder().WithId(SPECIFIC_TEAM_ID).Build();
            MockAuthServiceThrowsExeption(AuthOperations.Teams.Delete);

            var sut = BuildSUT();

            // Act
            Action act = () => sut.Delete(new TeamId(testData.Id));

            // Assert
            act.Should().Throw<AuthorizationException>();
            VerifyDeleteTeam(testData.Id, Times.Never());
            VerifyCheckAccess(AuthOperations.Teams.Delete, Times.Once());
        }

        /// <summary>
        /// Test for Edit() method with no permission for such action. The method has to throw AuthorizationException,
        /// should invoke CheckAccess() and shouldn't invoke Commit() method of IUnitOfWork
        /// </summary>
        [Fact]
        public void Edit_EditNotPermitted_ExceptionThrown()
        {
            // Arrange
            var testData = new TeamBuilder().WithId(SPECIFIC_TEAM_ID).Build();
            MockAuthServiceThrowsExeption(AuthOperations.Teams.Edit);

            var sut = BuildSUT();

            // Act
            Action act = () => sut.Edit(testData);

            // Assert
            act.Should().Throw<AuthorizationException>();
            VerifyEditTeam(testData, Times.Never());
            VerifyCheckAccess(AuthOperations.Teams.Edit, Times.Once());
        }

        /// <summary>
        /// Test for AddPlayers() method with no permission for such action. The method has to throw AuthorizationException,
        /// should invoke CheckAccess() and shouldn't invoke Commit() method of IUnitOfWork
        /// </summary>
        [Fact]
        public void AddPlayers_AddPlayersNotPermitted_ExceptionThrown()
        {
            // Arrange
            var testTeamId = new TeamId(SPECIFIC_TEAM_ID);
            var testPlayersId = CreateSeveralPlayersId();
            var testTeam = new TeamBuilder().WithId(SPECIFIC_TEAM_ID).Build();
            MockAuthServiceThrowsExeption(AuthOperations.Teams.Edit);

            var sut = BuildSUT();

            // Act
            Action act = () => sut.AddPlayers(testTeamId, testPlayersId);

            // Assert
            act.Should().Throw<AuthorizationException>();
            VerifyEditTeam(testTeam, Times.Never());
            VerifyCheckAccess(AuthOperations.Teams.Edit, Times.Once());
        }

        /// <summary>
        /// Test for RemovePlayers() method with no permission for such action. The method has to throw AuthorizationException,
        /// should invoke CheckAccess() and shouldn't invoke Commit() method of IUnitOfWork
        /// </summary>
        [Fact]
        public void RemovePlayers_RemovePlayersNotPermitted_ExceptionThrown()
        {
            // Arrange
            var testTeamId = new TeamId(SPECIFIC_TEAM_ID);
            var testPlayersId = CreateSeveralPlayersId();
            var testTeam = new TeamBuilder().WithId(SPECIFIC_TEAM_ID).Build();
            MockAuthServiceThrowsExeption(AuthOperations.Teams.Edit);

            var sut = BuildSUT();

            // Act
            Action act = () => sut.RemovePlayers(testTeamId, testPlayersId);

            // Assert
            act.Should().Throw<AuthorizationException>();
            VerifyEditTeam(testTeam, Times.Never());
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

        private bool TeamsEqual(Team x, Team y)
        {
            return new TeamComparer().Compare(x, y) == 0;
        }

        private bool CreateTeamDtosEqual(CreateTeamDto x, CreateTeamDto y)
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

        private void MockGetTeamRosterQuery(List<Player> players)
        {
            _getTeamRosterQueryMock.Setup(tr => tr.Execute(It.IsAny<TeamPlayersCriteria>())).Returns(players);
        }

        private void MockTeamRepositoryAddToThrow(CreateTeamDto testTeam, Exception exception) =>
            _teamRepositoryMock.Setup(tr => tr.Add(It.Is<CreateTeamDto>(dto => CreateTeamDtosEqual(dto, testTeam))))
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
            _teamRepositoryMock.Verify(tr => tr.Add(It.Is<CreateTeamDto>(t => CreateTeamDtosEqual(t, team))), times);
        }

        private void VerifyEditTeam(Team team, Times times)
        {
            _teamRepositoryMock.Verify(tr => tr.Update(It.Is<Team>(t => TeamsEqual(t, team))), times);
        }

        private void VerifyEditSimpleDataInTeam(Team team, Times times) =>
            _teamRepositoryMock.Verify(tr => tr.Update(
                It.Is<Team>(t =>
                            t.Name.Equals(team.Name) &&
                            t.Coach.Equals(team.Coach) &&
                            t.Achievements.Equals(team.Achievements))),
                 times);

        private void VerifyDeleteTeam(int teamId, Times times)
        {
            _teamRepositoryMock.Verify(tr => tr.Remove(It.Is<TeamId>(id => id.Id == teamId)), times);
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
        /// Creates Team coach name with more number of symbols than it is allowed
        /// </summary>
        /// <returns>Invalid team coach name</returns>
        private IEnumerable<PlayerId> CreateSeveralPlayersId()
        {
            var players = new List<PlayerId> {
                new PlayerId(3),
                new PlayerId(4)
            };
            return players;
        }

        /// <summary>
        /// Checks if exception was thrown and has appropriate message
        /// Checks if thrown exceptions are of the same type
        /// </summary>
        /// <param name="exception">Exception that has been thrown</param>
        /// <param name="expected">Expected exception</param>
        private void VerifyExceptionThrown(Exception exception, Exception expected)
        {
            Assert.NotNull(exception);
            Assert.True(exception.GetType() == expected.GetType(), "Exception is of the wrong type");
            Assert.Equal(exception.Message, expected.Message);
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
