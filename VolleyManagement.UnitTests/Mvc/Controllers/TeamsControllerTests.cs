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
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Contracts.Authorization;
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
        private const int TEAM_ID = 1;
        private const int SPECIFIED_PLAYER_ID = 4;
        private const int PHOTO_ID = 1;
        private const string FILE_DIR = "/Content/Photo/Teams/";
        private const string SPECIFIED_PLAYER_NAME = "Test name";
        private const string SPECIFIED_EXCEPTION_MESSAGE = "Test exception message";
        private const string ACHIEVEMENTS = "TestAchievements";
        private const string TEAM_NAME = "TestName";
        private const string PLAYER_FIRSTNAME = "Test";
        private const string PLAYER_LASTNAME = "Test";
        private const string COACH = "TestCoach";
        private const int TEST_TEAM_ID = 1;
        private const string ASSERT_FAIL_JSON_RESULT_MESSAGE = "Json result must be returned to user.";
        private const string FILE_NOT_FOUND_EX_MESSAGE = "File not found";
        private const string FILE_LOAD_EX_MESSAGE = "File size must be less then 1 MB and greater then 0 MB";

        private readonly Mock<ITeamService> _teamServiceMock = new Mock<ITeamService>();
        private readonly Mock<HttpContextBase> _httpContextMock = new Mock<HttpContextBase>();
        private readonly Mock<HttpRequestBase> _httpRequestMock = new Mock<HttpRequestBase>();
        private readonly Mock<HttpPostedFileBase> _httpPostedFileBaseMock = new Mock<HttpPostedFileBase>();
        private readonly Mock<IAuthorizationService> _authServiceMock = new Mock<IAuthorizationService>();
        private readonly Mock<IFileService> _fileServiceMock = new Mock<IFileService>();

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
            this._kernel.Bind<IAuthorizationService>().ToConstant(this._authServiceMock.Object);
            this._kernel.Bind<HttpContextBase>().ToConstant(this._httpContextMock.Object);
            this._kernel.Bind<HttpPostedFileBase>().ToConstant(this._httpPostedFileBaseMock.Object);
            this._kernel.Bind<IFileService>().ToConstant(this._fileServiceMock.Object);
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
        /// Delete method test. Model state is not valid.
        /// The method should return message as JavaScript Object Notation.
        /// </summary>
        [TestMethod]
        public void Delete_TeamNotValid_JsonReturned()
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
        /// Create method test. Team is not valid. Argument exception is thrown.
        /// Json is Returned
        /// </summary>
        [TestMethod]
        public void Create_TeamNotValid_ArgumentExceptionThrown()
        {
            // Arrange
            var viewModel = new TeamMvcViewModelBuilder().Build();
            _teamServiceMock.Setup(ts => ts.Create(It.IsAny<Team>()))
                                           .Throws(new ArgumentException(SPECIFIED_EXCEPTION_MESSAGE));

            // Act
            var sut = _kernel.Get<TeamsController>();
            sut.Create(viewModel);
            var modelState = sut.ModelState[string.Empty];

            // Assert
            _teamServiceMock.Verify(ts => ts.Create(It.IsAny<Team>()), Times.Once());
            Assert.AreEqual(modelState.Errors.Count, 1);
            Assert.AreEqual(modelState.Errors[0].ErrorMessage, SPECIFIED_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// Create method test. Model state is invalid.
        /// Json is Returned
        /// </summary>
        [TestMethod]
        public void Create_InValidTeamViewModel_JsonReturned()
        {
            // Arrange
            var viewModel = new TeamMvcViewModelBuilder().Build();
            var sut = this._kernel.Get<TeamsController>();
            sut.ModelState.AddModelError(string.Empty, string.Empty);

            // Act
            var result = sut.Create(viewModel) as JsonResult;

            // Assert
            _teamServiceMock.Verify(ts => ts.Create(It.IsAny<Team>()), Times.Never());
            Assert.IsNotNull(result, ASSERT_FAIL_JSON_RESULT_MESSAGE);
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
            var modelState = sut.ModelState[string.Empty];

            // Assert
            _teamServiceMock.Verify(ts => ts.Create(It.IsAny<Team>()), Times.Once());
            _teamServiceMock.Verify(ts => ts.UpdateRosterTeamId(It.IsAny<List<Player>>(), It.IsAny<int>()), Times.Never());
            Assert.AreEqual(modelState.Errors.Count, 1);
            Assert.AreEqual(modelState.Errors[0].ErrorMessage, SPECIFIED_EXCEPTION_MESSAGE);
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
            var modelState = sut.ModelState[string.Empty];

            // Assert
            _teamServiceMock.Verify(ts => ts.Create(It.IsAny<Team>()), Times.Once());
            _teamServiceMock.Verify(ts => ts.UpdateRosterTeamId(It.IsAny<List<Player>>(), It.IsAny<int>()), Times.Never());
            Assert.AreEqual(modelState.Errors.Count, 1);
            Assert.AreEqual(modelState.Errors[0].ErrorMessage, SPECIFIED_EXCEPTION_MESSAGE);
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
            _teamServiceMock.Setup(ts => ts.UpdateRosterTeamId(It.IsAny<List<Player>>(), It.IsAny<int>()))
                                           .Throws(new MissingEntityException(SPECIFIED_EXCEPTION_MESSAGE));

            // Act
            var sut = _kernel.Get<TeamsController>();
            sut.Create(viewModel);
            var modelState = sut.ModelState[string.Empty];

            // Assert
            _teamServiceMock.Verify(ts => ts.UpdateRosterTeamId(It.IsAny<List<Player>>(), It.IsAny<int>()), Times.Once());
            Assert.AreEqual(modelState.Errors.Count, 1);
            Assert.AreEqual(modelState.Errors[0].ErrorMessage, SPECIFIED_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// Create method test. Captain of another team
        /// </summary>
        [TestMethod]
        public void Create_RosterPlayerIsCaptainOfAnotherTeam_ValidationExceptionThrown()
        {
            // Arrange
            var viewModel = new TeamMvcViewModelBuilder().Build();
            _teamServiceMock.Setup(ts => ts.UpdateRosterTeamId(It.IsAny<List<Player>>(), It.IsAny<int>()))
                                           .Throws(new ValidationException(SPECIFIED_EXCEPTION_MESSAGE));

            // Act
            var sut = _kernel.Get<TeamsController>();
            sut.Create(viewModel);
            var modelState = sut.ModelState[string.Empty];

            // Assert
            _teamServiceMock.Verify(ts => ts.UpdateRosterTeamId(It.IsAny<List<Player>>(), It.IsAny<int>()), Times.Once());
            Assert.AreEqual(modelState.Errors.Count, 1);
            Assert.AreEqual(modelState.Errors[0].ErrorMessage, SPECIFIED_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// Create method test. Roster players updated
        /// </summary>
        [TestMethod]
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
            _teamServiceMock.Setup(ts => ts.GetTeamRoster(It.IsAny<int>())).Returns(rosterDomain.ToList());

            var rosterPlayer = new PlayerNameViewModel() { Id = SPECIFIED_PLAYER_ID, FullName = SPECIFIED_PLAYER_NAME };
            var roster = new List<PlayerNameViewModel>() { rosterPlayer };
            var viewModel = new TeamMvcViewModelBuilder().WithRoster(roster).Build();

            _teamServiceMock.Setup(ts => ts.Create(It.IsAny<Team>()))
                                           .Callback<Team>(t => t.Id = SPECIFIED_TEAM_ID);

            // Act
            var sut = _kernel.Get<TeamsController>();
            sut.Create(viewModel);

            // Assert
            _teamServiceMock.Verify(
                             ts => ts.UpdateRosterTeamId(It.IsAny<List<Player>>(), It.IsAny<int>()),
                             Times.Once());
        }

        /// <summary>
        /// Edit method test. Positive test
        /// </summary>
        [TestMethod]
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
            _teamServiceMock.Setup(ts => ts.GetTeamRoster(It.IsAny<int>())).Returns(roster.ToList());

            var viewModel = CreateViewModel();
            var expectedDomain = viewModel.ToDomain();
            var comparer = new TeamComparer();
            _teamServiceMock.Setup(ts => ts.Create(It.Is<Team>(t => comparer.AreEqual(t, expectedDomain))))
                                           .Callback<Team>(t => t.Id = SPECIFIED_TEAM_ID);

            // Act
            var sut = this._kernel.Get<TeamsController>();
            sut.Edit(viewModel);

            // Assert
            Assert.AreEqual(viewModel.Id, SPECIFIED_TEAM_ID);
        }

        /// <summary>
        /// Edit method test. Team is not valid. Argument exception is thrown.
        /// Json is Returned
        /// </summary>
        [TestMethod]
        public void Edit_TeamNotValid_ArgumentExceptionThrown()
        {
            // Arrange
            var viewModel = new TeamMvcViewModelBuilder().Build();
            _teamServiceMock.Setup(ts => ts.Edit(It.IsAny<Team>()))
                                           .Throws(new ArgumentException(SPECIFIED_EXCEPTION_MESSAGE));

            // Act
            var sut = _kernel.Get<TeamsController>();
            sut.Edit(viewModel);
            var modelState = sut.ModelState[string.Empty];

            // Assert
            _teamServiceMock.Verify(ts => ts.Edit(It.IsAny<Team>()), Times.Once());
            Assert.AreEqual(modelState.Errors.Count, 1);
            Assert.AreEqual(modelState.Errors[0].ErrorMessage, SPECIFIED_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// Edit method test. Model state is invalid.
        /// Json is Returned
        /// </summary>
        [TestMethod]
        public void Edit_InValidTeamViewModel_JsonReturned()
        {
            // Arrange
            var viewModel = new TeamMvcViewModelBuilder().Build();
            var sut = this._kernel.Get<TeamsController>();
            sut.ModelState.AddModelError(string.Empty, string.Empty);

            // Act
            var result = sut.Edit(viewModel) as JsonResult;

            // Assert
            _teamServiceMock.Verify(ts => ts.Edit(It.IsAny<Team>()), Times.Never());
            Assert.IsNotNull(result, ASSERT_FAIL_JSON_RESULT_MESSAGE);
        }

        /// <summary>
        /// Edit method test. Invalid captain Id
        /// </summary>
        [TestMethod]
        public void Edit_InvalidCaptainId_MissingEntityExceptionThrown()
        {
            // Arrange
            var viewModel = new TeamMvcViewModelBuilder().Build();
            _teamServiceMock.Setup(ts => ts.Edit(It.IsAny<Team>()))
                                           .Throws(new MissingEntityException(SPECIFIED_EXCEPTION_MESSAGE));

            // Act
            var sut = _kernel.Get<TeamsController>();
            sut.Edit(viewModel);
            var modelState = sut.ModelState[string.Empty];

            // Assert
            _teamServiceMock.Verify(ts => ts.Edit(It.IsAny<Team>()), Times.Once());
            _teamServiceMock.Verify(ts => ts.UpdateRosterTeamId(It.IsAny<List<Player>>(), It.IsAny<int>()), Times.Never());
            Assert.AreEqual(modelState.Errors.Count, 1);
            Assert.AreEqual(modelState.Errors[0].ErrorMessage, SPECIFIED_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// Edit method test. Captain of another team
        /// </summary>
        [TestMethod]
        public void Edit_CaptainOfAnotherTeam_ValidationExceptionThrown()
        {
            // Arrange
            var viewModel = new TeamMvcViewModelBuilder().Build();
            _teamServiceMock.Setup(ts => ts.Edit(It.IsAny<Team>()))
                                           .Throws(new ValidationException(SPECIFIED_EXCEPTION_MESSAGE));

            // Act
            var sut = _kernel.Get<TeamsController>();
            sut.Edit(viewModel);
            var modelState = sut.ModelState[string.Empty];

            // Assert
            _teamServiceMock.Verify(ts => ts.Edit(It.IsAny<Team>()), Times.Once());
            _teamServiceMock.Verify(ts => ts.UpdateRosterTeamId(It.IsAny<List<Player>>(), It.IsAny<int>()), Times.Never());
            Assert.AreEqual(modelState.Errors.Count, 1);
            Assert.AreEqual(modelState.Errors[0].ErrorMessage, SPECIFIED_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// Edit method test. Invalid player Id
        /// </summary>
        [TestMethod]
        public void Edit_InvalidRosterPlayerId_MissingEntityExceptionThrown()
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

            var viewModel = CreateViewModel();
            var comparer = new TeamComparer();
            _teamServiceMock.Setup(ts => ts.UpdateRosterTeamId(It.IsAny<List<Player>>(), It.IsAny<int>()))
                                           .Throws(new MissingEntityException(SPECIFIED_EXCEPTION_MESSAGE));

            // Act
            var sut = _kernel.Get<TeamsController>();
            sut.Edit(viewModel);
            var modelState = sut.ModelState[string.Empty];

            // Assert
            _teamServiceMock.Verify(ts => ts.UpdateRosterTeamId(It.IsAny<List<Player>>(), It.IsAny<int>()), Times.Once());
            Assert.AreEqual(modelState.Errors.Count, 1);
            Assert.AreEqual(modelState.Errors[0].ErrorMessage, SPECIFIED_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// Edit method test. Captain of another team
        /// </summary>
        [TestMethod]
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
            _teamServiceMock.Setup(ts => ts.GetTeamRoster(It.IsAny<int>())).Returns(roster.ToList());

            var viewModel = CreateViewModel();
            _teamServiceMock.Setup(ts => ts.UpdateRosterTeamId(It.IsAny<List<Player>>(), It.IsAny<int>()))
                                           .Throws(new ValidationException(SPECIFIED_EXCEPTION_MESSAGE));

            // Act
            var sut = _kernel.Get<TeamsController>();
            sut.Edit(viewModel);
            var modelState = sut.ModelState[string.Empty];

            // Assert
            _teamServiceMock.Verify(ts => ts.UpdateRosterTeamId(It.IsAny<List<Player>>(), It.IsAny<int>()), Times.Once());
            Assert.AreEqual(modelState.Errors.Count, 1);
            Assert.AreEqual(modelState.Errors[0].ErrorMessage, SPECIFIED_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// Edit method test. Roster players updated
        /// </summary>
        [TestMethod]
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
            _teamServiceMock.Setup(ts => ts.GetTeamRoster(It.IsAny<int>())).Returns(rosterDomain.ToList());

            var rosterPlayer = new PlayerNameViewModel() { Id = SPECIFIED_PLAYER_ID, FullName = SPECIFIED_PLAYER_NAME };
            var roster = new List<PlayerNameViewModel>() { rosterPlayer };
            var viewModel = new TeamMvcViewModelBuilder().WithRoster(roster).Build();

            _teamServiceMock.Setup(ts => ts.Edit(It.IsAny<Team>()))
                                           .Callback<Team>(t => t.Id = SPECIFIED_TEAM_ID);

            // Act
            var sut = _kernel.Get<TeamsController>();
            sut.Edit(viewModel);

            // Assert
            _teamServiceMock.Verify(
                             ts => ts.UpdateRosterTeamId(It.IsAny<List<Player>>(), It.IsAny<int>()),
                             Times.Once());
        }

        /// <summary>
        /// Test for Details method. Team with specified identifier does not exist. HttpNotFoundResult is returned.
        /// </summary>
        [TestMethod]
        public void Details_NonExistentTeam_HttpNotFoundResultIsReturned()
        {
            // Arrange
            SetupGet(TEST_TEAM_ID, null);

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
            SetupRequestRawUrl("/Teams");
            SetupControllerContext();

            var expected = CreateViewModel();

            // Act
            var actual = TestExtensions.GetModel<TeamRefererViewModel>(this._sut.Details(SPECIFIED_TEAM_ID));

            // Assert
            TestHelper.AreEqual<TeamViewModel>(expected, actual.Model, new TeamViewModelComparer());
            Assert.AreEqual(actual.CurrentReferrer, this._sut.Request.RawUrl);
        }

        /// <summary>
        /// Test for Edit method (GET action). Valid team id.  Team view model is returned.
        /// </summary>
        [TestMethod]
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
            _teamServiceMock.Setup(ts => ts.GetTeamRoster(It.IsAny<int>())).Returns(roster.ToList());
            SetupControllerContext();

            var expected = CreateViewModel();

            // Act
            var actual = TestExtensions.GetModel<TeamViewModel>(this._sut.Edit(SPECIFIED_TEAM_ID));

            // Assert
            TestHelper.AreEqual<TeamViewModel>(expected, actual, new TeamViewModelComparer());
        }

        /// <summary>
        /// Test for Edit method. Team with specified identifier does not exist. HttpNotFoundResult is returned.
        /// </summary>
        [TestMethod]
        public void Edit_NotExistedTeam_HttpNotFoundResultIsReturned()
        {
            // Arrange
            SetupGet(TEAM_ID, null as Team);

            // Act
            var controller = _kernel.Get<TeamsController>();
            var result = controller.Edit(TEAM_ID);

            // Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        /// <summary>
        /// Test for GetAllTeams method. All teams are requested. JsonResult with all teams is returned.
        /// </summary>
        [TestMethod]
        public void GetAllTeams_GetTeams_JsonResultIsReturned()
        {
            // Arrange
            var testData = MakeTestTeams();
            SetupGetAllTeams(testData);

            // Act
            var result = this._sut.GetAllTeams();

            // Assert
            Assert.IsNotNull(result, ASSERT_FAIL_JSON_RESULT_MESSAGE);
        }

        [TestMethod]
        public void AddPhoto_PhotoAdded_RequestRedirectToEdit()
        {
            // Arrange
            SetupHttpPostedFileBaseMock();

            // Act
            var actionResult = _sut.AddPhoto(_httpPostedFileBaseMock.Object, PHOTO_ID) as RedirectToRouteResult;

            // Assert
            VerifyFileServiceUpload(Times.Once());
            VerifyRedirect("Edit", actionResult);
        }

        [TestMethod]
        [ExpectedException(typeof(FileLoadException))]
        public void AddPhoto_InvalidFile_FileLoadExceptionThrown()
        {
            // Arrange
            SetupHttpPostedFileBaseMock();
            SetupFileServiceMockThrowsFileLoadException(_httpPostedFileBaseMock.Object);

            // Act
            _sut.AddPhoto(_httpPostedFileBaseMock.Object, PHOTO_ID);
        }

        [TestMethod]
        public void DeletePhoto_PhotoDeleted_RequestRedirectToEdit()
        {
            // Arrange
            SetupFileServiceMock();

            // Act
            var actionResult = _sut.DeletePhoto(PHOTO_ID) as RedirectToRouteResult;

            // Assert
            VerifyFileServiceDelete(Times.Once());
            VerifyRedirect("Edit", actionResult);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void DeletePhoto_InvalidPathToFile_FileNotFoundExceptionThrown()
        {
            // Arrange
            SetupFileServiceMockThrowsFileNotFoundException();

            // Act
            _sut.DeletePhoto(PHOTO_ID);
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

        private List<TeamViewModel> MakeTestTeamViewModels(List<Team> teams)
        {
            return teams.Select(ct => new TeamMvcViewModelBuilder()
                .WithId(ct.Id)
                .WithName(ct.Name)
                .WithAchievements(ct.Achievements)
                .WithCoach(ct.Coach)
                .WithRoster(CreateRoster())
                .WithCaptain(CreatePlayerNameModel(PLAYER_FIRSTNAME, PLAYER_LASTNAME, SPECIFIED_PLAYER_ID))
                .Build())
                .ToList();
        }

        private void VerifyRedirect(string actionName, RedirectToRouteResult result)
        {
            Assert.AreEqual(actionName, result.RouteValues["action"]);
        }

        private void VerifyFileServiceUpload(Times times)
        {
            _fileServiceMock.Verify(ts => ts.Upload(PHOTO_ID, _httpPostedFileBaseMock.Object, FILE_DIR), times);
        }

        private void VerifyFileServiceDelete(Times times)
        {
            _fileServiceMock.Verify(ts => ts.Delete(PHOTO_ID, FILE_DIR), times);
        }

        private void MockTeamServiceGetTeam(Team team)
        {
            _teamServiceMock.Setup(ts => ts.Get(It.IsAny<int>())).Returns(team);
        }

        private void SetupGet(int teamId, Team team)
        {
            this._teamServiceMock.Setup(tr => tr.Get(teamId)).Returns(team);
        }

        private void SetupControllerContext()
        {
            this._sut.ControllerContext = new ControllerContext(this._httpContextMock.Object, new RouteData(), this._sut);
        }

        private void SetupGetAllTeams(List<Team> teams)
        {
            this._teamServiceMock.Setup(tr => tr.Get()).Returns(teams);
        }

        private void SetupRequestRawUrl(string rawUrl)
        {
            this._httpRequestMock.Setup(x => x.RawUrl).Returns(rawUrl);
        }

        private void SetupHttpPostedFileBaseMock()
        {
            this._httpPostedFileBaseMock.Setup(x => x.FileName).Returns("1.jpg");
        }

        private void SetupFileServiceMockThrowsFileLoadException(HttpPostedFileBase file)
        {
            _fileServiceMock.Setup(x => x.Upload(PHOTO_ID, file, FILE_DIR))
                .Throws(new FileLoadException(FILE_LOAD_EX_MESSAGE));
        }

        private void SetupFileServiceMockThrowsFileNotFoundException()
        {
            _fileServiceMock.Setup(x => x.Delete(PHOTO_ID, FILE_DIR))
                .Throws(new FileNotFoundException(FILE_NOT_FOUND_EX_MESSAGE));
        }

        private void SetupFileServiceMock()
        {
            _fileServiceMock.Setup(x => x.Delete(PHOTO_ID, FILE_DIR));
        }

        private List<Team> MakeTestTeams()
        {
            return new TeamServiceTestFixture().TestTeams().Build();
        }
    }
}
