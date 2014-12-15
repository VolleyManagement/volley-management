﻿namespace VolleyManagement.UnitTests.WebApi
{
    using System.Collections.Generic;
    using System.Linq;
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
            var actual = new List<Tournament>();
            foreach (var t in sut.Get().ToList())
            {
                actual.Add(ViewModelToDomain.Map(t));
            }

            // Assert
            CollectionAssert.AreEqual(expected, actual, new TournamentComparer());
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
            var testTournaments = this._testFixture.TestTournaments()
                                      .Build();
            var tournamentToDelete = testTournaments.Last().Id;
            var tournamentService = _tournamentServiceMock.Object;

            tournamentService.Delete(tournamentToDelete);

            _tournamentServiceMock.Verify(m => m.Delete(tournamentToDelete));
        }

        /// <summary>
        /// Mocks test data
        /// </summary>
        /// <param name="testData">Data to mock</param>
        private void MockTournaments(IEnumerable<Tournament> testData)
        {
            this._tournamentServiceMock.Setup(tr => tr.GetAll()).Returns(testData.AsQueryable());
        }
    }
}
