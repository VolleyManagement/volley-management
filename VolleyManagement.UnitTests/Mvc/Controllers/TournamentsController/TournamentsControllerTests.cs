namespace VolleyManagement.UnitTests.Mvc.Controllers.TournamentsController
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using Contracts;
    using Domain.Tournaments;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;

    using VolleyManagement.Mvc.Controllers;
    using VolleyManagement.UnitTests.Services.TournamentService;

    /// <summary>
    /// Tests for MVC TournamentController class.
    /// </summary>
    [TestClass]
    public class TournamentsControllerTests
    {
        /// <summary>
        /// Test Fixture
        /// </summary>
        private readonly TournamentServiceTestFixture _testFixture =
            new TournamentServiceTestFixture();

        /// <summary>
        /// Tournaments Service Mock
        /// </summary>
        private readonly Mock<ITournamentService> _tournamentServiceMock =
            new Mock<ITournamentService>();

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
        /// Test for Index action. The action should return not empty tournaments list
        /// </summary>
        [TestMethod]
        public void Index_TournamentsExist_TournamentsReturned()
        {
            // Arrange
            var testData = this._testFixture.TestTournaments()
                                       .Build();
            this.MockTournaments(testData);

            var sut = this._kernel.Get<TournamentsController>();

            var expected = new TournamentServiceTestFixture()
                                            .TestTournaments()
                                            .Build()
                                            .ToList();

            // Act
            var result = sut.Index() as ViewResult;

            var actual = (IEnumerable<Tournament>)result.ViewData.Model;

            // Assert
            CollectionAssert.AreEqual(expected, actual.ToList(), new TournamentComparer());
        }

        /// <summary>
        /// Test with negative scenario for Index action.
        /// The action should thrown Argument null exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void Index_TournamentsDoNotExist_ExceptionThrown()
        {
            List<Tournament> testData = null;
            this.MockTournaments(testData);
        }

        /// <summary>
        /// Mocks test data
        /// </summary>
        /// <param name="testData">Data to mock</param>
        private void MockTournaments(IEnumerable<Tournament> testData)
        {
            this._tournamentServiceMock.Setup(tr => tr.GetAll())
                .Returns(testData.AsQueryable());
        }
    }
}