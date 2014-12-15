namespace VolleyManagement.UnitTests.Services.TournamentService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using Ninject;

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
        public void FindById_Id1_TournamentFounded()
        {
            _tournamentRepositoryMock.Setup(tr => tr.FindWhere(It.IsAny<Expression<Func<Tournament, bool>>>()))
                .Returns(new List<Tournament>() { new Tournament { Id = 1 } }.AsQueryable());
            var tournamentService = this._kernel.Get<TournamentService>();
            int id = 1;
            var tournament = new TournamentBuilder().WithId(1).Build();

            Assert.AreEqual(tournament.Id, tournamentService.FindById(id).Id);
        }

        /// <summary>
        /// Test for FinById method. Null returned.
        /// </summary>
        [TestMethod]
        public void FindById_NotExistingTournament_NullReturned()
        {
            _tournamentRepositoryMock.Setup(tr => tr.FindWhere(It.IsAny<Expression<Func<Tournament, bool>>>()))
                           .Returns(new List<Tournament>() { null }.AsQueryable());
            var tournamentService = this._kernel.Get<TournamentService>();
            var tournament = tournamentService.FindById(1);

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
            Func<Tournament, bool> tournamentsMatch = delegate(Tournament t)
            {
                return t.Id.Equals(testTournament.Id)
                    && t.Name.Equals(testTournament.Name);
            };

            // Act
            var sut = this._kernel.Get<TournamentService>();
            sut.Edit(testTournament);

            // Assert
            this._tournamentRepositoryMock.Verify(tr => tr.Update(It.Is<Tournament>(t => tournamentsMatch(t))), Times.Once());
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
            var sut = this._kernel.Get<TournamentService>();
            sut.Create(new Tournament());

            _tournamentRepositoryMock.Verify(tr => tr.Add(It.IsAny<Tournament>()), Times.Once());
            this._unitOfWorkMock.Verify(u => u.Commit(), Times.Once());
        }
    }
}
