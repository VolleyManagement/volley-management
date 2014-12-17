namespace VolleyManagement.UnitTests.Services.TournamentService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Contracts;
    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.Domain.Tournaments;
    using VolleyManagement.Services;

    /// <summary>
    /// Tests for TournamentService class.
    /// </summary>
    [TestClass]
    public class TournamentServiceTests
    {
        /// <summary>
        /// Test Fixture.
        /// </summary>
        private readonly TournamentServiceTestFixture _testFixture = new TournamentServiceTestFixture();

        /// <summary>
        /// Tournaments Repository Mock.
        /// </summary>
        private readonly Mock<ITournamentRepository> _tournamentRepositoryMock = new Mock<ITournamentRepository>();

        /// <summary>
        /// Unit of work mock.
        /// </summary>
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

        /// <summary>
        /// ITournament service mock
        /// </summary>
        private readonly Mock<ITournamentService> _tournamentServiceMock = new Mock<ITournamentService>();

        /// <summary>
        /// IoC for tests.
        /// </summary>
        private IKernel _kernel;

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            this._kernel = new StandardKernel();
            this._kernel.Bind<ITournamentRepository>()
                   .ToConstant(this._tournamentRepositoryMock.Object);
            this._tournamentRepositoryMock.Setup(tr => tr.UnitOfWork).Returns(_unitOfWorkMock.Object);
        }

        /// <summary>
        /// Test for FinById method.
        /// </summary>
        [TestMethod]
        public void FindById_Id1_TournamentFound()
        {
            // Arrange
            var tournamentService = this._kernel.Get<TournamentService>();
            int id = 1;
            var tournament = new TournamentBuilder()
                .WithId(1)
                .WithName("Name")
                .WithDescription("Description")
                .WithScheme(TournamentSchemeEnum.One)
                .WithSeason("2014/2015")
                .WithRegulationsLink("link")
                .Build();
            MockTournamentServiceFindWhere(new List<Tournament>() { tournament });

            //// Act
            var actualResult = tournamentService.FindById(id);

            // Assert
            AssertExtensions.AreEqual<Tournament>(tournament, actualResult, new TournamentComparer());
        }

        /// <summary>
        /// Test for FinById method. Null returned.
        /// </summary>
        [TestMethod]
        public void FindById_NotExistingTournament_NullReturned()
        {
            // Arrange
            MockTournamentServiceFindWhereAlternativeStory();
            var tournamentService = this._kernel.Get<TournamentService>();

            // Act
            var tournament = tournamentService.FindById(1);

            // Assert
            Assert.IsNull(tournament);
        }

        /// <summary>
        /// Test for GetAll() method. The method should return existing tournaments
        /// (order is important).
        /// </summary>
        [TestMethod]
        public void GetAll_TournamentsExist_TournamentsReturned()
        {
            // Arrange
            var testData = this._testFixture.TestTournaments()
                                       .Build();
            _tournamentRepositoryMock.Setup(tr => tr.FindAll()).Returns(testData.AsQueryable());
            var sut = this._kernel.Get<TournamentService>();
            var expected = new TournamentServiceTestFixture()
                                            .TestTournaments()
                                            .Build()
                                            .ToList();

            // Act
            var actual = sut.GetAll().ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual, new TournamentComparer());
        }

        /// <summary>
        /// Test for Edit() method. The method should invoke Update() method of ITournamentRepository
        /// and Commit() method of IUnitOfWork.
        /// </summary>
        [TestMethod]
        public void Edit_TournamentAsParam_TournamentEdited()
        {
            // Arrange
            var testTournament = new TournamentBuilder()
                                        .WithId(1)
                                        .WithName("Test Tournament")
                                        .Build();

            // Act
            var sut = this._kernel.Get<TournamentService>();
            sut.Edit(testTournament);

            // Assert
            this._tournamentRepositoryMock.Verify(
                tr => tr.Update(It.Is<Tournament>(t => TournamentsAreEqual(t, testTournament))),
                Times.Once());
            this._unitOfWorkMock.Verify(u => u.Commit(), Times.Once());
        }

        /// <summary>
        /// Test for Edit() method with null as input parameter. The method should throw InvalidOperationException
        /// and shouldn't invoke Commit() method of IUnitOfWork.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Edit_TournamentNullAsParam_ExceptionThrown()
        {
            // Arrange
            Tournament testTournament = null;
            _tournamentRepositoryMock.Setup(tr => tr.Update(null)).Throws<InvalidOperationException>();

            // Act
            var sut = this._kernel.Get<TournamentService>();
            sut.Edit(testTournament);

            // Assert
            this._unitOfWorkMock.Verify(u => u.Commit(), Times.Never());
        }

        /// <summary>
        /// Test for Create() method. The method should return a created tournament.
        /// </summary>
        [TestMethod]
        public void Create_TournamentNotExist_TournamentCreated()
        {
            // Arrange
            var newTournament = new TournamentBuilder()
                                        .WithId(4)
                                        .WithName("New Tournament")
                                        .Build();

            // Act
            var sut = this._kernel.Get<TournamentService>();
            sut.Create(newTournament);

            // Assert
            this._tournamentRepositoryMock.Verify(
                tr => tr.Add(It.Is<Tournament>(t => TournamentsAreEqual(t, newTournament))));
            this._unitOfWorkMock.Verify(u => u.Commit());
        }

        /// <summary>
        /// Find out whether two tournament objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given tournaments have the same properties.</returns>
        private bool TournamentsAreEqual(Tournament x, Tournament y)
        {
            TournamentComparer comparer = new TournamentComparer();
            return comparer.Compare(x, y) == 0;
        }

        /// <summary>
        /// Mocks FindWhere method.
        /// </summary>
        /// <param name="testData">Test data to mock.</param>
        private void MockTournamentServiceFindWhere(IEnumerable<Tournament> testData)
        {
<<<<<<< HEAD
            _tournamentRepositoryMock.Setup(tr => tr.FindWhere(It.IsAny<Expression<Func<Tournament, bool>>>()))
                .Returns(testData.AsQueryable());

=======
            _tournamentServiceMock.Setup(ts => ts.FindById(It.IsAny<int>()))
                .Returns(
                    new Tournament
                    {
                        Id = 1,
                        Name = "Name",
                        Description = "Description",
                        Scheme = TournamentSchemeEnum.One,
                        Season = "2014/2015",
                        RegulationsLink = "link"
                    });
>>>>>>> f0a61730d788b6f68729009676430e563dcbb89a
        }

        /// <summary>
        /// Mocks FindWhere method.
        /// </summary>
        private void MockTournamentServiceFindWhereAlternativeStory()
        {
            _tournamentRepositoryMock.Setup(tr => tr.FindWhere(It.IsAny<Expression<Func<Tournament, bool>>>()))
                           .Returns(new List<Tournament>() { null }.AsQueryable());
        }
    }
}
