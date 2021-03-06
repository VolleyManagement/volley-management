﻿namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Contracts;
    using Xunit;
    using Moq;
    using Contracts.Authorization;
    using Contracts.Exceptions;
    using Domain.PlayersAggregate;
    using Domain.TeamsAggregate;
    using FluentAssertions;
    using UI.Areas.Mvc.Controllers;
    using UI.Areas.Mvc.ViewModels.Players;
    using UI.Areas.Mvc.ViewModels.Teams;
    using ViewModels;
    using Services.PlayerService;
    using Services.TeamService;

    /// <summary>
    /// Tests for MVC TeamsController class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TeamsControllerTests
    {
        private const int TEAM_UNEXISTING_ID_TO_DELETE = 4;
        private const int SPECIFIED_TEAM_ID = 4;
        private const int TEAM_ID = 1;
        private const int SPECIFIED_PLAYER_ID = 4;
        private const string SPECIFIED_FIRST_PLAYER_NAME = "Test";
        private const string SPECIFIED_LAST_PLAYER_NAME = "Name";
        private const string SPECIFIED_EXCEPTION_MESSAGE = "Test exception message";
        private const string ACHIEVEMENTS = "TestAchievements";
        private const string TEAM_NAME = "TestName";
        private const string PLAYER_FIRSTNAME = "Test";
        private const string PLAYER_LASTNAME = "Test";
        private const string COACH = "TestCoach";
        private const int TEST_TEAM_ID = 1;
        private const string ASSERT_FAIL_JSON_RESULT_MESSAGE = "Json result must be returned to user.";

        private Mock<ITeamService> _teamServiceMock;
        private Mock<HttpContextBase> _httpContextMock;
        private Mock<HttpRequestBase> _httpRequestMock;
        private Mock<HttpPostedFileBase> _httpPostedFileBaseMock;
        private Mock<IAuthorizationService> _authServiceMock;
        private Mock<IFileService> _fileServiceMock;
        private Mock<IPlayerService> _playerServiceMock;

        /// <summary>
        /// Initializes test data
        /// </summary>
        public TeamsControllerTests()
        {
            _teamServiceMock = new Mock<ITeamService>();
            _httpContextMock = new Mock<HttpContextBase>();
            _httpRequestMock = new Mock<HttpRequestBase>();
            _httpPostedFileBaseMock = new Mock<HttpPostedFileBase>();
            _authServiceMock = new Mock<IAuthorizationService>();
            _fileServiceMock = new Mock<IFileService>();
            _playerServiceMock = new Mock<IPlayerService>();

            _httpContextMock.SetupGet(c => c.Request).Returns(_httpRequestMock.Object);
        }

        /// <summary>
        /// Delete method test. The method should invoke Delete() method of ITeamService
        /// and return result as JavaScript Object Notation.
        /// </summary>
        [Fact]
        public void Delete_PlayerExists_PlayerIsDeleted()
        {
            // Act
            var sut = BuildSUT();
            var actual = sut.Delete(TEAM_UNEXISTING_ID_TO_DELETE);

            // Assert
            _teamServiceMock.Verify(ps => ps.Delete(It.Is<TeamId>(id => id.Id == TEAM_UNEXISTING_ID_TO_DELETE)), Times.Once());
            Assert.NotNull(actual);
        }

        /// <summary>
        /// Delete method test. Input parameter is team id, which doesn't exist in database.
        /// The method should return message as JavaScript Object Notation.
        /// </summary>
        [Fact]
        public void Delete_PlayerDoesntExist_JsonReturned()
        {
            // Arrange
            _teamServiceMock.Setup(ps => ps.Delete(new TeamId(TEAM_UNEXISTING_ID_TO_DELETE))).Throws<MissingEntityException>();

            // Act
            var sut = BuildSUT();
            var actual = sut.Delete(TEAM_UNEXISTING_ID_TO_DELETE);

            // Assert
            Assert.NotNull(actual);
        }

        /// <summary>
        /// Create method test. Positive test
        /// </summary>
        [Fact]
        public void Create_CreateTeamDtoPassed_EntityIdIsSet()
        {
            // Arrange
            var viewModel = new TeamMvcViewModelBuilder().WithId(0).Build();
            var returnedviewModel = new TeamMvcViewModelBuilder().WithId(SPECIFIED_TEAM_ID).Build();
            var returnedDomain = returnedviewModel.ToDomain();
            _teamServiceMock.Setup(ts =>
                ts.Create(It.IsAny<CreateTeamDto>()))
                .Returns(returnedDomain);

            var players = MakeTestPlayers();

            _playerServiceMock.Setup(ps => ps.CreateBulk(It.IsAny<ICollection<CreatePlayerDto>>()))
                .Returns(players);

            // Act
            var sut = BuildSUT();
            var result = sut.Create(viewModel);

            // Assert
            Assert.NotNull(result.Data);
            Assert.Equal(viewModel.Id, SPECIFIED_TEAM_ID);
        }

        /// <summary>
        /// Create method test. Team is not valid. Argument exception is thrown.
        /// Json is Returned
        /// </summary>
        [Fact]
        public void Create_TeamNotValid_ArgumentExceptionThrown()
        {
            // Arrange
            var viewModel = new TeamMvcViewModelBuilder().Build();
            _teamServiceMock.Setup(ts => ts.Create(It.IsAny<CreateTeamDto>()))
                                           .Throws(new ArgumentException(SPECIFIED_EXCEPTION_MESSAGE));
            var players = MakeTestPlayers();

            _playerServiceMock.Setup(ps => ps.CreateBulk(It.IsAny<ICollection<CreatePlayerDto>>()))
                .Returns(players);

            var sut = BuildSUT();

            // Act
            var jsonResult = sut.Create(viewModel);
            var actualMessage = jsonResult.Data.ToString();
            var expetedMessage = $"{{ Success = False, Message = {SPECIFIED_EXCEPTION_MESSAGE} }}";

            // Assert
            _teamServiceMock.Verify(ts => ts.Create(It.IsAny<CreateTeamDto>()), Times.Once());
            Assert.Equal(actualMessage, expetedMessage);
        }

        /// <summary>
        /// Create method test. Model state is invalid.
        /// Json is Returned
        /// </summary>
        [Fact]
        public void Create_InValidTeamViewModel_JsonReturned()
        {
            // Arrange
            var viewModel = new TeamMvcViewModelBuilder().Build();
            var sut = BuildSUT();
            sut.ModelState.AddModelError(string.Empty, string.Empty);

            // Act
            var result = sut.Create(viewModel);

            // Assert
            _teamServiceMock.Verify(ts => ts.Create(It.IsAny<CreateTeamDto>()), Times.Never());
            result.Should().NotBeNull(ASSERT_FAIL_JSON_RESULT_MESSAGE);
        }

        /// <summary>
        /// Create method test. Invalid captain Id
        /// </summary>
        [Fact]
        public void Create_InvalidCaptainId_MissingEntityExceptionThrown()
        {
            // Arrange
            var viewModel = new TeamMvcViewModelBuilder().Build();
            var players = MakeTestPlayers();

            _playerServiceMock.Setup(ps => ps.CreateBulk(It.IsAny<ICollection<CreatePlayerDto>>()))
                .Returns(players);


            _teamServiceMock.Setup(ts => ts.Create(It.IsAny<CreateTeamDto>()))
                .Throws(new MissingEntityException(SPECIFIED_EXCEPTION_MESSAGE));

            // Act
            var sut = BuildSUT();

            var jsonResult = sut.Create(viewModel);
            var actualMessage = jsonResult.Data.ToString();
            var expetedMessage = $"{{ Success = False, Message = {SPECIFIED_EXCEPTION_MESSAGE} }}";

            // Assert
            _teamServiceMock.Verify(ts => ts.Create(It.IsAny<CreateTeamDto>()), Times.Once());
            Assert.Equal(actualMessage, expetedMessage);
        }

        /// <summary>
        /// Create method test. Captain of another team
        /// </summary>
        [Fact]
        public void Create_CaptainOfAnotherTeam_ValidationExceptionThrown()
        {
            // Arrange
            var viewModel = new TeamMvcViewModelBuilder().Build();
            _teamServiceMock.Setup(ts => ts.Create(It.IsAny<CreateTeamDto>()))
                                           .Throws(new ValidationException(SPECIFIED_EXCEPTION_MESSAGE));

            var players = MakeTestPlayers();

            _playerServiceMock.Setup(ps => ps.CreateBulk(It.IsAny<ICollection<CreatePlayerDto>>()))
                .Returns(players);
            // Act
            var sut = BuildSUT();
            var jsonResult = sut.Create(viewModel);

            // Assert
            _teamServiceMock.Verify(ts => ts.Create(It.IsAny<CreateTeamDto>()), Times.Once());
            _teamServiceMock.Verify(ts => ts.ChangeCaptain(It.IsAny<TeamId>(), It.IsAny<PlayerId>()), Times.Never());
            Assert.NotNull(jsonResult);
        }

        /// <summary>
        /// Create method test. Captain of another team
        /// </summary>
        [Fact]
        public void Create_RosterPlayerIsCaptainOfAnotherTeam_ValidationExceptionThrown()
        {
            // Arrange
            var viewModel = new TeamMvcViewModelBuilder().Build();
            _teamServiceMock.Setup(ts => ts.Create(It.IsAny<CreateTeamDto>())).Returns(MakeTestTeams().First());

            // Act
            var sut = BuildSUT();
            var jsonResult = sut.Create(viewModel);
            var modelResult = jsonResult;

            // Assert
            Assert.NotNull(modelResult);
        }

        /// <summary>
        /// Create method test. Roster players updated
        /// </summary>
        [Fact]
        public void Create_RosterPlayerPassed_PlayersUpdated()
        {
            // Arrange
            var team = CreateTeam();
            var captain = CreatePlayer(SPECIFIED_PLAYER_ID);
            var rosterDomain = new PlayerServiceTestFixture()
                                .TestPlayers()
                                .AddPlayer(captain)
                                .Build();

            MockTeamServiceGetTeam(team);
            _teamServiceMock.Setup(ts => ts.GetTeamCaptain(It.IsAny<Team>())).Returns(captain);
            _teamServiceMock.Setup(ts => ts.GetTeamRoster(It.IsAny<TeamId>())).Returns(rosterDomain.ToList());
            _teamServiceMock.Setup(ts => ts.AddPlayers(It.IsAny<TeamId>(), It.IsAny<List<PlayerId>>()));
            _playerServiceMock.Setup(ps => ps.CreateBulk(It.IsAny<ICollection<CreatePlayerDto>>()))
                .Returns(rosterDomain);


            var rosterPlayer = new PlayerNameViewModel() {
                Id = SPECIFIED_PLAYER_ID,
                FirstName = SPECIFIED_FIRST_PLAYER_NAME,
                LastName = SPECIFIED_LAST_PLAYER_NAME
            };

            var roster = new List<PlayerNameViewModel>() { rosterPlayer };
            var viewModel = new TeamMvcViewModelBuilder().WithRoster(roster).WithAddedPlayers().Build();

            _teamServiceMock.Setup(ts => ts.Create(It.IsAny<CreateTeamDto>())).Returns(MakeTestTeams().First);

            // Act
            var sut = BuildSUT();
            sut.Create(viewModel);

            // Assert
            _playerServiceMock.Verify(
                             ts => ts.CreateBulk(It.IsAny<List<CreatePlayerDto>>()),
                             Times.Once());
        }

        /// <summary>
        /// Edit method test. Positive test
        /// </summary>
        [Fact]
        public void Edit_TeamPassed_EntityIdIsSet()
        {
            // Arrange
            var team = CreateTeam();
            var captain = CreatePlayer(SPECIFIED_PLAYER_ID);
            var roster = new PlayerServiceTestFixture()
                                .TestPlayers()
                                .AddPlayer(captain)
                                .Build();

            MockTeamServiceGetTeam(team);
            _teamServiceMock.Setup(ts => ts.GetTeamCaptain(It.IsAny<Team>())).Returns(captain);
            _teamServiceMock.Setup(ts => ts.GetTeamRoster(It.IsAny<TeamId>())).Returns(roster.ToList());

            var viewModel = CreateViewModel();
            var expectedDomain = viewModel.ToDomain();
            var comparer = new CreateTeamDtoComparer();
            _teamServiceMock.Setup(ts =>
                ts.Create(It.Is<CreateTeamDto>(t => comparer.Compare(t, expectedDomain) == 0)));

            // Act
            var sut = BuildSUT();
            sut.Edit(viewModel);

            // Assert
            Assert.Equal(viewModel.Id, SPECIFIED_TEAM_ID);
        }

        /// <summary>
        /// Edit method test. Team is not valid. Argument exception is thrown.
        /// Json is Returned
        /// </summary>
        [Fact]
        public void Edit_TeamNotValid_ArgumentExceptionThrown()
        {
            // Arrange
            var viewModel = new TeamMvcViewModelBuilder().Build();
            _teamServiceMock.Setup(ts => ts.Edit(It.IsAny<Team>()))
                                           .Throws(new ArgumentException(SPECIFIED_EXCEPTION_MESSAGE));
            _teamServiceMock.Setup(ts => ts.Get(viewModel.Id)).Returns(MakeTestTeams().First());
            _playerServiceMock.Setup(ps => ps.GetPlayerTeam(It.IsAny<Player>())).Returns(MakeTestTeams().First());
            _playerServiceMock.Setup(ps => ps.CreateBulk(It.IsAny<ICollection<CreatePlayerDto>>()))
                .Returns(MakeTestPlayers());
            // Act
            var sut = BuildSUT();
            var jsonResult = sut.Edit(viewModel);
            var modelResult = jsonResult;

            // Assert
            _teamServiceMock.Verify(ts => ts.Edit(It.IsAny<Team>()), Times.Once());
            Assert.NotNull(modelResult);
        }

        /// <summary>
        /// Edit method test. Model state is invalid.
        /// Json is Returned
        /// </summary>
        [Fact]
        public void Edit_InValidTeamViewModel_JsonReturned()
        {
            // Arrange
            var viewModel = new TeamMvcViewModelBuilder().Build();
            var sut = BuildSUT();
            sut.ModelState.AddModelError(string.Empty, string.Empty);

            // Act
            var result = sut.Edit(viewModel);

            // Assert
            _teamServiceMock.Verify(ts => ts.Edit(It.IsAny<Team>()), Times.Never());
            result.Should().NotBeNull(ASSERT_FAIL_JSON_RESULT_MESSAGE);
        }

        /// <summary>
        /// Edit method test. Invalid captain Id
        /// </summary>
        [Fact]
        public void Edit_InvalidCaptainId_MissingEntityExceptionThrown()
        {
            // Arrange
            var viewModel = new TeamMvcViewModelBuilder().Build();
            _teamServiceMock.Setup(ts => ts.Edit(It.IsAny<Team>()))
                                           .Throws(new MissingEntityException(SPECIFIED_EXCEPTION_MESSAGE));
            _teamServiceMock.Setup(ts => ts.Get(viewModel.Id)).Returns(MakeTestTeams().First());
            _playerServiceMock.Setup(ps => ps.GetPlayerTeam(It.IsAny<Player>())).Returns(MakeTestTeams().First());
            _playerServiceMock.Setup(ps => ps.CreateBulk(It.IsAny<ICollection<CreatePlayerDto>>()))
                .Returns(MakeTestPlayers());
            // Act
            var sut = BuildSUT();
            var jsonResult = sut.Edit(viewModel);
            var modelResult = jsonResult;

            // Assert
            _teamServiceMock.Verify(ts => ts.Edit(It.IsAny<Team>()), Times.Once());
            _teamServiceMock.Verify(ts => ts.ChangeCaptain(It.IsAny<TeamId>(), It.IsAny<PlayerId>()), Times.Never());
            Assert.NotNull(modelResult);
        }

        /// <summary>
        /// Edit method test. Captain of another team
        /// </summary>
        [Fact]
        public void Edit_CaptainOfAnotherTeam_ValidationExceptionThrown()
        {
            // Arrange
            var viewModel = new TeamMvcViewModelBuilder().Build();
            _teamServiceMock.Setup(ts => ts.Edit(It.IsAny<Team>()))
                                           .Throws(new ValidationException(SPECIFIED_EXCEPTION_MESSAGE));
            _teamServiceMock.Setup(ts => ts.Get(viewModel.Id)).Returns(MakeTestTeams().First());
            _playerServiceMock.Setup(ps => ps.GetPlayerTeam(It.IsAny<Player>())).Returns(MakeTestTeams().First());
            _playerServiceMock.Setup(ps => ps.CreateBulk(It.IsAny<ICollection<CreatePlayerDto>>()))
                .Returns(MakeTestPlayers());
            // Act
            var sut = BuildSUT();
            var jsonResult = sut.Edit(viewModel);

            // Assert
            _teamServiceMock.Verify(ts => ts.Edit(It.IsAny<Team>()), Times.Once());
            _teamServiceMock.Verify(ts => ts.ChangeCaptain(It.IsAny<TeamId>(), It.IsAny<PlayerId>()), Times.Never());
            Assert.NotNull(jsonResult);
        }

        /// <summary>
        /// Edit method test. Invalid player Id
        /// </summary>
        [Fact]
        public void Edit_InvalidRosterPlayerId_ArgumentExceptionThrown()
        {
            // Arrange
            var team = CreateTeam();
            var captain = CreatePlayer(SPECIFIED_PLAYER_ID);
            var roster = new PlayerServiceTestFixture()
                                .TestPlayers()
                                .AddPlayer(captain)
                                .Build();

            MockTeamServiceGetTeam(team);
            _teamServiceMock.Setup(ts => ts.GetTeamCaptain(It.IsAny<Team>())).Returns(captain);
            _teamServiceMock.Setup(ts => ts.GetTeamRoster(It.IsAny<TeamId>())).Returns(roster.ToList());

            var viewModel = CreateViewModel();
            _teamServiceMock.Setup(ts => ts.Edit(It.IsAny<Team>()))
                                           .Throws(new MissingEntityException(SPECIFIED_EXCEPTION_MESSAGE));

            // Act
            var sut = BuildSUT();
            var jsonResult = sut.Edit(viewModel);

            // Assert
            Assert.NotNull(jsonResult);
        }

        /// <summary>
        /// Edit method test. Captain of another team
        /// </summary>
        [Fact]
        public void Edit_RosterPlayerIsCaptainOfAnotherTeam_ValidationExceptionThrown()
        {
            // Arrange
            var team = CreateTeam();
            var captain = CreatePlayer(SPECIFIED_PLAYER_ID);
            var roster = new PlayerServiceTestFixture()
                                .TestPlayers()
                                .AddPlayer(captain)
                                .Build();

            MockTeamServiceGetTeam(team);
            _teamServiceMock.Setup(ts => ts.GetTeamCaptain(It.IsAny<Team>())).Returns(captain);
            _teamServiceMock.Setup(ts => ts.GetTeamRoster(It.IsAny<TeamId>())).Returns(roster.ToList());

            var viewModel = CreateViewModel();

            // Act
            var sut = BuildSUT();
            var jsonResult = sut.Edit(viewModel);

            // Assert
            Assert.NotNull(jsonResult);
        }

        /// <summary>
        /// Edit method test. Roster players updated
        /// </summary>
        [Fact]
        public void Edit_RosterPlayerPassed_PlayersUpdated()
        {
            // Arrange
            var team = CreateTeam();
            var captain = CreatePlayer(SPECIFIED_PLAYER_ID);
            var rosterDomain = new PlayerServiceTestFixture()
                                .TestPlayers()
                                .AddPlayer(captain)
                                .Build();

            MockTeamServiceGetTeam(team);
            _teamServiceMock.Setup(ts => ts.GetTeamCaptain(It.IsAny<Team>())).Returns(captain);
            _teamServiceMock.Setup(ts => ts.GetTeamRoster(It.IsAny<TeamId>())).Returns(rosterDomain.ToList());

            var rosterPlayer = new PlayerNameViewModel() { Id = SPECIFIED_PLAYER_ID, FirstName = SPECIFIED_FIRST_PLAYER_NAME, LastName = SPECIFIED_LAST_PLAYER_NAME };
            var roster = new List<PlayerNameViewModel>() { rosterPlayer };
            var viewModel = new TeamMvcViewModelBuilder().WithRoster(roster).Build();

            _teamServiceMock.Setup(ts => ts.Edit(It.IsAny<Team>()));
            _playerServiceMock.Setup(ps => ps.GetPlayerTeam(It.IsAny<Player>())).Returns(team);
            _playerServiceMock.Setup(ps => ps.CreateBulk(It.IsAny<ICollection<CreatePlayerDto>>()))
                .Returns(rosterDomain);


            // Act
            var sut = BuildSUT();
            var jsonResult = sut.Edit(viewModel);

            // Assert
            Assert.NotNull(jsonResult);
        }

        /// <summary>
        /// Test for Details method. Team with specified identifier does not exist. HttpNotFoundResult is returned.
        /// </summary>
        [Fact]
        public void Details_NonExistentTeam_HttpNotFoundResultIsReturned()
        {
            // Arrange
            SetupGet(TEST_TEAM_ID, null);

            // Act
            var sut = BuildSUT();
            var result = sut.Details(TEST_TEAM_ID);

            // Assert
            Assert.IsType<HttpNotFoundResult>(result);
        }

        /// <summary>
        /// Test for Details method. Team with specified identifier exists. View model of Team is returned.
        /// </summary>
        [Fact]
        public void Details_ExistingTeam_TeamViewModelIsReturned()
        {
            // Arrange
            var team = CreateTeam();
            var captain = CreatePlayer(SPECIFIED_PLAYER_ID);
            var roster = new PlayerServiceTestFixture()
                                .TestPlayers()
                                .AddPlayer(captain)
                                .Build();

            MockTeamServiceGetTeam(team);
            _teamServiceMock.Setup(ts => ts.GetTeamCaptain(It.IsAny<Team>())).Returns(captain);
            _teamServiceMock.Setup(ts => ts.GetTeamRoster(It.IsAny<TeamId>())).Returns(roster.ToList());
            SetupRequestRawUrl("/Teams");
            var sut = BuildSUT();

            SetupControllerContext(sut);

            var expected = CreateViewModel();

            // Act
            var actual = TestExtensions.GetModel<TeamRefererViewModel>(sut.Details(SPECIFIED_TEAM_ID));

            // Assert
            Assert.Equal<TeamViewModel>(expected, actual.Model, new TeamViewModelComparer());
            Assert.Equal(actual.CurrentReferrer, sut.Request.RawUrl);
        }

        /// <summary>
        /// Test for Edit method (GET action). Valid team id.  Team view model is returned.
        /// </summary>
        [Fact]
        public void EditGetAction_TeamId_TeamViewModelIsReturned()
        {
            var team = CreateTeam();
            var captain = CreatePlayer(SPECIFIED_PLAYER_ID);
            var roster = new PlayerServiceTestFixture()
                                .TestPlayers()
                                .AddPlayer(captain)
                                .Build();

            MockTeamServiceGetTeam(team);
            _teamServiceMock.Setup(ts => ts.GetTeamCaptain(It.IsAny<Team>())).Returns(captain);
            _teamServiceMock.Setup(ts => ts.GetTeamRoster(It.IsAny<TeamId>())).Returns(roster.ToList());

            var sut = BuildSUT();
            SetupControllerContext(sut);

            var expected = CreateViewModel();

            // Act
            var actual = TestExtensions.GetModel<TeamViewModel>(sut.Edit(SPECIFIED_TEAM_ID));

            // Assert
            Assert.Equal<TeamViewModel>(expected, actual, new TeamViewModelComparer());
        }

        /// <summary>
        /// Test for Edit method. Team with specified identifier does not exist. HttpNotFoundResult is returned.
        /// </summary>
        [Fact]
        public void Edit_NotExistedTeam_HttpNotFoundResultIsReturned()
        {
            // Arrange
            SetupGet(TEAM_ID, null as Team);

            // Act
            var sut = BuildSUT();
            var result = sut.Edit(TEAM_ID);

            // Assert
            Assert.IsType<HttpNotFoundResult>(result);
        }

        /// <summary>
        /// Test for GetAllTeams method. All teams are requested. JsonResult with all teams is returned.
        /// </summary>
        [Fact]
        public void GetAllTeams_GetTeams_JsonResultIsReturned()
        {
            // Arrange
            var testData = MakeTestTeams();
            SetupGetAllTeams(testData);

            // Act
            var sut = BuildSUT();
            var result = sut.GetAllTeams();

            // Assert
            result.Should().NotBeNull(ASSERT_FAIL_JSON_RESULT_MESSAGE);
        }

        private TeamsController BuildSUT()
        {
            return new TeamsController(
                _teamServiceMock.Object,
                _authServiceMock.Object,
                _fileServiceMock.Object,
                _playerServiceMock.Object);
        }

        private Team CreateTeam()
        {
            return new TeamBuilder()
                         .WithId(SPECIFIED_TEAM_ID)
                         .WithCaptain(new PlayerId(SPECIFIED_PLAYER_ID))
                         .WithCoach(COACH)
                         .WithAchievements(ACHIEVEMENTS)
                         .WithName(TEAM_NAME)
                         .Build();
        }

        private Player CreatePlayer(int id)
        {
            return new PlayerBuilder(id, PLAYER_FIRSTNAME, PLAYER_LASTNAME)
                        .Build();
        }

        private PlayerNameViewModel CreatePlayerNameModel(string firstname, string lastname, int id)
        {
            return new PlayerNameViewModel() {
                FirstName = firstname,
                LastName = lastname,
                Id = id
            };
        }

        private List<PlayerNameViewModel> CreateRoster()
        {
            const int FIRST_PLAYER_ID = 1;
            const string FIRST_PLAYER_FIRSTNAME = "FirstNameA";
            const string FIRST_PLAYER_LASTNAME = "LastNameA";
            const int SECOND_PLAYER_ID = 2;
            const string SECOND_PLAYER_FIRSTNAME = "FirstNameB";
            const string SECOND_PLAYER_LASTNAME = "LastNameB";
            const int THIRD_PLAYER_ID = 3;
            const string THIRD_PLAYER_FIRSTNAME = "FirstNameC";
            const string THIRD_PLAYER_LASTNAME = "LastNameC";
            var roster = new List<PlayerNameViewModel>()
            {
                CreatePlayerNameModel(FIRST_PLAYER_FIRSTNAME, FIRST_PLAYER_LASTNAME, FIRST_PLAYER_ID),
                CreatePlayerNameModel(SECOND_PLAYER_FIRSTNAME, SECOND_PLAYER_LASTNAME, SECOND_PLAYER_ID),
                CreatePlayerNameModel(THIRD_PLAYER_FIRSTNAME, THIRD_PLAYER_LASTNAME, THIRD_PLAYER_ID),
                CreatePlayerNameModel(PLAYER_FIRSTNAME, PLAYER_LASTNAME, SPECIFIED_PLAYER_ID)
            };

            return roster;
        }

        private TeamViewModel CreateViewModel()
        {
            var cap = CreatePlayerNameModel(PLAYER_FIRSTNAME, PLAYER_LASTNAME, SPECIFIED_PLAYER_ID);
            var players = CreateRoster();
            return new TeamMvcViewModelBuilder()
                          .WithId(SPECIFIED_TEAM_ID)
                          .WithCoach(COACH)
                          .WithAchievements(ACHIEVEMENTS)
                          .WithName(TEAM_NAME)
                          .WithCaptain(cap)
                          .WithRoster(players)
                          .Build();
        }

        private void MockTeamServiceGetTeam(Team team)
        {
            _teamServiceMock.Setup(ts => ts.Get(It.IsAny<int>())).Returns(team);
        }

        private void SetupGet(int teamId, Team team)
        {
            _teamServiceMock.Setup(tr => tr.Get(teamId)).Returns(team);
        }

        private void SetupControllerContext(TeamsController sut)
        {
            sut.ControllerContext = new ControllerContext(_httpContextMock.Object, new RouteData(), sut);
        }

        private void SetupGetAllTeams(List<Team> teams)
        {
            _teamServiceMock.Setup(tr => tr.Get()).Returns(teams);
        }

        private void SetupRequestRawUrl(string rawUrl)
        {
            _httpRequestMock.Setup(x => x.RawUrl).Returns(rawUrl);
        }

        private List<Team> MakeTestTeams()
        {
            return new TeamServiceTestFixture().TestTeams().Build();
        }

        private List<Player> MakeTestPlayers()
        {
            return new PlayerServiceTestFixture().TestPlayers().Build();
        }
    }
}