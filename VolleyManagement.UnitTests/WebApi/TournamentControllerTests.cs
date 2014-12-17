namespace VolleyManagement.UnitTests.WebApi
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Hosting;
    using Contracts;
    using Domain.Tournaments;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using Services.TournamentService;
    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.WebApi.Controllers;
    using VolleyManagement.WebApi.Mappers;
    using VolleyManagement.WebApi.ViewModels.Tournaments;

    /// <summary>
    /// Tests for TournamentController class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TournamentControllerTests
    {
        /// <summary>
        /// Test Fixture
        /// </summary>
        private readonly TournamentServiceTestFixture _testFixture = new TournamentServiceTestFixture();

        /// <summary>
        /// Tournaments Service Mock
        /// </summary>
        private readonly Mock<ITournamentService> _tournamentServiceMock = new Mock<ITournamentService>();

        /// <summary>
        /// Tournaments Repository Mock
        /// </summary>
        private readonly Mock<ITournamentRepository> _tournamentRepositoryMock = new Mock<ITournamentRepository>();

        /// <summary>
        /// IoC for tests
        /// </summary>
        private IKernel _kernel;

        /// <summary>
        /// Initializes test data
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            this._kernel = new StandardKernel();
            this._kernel.Bind<ITournamentService>()
                   .ToConstant(this._tournamentServiceMock.Object);
        }

        /// <summary>
        /// Test for Get() by key method. The method should return specific tournament
        /// </summary>
        [TestMethod]
        public void Get_SpecificTournamentExist_TournamentReturned()
        {
            // Arrange
            var tournament = new TournamentBuilder().WithId(5).Build();
            MockSingleTournament(tournament);
            var tournamentsController = _kernel.Get<TournamentsController>();
            GetRequestForController(tournamentsController);

            // Act
            var response = tournamentsController.Get(tournament.Id);
            var result = GetTournamentViewModelFromResponse(response);

            // Assert
            Assert.AreEqual(tournament.Id, result.Id);
        }

        /// <summary>
        /// Test for Get() method. The method should return existing tournaments
        /// </summary>
        [TestMethod]
        public void Get_TournamentsExist_TournamentsReturned()
        {
            // Arrange
            var testData = this._testFixture.TestTournaments()
                                       .Build();
            this.MockTournaments(testData);

            // sut - stands for System Under Test
            var sut = this._kernel.Get<TournamentsController>();

            // Expected result
            var expected = new TournamentServiceTestFixture()
                                            .TestTournaments()
                                            .Build()
                                            .ToList();

            // Actual result
            var actual = sut.Get().ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual, new TournamentsViewModelComparer());
        }

        /// <summary>
        /// Test Post method.
        /// </summary>
        [TestMethod]
        public void Post_NewTournament_CreateMethodInvoked()
        {
            // Arrange
            _tournamentServiceMock.Setup(ts => ts.Create(It.IsAny<Tournament>())).Verifiable();
            var tournament = new TournamentBuilder().WithId(1).Build();
            var tournamentService = _tournamentServiceMock.Object;

            // Act
            tournamentService.Create(tournament);

            // Assert
            _tournamentServiceMock.Verify();
        }

        /// <summary>
        /// Test for Delete() method
        /// </summary>
        [TestMethod]
        public void Delete_TournamentExist_TournamentDeleted()
        {
            // Arrange
            var testTournaments = this._testFixture.TestTournaments()
                          .Build();
            var tournamentToDeleteID = testTournaments.Last().Id;
            var controller = this._kernel.Get<TournamentsController>();
            GetRequestForController(controller);

            // Act
            var response = controller.Delete(tournamentToDeleteID);

            // Assert
            Assert.AreEqual(HttpStatusCode.Accepted, response.StatusCode);
        }

        /// <summary>
        /// Gets request message for controller
        /// </summary>
        /// <param name="controller">Current controller</param>
        public void GetRequestForController(TournamentsController controller)
        {
            controller.Request = new HttpRequestMessage();
            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
        }

        /// <summary>
        /// Mocks test data
        /// </summary>
        /// <param name="testData">Data to mock</param>
        private void MockTournaments(IEnumerable<Tournament> testData)
        {
            _tournamentServiceMock.Setup(tr => tr.GetAll()).Returns(testData.AsQueryable());
        }

        /// <summary>
        /// Mocks test data
        /// </summary>
        /// <param name="testData">Data to mock</param>
        private void MockSingleTournament(Tournament testData)
        {
            _tournamentServiceMock.Setup(tr => tr.FindById(testData.Id)).Returns(testData);
        }

        /// <summary>
        /// Gets tournament view model from response content
        /// </summary>
        /// <param name="response">Http response message</param>
        /// <returns>Tournament view model</returns>
        private TournamentViewModel GetTournamentViewModelFromResponse(HttpResponseMessage response)
        {
            ObjectContent content = response.Content as ObjectContent;
            return content.Value as TournamentViewModel;
        }
    }
}
