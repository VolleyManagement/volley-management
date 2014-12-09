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
            this.MockTournaments(testData);

            // sut - stands for System Under Test
            var sut = this._kernel.Get<TournamentService>();

            // Expected result
            var expected = new TournamentServiceTestFixture()
                                            .TestTournaments()
                                            .Build()
                                            .ToList();

            // Actual result
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
            this.MockUnitOfWork();
            Func<Tournament, bool> tournamentsMatch = delegate(Tournament t)
            {
                return t.Id.Equals(testTournament.Id)
                    && t.Name.Equals(testTournament.Name);
            };

            // System Under Test
            var sut = this._kernel.Get<TournamentService>();
            sut.Edit(testTournament);

            this._tournamentRepositoryMock.Verify(tr => tr.Update(It.Is<Tournament>(t => tournamentsMatch(t))), Times.Once());
            this._unitOfWorkMock.Verify(u => u.Commit(), Times.Once());
        }

        /// <summary>
        /// Test for Create() method. The method should return a created tournament.
        /// </summary>
        [TestMethod]
        public void Create_TournamentNotExist_TournamentCreated()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            _tournamentRepositoryMock.Setup(tr => tr.UnitOfWork).Returns(unitOfWorkMock.Object);

            var sut = this._kernel.Get<TournamentService>();
            sut.Create(new Tournament());

            _tournamentRepositoryMock.Verify(tr => tr.Add(It.IsAny<Tournament>()), Times.Once());
            unitOfWorkMock.Verify(u => u.Commit(), Times.Once());
        }

        /// <summary>
        /// Mocks test data
        /// </summary>
        /// <param name="testData">Data to mock</param>
        private void MockTournaments(IEnumerable<Tournament> testData)
        {
            this._tournamentRepositoryMock.Setup(tr => tr.FindAll()).Returns(testData.AsQueryable());
        }

        /// <summary>
        /// Mocks unit of work
        /// </summary>
        private void MockUnitOfWork()
        {
            this._tournamentRepositoryMock.Setup(tr => tr.UnitOfWork).Returns(_unitOfWorkMock.Object);
        }
    }
}
