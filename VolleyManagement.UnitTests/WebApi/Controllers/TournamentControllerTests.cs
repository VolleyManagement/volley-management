namespace VolleyManagement.UnitTests.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using Contracts;
    using Domain.Tournaments;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using Services.TournamentService;

    using VolleyManagement.UI.Areas.WebApi.ApiControllers;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Tournaments;
    using VolleyManagement.UnitTests.WebApi.ViewModels;

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
        /// IoC for tests
        /// </summary>
        private IKernel _kernel;

        /// <summary>
        /// Initializes test data
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<ITournamentService>()
                   .ToConstant(_tournamentServiceMock.Object);
        }

        /// <summary>
        /// Test for Get() by key method. The method should return specific tournament
        /// </summary>
        [TestMethod]
        [Ignore]// BUG: FIX ASAP
        public void Get_SpecificTournamentExist_TournamentReturned()
        {
            // Arrange
            var tournament = new TournamentBuilder().WithId(5).Build();
            MockSingleTournament(tournament);
            var tournamentsController = _kernel.Get<TournamentsController>();

            // Act
            var response = tournamentsController.GetTournaments();
            //var result = TestExtensions.GetModelFromResponse<TournamentViewModel>(response);

            // Assert
            //Assert.AreEqual(tournament.Id, result.Id);
        }

        /// <summary>
        /// Test for Get() by key method. The method should return specific tournament
        /// </summary>
        [TestMethod]
        [Ignore]// BUG: FIX ASAP
        public void Get_GeneralException_ExceptionThrown()
        {
            // Arrange
            var tournamentId = 5;
            _tournamentServiceMock.Setup(ts => ts.Get(tournamentId))
               .Throws(new ArgumentNullException());
            var tournamentsController = _kernel.Get<TournamentsController>();
            // var expected = HttpStatusCode.NotFound;

            // Act
            var actual = tournamentsController.GetTournament(tournamentId);

            // Assert
            // Assert.AreEqual(expected, actual.StatusCode);
        }

        /// <summary>
        /// Test for Get() method. The method should return existing tournaments
        /// </summary>
        [TestMethod]
        [Ignore]// BUG: FIX ASAP
        public void Get_TournamentsExist_TournamentsReturned()
        {
            // Arrange
            //var testData = _testFixture.TestTournaments()
            //                           .Build();
            //MockTournaments(testData);

            //var sut = _kernel.Get<TournamentsController>();

            //// Expected result
            //var domainTournaments = new TournamentServiceTestFixture()
            //                                .TestTournaments()
            //                                .Build()
            //                                .ToList();
            //var expected = new List<TournamentViewModel>();
            //foreach (var item in domainTournaments)
            //{
            //    expected.Add(DomainToViewModel.Map(item));
            //}

            //// Actual result
            //var actual = sut.Get().ToList();

            //// Assert
            //CollectionAssert.AreEqual(expected, actual, new TournamentViewModelComparer());
        }

        /// <summary>
        /// Test Post method. Basic story.
        /// </summary>
        [TestMethod]
        [Ignore]// BUG: FIX ASAP
        public void Post_ValidViewModel_TournamentCreated()
        {
            // Arrange
            //var controller = _kernel.Get<TournamentsController>();
            //TestExtensions.SetControllerRequest(controller);
            //var expected = new TournamentViewModelBuilder().Build();

            //// Act
            //var response = controller.Post(expected);
            //var actual = TestExtensions.GetModelFromResponse<TournamentViewModel>(response);

            //// Assert
            //_tournamentServiceMock.Verify(ts => ts.Create(It.IsAny<Tournament>()), Times.Once());
            //Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            //AssertExtensions.AreEqual<TournamentViewModel>(expected, actual, new TournamentViewModelComparer());
        }

        /// <summary>
        /// Test for Delete() method
        /// </summary>
        [TestMethod]
        [Ignore]// BUG: FIX ASAP
        public void Delete_TournamentExist_TournamentDeleted()
        {
            //// Arrange
            //var testTournaments = _testFixture.TestTournaments()
            //              .Build();
            //var tournamentToDeleteID = testTournaments.Last().Id;
            //var controller = _kernel.Get<TournamentsController>();
            //TestExtensions.SetControllerRequest(controller);

            //// Act
            //var response = controller.Delete(tournamentToDeleteID);

            //// Assert
            //Assert.AreEqual(HttpStatusCode.Accepted, response.StatusCode);
        }

        /// <summary>
        /// Test Put method. Basic story.
        /// </summary>
        [TestMethod]
        [Ignore]// BUG: FIX ASAP
        public void Put_ValidViewModel_TournamentUpdated()
        {
        //{
        //    // Arrange
        //    var controller = _kernel.Get<TournamentsController>();
        //    TestExtensions.SetControllerRequest(controller);
        //    var expected = new TournamentViewModelBuilder().Build();

        //    // Act
        //    var actual = controller.Put(expected.Id, expected);

        //    // Assert
        //    _tournamentServiceMock.Verify(us => us.Edit(It.IsAny<Tournament>()), Times.Once());
        //    Assert.AreEqual(HttpStatusCode.OK, actual.StatusCode);
        }

        /// <summary>
        /// Test Put method. Invalid data
        /// </summary>
        [TestMethod]
        [Ignore]// BUG: FIX ASAP
        public void Put_InvalidData_BadRequestReturned()
        {
            // Arrange
            //var controller = _kernel.Get<TournamentsController>();
            //TestExtensions.SetControllerRequest(controller);
            //var expected = new TournamentViewModelBuilder().Build();
            //var invalidKey = expected.Id + 1;

            //// Act
            //var actual = controller.Put(invalidKey, expected);

            //// Assert
            //_tournamentServiceMock.Verify(us => us.Edit(It.IsAny<Tournament>()), Times.Never());
            //Assert.AreEqual(HttpStatusCode.BadRequest, actual.StatusCode);
        }

        /// <summary>
        /// Test for Put method. The method should return "Bad request" status
        /// </summary>
        [TestMethod]
        [Ignore]// BUG: FIX ASAP
        public void Put_ArgumentException_BadRequestReturned()
        {
            // Arrange
            //var expected = new TournamentViewModelBuilder().Build();
            //_tournamentServiceMock.Setup(us => us.Edit(It.IsAny<Tournament>()))
            //   .Throws(new ArgumentException());
            //var controller = _kernel.Get<TournamentsController>();
            //TestExtensions.SetControllerRequest(controller);

            //// Act
            //var actual = controller.Put(expected.Id, expected);

            //// Assert
            //Assert.AreEqual(HttpStatusCode.BadRequest, actual.StatusCode);
        }

        /// <summary>
        /// Test for Put method. The method should return "Internal server error" status
        /// </summary>
        [TestMethod]
        [Ignore]// BUG: FIX ASAP
        public void Put_GeneralException_InternalServerErrorReturned()
        {
            //// Arrange
            //var expected = new TournamentViewModelBuilder().Build();
            //_tournamentServiceMock.Setup(us => us.Edit(It.IsAny<Tournament>()))
            //   .Throws(new Exception());
            //var controller = _kernel.Get<TournamentsController>();
            //TestExtensions.SetControllerRequest(controller);

            //// Act
            //var actual = controller.Put(expected.Id, expected);

            //// Assert
            //Assert.AreEqual(HttpStatusCode.InternalServerError, actual.StatusCode);
        }

        /// <summary>
        /// Mocks test data
        /// </summary>
        /// <param name="testData">Data to mock</param>
        private void MockTournaments(IEnumerable<Tournament> testData)
        {
            //_tournamentServiceMock.Setup(tr => tr.GetAll()).Returns(testData.AsQueryable());
        }

        /// <summary>
        /// Mocks test data
        /// </summary>
        /// <param name="testData">Data to mock</param>
        private void MockSingleTournament(Tournament testData)
        {
            //_tournamentServiceMock.Setup(tr => tr.FindById(testData.Id)).Returns(testData);
        }
    }
}
