namespace VolleyManagement.UnitTests.Services.TournamentService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Crosscutting.Contracts.Providers;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Domain.TournamentsAggregate;
    using VolleyManagement.Services;

    /// <summary>
    /// Tests for TournamentService class.
    /// </summary>
    [ExcludeFromCodeCoverage]
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
        /// TimeProvider mock
        /// </summary>
        private readonly Mock<TimeProvider> _timeMock = new Mock<TimeProvider>();

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
            _kernel = new StandardKernel();
            _kernel.Bind<ITournamentRepository>()
                   .ToConstant(_tournamentRepositoryMock.Object);
            _tournamentRepositoryMock.Setup(tr => tr.UnitOfWork).Returns(_unitOfWorkMock.Object);
            this._timeMock.SetupGet(tp => tp.UtcNow).Returns(new DateTime(2015, 04, 01));
            TimeProvider.Current = this._timeMock.Object;
        }

        /// <summary>
        /// Cleanup test data
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            TimeProvider.ResetToDefault();
        }

        /// <summary>
        /// Test for FinById method.
        /// </summary>
        [TestMethod]
        public void FindById_Existing_TournamentFound()
        {
            // Arrange
            var tournamentService = _kernel.Get<TournamentService>();
            int id = 1;
            var tournament = new TournamentBuilder()
                .WithId(1)
                .WithName("Name")
                .WithDescription("Description")
                .WithScheme(TournamentSchemeEnum.One)
                .WithSeason(2014)
                .WithRegulationsLink("link")
                .Build();
            MockRepositoryFindWhere(new List<Tournament>() { tournament });

            //// Act
            var actualResult = tournamentService.Get(id);

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
            MockRepositoryFindWhere(new List<Tournament>() { null });
            var tournamentService = _kernel.Get<TournamentService>();

            // Act
            var tournament = tournamentService.Get(1);

            // Assert
            Assert.IsNull(tournament);
        }

        /// <summary>
        /// Test for Get() method. The method should return existing tournaments
        /// (order is important).
        /// </summary>
        [TestMethod]
        public void GetAll_TournamentsExist_TournamentsReturned()
        {
            // Arrange
            var testData = _testFixture.TestTournaments()
                                       .Build();
            MockRepositoryFindAll(testData);
            var sut = _kernel.Get<TournamentService>();
            var expected = new TournamentServiceTestFixture()
                                            .TestTournaments()
                                            .Build()
                                            .ToList();

            // Act
            var actual = sut.Get().ToList();

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
            var sut = _kernel.Get<TournamentService>();
            sut.Edit(testTournament);

            // Assert
            _tournamentRepositoryMock.Verify(
                tr => tr.Update(It.Is<Tournament>(t => TournamentsAreEqual(t, testTournament))),
                Times.Once());
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Once());
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
            var sut = _kernel.Get<TournamentService>();
            sut.Edit(testTournament);

            // Assert
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Never());
        }

        /// <summary>
        /// Test for Edit() method where input tournament has non-unique name. The method should throw ArgumentException
        /// and shouldn't invoke Commit() method of IUnitOfWork.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TournamentValidationException))]
        public void Edit_TournamentWithNonUniqueName_ExceptionThrown()
        {
            // Arrange
            var testData = new TournamentServiceTestFixture()
                                    .AddTournament(new TournamentBuilder()
                                                        .WithId(1)
                                                        .WithName("Non-Unique Tournament")
                                                        .Build())
                                    .Build();
            MockRepositoryFindWhere(testData);

            Tournament nonUniqueNameTournament = new TournamentBuilder()
                                                        .WithId(2)
                                                        .WithName("Non-Unique Tournament")
                                                        .Build();

            // Act
            var sut = _kernel.Get<TournamentService>();
            sut.Edit(nonUniqueNameTournament);

            // Assert
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Never());
        }

        /// <summary>
        /// Test for Create() method where input applying end date goes before start applying date
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TournamentValidationException), "Начало периода заявок должно быть раньше чем его окнчание")]
        public void Create_TournamentWithInvalidApplyingDate_ExceptionThrown()
        {
            // Arrange
            var tournamentBuilder = new TournamentBuilder();
            var now = TimeProvider.Current.UtcNow;
            var newTournament = tournamentBuilder
                .WithApplyingPeriodStart(now.AddDays(1))
                .WithApplyingPeriodEnd(now.AddDays(-1))
                .Build();

            // Act
            var sut = _kernel.Get<TournamentService>();
            sut.Create(newTournament);

            // Assert
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Never());
        }

        /// <summary>
        /// Test for Create() method where input applying start date goes before now
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TournamentValidationException), "Период заявок должен следовать перед началом игр")]
        public void Create_TournamentWithInvalidGameStartDate_ExceptionThrown()
        {
            // Arrange
            var tournamentBuilder = new TournamentBuilder();
            var now = TimeProvider.Current.UtcNow;
            var newTournament = tournamentBuilder
                .WithGamesStart(now.AddMonths(Domain.Constants.Tournament.MINIMUN_REGISTRATION_PERIOD_MONTH - 1))
                .Build();

            // Act
            var sut = _kernel.Get<TournamentService>();
            sut.Create(newTournament);

            // Assert
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Never());
        }

        /// <summary>
        /// Test for Create() method where input applying start date goes before now
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TournamentValidationException), "Окончание трансферного периода должно быть раньше окончания игр")]
        public void Create_TournamentTransferEndGoesAfterGamesEnd_ExceptionThrown()
        {
            // Arrange
            var tournamentBuilder = new TournamentBuilder();
            var now = TimeProvider.Current.UtcNow;
            int tournamentPeriodMonth = TournamentBuilder.TRANSFER_PERIOD_MONTH;
            var newTournament = tournamentBuilder
                .WithGamesEnd(now.AddMonths(Domain.Constants.Tournament.MINIMUN_REGISTRATION_PERIOD_MONTH + tournamentPeriodMonth))
                .WithTransferEnd(now.AddMonths(Domain.Constants.Tournament.MINIMUN_REGISTRATION_PERIOD_MONTH + tournamentPeriodMonth + 1))
                .Build();

            // Act
            var sut = _kernel.Get<TournamentService>();
            sut.Create(newTournament);

            // Assert
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Never());
        }

        /// <summary>
        /// Test for Create() method where input applying start date goes before now
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TournamentValidationException), "Начальная дата турнира должна следовать перед ее окончанием")]
        public void Create_TournamentGamesStartGoesAfterGamesEnd_ExceptionThrown()
        {
            // Arrange
            var tournamentBuilder = new TournamentBuilder();
            var now = TimeProvider.Current.UtcNow;
            var newTournament = tournamentBuilder
                .WithGamesStart(now.AddMonths(Domain.Constants.Tournament.MINIMUN_REGISTRATION_PERIOD_MONTH))
                .WithGamesEnd(now.AddMonths(Domain.Constants.Tournament.MINIMUN_REGISTRATION_PERIOD_MONTH))
                .Build();

            // Act
            var sut = _kernel.Get<TournamentService>();
            sut.Create(newTournament);

            // Assert
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Never());
        }

        /// <summary>
        /// Test for Create() method. Transfer end before transfer start
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TournamentValidationException), "Начало трансферного периода должно быть раньше чем его окнчание")]
        public void Create_TournamentTransferEndBeforeStart_ExceptionThrown()
        {
            // Arrange
            var tournamentBuilder = new TournamentBuilder();
            var now = TimeProvider.Current.UtcNow;
            var newTournament = tournamentBuilder
                .WithTransferStart(now.AddMonths(Domain.Constants.Tournament.MINIMUN_REGISTRATION_PERIOD_MONTH + 1).AddDays(1))
                .WithTransferEnd(now.AddMonths(Domain.Constants.Tournament.MINIMUN_REGISTRATION_PERIOD_MONTH + 1))
                .Build();

            // Act
            var sut = _kernel.Get<TournamentService>();
            sut.Create(newTournament);

            // Assert
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Never());
        }

        /// <summary>
        /// Test for Create() method. Transfer start is before than tournaments start
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TournamentValidationException), "Начало трансферного окна должно быть после начала игр")]
        public void Create_TournamentTransferStartBeforeTournamentStart_ExceptionThrown()
        {
            // Arrange
            var tournamentBuilder = new TournamentBuilder();
            var now = TimeProvider.Current.UtcNow;
            var newTournament = tournamentBuilder
                .WithGamesStart(now.AddMonths(Domain.Constants.Tournament.MINIMUN_REGISTRATION_PERIOD_MONTH + 1))
                .WithTransferStart(now.AddMonths(Domain.Constants.Tournament.MINIMUN_REGISTRATION_PERIOD_MONTH + 1).AddDays(-1))
                .Build();

            // Act
            var sut = _kernel.Get<TournamentService>();
            sut.Create(newTournament);

            // Assert
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Never());
        }

        /// <summary>
        /// Test for Create() method. Tournament end is before than tournaments start
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TournamentValidationException), "Начальная дата турнира должна следовать перед ее окончанием")]
        public void Create_TournamentTournamentEndBeforeTournamentStart_ExceptionThrown()
        {
            // Arrange
            var tournamentBuilder = new TournamentBuilder();
            var now = TimeProvider.Current.UtcNow;
            var newTournament = tournamentBuilder
                .WithGamesStart(now.AddMonths(Domain.Constants.Tournament.MINIMUN_REGISTRATION_PERIOD_MONTH + 1).AddDays(1))
                .WithGamesEnd(now.AddMonths(Domain.Constants.Tournament.MINIMUN_REGISTRATION_PERIOD_MONTH + 1))
                .Build();

            // Act
            var sut = _kernel.Get<TournamentService>();
            sut.Create(newTournament);

            // Assert
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Never());
        }

        /// <summary>
        /// Test for Create() method. The method should return a created tournament.
        /// </summary>
        [TestMethod]
        public void Create_TournamentNotExist_TournamentCreated()
        {
            // Arrange
            var newTournament = new TournamentBuilder().Build();

            // Act
            var sut = _kernel.Get<TournamentService>();
            sut.Create(newTournament);

            // Assert
            _tournamentRepositoryMock.Verify(
                tr => tr.Add(It.Is<Tournament>(t => TournamentsAreEqual(t, newTournament))));
            _unitOfWorkMock.Verify(u => u.Commit());
        }

        /// <summary>
        /// Test for Create() method with null as a parameter. The method should throw InvalidOperationException
        /// and shouldn't invoke Commit() method of IUnitOfWork.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Create_TournamentNullAsParam_ExceptionThrown()
        {
            // Arrange
            Tournament testTournament = null;
            _tournamentRepositoryMock.Setup(tr => tr.Add(null)).Throws<InvalidOperationException>();

            // Act
            var sut = _kernel.Get<TournamentService>();
            sut.Create(testTournament);

            // Assert
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Never());
        }

        /// <summary>
        /// Test for Create() method where tournament name should be unique. The method should throw ArgumentException
        /// and shouldn't invoke Commit() method of IUnitOfWork.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TournamentValidationException))]
        public void Create_TournamentWithNonUniqueName_ExceptionThrown()
        {
            // Arrange
            var testData = new TournamentServiceTestFixture()
                                    .AddTournament(new TournamentBuilder()
                                                        .WithId(1)
                                                        .WithName("Tournament 5")
                                                        .Build())
                                    .Build();
            MockRepositoryFindWhere(testData);

            Tournament nonUniqueNameTournament = new TournamentBuilder()
                                                        .WithId(2)
                                                        .WithName("Tournament 5")
                                                        .Build();

            // Act
            var sut = _kernel.Get<TournamentService>();
            sut.Create(nonUniqueNameTournament);

            // Assert
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Never());
        }

        /// <summary>
        /// GetActual method test. The method should invoke Find() method of ITournamentRepository
        /// </summary>
        public void GetActual_ActualTournamentsRequest_FindCalled()
        {
            // Act
            var tournamentService = _kernel.Get<TournamentService>();
            tournamentService.GetActual();

            // Assert
            //_tournamentRepositoryMock.Verify(m => m.Find(), Times.Once());
        }

        /// <summary>
        /// GetActual method test. The method should return actual tournaments
        /// </summary>
        [TestMethod]
        public void GetActual_TournamentsExist_ActualTournamentsReturnes()
        {
            // Arrange
            var tournamentService = _kernel.Get<TournamentService>();
            var testData = _testFixture.TestTournaments()
                                       .Build();
            MockRepositoryFindAll(testData);

            DateTime now = TimeProvider.Current.UtcNow;
            DateTime limitStartDate = now.AddMonths(VolleyManagement.Domain.Constants.Tournament.UPCOMING_TOURNAMENTS_MONTH_LIMIT);

            var expected = new TournamentServiceTestFixture()
                                            .TestTournaments()
                                            .Build().Where(tr => tr.GamesEnd >= now
                                                && tr.GamesStart <= limitStartDate)
                                            .ToList();

            // Act
            var actual = tournamentService.GetActual().ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual, new TournamentComparer());
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
        /// Mocks Find method.
        /// </summary>
        /// <param name="testData">Test data to mock.</param>
        private void MockRepositoryFindAll(IEnumerable<Tournament> testData)
        {
            //_tournamentRepositoryMock.Setup(tr => tr.Find()).Returns(testData.AsQueryable());
        }

        /// <summary>
        /// Mocks FindWhere method.
        /// </summary>
        /// <param name="testData">Test data to mock.</param>
        private void MockRepositoryFindWhere(IEnumerable<Tournament> testData)
        {
        }
    }
}
