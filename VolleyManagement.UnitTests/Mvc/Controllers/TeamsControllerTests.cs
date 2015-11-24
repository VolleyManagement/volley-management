namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Contracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Domain.PlayersAggregate;
    using VolleyManagement.Domain.TeamsAggregate;
    using VolleyManagement.UI.Areas.Mvc.Controllers;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Players;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Teams;
    using VolleyManagement.UnitTests.Mvc.ViewModels;
    using VolleyManagement.UnitTests.Services.PlayerService;
    using VolleyManagement.UnitTests.Services.TeamService;

    /// <summary>
    /// Tests for MVC TeamsController class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TeamsControllerTests
    {
        private const int TEAM_UNEXISTING_ID_TO_DELETE = 4;
        private const int SPECIFIED_TEAM_ID = 4;
        private const int SPECIFIED_PLAYER_ID = 4;
        private const string SPECIFIED_PLAYER_NAME = "Test name";
        private const string SPECIFIED_EXCEPTION_MESSAGE = "Test exception message";
        private const string ACHIEVEMENTS = "TestAchievements";
        private const string TEAM_NAME = "TestName";
        private const string PLAYER_FIRSTNAME = "Test";
        private const string PLAYER_LASTNAME = "Test";
        private const string COACH = "TestCoach";
        private const int TEST_TEAM_ID = 1;

        private readonly Mock<ITeamService> _teamServiceMock = new Mock<ITeamService>();
        private readonly Mock<HttpContextBase> _httpContextMock = new Mock<HttpContextBase>();
        private readonly Mock<HttpRequestBase> _httpRequestMock = new Mock<HttpRequestBase>();

        private IKernel _kernel;
        private TeamsController _sut;

        /// <summary>
        /// Initializes test data
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            this._kernel = new StandardKernel();
            this._kernel.Bind<ITeamService>().ToConstant(this._teamServiceMock.Object);
            this._httpContextMock.SetupGet(c => c.Request).Returns(this._httpRequestMock.Object);
            this._sut = this._kernel.Get<TeamsController>();
        }

        /// <summary>
        /// Delete method test. The method should invoke Delete() method of ITeamService
        /// and return result as JavaScript Object Notation.
        /// </summary>
        [TestMethod]
        public void Delete_PlayerExists_PlayerIsDeleted()
        {
            // Act
            var sut = this._kernel.Get<TeamsController>();
            var actual = sut.Delete(TEAM_UNEXISTING_ID_TO_DELETE) as JsonResult;

            // Assert
            _teamServiceMock.Verify(ps => ps.Delete(It.Is<int>(id => id == TEAM_UNEXISTING_ID_TO_DELETE)), Times.Once());
            Assert.IsNotNull(actual);
        }

        /// <summary>
        /// Delete method test. Input parameter is team id, which doesn't exist in database.
        /// The method should return message as JavaScript Object Notation.
        /// </summary>
        [TestMethod]
        public void Delete_PlayerDoesntExist_JsonReturned()
        {
            // Arrange
            _teamServiceMock.Setup(ps => ps.Delete(TEAM_UNEXISTING_ID_TO_DELETE)).Throws<MissingEntityException>();

            // Act
            var sut = this._kernel.Get<TeamsController>();
            var actual = sut.Delete(TEAM_UNEXISTING_ID_TO_DELETE) as JsonResult;

            // Assert
            Assert.IsNotNull(actual);
        }

        /// <summary>
        /// Create method test. Positive test
        /// </summary>
        [TestMethod]
        public void Create_TeamPassed_EntityIdIsSet()
        {
            // Arrange
            var viewModel = new TeamMvcViewModelBuilder().WithId(0).Build();
            var expectedDomain = viewModel.ToDomain();
            var comparer = new TeamComparer();
            _teamServiceMock.Setup(ts => ts.Create(It.Is<Team>(t => comparer.AreEqual(t, expectedDomain))))
                                           .Callback<Team>(t => t.Id = SPECIFIED_TEAM_ID);

            // Act
            var sut = this._kernel.Get<TeamsController>();
            sut.Create(viewModel);

            // Assert
            Assert.AreEqual(viewModel.Id, SPECIFIED_TEAM_ID);
        }

        /// <summary>
        /// Create method test. Invalid captain Id
        /// </summary>
        [TestMethod]
        public void Create_InvalidCaptainId_MissingEntityExceptionThrown()
        {
            // Arrange
            var viewModel = new TeamMvcViewModelBuilder().Build();
            _teamServiceMock.Setup(ts => ts.Create(It.IsAny<Team>()))
                                           .Throws(new MissingEntityException(SPECIFIED_EXCEPTION_MESSAGE));

            // Act
            var sut = _kernel.Get<TeamsController>();
            sut.Create(viewModel);
            bool gotException = sut.ModelState.Values
                                   .Select(ms => ms.Errors
                                                   .Select(me => me.ErrorMessage == SPECIFIED_EXCEPTION_MESSAGE))
                                   .Count() == 1;

            // Assert
            _teamServiceMock.Verify(ts => ts.Create(It.IsAny<Team>()), Times.Once());
            _teamServiceMock.Verify(ts => ts.UpdatePlayerTeam(It.IsAny<int>(), It.IsAny<int>()), Times.Never());
            Assert.IsTrue(gotException);
        }

        /// <summary>
        /// Create method test. Captain of another team
        /// </summary>
        [TestMethod]
        public void Create_CaptainOfAnotherTeam_ValidationExceptionThrown()
        {
            // Arrange
            var viewModel = new TeamMvcViewModelBuilder().Build();
            _teamServiceMock.Setup(ts => ts.Create(It.IsAny<Team>()))
                                           .Throws(new ValidationException(SPECIFIED_EXCEPTION_MESSAGE));

            // Act
            var sut = _kernel.Get<TeamsController>();
            sut.Create(viewModel);
            bool gotException = sut.ModelState.Values
                                   .Select(ms => ms.Errors
                                                   .Select(me => me.ErrorMessage == SPECIFIED_EXCEPTION_MESSAGE))
                                   .Count() == 1;

            // Assert
            _teamServiceMock.Verify(ts => ts.Create(It.IsAny<Team>()), Times.Once());
            _teamServiceMock.Verify(ts => ts.UpdatePlayerTeam(It.IsAny<int>(), It.IsAny<int>()), Times.Never());
            Assert.IsTrue(gotException);
        }

        /// <summary>
        /// Create method test. Invalid player Id
        /// </summary>
        [TestMethod]
        public void Create_InvalidRosterPlayerId_MissingEntityExceptionThrown()
        {
            // Arrange
            var viewModel = new TeamMvcViewModelBuilder().Build();
            var comparer = new TeamComparer();
            _teamServiceMock.Setup(ts => ts.UpdatePlayerTeam(It.IsAny<int>(), It.IsAny<int>()))
                                           .Throws(new MissingEntityException(SPECIFIED_EXCEPTION_MESSAGE));

            // Act
            var sut = _kernel.Get<TeamsController>();
            sut.Create(viewModel);
            bool gotException = sut.ModelState.Values
                                   .Select(ms => ms.Errors
                                                   .Select(me => me.ErrorMessage == SPECIFIED_EXCEPTION_MESSAGE))
                                   .Count() >= 1;

            // Assert
            _teamServiceMock.Verify(ts => ts.UpdatePlayerTeam(It.IsAny<int>(), It.IsAny<int>()), Times.AtLeastOnce());
            Assert.IsTrue(gotException);
        }

        /// <summary>
        /// Create method test. Captain of another team
        /// </summary>
        [TestMethod]
        public void Create_RosterPlayerIsCaptainOfAnotherTeam_ValidationExceptionThrown()
        {
            // Arrange
            var viewModel = new TeamMvcViewModelBuilder().Build();
            _teamServiceMock.Setup(ts => ts.UpdatePlayerTeam(It.IsAny<int>(), It.IsAny<int>()))
                                           .Throws(new ValidationException(SPECIFIED_EXCEPTION_MESSAGE));

            // Act
            var sut = _kernel.Get<TeamsController>();
            sut.Create(viewModel);
            bool gotException = sut.ModelState.Values
                                   .Select(ms => ms.Errors
                                                   .Select(me => me.ErrorMessage == SPECIFIED_EXCEPTION_MESSAGE))
                                   .Count() >= 1;

            // Assert
            _teamServiceMock.Verify(ts => ts.UpdatePlayerTeam(It.IsAny<int>(), It.IsAny<int>()), Times.AtLeastOnce());
            Assert.IsTrue(gotException);
        }

        /// <summary>
        /// Create method test. Roster players updated
        /// </summary>
        [TestMethod]
        public void Create_RosterPlayerPassed_PlayersUpdated()
        {
            // Arrange
            var rosterPlayer = new PlayerNameViewModel() { Id = SPECIFIED_PLAYER_ID, FullName = SPECIFIED_PLAYER_NAME };
            var roster = new List<PlayerNameViewModel>() { rosterPlayer };
            var viewModel = new TeamMvcViewModelBuilder().WithRoster(roster).Build();
            int expectedCountOfUpdates = viewModel.Roster.Count();

            _teamServiceMock.Setup(ts => ts.Create(It.IsAny<Team>()))
                                           .Callback<Team>(t => t.Id = SPECIFIED_TEAM_ID);

            // Act
            var sut = _kernel.Get<TeamsController>();
            sut.Create(viewModel);

            // Assert
            _teamServiceMock.Verify(
                             ts => ts.UpdatePlayerTeam(It.IsAny<int>(), It.IsAny<int>()),
                             Times.Exactly(expectedCountOfUpdates));

            _teamServiceMock.Verify(
                             ts => ts.UpdatePlayerTeam(
                                      It.Is<int>(pId => pId == SPECIFIED_PLAYER_ID),
                                      It.Is<int>(pId => pId == SPECIFIED_TEAM_ID)),
                             Times.Once());
        }

        /// <summary>
        /// Test for Details method. Team with specified identifier does not exist. HttpNotFoundResult is returned.
        /// </summary>
        [TestMethod]
        public void Details_NonExistentTeam_HttpNotFoundResultIsReturned()
        {
            // Arrange
            MockSetupGet(null);

            // Act
            var result = this._sut.Details(TEST_TEAM_ID);

            // Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        /// <summary>
        /// Test for Details method. Team with specified identifier exists. View model of Team is returned.
        /// </summary>
        [TestMethod]
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
            _teamServiceMock.Setup(ts => ts.GetTeamRoster(It.IsAny<int>())).Returns(roster.ToList());
            MockSetupControllerContext();

            var expected = CreateViewModel();

            // Act
            var actual = TestExtensions.GetModel<TeamViewModel>(this._sut.Details(SPECIFIED_TEAM_ID));

            // Assert
            TestHelper.AreEqual<TeamViewModel>(expected, actual, new TeamViewModelComparer());
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

        private PlayerNameViewModel CreatePlayerNameModel(string firstname, string lastname, int id)
        {
            return new PlayerNameViewModel()
            {
                FullName = string.Format("{1} {0}", firstname, lastname),
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

        /// <summary>
        /// Makes team with specified identifier filled with test data.
        /// </summary>
        /// <param name="teamId">Identifier of the team.</param>
        /// <returns>Team filled with test data.</returns>
        private Team MakeTestTeam(int teamId)
        {
            return new TeamBuilder().WithId(teamId).Build();
        }

        /// <summary>
        /// Makes team view model filled with test data.
        /// </summary>
        /// <param name="teamId">Identifier of the team.</param>
        /// <returns>Team view model filled with test data.</returns>
        private TeamViewModel MakeTestTeamViewModel(int teamId)
        {
            return new TeamMvcViewModelBuilder().WithId(teamId).Build();
        }

        /// <summary>
        /// Sets up a mock for Get method of Team service with any parameter to return specified team.
        /// </summary>
        /// <param name="team">Team that will be returned by Get method of Team service.</param>
        private void MockSetupGet(Team team)
        {
            this._teamServiceMock.Setup(tr => tr.Get(It.IsAny<int>())).Returns(team);
        }

        /// <summary>
        /// Sets up a mock for ControllerContext property of the controller.
        /// </summary>
        private void MockSetupControllerContext()
        {
            this._sut.ControllerContext = new ControllerContext(this._httpContextMock.Object, new RouteData(), this._sut);
        }
    }
}
