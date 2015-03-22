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
    using System.Web.Http.OData.Results;

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

            // Arrange
            int countOfReturnedId = 1;
            var controller = _kernel.Get<TournamentsController>();
            _tournamentServiceMock.Setup(ts => ts.Edit(It.IsAny<Tournament>()));
            _tournamentServiceMock.Setup(ts => ts.Get().Count()).Returns(countOfReturnedId);  // need I add smth to Count params? 
            var expected = new TournamentViewModelBuilder().Build();

            // Act
            var input = new TournamentViewModelBuilder().Build();
            var actual = ((UpdatedODataResult<TournamentViewModel>)controller.Put(input.Id, input)).Entity;

            // Assert
            _tournamentServiceMock.Verify(ts => ts.Edit(It.IsAny<Tournament>()), Times.Once());
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test Put method. Invalid id
        /// </summary>
        [TestMethod]
        public void Put_InvalidId_BadRequestReturned()
        {
            // Arrange
            int countOfReturnedId = 0;
            var controller = _kernel.Get<TournamentsController>();
            _tournamentServiceMock.Setup(ts => ts.Get().Count()).Returns(countOfReturnedId);  // need I add smth to Count params? 
            
            //// Act
            var input = new TournamentViewModelBuilder().Build();
            var actual = controller.Put(input.Id, input);

            //// Assert
            _tournamentServiceMock.Verify(ts => ts.Edit(It.IsAny<Tournament>()), Times.Never());
            Assert.IsTrue(actual is System.Web.Http.Results.InvalidModelStateResult);
            // Assert.AreEqual(HttpStatusCode.BadRequest, actual.StatusCode);
        }

        /// <summary>
        /// Test Put method. Invalid id
        /// </summary>
        [TestMethod]
        public void Put_InvalidModelState_BadRequestReturned()
        {
            // Arrange
            var controller = _kernel.Get<TournamentsController>();
            string invalidParameter = null;

            //// Act
            var input = new TournamentViewModelBuilder().WithName(invalidParameter).Build();
            var actual = controller.Put(input.Id, input);

            //// Assert
            _tournamentServiceMock.Verify(ts => ts.Edit(It.IsAny<Tournament>()), Times.Never());
            Assert.IsTrue(actual is System.Web.Http.Results.InvalidModelStateResult);
        }

        /// <summary>
        /// Test for Put method. The method should return "Internal server error" status
        /// </summary>
        [TestMethod]
        public void Put_WithinEditOperationException_BadRequestReturned()
        {
            // Arrange
            var controller = _kernel.Get<TournamentsController>();
            _tournamentServiceMock.Setup(ts => ts.Edit(It.IsAny<Tournament>())).Throws(new Exception());

            //// Act
            var input = new TournamentViewModelBuilder().Build();
            var actual = controller.Put(input.Id, input);

            //// Assert
            _tournamentServiceMock.Verify(ts => ts.Edit(It.IsAny<Tournament>()), Times.AtLeastOnce());
            Assert.IsTrue(actual is System.Web.Http.Results.InvalidModelStateResult);
            // Assert.AreEqual(HttpStatusCode.BadRequest, actual.StatusCode);
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
