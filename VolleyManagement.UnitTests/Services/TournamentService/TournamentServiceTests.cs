﻿namespace VolleyManagement.UnitTests.Services.TournamentService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Crosscutting.Contracts.Providers;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Exceptions;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Data.Queries.Team;
    using VolleyManagement.Data.Queries.Tournament;
    using VolleyManagement.Domain.TeamsAggregate;
    using VolleyManagement.Domain.TournamentsAggregate;
    using VolleyManagement.Services;
    using VolleyManagement.UnitTests.Mvc.ViewModels;
    using VolleyManagement.UnitTests.Services.TeamService;

    /// <summary>
    /// Tests for TournamentService class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TournamentServiceTests
    {
        private const int MINIMUM_REGISTRATION_PERIOD_MONTH = 3;

        private const int FIRST_TOURNAMENT_ID = 1;

        private const int SPECIFIC_TEAM_ID = 2;

        private const int EMPTY_TEAM_LIST_COUNT = 0;

        private const int EXPECTED_NOTSTARTED_TOURNAMENTS_COUNT = 3;

        private readonly DateTime _dateForCurrentState = new DateTime(2015, 09, 30);

        private readonly DateTime _dateForFinishedState = new DateTime(2016, 09, 30);

        private readonly DateTime _dateForNotStartedState = new DateTime(2015, 02, 28);

        private readonly TournamentServiceTestFixture _testFixture = new TournamentServiceTestFixture();

        private readonly Mock<ITournamentRepository> _tournamentRepositoryMock = new Mock<ITournamentRepository>();

        private readonly Mock<IQuery<Tournament, UniqueTournamentCriteria>> _uniqueTournamentQueryMock =
            new Mock<IQuery<Tournament, UniqueTournamentCriteria>>();

        private readonly Mock<IQuery<List<Tournament>, GetAllCriteria>> _getAllQueryMock =
            new Mock<IQuery<List<Tournament>, GetAllCriteria>>();

        private readonly Mock<IQuery<Tournament, FindByIdCriteria>> _getByIdQueryMock =
            new Mock<IQuery<Tournament, FindByIdCriteria>>();

        private readonly Mock<IQuery<List<Team>, FindByTournamentIdCriteria>> _getAllTeamsQuery =
            new Mock<IQuery<List<Team>, FindByTournamentIdCriteria>>();

        private readonly Mock<IQuery<TournamentScheduleDto, TournamentScheduleInfoCriteria>> _getTorunamentDto =
            new Mock<IQuery<TournamentScheduleDto, TournamentScheduleInfoCriteria>>(); 

        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

        private readonly Mock<TimeProvider> _timeMock = new Mock<TimeProvider>();

        private IKernel _kernel;

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<ITournamentRepository>().ToConstant(_tournamentRepositoryMock.Object);
            _kernel.Bind<IQuery<Tournament, UniqueTournamentCriteria>>().ToConstant(_uniqueTournamentQueryMock.Object);
            _kernel.Bind<IQuery<List<Tournament>, GetAllCriteria>>().ToConstant(_getAllQueryMock.Object);
            _kernel.Bind<IQuery<Tournament, FindByIdCriteria>>().ToConstant(_getByIdQueryMock.Object);
            _kernel.Bind<IQuery<List<Team>, FindByTournamentIdCriteria>>().ToConstant(_getAllTeamsQuery.Object);
            _kernel.Bind<IQuery<TournamentScheduleDto, TournamentScheduleInfoCriteria>>().ToConstant(_getTorunamentDto.Object); 
            _tournamentRepositoryMock.Setup(tr => tr.UnitOfWork).Returns(_unitOfWorkMock.Object);
            _timeMock.SetupGet(tp => tp.UtcNow).Returns(new DateTime(2015, 06, 01));
            TimeProvider.Current = _timeMock.Object;
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
            var sut = _kernel.Get<TournamentService>();

            var tournament = CreateAnyTournament(FIRST_TOURNAMENT_ID);
            MockGetByIdQuery(tournament);

            //// Act
            var actualResult = sut.Get(FIRST_TOURNAMENT_ID);

            // Assert
            TestHelper.AreEqual<Tournament>(tournament, actualResult, new TournamentComparer());
        }

        /// <summary>
        /// Test for FinById method. Null returned.
        /// </summary>
        [TestMethod]
        public void FindById_NotExistingTournament_NullReturned()
        {
            // Arrange
            MockGetByIdQuery(null);
            var sut = _kernel.Get<TournamentService>();

            // Act
            var tournament = sut.Get(1);

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
            MockGetAllTournamentsQuery(testData);
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
        /// Test for GetAllTournamentTeams method.
        /// The method should return existing teams in specific tournament
        /// </summary>
        [TestMethod]
        public void GetAllTournamentTeams_TeamsExist_TeamsReturned()
        {
            // Arrange
            var testData = new TeamServiceTestFixture().TestTeams().Build();
            MockGetAllTournamentTeamsQuery(testData);
            var sut = _kernel.Get<TournamentService>();
            var expected = new TeamServiceTestFixture().TestTeams().Build();

            // Act
            var actual = sut.GetAllTournamentTeams(It.IsAny<int>());

            // Assert
            CollectionAssert.AreEqual(expected, actual, new TeamComparer());
        }

        /// <summary>
        /// Test for GetAllTournamentTeams method.
        /// No teams exists in tournament.
        /// The method should return empty team list.
        /// </summary>
        [TestMethod]
        public void GetAllTournamentTeams_TeamsNotExist_EmptyTeamListReturned()
        {
            // Arrange
            var testData = new TeamServiceTestFixture().Build();
            MockGetAllTournamentTeamsQuery(testData);
            var sut = _kernel.Get<TournamentService>();

            // Act
            var actual = sut.GetAllTournamentTeams(It.IsAny<int>());

            // Assert
            Assert.AreEqual(actual.Count, EMPTY_TEAM_LIST_COUNT);
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
            var sut = _kernel.Get<TournamentService>();

            // Act
            sut.Edit(testTournament);

            // Assert
            VerifyEditTournament(testTournament, Times.Once());
        }

        /// <summary>
        /// Test for Edit() method with null as input parameter. The method should throw NullReferenceException
        /// and shouldn't invoke Commit() method of IUnitOfWork.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Edit_TournamentNullAsParam_ExceptionThrown()
        {
            // Arrange
            Tournament testTournament = null;
            _tournamentRepositoryMock.Setup(tr => tr.Update(null)).Throws<NullReferenceException>();
            var sut = _kernel.Get<TournamentService>();

            // Act
            sut.Edit(testTournament);

            // Assert
            VerifyEditTournament(testTournament, Times.Never());
        }

        /// <summary>
        /// Test for Edit() method where input tournament has non-unique name. The method should
        /// throw TournamentValidationException and shouldn't invoke Commit() method of IUnitOfWork.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TournamentValidationException))]
        public void Edit_TournamentWithNonUniqueName_ExceptionThrown()
        {
            // Arrange
            var testData = new TournamentBuilder()
                                        .WithId(1)
                                        .WithName("Non-Unique Tournament")
                                        .Build();

            Tournament nonUniqueNameTournament = new TournamentBuilder()
                                                        .WithId(2)
                                                        .WithName("Non-Unique Tournament")
                                                        .Build();

            MockGetUniqueTournamentQuery(testData);
            var sut = _kernel.Get<TournamentService>();

            // Act
            sut.Edit(nonUniqueNameTournament);

            // Assert
            VerifyEditTournament(nonUniqueNameTournament, Times.Never());
        }

        /// <summary>
        /// Test for Create() method. Tournament's applying start date comes before current date.
        /// Exception is thrown during tournament creation.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TournamentValidationException), "Вы не можете указывать начало периода заявок в прошлом")]
        public void Create_TournamentApplyingStartDateBeforeCurrentDate_ExceptionThrown()
        {
            const int APPLYING_PERIOD_START_DAYS_DELTA = -1;

            // Arrange
            var now = TimeProvider.Current.UtcNow;
            var newTournament = new TournamentBuilder()
                .WithApplyingPeriodStart(now.AddDays(APPLYING_PERIOD_START_DAYS_DELTA))
                .Build();
            var sut = _kernel.Get<TournamentService>();

            // Act
            sut.Create(newTournament);

            // Assert
            VerifyCreateTournament(newTournament, Times.Never());
        }

        /// <summary>
        /// Test for Create() method. Tournament's applying end date comes before tournament's applying start date.
        /// Exception is thrown during tournament creation.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TournamentValidationException), "Начало периода заявок должно быть раньше чем его окончание")]
        public void Create_TournamentApplyingEndDateBeforeApplyingStartDate_ExceptionThrown()
        {
            const int APPLYING_PERIOD_START_DAYS_DELTA = 1;

            // Arrange
            var now = TimeProvider.Current.UtcNow;
            var newTournament = new TournamentBuilder()
                .WithApplyingPeriodStart(now.AddDays(APPLYING_PERIOD_START_DAYS_DELTA))
                .WithApplyingPeriodEnd(now)
                .Build();
            var sut = _kernel.Get<TournamentService>();

            // Act
            sut.Create(newTournament);

            // Assert
            VerifyCreateTournament(newTournament, Times.Never());
        }

        /// <summary>
        /// Test for Create() method. Tournament's games start date comes before tournament's applying end date.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TournamentValidationException), "Период заявок должен следовать перед началом игр")]
        public void Create_TournamentGameStartDateBeforeApplyingEndDate_ExceptionThrown()
        {
            const int GAMES_START_MONTHS_DELTA = -1;

            // Arrange
            var now = TimeProvider.Current.UtcNow;
            var newTournament = new TournamentBuilder()
                .WithGamesStart(now.AddMonths(MINIMUM_REGISTRATION_PERIOD_MONTH + GAMES_START_MONTHS_DELTA))
                .Build();
            var sut = _kernel.Get<TournamentService>();

            // Act
            sut.Create(newTournament);

            // Assert
            VerifyCreateTournament(newTournament, Times.Never());
        }

        /// <summary>
        /// Test for Create() method. Tournament's transfer start date is null and tournament's transfer end date is null.
        /// Tournament is created successfully.
        /// </summary>
        [TestMethod]
        public void Create_TournamentNoTransferPeriod_TournamentCreated()
        {
            // Arrange
            var newTournament = new TournamentBuilder().WithNoTransferPeriod().Build();
            var sut = _kernel.Get<TournamentService>();

            // Act
            sut.Create(newTournament);

            // Assert
            VerifyCreateTournament(newTournament, Times.Once());
        }

        /// <summary>
        /// Test for Create() method. Tournament's transfer start date is null and tournament's transfer end date is not null.
        /// Exception is thrown during tournament creation.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TournamentValidationException),
            "При наличии трансферного периода необходимо указать дату начала периода")]
        public void Create_TournamentNoTransferStart_ExceptionThrown()
        {
            // Arrange
            var newTournament = new TournamentBuilder().WithTransferStart(null).Build();
            var sut = _kernel.Get<TournamentService>();

            // Act
            sut.Create(newTournament);

            // Assert
            VerifyCreateTournament(newTournament, Times.Never());
        }

        /// <summary>
        /// Test for Create() method. Tournament's transfer start date is not null and tournament's transfer end date is null.
        /// Exception is thrown during tournament creation.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TournamentValidationException),
            "При наличии трансферного периода необходимо указать дату окончания периода")]
        public void Create_TournamentNoTransferEnd_ExceptionThrown()
        {
            // Arrange
            var newTournament = new TournamentBuilder().WithTransferEnd(null).Build();
            var sut = _kernel.Get<TournamentService>();

            // Act
            sut.Create(newTournament);

            // Assert
            VerifyCreateTournament(newTournament, Times.Never());
        }

        /// <summary>
        /// Test for Create() method. Tournament's transfer end date comes after tournament's games end date.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TournamentValidationException),
            "Окончание трансферного периода должно быть раньше окончания игр")]
        public void Create_TournamentTransferEndDateAfterGamesEndDate_ExceptionThrown()
        {
            const int TRANSFER_START_MONTHS_DELTA = 2;
            const int TRANSFER_END_MONTHS_DELTA = 2;
            const int TRANSFER_END_DAYS_DELTA = 1;

            // Arrange
            var now = TimeProvider.Current.UtcNow;
            var newTournament = new TournamentBuilder()
                .WithGamesEnd(now.AddMonths(MINIMUM_REGISTRATION_PERIOD_MONTH + TRANSFER_START_MONTHS_DELTA))
                .WithTransferEnd(now.AddMonths(MINIMUM_REGISTRATION_PERIOD_MONTH + TRANSFER_END_MONTHS_DELTA)
                    .AddDays(TRANSFER_END_DAYS_DELTA))
                .Build();
            var sut = _kernel.Get<TournamentService>();

            // Act
            sut.Create(newTournament);

            // Assert
            VerifyCreateTournament(newTournament, Times.Never());
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
                .WithGamesStart(now.AddMonths(MINIMUM_REGISTRATION_PERIOD_MONTH))
                .WithGamesEnd(now.AddMonths(MINIMUM_REGISTRATION_PERIOD_MONTH))
                .Build();
            var sut = _kernel.Get<TournamentService>();

            // Act
            sut.Create(newTournament);

            // Assert
            VerifyCreateTournament(newTournament, Times.Never());
        }

        /// <summary>
        /// Test for Create() method. Tournament's transfer end date comes before tournament's transfer start date.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TournamentValidationException),
            "Начало трансферного периода должно быть раньше чем его окончание")]
        public void Create_TournamentTransferEndDateBeforeTransferStartDate_ExceptionThrown()
        {
            const int TRANSFER_START_MONTHS_DELTA = 2;
            const int TRANSFER_START_DAYS_DELTA = 1;
            const int TRANSFER_END_MONTHS_DELTA = 2;

            // Arrange
            var now = TimeProvider.Current.UtcNow;
            var newTournament = new TournamentBuilder()
                .WithTransferStart(now.AddMonths(MINIMUM_REGISTRATION_PERIOD_MONTH + TRANSFER_START_MONTHS_DELTA)
                    .AddDays(TRANSFER_START_DAYS_DELTA))
                .WithTransferEnd(now.AddMonths(MINIMUM_REGISTRATION_PERIOD_MONTH + TRANSFER_END_MONTHS_DELTA))
                .Build();
            var sut = _kernel.Get<TournamentService>();

            // Act
            sut.Create(newTournament);

            // Assert
            VerifyCreateTournament(newTournament, Times.Never());
        }

        /// <summary>
        /// Test for Create() method. Tournament's transfer start date comes before tournament's games start date.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TournamentValidationException), "Начало трансферного окна должно быть после начала игр")]
        public void Create_TournamentTransferStartDateBeforeGamesStartDate_ExceptionThrown()
        {
            const int GAMES_START_MONTHS_DELTA = 1;
            const int TRANSFER_START_MONTHS_DELTA = 1;
            const int TRANSFER_START_DAYS_DELTA = -1;

            // Arrange
            var now = TimeProvider.Current.UtcNow;
            var newTournament = new TournamentBuilder()
                .WithGamesStart(now.AddMonths(MINIMUM_REGISTRATION_PERIOD_MONTH + GAMES_START_MONTHS_DELTA))
                .WithTransferStart(now.AddMonths(MINIMUM_REGISTRATION_PERIOD_MONTH + TRANSFER_START_MONTHS_DELTA)
                    .AddDays(TRANSFER_START_DAYS_DELTA))
                .Build();
            var sut = _kernel.Get<TournamentService>();

            // Act
            sut.Create(newTournament);

            // Assert
            VerifyCreateTournament(newTournament, Times.Never());
        }

        /// <summary>
        /// Test for Create() method. Tournament's games end date comes before tournament's games start date.
        /// Exception is thrown during tournament creation.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TournamentValidationException), "Начальная дата турнира должна следовать перед ее окончанием")]
        public void Create_TournamentGamesEndDateBeforeGamesStartDate_ExceptionThrown()
        {
            const int GAMES_START_MONTHS_DELTA = 1;
            const int GAMES_START_DAYS_DELTA = 1;
            const int GAMES_END_MONTHS_DELTA = 1;

            // Arrange
            var now = TimeProvider.Current.UtcNow;
            var newTournament = new TournamentBuilder()
                .WithGamesStart(now.AddMonths(MINIMUM_REGISTRATION_PERIOD_MONTH + GAMES_START_MONTHS_DELTA)
                    .AddDays(GAMES_START_DAYS_DELTA))
                .WithGamesEnd(now.AddMonths(MINIMUM_REGISTRATION_PERIOD_MONTH + GAMES_END_MONTHS_DELTA))
                .Build();
            var sut = _kernel.Get<TournamentService>();

            // Act
            sut.Create(newTournament);

            // Assert
            VerifyCreateTournament(newTournament, Times.Never());
        }

        /// <summary>
        /// Test for Create() method. The method should return a created tournament.
        /// </summary>
        [TestMethod]
        public void Create_TournamentNotExist_TournamentCreated()
        {
            // Arrange
            DateTime applyingPeriodStart = DateTime.UtcNow.AddDays(1);
            DateTime applyingPeriodEnd = applyingPeriodStart.AddDays(1);
            DateTime gamesStart = applyingPeriodEnd.AddDays(1);
            DateTime transferStart = gamesStart.AddDays(1);
            DateTime transferEnd = transferStart.AddDays(1);
            DateTime gamesEnd = transferEnd.AddDays(1);

            var newTournament = new TournamentBuilder().WithApplyingPeriodStart(applyingPeriodStart)
                                                       .WithApplyingPeriodEnd(applyingPeriodEnd)
                                                       .WithGamesStart(gamesStart)
                                                       .WithGamesEnd(gamesEnd)
                                                       .WithTransferStart(transferStart)
                                                       .WithTransferEnd(transferEnd)
                                                       .Build();

            // Act
            var sut = _kernel.Get<TournamentService>();
            sut.Create(newTournament);

            // Assert
            VerifyCreateTournament(newTournament, Times.Once());
        }

        /// <summary>
        /// Test for Create() method with null as a parameter. The method should throw ArgumentNullException
        /// and shouldn't invoke Commit() method of IUnitOfWork.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_TournamentNullAsParam_ExceptionThrown()
        {
            // Arrange
            Tournament testTournament = null;
            _tournamentRepositoryMock.Setup(tr => tr.Add(null)).Throws<InvalidOperationException>();

            // Act
            var sut = _kernel.Get<TournamentService>();
            sut.Create(testTournament);

            // Assert
            VerifyCreateTournament(testTournament, Times.Never());
        }

        /// <summary>
        /// Test for Create(). Tournament name is not unique.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TournamentValidationException), "Название турнира должно быть уникальным")]
        public void Create_TournamentNotUniqueName_ExceptionThrown()
        {
            // Arrange
            var newTournament = new TournamentBuilder().WithId(FIRST_TOURNAMENT_ID).WithName("Tournament 1").Build();
            _uniqueTournamentQueryMock
                .Setup(tr => tr.Execute(It.Is<UniqueTournamentCriteria>(cr => cr.Name == newTournament.Name)))
                .Returns(newTournament);
            var sut = _kernel.Get<TournamentService>();

            // Act
            sut.Create(newTournament);

            // Assert
            VerifyCreateTournament(newTournament, Times.Never());
        }

        /// <summary>
        /// Test for Create() method. Tournament's division count is out of range. Exception is thrown during tournament creation.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Количество дивизионов в турнире не должно выходить за установленный диапазон")]
        public void Create_TournamentDivisionCountOutOfRange_ExceptionThrown()
        {
            // Arrange
            var newTournament = new TournamentBuilder().WithNoDivisions().Build();
            var sut = _kernel.Get<TournamentService>();

            // Act
            sut.Create(newTournament);

            // Assert
            VerifyCreateTournament(newTournament, Times.Never());
        }

        /// <summary>
        /// Test for Create() method. Tournament's divisions do not have unique names. Exception is thrown during tournament creation.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Дивизионы в рамках турнира не могут иметь одинаковых названий")]
        public void Create_TournamentDivisionNamesNotUnique_ExceptionThrown()
        {
            // Arrange
            var newTournament = new TournamentBuilder().WithNonUniqueNameDivisions().Build();
            var sut = _kernel.Get<TournamentService>();

            // Act
            sut.Create(newTournament);

            // Assert
            VerifyCreateTournament(newTournament, Times.Never());
        }

        /// <summary>
        /// Test for Create() method. Group count in tournament's divisions is out of range.
        /// Exception is thrown during tournament creation.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Количество групп в дивизионе не должно выходить за установленный диапазон")]
        public void Create_TournamentDivisionGroupCountOutOfRange_ExceptionThrown()
        {
            // Arrange
            var newTournament = new TournamentBuilder().WithNoDivisionsGroups().Build();
            var sut = _kernel.Get<TournamentService>();

            // Act
            sut.Create(newTournament);

            // Assert
            VerifyCreateTournament(newTournament, Times.Never());
        }

        /// <summary>
        /// Test for Create() method. Groups in tournament's divisions do not have unique names.
        /// Exception is thrown during tournament creation.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Группы в рамках дивизиона не могут иметь одинаковых названий")]
        public void Create_TournamentDivisionGroupNamesNotUnique_ExceptionThrown()
        {
            // Arrange
            var newTournament = new TournamentBuilder().WithDivisionsNonUniqueNameGroups().Build();
            var sut = _kernel.Get<TournamentService>();

            // Act
            sut.Create(newTournament);

            // Assert
            VerifyCreateTournament(newTournament, Times.Never());
        }

        /// <summary>
        /// Test for AddTeamsToTournament method.
        /// Valid teams have to be added.
        /// </summary>
        [TestMethod]
        public void AddTeamsToTournament_ValidTeamList_TeamsAreAdded()
        {
            // Arrange
            var testData = new TeamServiceTestFixture().TestTeams().Build();
            MockGetAllTournamentTeamsQuery(new TeamServiceTestFixture().Build());
            var sut = _kernel.Get<TournamentService>();

            // Act
            sut.AddTeamsToTournament(testData, FIRST_TOURNAMENT_ID);

            // Assert
            VerifyTeamsAdded(FIRST_TOURNAMENT_ID, Times.Exactly(testData.Count), Times.Once());
        }

        /// <summary>
        /// Test for AddTeamsToTournament method.
        /// InValid teams must not be added.
        /// Method have to throw ArgumentException
        /// </summary>
        [TestMethod]
        public void AddTeamsToTournament_InValidTeamList_ArgumentExceptionThrown()
        {
            bool gotException = false;

            // Arrange
            var testData = new TeamServiceTestFixture().TestTeams().Build();
            MockGetAllTournamentTeamsQuery(new TeamServiceTestFixture().TestTeams().Build());
            var sut = _kernel.Get<TournamentService>();

            // Act
            try
            {
                sut.AddTeamsToTournament(testData, FIRST_TOURNAMENT_ID);
            }
            catch (ArgumentException)
            {
                gotException = true;
            }

            // Assert
            Assert.IsTrue(gotException);
            _unitOfWorkMock.Verify(uow => uow.Commit(), Times.Never());
        }

        /// <summary>
        /// Test for DeleteTeamFromTournament method.
        /// Team have to be removed from tournament
        /// </summary>
        [TestMethod]
        public void DeleteTeamFromTournament_TeamExist_TeamDeleted()
        {
            // Arrange
            var sut = _kernel.Get<TournamentService>();

            // Act
            sut.DeleteTeamFromTournament(SPECIFIC_TEAM_ID, FIRST_TOURNAMENT_ID);

            // Assert
            VerifyTeamDeleted(SPECIFIC_TEAM_ID, FIRST_TOURNAMENT_ID, Times.Once());
        }

        /// <summary>
        /// Test for DeleteTeamFromTournament method.
        /// Team is not exist in tournament
        /// Missing Entity Exception have to be thrown
        /// </summary>
        [TestMethod]
        public void DeleteTeamFromTournament_TeamNotExist_MissingEntityExceptionThrown()
        {
            bool gotException = false;

            // Arrange
            var sut = _kernel.Get<TournamentService>();
            _tournamentRepositoryMock
                .Setup(tr => tr.RemoveTeamFromTournament(It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new ConcurrencyException());

            // Act
            try
            {
                sut.DeleteTeamFromTournament(SPECIFIC_TEAM_ID, FIRST_TOURNAMENT_ID);
            }
            catch (MissingEntityException)
            {
                gotException = true;
            }

            // Assert
            Assert.IsTrue(gotException);
            _unitOfWorkMock.Verify(uow => uow.Commit(), Times.Never());
        }
        
        /// <summary>
        /// Test for Delete Tournament method.
        /// </summary>
        [TestMethod]
        public void Delete_TournamentExist_TournamentRemoved()
        {
            // Arrange
            var sut = _kernel.Get<TournamentService>();
            
            // Act
            sut.Delete(FIRST_TOURNAMENT_ID);
            
            // Assert
            VerifyDeleteTournament(FIRST_TOURNAMENT_ID, Times.Once());
        }

        /// <summary>
        /// GetActual method test. The method should invoke Find() method of ITournamentRepository
        /// </summary>
        [TestMethod]
        public void GetFinished_FinishTournamentsExist_FinishedTournamentsReturned()
        {
            // Arrange
            var sut = _kernel.Get<TournamentService>();
            var testData = _testFixture.WithFinishedTournaments().Build();
            MockGetAllTournamentsQuery(testData);

            var expected = new TournamentServiceTestFixture().WithFinishedTournaments().Build();

            // Act
            var actual = sut.GetFinished().ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual, new TournamentComparer());
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
            ////_tournamentRepositoryMock.Verify(m => m.Find(), Times.Once());
        }

        /// <summary>
        /// GetActual method test. The method should return actual tournaments
        /// </summary>
        [TestMethod]
        public void GetActual_TournamentsExist_ActualTournamentsReturnes()
        {
            // Arrange
            var sut = _kernel.Get<TournamentService>();
            var testData = _testFixture.TestTournaments()
                                       .Build();
            MockGetAllTournamentsQuery(testData);

            var expected = BuildActualTournamentsList();

            // Act
            var actual = sut.GetActual().ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual, new TournamentComparer());
        }

        /// <summary>
        /// GetActual method test. The method should return actual tournaments
        /// </summary>
        [TestMethod]
        public void GetActual_TournamentsExist_CurrentTournamentsReturned()
        {
            // Arrange
            MockTimeProviderUtcNow(_dateForCurrentState);
            var sut = _kernel.Get<TournamentService>();
            var testData = _testFixture.TestTournaments().Build();
            MockGetAllTournamentsQuery(testData);
            var expected = BuildActualTournamentsList();

            // Act
            var actual = sut.GetActual().ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual, new TournamentComparer());
        }

        /// <summary>
        /// GetFinished method test. The method should return finished tournaments
        /// </summary>
        [TestMethod]
        public void GetFinished_TournamentsExist_FinishedTournamentsReturned()
        {
            // Arrange
            MockTimeProviderUtcNow(_dateForFinishedState);
            var sut = _kernel.Get<TournamentService>();
            var testData = _testFixture.TestTournaments().Build();
            MockGetAllTournamentsQuery(testData);

            var expected = BuildActualTournamentsList();

            // Act
            var actual = sut.GetFinished().ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual, new TournamentComparer());
        }

        /// <summary>
        /// Get method test. The method returns all tournaments.
        /// Then select all not started tournaments
        /// </summary>
        [TestMethod]
        public void Get_TournamentsExist_NotStartedTournamentsReturned()
        {
            // Arrange
            MockTimeProviderUtcNow(_dateForNotStartedState);
            var sut = _kernel.Get<TournamentService>();
            var testData = _testFixture.TestTournaments().Build();
            MockGetAllTournamentsQuery(testData);

            // Act
            var actual = sut.Get().ToList();

            // Assert
            int actualCount = actual.Count(t => t.State == TournamentStateEnum.NotStarted);
            Assert.AreEqual(EXPECTED_NOTSTARTED_TOURNAMENTS_COUNT, actualCount);
        }

        private bool TournamentsAreEqual(Tournament x, Tournament y)
        {
            return new TournamentComparer().Compare(x, y) == 0;
        }

        private void MockGetAllTournamentsQuery(IEnumerable<Tournament> testData)
        {
            _getAllQueryMock.Setup(tr => tr.Execute(It.IsAny<GetAllCriteria>())).Returns(testData.ToList());
        }

        private void MockGetByIdQuery(Tournament testData)
        {
            _getByIdQueryMock.Setup(tr => tr.Execute(It.IsAny<FindByIdCriteria>())).Returns(testData);
        }

        private void MockGetUniqueTournamentQuery(Tournament testData)
        {
            _uniqueTournamentQueryMock.Setup(tr => tr.Execute(It.IsAny<UniqueTournamentCriteria>())).Returns(testData);
        }

        private void MockGetAllTournamentTeamsQuery(List<Team> testData)
        {
            _getAllTeamsQuery.Setup(tr => tr.Execute(It.IsAny<FindByTournamentIdCriteria>())).Returns(testData);
        }

        private void MockTimeProviderUtcNow(DateTime date)
        {
            _timeMock.Setup(c => c.UtcNow).Returns(date);
        }

        private Tournament CreateAnyTournament(int id)
        {
            return new TournamentBuilder()
                .WithId(id)
                .WithName("Name")
                .WithDescription("Description")
                .WithScheme(TournamentSchemeEnum.One)
                .WithSeason(2014)
                .WithRegulationsLink("link")
                .Build();
        }

        private List<Tournament> BuildActualTournamentsList()
        {
            return new TournamentServiceTestFixture()
                            .AddTournament(new TournamentBuilder()
                                            .WithId(1)
                                            .WithName("Tournament 1")
                                            .WithDescription("Tournament 1 description")
                                            .WithSeason(2014)
                                            .WithScheme(TournamentSchemeEnum.One)
                                            .WithRegulationsLink("www.Volleyball.dp.ua/Regulations/Tournaments('1')")
                                            .WithApplyingPeriodStart(new DateTime(2015, 02, 20))
                                            .WithApplyingPeriodEnd(new DateTime(2015, 06, 20))
                                            .WithGamesStart(new DateTime(2015, 06, 30))
                                            .WithGamesEnd(new DateTime(2015, 11, 30))
                                            .WithTransferStart(new DateTime(2015, 08, 20))
                                            .WithTransferEnd(new DateTime(2015, 09, 10))
                                            .Build())
                            .AddTournament(new TournamentBuilder()
                                            .WithId(2)
                                            .WithName("Tournament 2")
                                            .WithDescription("Tournament 2 description")
                                            .WithSeason(2014)
                                            .WithScheme(TournamentSchemeEnum.Two)
                                            .WithRegulationsLink("www.Volleyball.dp.ua/Regulations/Tournaments('2')")
                                            .WithApplyingPeriodStart(new DateTime(2015, 02, 20))
                                            .WithApplyingPeriodEnd(new DateTime(2015, 06, 20))
                                            .WithGamesStart(new DateTime(2015, 06, 30))
                                            .WithGamesEnd(new DateTime(2015, 11, 30))
                                            .WithTransferStart(new DateTime(2015, 08, 20))
                                            .WithTransferEnd(new DateTime(2015, 09, 10))
                                            .Build())
                            .AddTournament(new TournamentBuilder()
                                            .WithId(3)
                                            .WithName("Tournament 3")
                                            .WithDescription("Tournament 3 description")
                                            .WithSeason(2014)
                                            .WithScheme(TournamentSchemeEnum.TwoAndHalf)
                                            .WithRegulationsLink("www.Volleyball.dp.ua/Regulations/Tournaments('3')")
                                            .WithApplyingPeriodStart(new DateTime(2015, 02, 20))
                                            .WithApplyingPeriodEnd(new DateTime(2015, 06, 20))
                                            .WithGamesStart(new DateTime(2015, 06, 30))
                                            .WithGamesEnd(new DateTime(2015, 11, 30))
                                            .WithTransferStart(new DateTime(2015, 08, 20))
                                            .WithTransferEnd(new DateTime(2015, 09, 10))
                                            .Build())
                            .Build();
        }

        private void VerifyCreateTournament(Tournament tournament, Times times)
        {
            _tournamentRepositoryMock.Verify(tr => tr.Add(It.Is<Tournament>(t => TournamentsAreEqual(t, tournament))), times);
            _unitOfWorkMock.Verify(uow => uow.Commit(), times);
        }

        private void VerifyDeleteTournament(int tournamentId, Times times)
        {
            _tournamentRepositoryMock.Verify(tr => tr.Remove(tournamentId), times);
            _unitOfWorkMock.Verify(uow => uow.Commit(), times);
        }

        private void VerifyTeamsAdded(int tourmanentId, Times repositoryTimes, Times uowTimes)
        {
            _tournamentRepositoryMock.Verify(tr => tr.AddTeamToTournament(It.IsAny<int>(), tourmanentId), repositoryTimes);
            _unitOfWorkMock.Verify(uow => uow.Commit(), uowTimes);
        }

        private void VerifyTeamDeleted(int teamId, int tourmanentId, Times times)
        {
            _tournamentRepositoryMock.Verify(tr => tr.RemoveTeamFromTournament(teamId, tourmanentId), times);
            _unitOfWorkMock.Verify(uow => uow.Commit(), times);
        }

        private void VerifyEditTournament(Tournament tournament, Times times)
        {
            _tournamentRepositoryMock.Verify(tr => tr.Update(It.Is<Tournament>(t => TournamentsAreEqual(t, tournament))), times);
            _unitOfWorkMock.Verify(uow => uow.Commit(), times);
        }
    }
}
