namespace VolleyManagement.UnitTests.WebApi.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Web.Http;
    using System.Web.Http.Results;
    using System.Web.OData;
    using System.Web.OData.Results;

    using Contracts;
    using Domain.Tournaments;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using Services.TournamentService;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Crosscutting.Contracts.Providers;
    using VolleyManagement.Domain;
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
        /// ID for tests
        /// </summary>
        private const int SPECIFIC_TOURNAMENT_ID = 2;

        /// <summary>
        /// Message that should be passed to exception.
        /// </summary>
        private const string EXCEPTION_MESSAGE = "Test exception message.";

        /// <summary>
        /// Key used for getting exception from ModelStateDictionary.
        /// </summary>
        private const string KEY_FOR_EXCEPTION_MESSAGE = "keyForErrorMessage";

        /// <summary>
        /// A new but not saved tournament id
        /// </summary>
        private const int UNASSIGNED_ID = 0;

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
        public void Get_SpecificTournamentExist_TournamentReturned()
        {
            // Arrange
            var testData = _testFixture.TestTournaments()
                                            .Build();
            MockTournaments(testData);
            var tournamentsController = _kernel.Get<TournamentsController>();

            // Act
            var domainTournaments = new TournamentViewModelServiceTestFixture()
                                            .TestTournaments()
                                            .Build()
                                            .AsQueryable();
            var expected = domainTournaments.Single(dt => dt.Id == SPECIFIC_TOURNAMENT_ID);
            var result = tournamentsController.Get(SPECIFIC_TOURNAMENT_ID).Queryable.Single();

            // Assert
            _tournamentServiceMock.Verify(ts => ts.Get(), Times.Once());
            AssertExtensions.AreEqual<TournamentViewModel>(expected, result, new TournamentViewModelComparer());
        }

        /// <summary>
        /// Test for Get() method. The method should return existing tournaments
        /// </summary>
        [TestMethod]
        public void Get_TournamentsExist_TournamentsReturned()
        {
            // Arrange
            var testData = _testFixture.TestTournaments()
                                            .Build();
            MockTournaments(testData);
            var sut = _kernel.Get<TournamentsController>();

            //// Expected result
            var expected = new TournamentViewModelServiceTestFixture()
                                            .TestTournaments()
                                            .Build()
                                            .ToList();

            //// Actual result
            var actual = sut.GetTournaments().ToList();

            //// Assert
            _tournamentServiceMock.Verify(ts => ts.Get(), Times.Once());
            CollectionAssert.AreEqual(expected, actual, new TournamentViewModelComparer());
        }

        /// <summary>
        /// Test Post method. Basic story.
        /// </summary>
        [TestMethod]
        public void Post_IdCreated_IdReturnedWithEntity()
        {
            // Arrange
            var controller = _kernel.Get<TournamentsController>();
            var expectedId = 10;
            _tournamentServiceMock.Setup(ts => ts.Create(It.IsAny<Tournament>()))
                .Callback((Tournament t) => { t.Id = expectedId; });

            // Act
            var input = new TournamentViewModelBuilder().WithId(0).Build();
            var response = controller.Post(input);
            var actual = ((CreatedODataResult<TournamentViewModel>)response).Entity;

            // Assert
            _tournamentServiceMock.Verify(ts => ts.Create(It.IsAny<Tournament>()), Times.AtLeastOnce());
            Assert.AreEqual<int>(expectedId, actual.Id);
        }

        /// <summary>
        /// Test Post method. Does a valid ViewModel return after Tournament has been created.
        /// </summary>
        [TestMethod]
        public void Post_ValidViewModelTournament_ReturnedAfterCreatedWebApi()
        {
            // Arrange
            var controller = _kernel.Get<TournamentsController>();
            var input = new TournamentViewModelBuilder().WithId(UNASSIGNED_ID).Build();

            _tournamentServiceMock.Setup(ts => ts.Create(It.IsAny<Tournament>()))
                .Callback<Tournament>(t => { t.Id = SPECIFIC_TOURNAMENT_ID; });

            var expected = new TournamentViewModelBuilder().WithId(SPECIFIC_TOURNAMENT_ID).Build();

            // Act
            var response = controller.Post(input);
            var actual = ((CreatedODataResult<TournamentViewModel>)response).Entity;

            // Assert
            AssertExtensions.AreEqual<TournamentViewModel>(expected, actual, new TournamentViewModelComparer());
        }

        /// <summary>
        /// Test Post method(). Returns InvalidModelStateResult
        /// if the ModelState has some errors
        /// </summary>
        [TestMethod]
        public void Post_NotValidTournametnViewModel_ReturnBadRequestWebApi()
        {
            // Arrange
            var controller = _kernel.Get<TournamentsController>();
            controller.ModelState.Clear();
            var notValidViewModel = new TournamentViewModelBuilder().WithSeason(300).Build();
            controller.ModelState.AddModelError("NotValidSeason", "Season field isn't valid");

            // Act
            var result = controller.Post(notValidViewModel) as InvalidModelStateResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ModelState.Count == 1);
            Assert.IsTrue(result.ModelState.Keys.Contains("NotValidSeason"));
        }

        /// <summary>
        /// Test Post method. Is valid tournament domain model
        /// pass to Create Service method
        /// </summary>
        [TestMethod]
        public void Post_ValidTournamentDomain_PassToCreateMethod()
        {
            // Arrange
            var controller = _kernel.Get<TournamentsController>();
            var sent = new TournamentViewModelBuilder().Build();
            var expected = new TournamentViewModelBuilder().Build();

            var expectedDomain = new TournamentBuilder()
                .WithId(expected.Id)
                .WithName(expected.Name)
                .WithSeason(expected.Season)
                .WithScheme(Enum.GetValues(typeof(TournamentSchemeEnum))
                .Cast<TournamentSchemeEnum>()
                .FirstOrDefault(v => v.ToDescription() == expected.Scheme))
                .WithRegulationsLink(expected.RegulationsLink)
                .WithDescription(expected.Description)
                .Build();

            // Act
            controller.Post(sent);

            // Assert
            _tournamentServiceMock.Verify(
                trServ => trServ.Create(It.Is<Tournament>(t => new TournamentComparer().IsEqual(t, expectedDomain))),
                Times.Once());
        }

        /// <summary>
        /// Test Post method. Does method catch ArgumentException
        /// and returns BadRequest with some info
        /// </summary>
        [TestMethod]
        public void Post_CatchArgumentExeption_ReturnBadRequest()
        {
            // Arrange
            var controller = _kernel.Get<TournamentsController>();

            // Act
            var result = controller.Post(new TournamentViewModelBuilder().WithScheme("5").Build())
                as InvalidModelStateResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ModelState.Count > 0);
        }

        /// <summary>
        /// Test Post method. Does method catch TournamentValidationException
        /// and returns BadRequest with some info
        /// </summary>
        [TestMethod]
        public void Post_CatchTournamentValidationException_ReturnBadRequest()
        {
            // Arrange
            var controller = _kernel.Get<TournamentsController>();
            _tournamentServiceMock.Setup(
                ts => ts.Create(It.IsAny<Tournament>())).Throws(new TournamentValidationException());

            // Act
            var result = controller.Post(new TournamentViewModelBuilder().Build()) as InvalidModelStateResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ModelState.Count > 0);
        }

        /// <summary>
        /// Test for Delete() method
        /// </summary>
        [TestMethod]
        public void Delete_TournamentExist_TournamentDeleted()
        {
            //// Arrange
            var testTournaments = _testFixture.TestTournaments()
                          .Build();
            var tournamentToDeleteID = testTournaments.Last().Id;
            var controller = _kernel.Get<TournamentsController>();

            //// Act
            var response = controller.Delete(tournamentToDeleteID) as StatusCodeResult;

            //// Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        /// <summary>
        /// Test Put method. Valid ViewModel.
        /// </summary>
        [TestMethod]
        public void Put_ValidViewModel_TournamentUpdated()
        {
            // Arrange
            var expectedDomainTournament = new TournamentBuilder().Build();
            var controller = _kernel.Get<TournamentsController>();

            // Act
            var input = new TournamentViewModelBuilder().Build();
            controller.Put(input);

            // Assert
            var comparer = new TournamentComparer();
            _tournamentServiceMock.Verify(
                ts => ts.Edit(It.Is<Tournament>(t => comparer.Compare(t, expectedDomainTournament) == 0)),
                Times.Once());
        }

        /// <summary>
        /// Test Put method. The method should return "Bad request (Invalid model state)" status
        /// </summary>
        [TestMethod]
        public void Put_InvalidModelState_InvalidModelStateResultReturned()
        {
            // Arrange
            var controller = _kernel.Get<TournamentsController>();
            controller.ModelState.AddModelError(KEY_FOR_EXCEPTION_MESSAGE, EXCEPTION_MESSAGE);

            // Act
            var input = new TournamentViewModelBuilder().Build();
            var actualResult = controller.Put(input) as InvalidModelStateResult;
            var actualCorrectErrorCount = actualResult.ModelState.Single(msvp => msvp.Key == KEY_FOR_EXCEPTION_MESSAGE).Value
                                                                    .Errors.Count(error => error.ErrorMessage == EXCEPTION_MESSAGE);

            // Assert
            _tournamentServiceMock.Verify(ts => ts.Edit(It.IsAny<Tournament>()), Times.Never());
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(1, actualCorrectErrorCount);
        }

        /// <summary>
        /// Test for Put method. The method should return "Bad request" status
        /// </summary>
        [TestMethod]
        public void Put_ValidationExceptionThrown_BadRequestReturned()
        {
            // Arrange
            var controller = _kernel.Get<TournamentsController>();
            _tournamentServiceMock.Setup(ts => ts.Edit(It.IsAny<Tournament>()))
                .Throws(new TournamentValidationException(EXCEPTION_MESSAGE));

            // Act
            var input = new TournamentViewModelBuilder().Build();
            var actual = controller.Put(input) as BadRequestErrorMessageResult;

            // Assert
            _tournamentServiceMock.Verify(ts => ts.Edit(It.IsAny<Tournament>()), Times.Once());
            Assert.IsNotNull(actual);
            Assert.AreEqual<string>(actual.Message, EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// Test for Put method. The method should return "Internal server error" status
        /// </summary>
        [TestMethod]
        public void Put_WithinEditOperationException_InternalServerErrorResultReturned()
        {
            // Arrange
            var controller = _kernel.Get<TournamentsController>();
            _tournamentServiceMock.Setup(ts => ts.Edit(It.IsAny<Tournament>())).Throws(new Exception());

            // Act
            var input = new TournamentViewModelBuilder().Build();
            var actual = controller.Put(input) is InternalServerErrorResult;

            // Assert
            _tournamentServiceMock.Verify(ts => ts.Edit(It.IsAny<Tournament>()), Times.Once());
            Assert.IsTrue(actual);
        }

        /// <summary>
        /// Test for Map() method.
        /// The method should map tournament domain model to view model.
        /// </summary>
        [TestMethod]
        public void Map_TournamentAsParam_MappedToViewModelWebApi()
        {
            // Arrange
            var tournament = new TournamentBuilder()
                                        .WithId(1)
                                        .WithName("test")
                                        .WithDescription("Volley")
                                        .WithScheme(TournamentSchemeEnum.Two)
                                        .WithSeason(2016)
                                        .WithRegulationsLink("volley.dp.ua")
                                        .Build();
            var expected = new TournamentViewModelBuilder()
                                        .WithId(1)
                                        .WithName("test")
                                        .WithDescription("Volley")
                                        .WithScheme("2")
                                        .WithSeason(2016)
                                        .WithRegulationsLink("volley.dp.ua")
                                        .Build();

            // Act
            var actual = TournamentViewModel.Map(tournament);

            // Assert
            AssertExtensions.AreEqual<TournamentViewModel>(expected, actual, new TournamentViewModelComparer());
        }

        /// <summary>
        /// Test for Map() method.
        /// The method should map tournament view model to domain model.
        /// </summary>
        [TestMethod]
        public void Map_TournamentViewModelWebApi_MappedToDomainModel()
        {
            // Arrange
            var testViewModel = new TournamentViewModelBuilder()
                                        .WithId(2)
                                        .WithDescription("Volley")
                                        .WithName("test tournament")
                                        .WithScheme("2.5")
                                        .WithSeason(2016)
                                        .WithRegulationsLink("volley.dp.ua")
                                        .Build();
            var expected = new TournamentBuilder()
                                        .WithId(2)
                                        .WithDescription("Volley")
                                        .WithName("test tournament")
                                        .WithScheme(TournamentSchemeEnum.TwoAndHalf)
                                        .WithSeason(2016)
                                        .WithRegulationsLink("volley.dp.ua")
                                        .Build();

            // Act
            var actual = testViewModel.ToDomain();

            // Assert
            AssertExtensions.AreEqual<Tournament>(expected, actual, new TournamentComparer());
        }

        /// <summary>
        /// GetActual method test. The method should invoke GetActual() method of ITournamentService
        /// </summary>
        public void GetActual_ActualTournamentsRequest_GetActualCalled()
        {
            // Act
            var sut = this._kernel.Get<TournamentsController>();
            sut.GetActual();

            // Assert
            _tournamentServiceMock.Verify(m => m.GetActual(), Times.Once());
        }

        /// <summary>
        /// GetActual method test. The method should return JavaScript Object Notation collection
        /// </summary>
        [TestMethod]
        public void GetActual_ServiceGetActualCalled_JsonResultReturned()
        {
            // Arrange
            var sut = this._kernel.Get<TournamentsController>();

            // Act
            var result = sut.GetActual() as JsonResult<IQueryable<TournamentViewModel>>;
            var actual = result.Content.ToList();

            // Assert
            Assert.IsNotNull(result);
            _tournamentServiceMock.Verify(ts => ts.GetActual(), Times.Once());
        }

        /// <summary>
        /// Mock the Tournaments
        /// </summary>
        /// <param name="testData">Data what will be returned</param>
        private void MockTournaments(IList<Tournament> testData)
        {
            _tournamentServiceMock.Setup(tr => tr.Get())
                                            .Returns(testData.AsQueryable());
        }
    }
}
