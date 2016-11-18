namespace VolleyManagement.UnitTests.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Mvc.Comparers;
    using Ninject;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Authorization;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Domain.PlayersAggregate;
    using VolleyManagement.Domain.TeamsAggregate;
    using VolleyManagement.Domain.TournamentRequestAggregate;
    using VolleyManagement.Domain.TournamentsAggregate;
    using VolleyManagement.Domain.UsersAggregate;
    using VolleyManagement.UI.Areas.Admin.Controllers;
    using VolleyManagement.UI.Areas.Admin.Models;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Players;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Teams;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments;
    using VolleyManagement.UnitTests.Admin.Comparers;
    using VolleyManagement.UnitTests.Admin.ViewModels;
    using VolleyManagement.UnitTests.Mvc.ViewModels;
    using VolleyManagement.UnitTests.Services.PlayerService;
    using VolleyManagement.UnitTests.Services.TeamService;
    using VolleyManagement.UnitTests.Services.TournamentRequestService;
    using VolleyManagement.UnitTests.Services.TournamentService;
    using VolleyManagement.UnitTests.Services.UsersService;
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TournamentRequestControllerTest
    {
        private const string ACHIEVEMENTS = "TestAchievements";
        private const string TEAM_NAME = "TestName";
        private const string PLAYER_FIRSTNAME = "Test";
        private const string PLAYER_LASTNAME = "Test";
        private const string COACH = "TestCoach";
        private const int SPECIFIED_TEAM_ID = 4;
        private const int SPECIFIED_PLAYER_ID = 4;
        private const int TEST_TEAM_ID = 1;
        private const int TEST_TOURNAMENT_ID = 1;
        private const int REQUEST_ID = 1;
        private const int USER_ID = 1;
        private const string TEST_MESSAGE = "Hello, user!";
        private readonly Mock<ITournamentRequestService> _requestServiceMock = new Mock<ITournamentRequestService>();
        private readonly Mock<ITournamentService> _tournamentServiceMock = new Mock<ITournamentService>();
        private readonly Mock<ITeamService> _teamServiceMock = new Mock<ITeamService>();
        private readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();
        private readonly Mock<HttpContextBase> _httpContextMock = new Mock<HttpContextBase>();
        private readonly Mock<HttpRequestBase> _httpRequestMock = new Mock<HttpRequestBase>();

        private IKernel _kernel;
        private TournamentRequestController _sut;

        [TestInitialize]
        public void TestInit()
        {
            this._kernel = new StandardKernel();
            this._kernel.Bind<ITournamentRequestService>().ToConstant(this._requestServiceMock.Object);
            this._kernel.Bind<ITournamentService>().ToConstant(this._tournamentServiceMock.Object);
            this._kernel.Bind<ITeamService>().ToConstant(this._teamServiceMock.Object);
            this._kernel.Bind<IUserService>().ToConstant(this._userServiceMock.Object);
            this._httpContextMock.SetupGet(c => c.Request).Returns(this._httpRequestMock.Object);
            this._sut = this._kernel.Get<TournamentRequestController>();
        }

        [TestMethod]
        public void Index_ExistingRequests_TournamentRequestCollectionViewModelIsReturned()
        {
            // Arrange
            var requests = MakeTestTournamentsRequests();
            var team = CreateTeam();
            var tournament = MakeTestTournament(TEST_TOURNAMENT_ID);
            var user = CreateUser();

            MockTournamentRequestServiceGet(requests);
            MockTeamServiceGetTeam(team);
            MockTournamentServiceGetTournament(TEST_TOURNAMENT_ID, tournament);
            MockUserServiceGetUserDetails(USER_ID, user);

            var expected = CreateTournamentRequestViewModelList();

            // Act
            var actual = TestExtensions.GetModel<TournamentRequestCollectionViewModel>(_sut.Index()).Requests.ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual, new TournamentRequestViewModelComparer());
        }

        [TestMethod]
        public void TeamDetails_ExistingTeam_TeamViewModelIsReturned()
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
            _teamServiceMock.Setup(ts => ts.GetTeamRoster(It.IsAny<int>())).Returns(roster.ToList());

            var expected = CreateViewModel();

            // Act
            var actual = TestExtensions.GetModel<TeamViewModel>(this._sut.TeamDetails(SPECIFIED_TEAM_ID));

            // Assert
            TestHelper.AreEqual<TeamViewModel>(expected, actual, new TeamViewModelComparer());
        }

        [TestMethod]
        public void TeamDetails_NonExistentTeam_HttpNotFoundResultIsReturned()
        {
            // Arrange
            SetupGet(TEST_TEAM_ID, null);

            // Act
            var result = this._sut.TeamDetails(TEST_TEAM_ID);

            // Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void TournamentDetails_NonExistentTournament_HttpNotFoundResultIsReturned()
        {
            // Arrange
            MockTournamentServiceGetTournament(TEST_TOURNAMENT_ID, null as Tournament);

            // Act
            var result = this._sut.TournamentDetails(TEST_TOURNAMENT_ID);

            // Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void TournamentDetails_ExistingTournament_TournamentViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestTournament(TEST_TOURNAMENT_ID);
            var expected = MakeTestTournamentViewModel(TEST_TOURNAMENT_ID);
            MockTournamentServiceGetTournament(TEST_TOURNAMENT_ID, testData);

            // Act
            var actual = TestExtensions.GetModel<TournamentViewModel>(_sut.TournamentDetails(TEST_TOURNAMENT_ID));

            // Assert
            TestHelper.AreEqual<TournamentViewModel>(expected, actual, new TournamentViewModelComparer());
        }

        [TestMethod]
        public void Confirm_AnyRequest_RequestRedirectToIndex()
        {
            // Arrange
            var sut = _kernel.Get<TournamentRequestController>();

            // Act
            var actionResult = sut.Confirm(REQUEST_ID);

            // Assert
            AssertValidRedirectResult(actionResult, "Index");
        }

        [TestMethod]
        public void Confirm_AnyRequest_RequestConfirmed()
        {
            // Arrange
            var sut = _kernel.Get<TournamentRequestController>();

            // Act
            var actionResult = sut.Confirm(REQUEST_ID);

            // Assert
            AssertVerifyConfirm(REQUEST_ID);
        }

        [TestMethod]
        public void Confirm_NonExistentRequest_ThrowsMissingEntityException()
        {
            // Arrange
            SetupConfirmThrowsMissingEntityException();
            var sut = _kernel.Get<TournamentRequestController>();

            // Act
            var actionResult = sut.Confirm(REQUEST_ID);

            // Assert
            Assert.IsNotNull(actionResult, "InvalidOperation");
        }

        [TestMethod]
        public void Decline_NonExistentRequest_ThrowsMissingEntityException()
        {
            // Arrange
            SetupDeclineThrowsMissingEntityException();
            var messageViewModel = CreateMessageViewModel();

            // Act
            var actionResult = _sut.Decline(messageViewModel);

            // Assert
            Assert.IsNotNull(actionResult, "InvalidOperation");
        }

        [TestMethod]
        public void Decline_AnyRequest_RequestDeclined()
        {
            // Arrange
            var messageViewModel = CreateMessageViewModel();

            // Act
            var actionResult = _sut.Decline(messageViewModel);

            // Assert
            AssertVerifyDecline(REQUEST_ID);
        }

        [TestMethod]
        public void Decline_AnyRequest_RequestRedirectToIndex()
        {
            // Arrange
            var messageViewModel = CreateMessageViewModel();

            // Act
            var actionResult = _sut.Decline(messageViewModel);

            // Assert
            AssertValidRedirectResult(actionResult, "Index");
        }

        [TestMethod]
        public void Decline_AnyRequest_MessageViewModelIsReturned()
        {
            // Arrange
            var expected = SetupMessageViewModel();

            // Act
            var actual = TestExtensions.GetModel<MessageViewModel>(_sut.Decline(REQUEST_ID));

            // Assert
            TestHelper.AreEqual<MessageViewModel>(expected, actual, new MessageViewModelComparer());
        }

        [TestMethod]
        public void UserDetails_ExistingUser_UserViewModelIsReturned()
        {
            // Arrange
            var user = CreateUser();
            MockUserServiceGetUserDetails(USER_ID, user);

            var expected = CreateUserViewModel();

            // Act
            var actual = TestExtensions.GetModel<UserViewModel>(_sut.UserDetails(USER_ID));

            // Assert
            TestHelper.AreEqual<UserViewModel>(expected, actual, new Comparers.UserViewModelComparer());
        }

        [TestMethod]
        public void UserDetails_NonExistentUser_HttpNotFoundResultIsReturned()
        {
            // Arrange
            MockUserServiceGetUserDetails(USER_ID, null);

            // Act
            var result = this._sut.UserDetails(USER_ID);

            // Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        private static void AssertValidRedirectResult(ActionResult actionResult, string view)
        {
            var result = (RedirectToRouteResult)actionResult;
            Assert.IsFalse(result.Permanent, "Redirect should not be permanent");
            Assert.AreEqual(1, result.RouteValues.Count, string.Format("Redirect should forward to Requests.{0} action", view));
            Assert.AreEqual(view, result.RouteValues["action"], string.Format("Redirect should forward to Requests.{0} action", view));
        }

        private void MockTeamServiceGetTeam(Team team)
        {
            _teamServiceMock.Setup(ts => ts.Get(It.IsAny<int>())).Returns(team);
        }

        private void SetupGet(int teamId, Team team)
        {
            this._teamServiceMock.Setup(tr => tr.Get(teamId)).Returns(team);
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

        private PlayerNameViewModel CreatePlayerNameModel(string firstname, string lastname, int id)
        {
            return new PlayerNameViewModel()
            {
                FullName = string.Format("{1} {0}", firstname, lastname),
                Id = id
            };
        }

        private Team CreateTeam()
        {
            return new TeamBuilder()
                         .WithId(SPECIFIED_TEAM_ID)
                         .WithCaptain(SPECIFIED_PLAYER_ID)
                         .WithCoach(COACH)
                         .WithAchievements(ACHIEVEMENTS)
                         .WithName(TEAM_NAME)
                         .Build();
        }

        private Player CreatePlayer(int id)
        {
            return new PlayerBuilder()
                        .WithId(id)
                        .WithFirstName(PLAYER_FIRSTNAME)
                        .WithLastName(PLAYER_LASTNAME)
                        .WithTeamId(SPECIFIED_TEAM_ID)
                        .Build();
        }

        private Tournament MakeTestTournament(int tournamentId)
        {
            return new TournamentBuilder().WithId(tournamentId).Build();
        }

        private TournamentViewModel MakeTestTournamentViewModel(int tournamentId)
        {
            return new TournamentMvcViewModelBuilder().WithId(tournamentId).Build();
        }

        private List<TournamentRequest> MakeTestTournamentsRequests()
        {
            return new TournamentRequestServiceTestFixture().TestTournamentsRequests().Build();
        }

        private void MockTournamentServiceGetTournament(int tournamentId, Tournament tournament)
        {
            this._tournamentServiceMock.Setup(tr => tr.Get(tournamentId)).Returns(tournament);
        }

        private void AssertVerifyConfirm(int requestId)
        {
            _requestServiceMock.Verify(m => m.Confirm(It.Is<int>(id => id == requestId)), Times.Once());
        }

        private void SetupConfirmThrowsMissingEntityException()
        {
            _requestServiceMock.Setup(m => m.Confirm(It.IsAny<int>()))
                .Throws(new MissingEntityException(string.Empty));
        }

        private User CreateUser()
        {
            return new UserBuilder()
                         .WithId(USER_ID)
                         .Build();
        }

        private UserViewModel CreateUserViewModel()
        {
            var player = CreatePlayerViewModel();

            return new UserAdminViewModelBuilder().Build();
        }

        private PlayerViewModel CreatePlayerViewModel()
        {
            return new PlayerMvcViewModelBuilder().Build();
        }

        private void MockUserServiceGetUserDetails(int userId, User user)
        {
            this._userServiceMock.Setup(tr => tr.GetUserDetails(userId)).Returns(user);
        }

        private MessageViewModel CreateMessageViewModel()
        {
            return new MessageViewModel
            {
                Id = REQUEST_ID,
                Message = TEST_MESSAGE
            };
        }

        private MessageViewModel SetupMessageViewModel()
        {
            return new MessageViewModel
            {
                Id = REQUEST_ID,
            };
        }

        private void SetupDeclineThrowsMissingEntityException()
        {
            _requestServiceMock.Setup(m => m.Decline(It.IsAny<int>(), TEST_MESSAGE))
                .Throws(new MissingEntityException(string.Empty));
        }

        private void AssertVerifyDecline(int requestId)
        {
            _requestServiceMock.Verify(m => m.Decline(It.Is<int>(id => id == requestId), TEST_MESSAGE), Times.Once());
        }

        private void MockTournamentRequestServiceGet(List<TournamentRequest> requests)
        {
            this._requestServiceMock.Setup(tr => tr.Get()).Returns(requests);
        }

        private List<TournamentRequestViewModel> CreateTournamentRequestViewModelList()
        {
            return new TournamentRequestsViewModelsBuilder()
                       .TestTournamentsRequests()
                       .Build();
        }
    }
}
