namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using Contracts;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Domain.TeamsAggregate;
    using VolleyManagement.UI.Areas.Mvc.Controllers;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Players;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Teams;
    using VolleyManagement.UnitTests.Mvc.ViewModels;
    using VolleyManagement.UnitTests.Services.PlayerService;
    using VolleyManagement.UnitTests.Services.TeamService;
using VolleyManagement.Domain.PlayersAggregate;

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

        private readonly Mock<ITeamService> _teamServiceMock = new Mock<ITeamService>();
        private IKernel _kernel;

        /// <summary>
        /// Initializes test data
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            this._kernel = new StandardKernel();
            this._kernel.Bind<ITeamService>()
                   .ToConstant(this._teamServiceMock.Object);
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
        /// Details action method test. Team exists
        /// </summary>
        [TestMethod]
        public void Details_TeamExists_TeamIsReturned()
        {
            // Arrange
            var team = CreatestTeam();
            var captain = CreatePlayer(SPECIFIED_PLAYER_ID);
            var roster = new PlayerServiceTestFixture()
                                .TestPlayers()
                                .AddPlayer(captain)
                                .Build();

            _teamServiceMock.Setup(ts => ts.Get(It.IsAny<int>())).Returns(team);
            _teamServiceMock.Setup(ts => ts.GetTeamCaptain(It.IsAny<Team>())).Returns(captain);
            _teamServiceMock.Setup(ts => ts.GetTeamRoster(It.IsAny<int>())).Returns(roster.ToList());

            var expected = CreateViewModel();

            var sut = _kernel.Get<TeamsController>();

            // Act
            var actual = TestExtensions.GetModel<TeamViewModel>(sut.Details(SPECIFIED_TEAM_ID));

            // Assert
            TestHelper.AreEqual<TeamViewModel>(expected, actual, new TeamViewModelComparer());
        }

        /// <summary>
        /// Details action test. Team with such id does not exist.
        /// </summary>
        [TestMethod]
        public void Details_TeamDoesNotExist_NotFoundResualt()
        {
            // Arrange
            Team team = null;
            _teamServiceMock.Setup(ts => ts.Get(It.IsAny<int>())).Returns(team);
            var sut = this._kernel.Get<TeamsController>();
            var expected = (int)HttpStatusCode.NotFound;

            // Act
            var actual = (sut.Details(SPECIFIED_TEAM_ID) as HttpNotFoundResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private Team CreatestTeam()
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

        private PlayerNameViewModel CreatePlayerNameModel(string firstname, string last, int id)
        {
            return new PlayerNameViewModel()
            {
                FullName = string.Format("{0} {0}", firstname, firstname),
                Id = id
            };
        }
        
        private List<PlayerNameViewModel> CreateRoster()
        {
            var roster = new PlayerServiceTestFixture().TestPlayers().Build().Select(p => PlayerNameViewModel.Map(p)).ToList();
            roster.Add(CreatePlayerNameModel(PLAYER_FIRSTNAME, PLAYER_LASTNAME, SPECIFIED_PLAYER_ID));
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
    }
}
