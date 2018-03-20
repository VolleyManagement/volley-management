namespace VolleyManagement.UnitTests.Services.TournamentService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using GameReportService;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections;
    using Contracts;
    using Contracts.Authorization;
    using Contracts.Exceptions;
    using Crosscutting.Contracts.Providers;
    using Data.Contracts;
    using Data.Exceptions;
    using Data.Queries.Common;
    using Data.Queries.Division;
    using Data.Queries.Group;
    using Data.Queries.Team;
    using Data.Queries.Tournament;
    using Domain.GamesAggregate;
    using Domain.RolesAggregate;
    using Domain.TeamsAggregate;
    using Domain.TournamentsAggregate;
    using VolleyManagement.Services;
    using Mvc.ViewModels;
    using TeamService;
    using TournamentResources = Domain.Properties.Resources;

    /// <summary>
    /// Tests for TournamentService class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TournamentServiceTests
    {
        private const int MINIMUM_REGISTRATION_PERIOD_MONTH = 3;
        private const int FIRST_TOURNAMENT_ID = 1;
        private const int FIRST_DIVISION_ID = 1;
        private const int SPECIFIC_TEAM_ID = 2;
        private const int SPECIFIC_TOURNAMENT_ID = 2;
        private const int SPECIFIC_GAME_ID = 2;
        private const int SPECIFIC_NUMBER_OF_TIMES = 2;
        private const int EMPTY_TEAM_LIST_COUNT = 0;
        private const int EMPTY_GROUP_LIST_COUNT = 0;
        private const int EMPTY_DIVISION_LIST_COUNT = 0;
        private const int EXPECTED_NOTSTARTED_TOURNAMENTS_COUNT = 4;

        private readonly DateTime _dateForCurrentState = new DateTime(2015, 09, 30);
        private readonly DateTime _dateForFinishedState = new DateTime(2016, 09, 30);
        private readonly DateTime _dateForNotStartedState = new DateTime(2015, 02, 28);

        private readonly TournamentServiceTestFixture _testFixture = new TournamentServiceTestFixture();

        private Mock<ITournamentRepository> _tournamentRepositoryMock;
        private Mock<IAuthorizationService> _authServiceMock;
        private Mock<IGameService> _gameServiceMock;
        private Mock<IQuery<Tournament, UniqueTournamentCriteria>> _uniqueTournamentQueryMock;
        private Mock<IQuery<ICollection<Tournament>, GetAllCriteria>> _getAllQueryMock;
        private Mock<IQuery<Tournament, FindByIdCriteria>> _getByIdQueryMock;
        private Mock<IQuery<List<TeamTournamentDto>, FindByTournamentIdCriteria>> _getAllTournamentTeamsQuery;
        private Mock<IQuery<ICollection<Division>, TournamentDivisionsCriteria>> _getAllTournamentDivisionsQuery;
        private Mock<IQuery<ICollection<Group>, DivisionGroupsCriteria>> _getAllTournamentGroupsQuery;
        private Mock<IQuery<List<TeamTournamentAssignmentDto>, GetAllCriteria>> _getAllGroupsTeamsQuery;
        private Mock<IQuery<ICollection<Team>, GetAllCriteria>> _getAllTeamsQuery;
        private Mock<IQuery<TournamentScheduleDto, TournamentScheduleInfoCriteria>> _getTorunamentDto;
        private Mock<IQuery<Tournament, TournamentByGroupCriteria>> _getTournamentId;
        private Mock<IQuery<ICollection<Tournament>, OldTournamentsCriteria>> _getOldTournamentsQuery;
        private Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

        private Mock<TimeProvider> _timeMock = new Mock<TimeProvider>();

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _tournamentRepositoryMock = new Mock<ITournamentRepository>();
            _authServiceMock = new Mock<IAuthorizationService>();
            _gameServiceMock = new Mock<IGameService>();
            _uniqueTournamentQueryMock = new Mock<IQuery<Tournament, UniqueTournamentCriteria>>();
            _getAllQueryMock = new Mock<IQuery<ICollection<Tournament>, GetAllCriteria>>();
            _getByIdQueryMock = new Mock<IQuery<Tournament, FindByIdCriteria>>();
            _getAllTournamentTeamsQuery = new Mock<IQuery<List<TeamTournamentDto>, FindByTournamentIdCriteria>>();
            _getAllTournamentDivisionsQuery = new Mock<IQuery<ICollection<Division>, TournamentDivisionsCriteria>>();
            _getAllTournamentGroupsQuery = new Mock<IQuery<ICollection<Group>, DivisionGroupsCriteria>>();
            _getAllTeamsQuery = new Mock<IQuery<ICollection<Team>, GetAllCriteria>>();
            _getTorunamentDto = new Mock<IQuery<TournamentScheduleDto, TournamentScheduleInfoCriteria>>();
            _getAllGroupsTeamsQuery = new Mock<IQuery<List<TeamTournamentAssignmentDto>, GetAllCriteria>>();
            _getTournamentId = new Mock<IQuery<Tournament, TournamentByGroupCriteria>>();
            _getOldTournamentsQuery = new Mock<IQuery<ICollection<Tournament>, OldTournamentsCriteria>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

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

        #region FindById

        /// <summary>
        /// Test for FinById method.
        /// </summary>
        [TestMethod]
        public void FindById_Existing_TournamentFound()
        {
            // Arrange
            var sut = BuildSUT();

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
            var sut = BuildSUT();

            // Act
            var tournament = sut.Get(1);

            // Assert
            Assert.IsNull(tournament);
        }
        #endregion

        #region GetAll

        /// <summary>
        /// Test for Get() method. The method should return existing tournaments
        /// (order is important).
        /// </summary>
        [TestMethod]
        public void GetAll_TournamentsExist_TournamentsReturned()
        {
            // Arrange
            MockTimeProviderUtcNow(_dateForCurrentState);

            var testData = _testFixture.TestTournaments()
                                       .Build();
            MockGetOldTournamentsQuery(new List<Tournament>());
            MockGetAllTournamentsQuery(testData);
            var sut = BuildSUT();
            var expected = new TournamentServiceTestFixture()
                                            .TestTournaments()
                                            .Build();

            // Act
            var actual = sut.Get();

            // Assert
            TestHelper.AreEqual(expected, actual, new TournamentComparer());
        }
        #endregion

        #region GetAllTournamentTeams

        /// <summary>
        /// Test for GetAllTournamentTeams method.
        /// The method should return existing teams in specific tournament
        /// </summary>
        [TestMethod]
        public void GetAllTournamentTeams_TeamsExist_TeamsReturned()
        {
            // Arrange
            var testData = new TeamInTournamentTestFixture().WithTeamsInSingleDivisionSingleGroup().Build();
            MockGetAllTournamentTeamsQuery(testData);
            var sut = BuildSUT();
            var expected = new TeamInTournamentTestFixture().WithTeamsInSingleDivisionSingleGroup().Build();

            // Act
            var actual = sut.GetAllTournamentTeams(It.IsAny<int>());

            // Assert
            TestHelper.AreEqual(expected, actual, new TeamInTournamentComparer());
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
            var testData = new TeamInTournamentTestFixture().Build();
            MockGetAllTournamentTeamsQuery(testData);
            var sut = BuildSUT();

            // Act
            var actual = sut.GetAllTournamentTeams(It.IsAny<int>());

            // Assert
            Assert.AreEqual(actual.Count, EMPTY_TEAM_LIST_COUNT);
        }
        #endregion

        #region GetAllTournamentDivisions

        /// <summary>
        /// Test for GetAllTournamentDivisions method.
        /// The method should return existing Divisions in specific tournament
        /// </summary>
        [TestMethod]
        public void GetAllTournamentDivisions_DivisionsExist_DivisionsReturned()
        {
            // Arrange
            var testData = new DivisionTestFixture().TestDivisions().Build();
            MockGetAllTournamentDivisionsQuery(testData);
            var sut = BuildSUT();
            var expected = new DivisionTestFixture().TestDivisions().Build();

            // Act
            var actual = sut.GetAllTournamentDivisions(FIRST_TOURNAMENT_ID);

            // Assert
            TestHelper.AreEqual(expected, actual, new DivisionComparer());
        }

        /// <summary>
        /// Test for GetAllTournamentDivisions method.
        /// No divisions exists in tournament.
        /// The method should return empty division list.
        /// </summary>
        [TestMethod]
        public void GetAllTournamentDivisions_DivisionsNotExist_EmptyDivisionListReturned()
        {
            // Arrange
            var testData = new DivisionTestFixture().Build();
            MockGetAllTournamentDivisionsQuery(testData);
            var sut = BuildSUT();

            // Act
            var actual = sut.GetAllTournamentDivisions(FIRST_TOURNAMENT_ID);

            // Assert
            Assert.AreEqual(actual.Count, EMPTY_DIVISION_LIST_COUNT);
        }
        #endregion

        #region GetAllTournamentGroups

        /// <summary>
        /// Test for GetAllTournamentGroups method.
        /// The method should return existing Groups in specific tournament
        /// </summary>
        [TestMethod]
        public void GetAllTournamentGroups_GroupsExist_GroupsReturned()
        {
            // Arrange
            var testData = new GroupTestFixture().TestGroups().Build();
            MockGetAllTournamentGroupsQuery(testData);
            var sut = BuildSUT();
            var expected = new GroupTestFixture().TestGroups().Build();

            // Act
            var actual = sut.GetAllTournamentGroups(FIRST_DIVISION_ID);

            // Assert
            TestHelper.AreEqual(expected, actual, new GroupComparer());
        }

        /// <summary>
        /// Test for GetAllTournamentGroups method.
        /// No Groups exists in tournament.
        /// The method should return empty Group list.
        /// </summary>
        [TestMethod]
        public void GetAllTournamentGroups_GroupsNotExist_EmptyGroupListReturned()
        {
            // Arrange
            var testData = new GroupTestFixture().Build();
            MockGetAllTournamentGroupsQuery(testData);
            var sut = BuildSUT();

            // Act
            var actual = sut.GetAllTournamentGroups(FIRST_DIVISION_ID);

            // Assert
            Assert.AreEqual(actual.Count, EMPTY_GROUP_LIST_COUNT);
        }
        #endregion

        #region Edit

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
            var sut = BuildSUT();

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
            var sut = BuildSUT();

            // Act
            sut.Edit(testTournament);

            // Assert
            VerifyEditTournament(testTournament, Times.Never());
        }

        /// <summary>
        /// Test for Edit() method with no rights for such action. The method should throw AuthorizationException
        /// and shouldn't invoke Commit() method of IUnitOfWork.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AuthorizationException))]
        public void Edit_NoEditRights_ExceptionThrown()
        {
            // Arrange
            var testTournament = new TournamentBuilder().Build();
            MockAuthServiceThrowsExeption(AuthOperations.Tournaments.Edit);
            var sut = BuildSUT();

            // Act
            sut.Edit(testTournament);

            // Assert
            VerifyEditTournament(testTournament, Times.Never());
            VerifyCheckAccess(AuthOperations.Tournaments.Edit, Times.Once());
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

            var nonUniqueNameTournament = new TournamentBuilder()
                                                        .WithId(2)
                                                        .WithName("Non-Unique Tournament")
                                                        .Build();

            MockGetUniqueTournamentQuery(testData);
            var sut = BuildSUT();

            // Act
            sut.Edit(nonUniqueNameTournament);

            // Assert
            VerifyEditTournament(nonUniqueNameTournament, Times.Never());
        }
        #endregion

        #region Create

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
            var sut = BuildSUT();

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
            var sut = BuildSUT();

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
            var sut = BuildSUT();

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
            var sut = BuildSUT();

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
        [ExpectedException(
            typeof(TournamentValidationException),
            "При наличии трансферного периода необходимо указать дату начала периода")]
        public void Create_TournamentNoTransferStart_ExceptionThrown()
        {
            // Arrange
            var newTournament = new TournamentBuilder().WithTransferStart(null).Build();
            var sut = BuildSUT();

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
        [ExpectedException(
            typeof(TournamentValidationException),
            "При наличии трансферного периода необходимо указать дату окончания периода")]
        public void Create_TournamentNoTransferEnd_ExceptionThrown()
        {
            // Arrange
            var newTournament = new TournamentBuilder().WithTransferEnd(null).Build();
            var sut = BuildSUT();

            // Act
            sut.Create(newTournament);

            // Assert
            VerifyCreateTournament(newTournament, Times.Never());
        }

        /// <summary>
        /// Test for Create() method. Tournament's transfer end date comes after tournament's games end date.
        /// </summary>
        [TestMethod]
        [ExpectedException(
            typeof(TournamentValidationException),
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
            var sut = BuildSUT();

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
            var sut = BuildSUT();

            // Act
            sut.Create(newTournament);

            // Assert
            VerifyCreateTournament(newTournament, Times.Never());
        }

        /// <summary>
        /// Test for Create() method. Tournament's transfer end date comes before tournament's transfer start date.
        /// </summary>
        [TestMethod]
        [ExpectedException(
            typeof(TournamentValidationException),
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
            var sut = BuildSUT();

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
            var sut = BuildSUT();

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
            var sut = BuildSUT();

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
            var applyingPeriodStart = DateTime.UtcNow.AddDays(1);
            var applyingPeriodEnd = applyingPeriodStart.AddDays(1);
            var gamesStart = applyingPeriodEnd.AddDays(1);
            var transferStart = gamesStart.AddDays(1);
            var transferEnd = transferStart.AddDays(1);
            var gamesEnd = transferEnd.AddDays(1);

            var newTournament = new TournamentBuilder().WithApplyingPeriodStart(applyingPeriodStart)
                                                       .WithApplyingPeriodEnd(applyingPeriodEnd)
                                                       .WithGamesStart(gamesStart)
                                                       .WithGamesEnd(gamesEnd)
                                                       .WithTransferStart(transferStart)
                                                       .WithTransferEnd(transferEnd)
                                                       .Build();

            // Act
            var sut = BuildSUT();
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
            var sut = BuildSUT();
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
            var sut = BuildSUT();

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
            var sut = BuildSUT();

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
            var sut = BuildSUT();

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
            var sut = BuildSUT();

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
            var sut = BuildSUT();

            // Act
            sut.Create(newTournament);

            // Assert
            VerifyCreateTournament(newTournament, Times.Never());
        }

        /// <summary>
        /// Test for Create() method with no rights for such action. The method should throw AuthorizationException
        /// and shouldn't invoke Commit() method of IUnitOfWork.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AuthorizationException))]
        public void Create_NoCreateRights_ExceptionThrown()
        {
            // Arrange
            var testTournament = new TournamentBuilder().Build();
            MockAuthServiceThrowsExeption(AuthOperations.Tournaments.Create);
            var sut = BuildSUT();

            // Act
            sut.Create(testTournament);

            // Assert
            VerifyCreateTournament(testTournament, Times.Never());
            VerifyCheckAccess(AuthOperations.Tournaments.Create, Times.Once());
        }
        #endregion

        #region AddTeamsToTournament

        /// <summary>
        /// Test for AddTeamsToTournament method.
        /// Valid teams have to be added.
        /// </summary>
        [TestMethod]
        public void AddTeamsToTournament_ValidTeamList_TeamsAreAdded()
        {
            // Arrange
            var testData = new GroupTeamServiceTestFixture().TestGroupsTeams().Build();
            MockGetAllTournamentTeamsQuery(new TeamInTournamentTestFixture().Build());

            var tournament = new TournamentBuilder().WithScheme(TournamentSchemeEnum.PlayOff).Build();
            MockGetTournamentByGroupId(tournament);
            MockGetByIdQuery(tournament);
            MockGetTournamentGames(tournament.Id);

            var sut = BuildSUT();

            // Act
            sut.AddTeamsToTournament(testData);

            // Assert
            VerifyTeamsAdded(Times.Exactly(testData.Count), Times.Once());
        }

        /// <summary>
        /// Test for AddTeamsToTournament method.
        /// InValid teams must not be added.
        /// Method have to throw ArgumentException
        /// </summary>
        [TestMethod]
        public void AddTeamsToTournament_InValidTeamList_ArgumentExceptionThrown()
        {
            var gotException = false;

            // Arrange
            var tournament = new TournamentBuilder()
                .Build();
            var testData = new GroupTeamServiceTestFixture().TestGroupsTeams().Build();
            MockGetAllTournamentGroupTeamQuery(new GroupTeamServiceTestFixture().TestGroupsTeams().Build());
            MockGetTournamentByGroupId(tournament);
            var sut = BuildSUT();

            // Act
            try
            {
                sut.AddTeamsToTournament(testData);
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
        /// Test for AddTeamsToTournament() method with no rights for such action. The method should throw AuthorizationException
        /// and shouldn't invoke Commit() method of IUnitOfWork.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AuthorizationException))]
        public void AddTeamsToTournament_NoManageRights_ExceptionThrown()
        {
            // Arrange
            var testData = new GroupTeamServiceTestFixture().TestGroupsTeams().Build();
            MockAuthServiceThrowsExeption(AuthOperations.Tournaments.ManageTeams);
            var sut = BuildSUT();

            // Act
            sut.AddTeamsToTournament(testData);

            // Assert
            _unitOfWorkMock.Verify(uow => uow.Commit(), Times.Never());
            VerifyCheckAccess(AuthOperations.Tournaments.ManageTeams, Times.Once());
        }

        /// <summary>
        /// Test for AddTeamsToTournament method.
        /// Valid teams have to be added into playoff tournament.
        /// Schedule created.
        /// </summary>
        [TestMethod]
        public void AddTeamsToTournament_ValidTeamListPlayOffScheme_ScheduleCreated()
        {
            // Arrange
            var testData = new GroupTeamServiceTestFixture().TestGroupsTeams().Build();
            var tournament = new TournamentBuilder()
                .WithScheme(TournamentSchemeEnum.PlayOff)
                .Build();

            MockGetTournamentByGroupId(tournament);
            MockGetByIdQuery(tournament);
            MockGetTournamentGames(tournament.Id);
            var testTeamsData = new TeamInTournamentTestFixture().WithTeamsInSingleDivisionSingleGroup().Build();
            MockGetAllTournamentTeamsQueryTwoCalls(new TeamInTournamentTestFixture().Build(), testTeamsData);

            var sut = BuildSUT();

            // Act
            sut.AddTeamsToTournament(testData);

            // Assert
            VerifyCreateSchedule(FIRST_TOURNAMENT_ID, Times.Once(), Times.Once());
        }

        /// <summary>
        /// Test for AddTeamsToTournament() method. The method check if team already exist
        /// Throw exeption
        /// </summary>
        [TestMethod]
        public void AddTeamsToTournament_TeamIsAlreadyExist_ValidationExceptionThrown()
        {
            // Arrange
            var testData = new GroupTeamServiceTestFixture().TestGroupsTeams().Build();
            MockGetAllTournamentGroupTeamQuery(testData);
            var tournament = new TournamentBuilder().Build();
            MockGetByIdQuery(tournament);
            MockGetAllTournamentTeamsQuery(CreateTeamsInTournament());
            MockGetTournamentByGroupId(tournament);
            var sut = BuildSUT();
            Exception exception = null;
            var argExMessage =
                TournamentResources.TeamNameInCurrentGroupOfTournamentNotUnique;

            // Act
            try
            {
                sut.AddTeamsToTournament(testData);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(
                exception,
                new ArgumentException(argExMessage));
        }

        /// <summary>
        /// Test for AddTeamsToTournament() method. The method check if list of new Teams is empty
        /// Throw exeption
        /// </summary>
        [TestMethod]
        public void AddTeamsToTournament_CollectionOfNewTeamsIsEmpty_ValidationExceptionThrown()
        {
            // Arrange
            var testData = new GroupTeamServiceTestFixture().Build();

            var sut = BuildSUT();
            Exception exception = null;
            var argExMessage =
                TournamentResources.CollectionIsEmpty;

            // Act
            try
            {
                sut.AddTeamsToTournament(testData);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(
                exception,
                new ArgumentException(argExMessage));
        }

        /// <summary>
        /// Test for AddTeamsToTournament method.
        /// Valid teams have to be added.
        /// </summary>
        [TestMethod]
        public void AddTeamsToTournament_AddTeamInSecondGroupOfSecondDivision_TeamIsAdded()
        {
            // Arrange
            var testData = new GroupTeamServiceTestFixture()
                .TestGroupsTeamsWithTeamInSecondDivisionSecondGroup().Build();
            MockGetAllTournamentGroupTeamQuery(testData);

            var tournament = new TournamentBuilder().Build();
            MockGetByIdQuery(tournament);

            var teamsToAddInSecondDivision = new List<TeamTournamentDto> { new TeamTournamentDto { TeamId = SPECIFIC_TEAM_ID } };
            MockGetAllTournamentTeamsQuery(teamsToAddInSecondDivision);

            MockGetTournamentByGroupId(tournament);
            MockGetTournamentGames(tournament.Id);

            var sut = BuildSUT();

            // Act
            sut.AddTeamsToTournament(testData);

            // Assert
            VerifyTeamsAdded(Times.Exactly(testData.Count), Times.Once());
        }

        /// <summary>
        /// Test for AddTeamsToTournament method.
        /// Valid teams have to be added.
        /// </summary>
        [TestMethod]
        public void AddTeamsToTournament_AddThreeTeamsTwoAlreadyExist_ValidationExceptionThrown()
        {
            // Arrange
            var testData = new GroupTeamServiceTestFixture().TestGroupsTeams().Build();
            MockGetAllTournamentGroupTeamQuery(testData);

            var tournament = new TournamentBuilder().Build();
            MockGetByIdQuery(tournament);
            MockGetAllTournamentTeamsQuery(CreateTeamsInTournament());
            MockGetTournamentByGroupId(tournament);

            var newTeamsInTournament = new GroupTeamServiceTestFixture()
                .TestGroupsTeamsWithAlreadyExistTeam().Build();

            var sut = BuildSUT();

            Exception exception = null;

            var argExMessage =
                TournamentResources.TeamNameInCurrentGroupOfTournamentNotUnique;

            // Act
            try
            {
                sut.AddTeamsToTournament(newTeamsInTournament);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(
                exception,
                new ArgumentException(argExMessage));

            VerifyAddTeamsToTournament(Times.Exactly(2));
        }

        #endregion

        #region DeleteTeamFromTournament

        /// <summary>
        /// Test for DeleteTeamFromTournament method.
        /// Team have to be removed from tournament
        /// </summary>
        [TestMethod]
        public void DeleteTeamFromTournament_TeamExist_TeamDeleted()
        {
            // Arrange
            var tournament = new TournamentBuilder().Build();
            MockGetByIdQuery(tournament);

            var testTeamsData = new TeamInTournamentTestFixture().WithTeamsInSingleDivisionSingleGroup().Build();
            MockGetAllTournamentTeamsQuery(testTeamsData);
            MockGetTournamentGames(tournament.Id);

            var sut = BuildSUT();

            // Act
            sut.DeleteTeamFromTournament(SPECIFIC_TEAM_ID, FIRST_TOURNAMENT_ID);

            // Assert
            VerifyTeamDeleted(SPECIFIC_TEAM_ID, FIRST_TOURNAMENT_ID, Times.Once(), Times.Once());
        }

        /// <summary>
        /// Test for DeleteTeamFromTournament method.
        /// Team is not exist in tournament
        /// Missing Entity Exception have to be thrown
        /// </summary>
        [TestMethod]
        public void DeleteTeamFromTournament_TeamNotExist_MissingEntityExceptionThrown()
        {
            var gotException = false;

            // Arrange
            _tournamentRepositoryMock
                .Setup(tr => tr.RemoveTeamFromTournament(It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new ConcurrencyException());

            var sut = BuildSUT();

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
        /// Test for DeleteTeamFromTournament() method with no rights for such action. The method should throw AuthorizationException
        /// and shouldn't invoke Commit() method of IUnitOfWork.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AuthorizationException))]
        public void DeleteTeamFromTournament_NoManageRights_ExceptionThrown()
        {
            // Arrange
            MockAuthServiceThrowsExeption(AuthOperations.Tournaments.ManageTeams);
            var sut = BuildSUT();

            // Act
            sut.DeleteTeamFromTournament(SPECIFIC_TEAM_ID, FIRST_TOURNAMENT_ID);

            // Assert
            _unitOfWorkMock.Verify(uow => uow.Commit(), Times.Never());
            VerifyCheckAccess(AuthOperations.Tournaments.ManageTeams, Times.Once());
        }

        /// <summary>
        /// Test for DeleteTeamFromTournament method.
        /// Team have to be removed from playoff tournament
        /// Schedule created
        /// </summary>
        [TestMethod]
        public void DeleteTeamFromTournament_TeamExistPlayOffScheme_ScheduleCreated()
        {
            // Arrange
            var tournament = new TournamentBuilder()
                .WithScheme(TournamentSchemeEnum.PlayOff)
                .Build();

            MockGetByIdQuery(tournament);
            var testTeamsData = new TeamInTournamentTestFixture().WithTeamsInSingleDivisionSingleGroup().Build();
            MockGetAllTournamentTeamsQuery(testTeamsData);
            MockGetTournamentGames(tournament.Id);

            var sut = BuildSUT();

            // Act
            sut.DeleteTeamFromTournament(SPECIFIC_TEAM_ID, FIRST_TOURNAMENT_ID);

            // Assert
            VerifyCreateSchedule(FIRST_TOURNAMENT_ID, Times.Once(), Times.Once());
        }

        #endregion

        #region Delete

        /// <summary>
        /// Test for Delete Tournament without Teams method.
        /// </summary>
        [TestMethod]
        public void Delete_DeleteTournamentsWithNoTeams_TournamentRemoved()
        {
            // Arrange
            var sut = BuildSUT();

            // Act
            sut.Delete(FIRST_TOURNAMENT_ID);

            // Assert
            VerifyDeleteTournament(FIRST_TOURNAMENT_ID, Times.Once());
        }

        /// <summary>
        /// Test for Delete Tournament with Teams method.
        /// </summary>
        [TestMethod]
        public void Delete_DeleteTournamentsWithTeams_TournamentRemoved()
        {
            // Arrange
            var tournament = new TournamentBuilder()
                .WithScheme(TournamentSchemeEnum.PlayOff)
                .Build();

            MockGetByIdQuery(tournament);
            var existingTeams = CreateTeamsInTournament();
            MockGetAllTournamentTeamsQuery(existingTeams);
            var sut = BuildSUT();

            // Act
            sut.Delete(FIRST_TOURNAMENT_ID);

            // Assert
            VerifyDeleteTournament(FIRST_TOURNAMENT_ID, Times.Once());
        }

        /// <summary>
        /// Test for Delete teams from current Tournament
        /// </summary>
        [TestMethod]
        public void Delete_DeleteTournamentsWithTeams_TeamsRemoved()
        {
            // Arrange
            var tournament = new TournamentBuilder()
                .WithScheme(TournamentSchemeEnum.PlayOff)
                .Build();

            MockGetByIdQuery(tournament);
            var existingTeams = CreateTeamsInTournament();
            MockGetAllTournamentTeamsQuery(existingTeams);
            var sut = BuildSUT();

            // Act
            sut.Delete(FIRST_TOURNAMENT_ID);

            // Assert
            VerifyTeamsRemoved(FIRST_TOURNAMENT_ID, Times.Exactly(SPECIFIC_NUMBER_OF_TIMES));
        }

        /// <summary>
        /// Test for Delete divisions from current Tournament
        /// </summary>
        [TestMethod]
        public void Delete_DeleteTournamentsWithDivisions_DivisionsRemoved()
        {
            // Arrange
            var tournament = new TournamentBuilder()
                .WithScheme(TournamentSchemeEnum.PlayOff)
                .Build();

            MockGetByIdQuery(tournament);
            MockGetAllTournamentDivisionsQuery(tournament.Divisions);
            var sut = BuildSUT();

            // Act
            sut.Delete(FIRST_TOURNAMENT_ID);

            // Assert
            VerifyDivisionsDeleted(Times.Exactly(SPECIFIC_NUMBER_OF_TIMES));
        }

        /// <summary>
        /// Test for Delete Game results from current Tournament
        /// </summary>
        [TestMethod]
        public void Delete_DeleteTournamentsWithGameResults_GameResultsRemoved()
        {
            // Arrange
            var tournament = new TournamentBuilder()
                .WithScheme(TournamentSchemeEnum.PlayOff)
                .Build();

            MockGetByIdQuery(tournament);
            var sut = BuildSUT();

            // Act
            sut.Delete(FIRST_TOURNAMENT_ID);

            // Assert
            _gameServiceMock.Verify(gs => gs.RemoveAllGamesInTournament(FIRST_TOURNAMENT_ID), Times.Once());
        }

        /// <summary>
        /// Test for Delete() method with no rights for such action. The method should throw AuthorizationException
        /// and shouldn't invoke Commit() method of IUnitOfWork.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AuthorizationException))]
        public void Delete_NoDeleteRights_ExceptionThrown()
        {
            // Arrange
            MockAuthServiceThrowsExeption(AuthOperations.Tournaments.Delete);
            var sut = BuildSUT();

            // Act
            sut.Delete(FIRST_TOURNAMENT_ID);

            // Assert
            VerifyDeleteTournament(FIRST_TOURNAMENT_ID, Times.Never());
            VerifyCheckAccess(AuthOperations.Tournaments.Delete, Times.Once());
        }
        #endregion

        #region Archive

        /// <summary>
        /// Test for Archive(int id) tournament method.
        /// </summary>
        [TestMethod]
        public void Archive_NotArchivedTournament_CommitInvoked()
        {
            // Arrange
            var expectedTournament = new TournamentBuilder()
                                    .WithArchiveParameter(false)
                                    .Build();
            var actualTournament = new TournamentBuilder()
                                    .WithArchiveParameter(false)
                                    .Build();
            MockGetByIdQuery(actualTournament);
            MockGetUniqueTournamentQuery(expectedTournament);
            var sut = BuildSUT();

            // Act
            sut.Archive(FIRST_TOURNAMENT_ID);

            // Assert
            VerifyCommit(Times.Once(), "The tournament was not archived.");
        }

        /// <summary>
        /// Test for Archive() method with any state. Whetever 'CheckAccess' method invokes
        /// </summary>
        [TestMethod]
        public void Archive_AnyState_AuthorizationCheckInvoked()
        {
            // Arrange
            var testTournament = new TournamentBuilder().Build();
            MockGetByIdQuery(testTournament);
            var sut = BuildSUT();

            // Act
            sut.Archive(FIRST_TOURNAMENT_ID);

            // Assert
            VerifyCheckAccess(AuthOperations.Tournaments.Archive, Times.Once());
        }

        #endregion

        #region ArchiveOld

        /// <summary>
        /// Test for ArchiveOld() method. Old tournaments exist. Old tournaments archived
        /// </summary>
        [TestMethod]
        public void ArchiveOld_OldTournamentsExist_OldTournamentsArchived()
        {
            // Arrange
            var testData = _testFixture.WithFinishedTournaments().Build();
            MockGetOldTournamentsQuery(testData);
            MockGetAllTournamentsQuery(testData);

            var sut = BuildSUT();

            MockTimeProviderUtcNow(_dateForFinishedState);

            // Act
            sut.ArchiveOld();

            // Assert
            VerifyCommit(Times.Once(), "Old tournaments were not archived.");
        }

        /// <summary>
        /// Test for ArchiveOld() method. Old tournaments don't exist. No tournaments archived
        /// </summary>
        [TestMethod]
        public void ArchiveOld_NoOldTournamentsExist_NoTournamentsArchived()
        {
            var testData = _testFixture.Build();
            MockGetOldTournamentsQuery(testData);
            MockGetAllTournamentsQuery(testData);

            var sut = BuildSUT();

            MockTimeProviderUtcNow(_dateForFinishedState);

            // Act
            sut.ArchiveOld();

            // Assert
            VerifyCommit(Times.Never(), "No tournaments should be archived.");
        }
        #endregion

        #region GetActual

        /// <summary>
        /// GetActual method test. The method should return actual tournaments
        /// </summary>
        [TestMethod]
        public void GetActual_TournamentsExist_ActualTournamentsReturnes()
        {
            // Arrange
            var testData = _testFixture.TestTournaments().Build();
            MockGetAllTournamentsQuery(testData);
            MockGetOldTournamentsQuery(new List<Tournament>());

            var sut = BuildSUT();

            var expected = BuildActualTournamentsList();

            // Act
            var actual = sut.GetActual();

            // Assert
            TestHelper.AreEqual(expected, actual, new TournamentComparer());
        }

        /// <summary>
        /// GetActual method test. The method should return actual tournaments
        /// </summary>
        [TestMethod]
        public void GetActual_TournamentsExist_CurrentTournamentsReturned()
        {
            // Arrange
            MockTimeProviderUtcNow(_dateForCurrentState);

            var testData = _testFixture.TestTournaments().Build();
            MockGetAllTournamentsQuery(testData);
            MockGetOldTournamentsQuery(new List<Tournament>());
            var expected = BuildActualTournamentsList();

            var sut = BuildSUT();

            // Act
            var actual = sut.GetActual();

            // Assert
            TestHelper.AreEqual(expected, actual, new TournamentComparer());
        }

        /// <summary>
        /// GetActual method test. Tournament list includes archived tournaments.
        /// The method should return actual tournaments
        /// </summary>
        [TestMethod]
        public void GetActual_ActualTournamentsWithArchived__ActualOnlyReturned()
        {
            // Arrange
            MockTimeProviderUtcNow(_dateForCurrentState);

            var testData = _testFixture.TestTournaments()
                .WithArchivedTournaments()
                .Build();
            MockGetAllTournamentsQuery(testData);
            MockGetOldTournamentsQuery(new List<Tournament>());

            var sut = BuildSUT();

            var expected = BuildActualTournamentsList();

            // Act
            var actual = sut.GetActual();

            // Assert
            TestHelper.AreEqual(expected, actual, new TournamentComparer());
        }
        #endregion

        #region GetArchived

        /// <summary>
        /// GetArchived method test. The method should return archived tournaments
        /// </summary>
        [TestMethod]
        public void GetArchived_ArchivedTournamentsExist_ArchivedTournamentsReturned()
        {
            // Arrange
            MockTimeProviderUtcNow(_dateForCurrentState);
            var testData = _testFixture.TestTournaments()
                .WithArchivedTournaments()
                .Build();
            MockGetAllTournamentsQuery(testData);
            MockGetOldTournamentsQuery(new List<Tournament>());

            var sut = BuildSUT();

            var expected = BuildArchivedTournamentsList();

            // Act
            var actual = sut.GetArchived();

            // Assert
            TestHelper.AreEqual(expected, actual, new TournamentComparer());
        }

        /// <summary>
        /// Test for GetArchived() with any state. The method should invoke CheckAccess method.
        /// </summary>
        [TestMethod]
        public void GetArchived_AnyState_AuthorizationCheckInvoked()
        {
            // Arrange
            var testData = _testFixture.TestTournaments()
                .WithArchivedTournaments()
                .Build();
            MockGetAllTournamentsQuery(testData);
            MockGetOldTournamentsQuery(testData);
            var sut = BuildSUT();

            // Act
            sut.GetArchived();

            // Assert
            VerifyCheckAccess(AuthOperations.Tournaments.ViewArchived, Times.Once());
        }
        #endregion

        #region GetFinished

        /// <summary>
        /// GetActual method test. The method should invoke Find() method of ITournamentRepository
        /// </summary>
        [TestMethod]
        public void GetFinished_FinishTournamentsExist_FinishedTournamentsReturned()
        {
            // Arrange
            var testData = _testFixture.WithFinishedTournaments().Build();
            MockGetAllTournamentsQuery(testData);
            MockGetOldTournamentsQuery(new List<Tournament>());

            var sut = BuildSUT();
            var expected = new TournamentServiceTestFixture()
                .WithFinishedTournaments()
                .Build();

            // Act
            var actual = sut.GetFinished();

            // Assert
            TestHelper.AreEqual(expected, actual, new TournamentComparer());
        }

        /// <summary>
        /// GetFinished method test. The method should return finished tournaments
        /// </summary>
        [TestMethod]
        public void GetFinished_TournamentsExist_FinishedTournamentsReturned()
        {
            // Arrange
            MockTimeProviderUtcNow(_dateForFinishedState);
            var testData = _testFixture.TestTournaments().Build();
            MockGetAllTournamentsQuery(testData);
            MockGetOldTournamentsQuery(new List<Tournament>());

            var sut = BuildSUT();
            var expected = BuildActualTournamentsList();

            // Act
            var actual = sut.GetFinished();

            // Assert
            TestHelper.AreEqual(expected, actual, new TournamentComparer());
        }

        /// <summary>
        /// GetFinished method test. Tournament list includes archived tournaments.
        /// The method should return finished tournaments
        /// </summary>
        [TestMethod]
        public void GetFinished_FinishedTournamentsWithArchived_OnlyFinishedTournamentsReturned()
        {
            // Arrange
            var testData = _testFixture
                .WithFinishedTournaments()
                .WithArchivedTournaments()
                .Build();
            MockGetAllTournamentsQuery(testData);
            MockGetOldTournamentsQuery(new List<Tournament>());

            var sut = BuildSUT();
            var expected = new TournamentServiceTestFixture()
                .WithFinishedTournaments()
                .Build();

            // Act
            var actual = sut.GetFinished();

            // Assert
            TestHelper.AreEqual(expected, actual, new TournamentComparer());
        }
        #endregion

        #region Get

        /// <summary>
        /// Get method test. The method returns all tournaments.
        /// Then select all not started tournaments
        /// </summary>
        [TestMethod]
        public void Get_TournamentsExist_NotStartedTournamentsReturned()
        {
            // Arrange
            MockTimeProviderUtcNow(_dateForNotStartedState);
            var testData = _testFixture.TestTournaments().Build();
            MockGetOldTournamentsQuery(testData);
            MockGetAllTournamentsQuery(testData);

            var sut = BuildSUT();

            // Act
            var actual = sut.Get();

            // Assert
            var actualCount = actual.Count(t => t.State == TournamentStateEnum.NotStarted);
            Assert.AreEqual(EXPECTED_NOTSTARTED_TOURNAMENTS_COUNT, actualCount);
        }
        #endregion

        #region Private

        private TournamentService BuildSUT()
        {
            return new TournamentService(
                _tournamentRepositoryMock.Object,
                _uniqueTournamentQueryMock.Object,
                _getAllQueryMock.Object,
                _getByIdQueryMock.Object,
                _getAllTeamsQuery.Object,
                _getAllTournamentDivisionsQuery.Object,
                _getAllTournamentGroupsQuery.Object,
                _getTorunamentDto.Object,
                _getTournamentId.Object,
                _getOldTournamentsQuery.Object,
                _getAllTournamentTeamsQuery.Object,
                _authServiceMock.Object,
                _gameServiceMock.Object);
        }

        private bool TournamentsAreEqual(Tournament x, Tournament y)
        {
            return new TournamentComparer().Compare(x, y) == 0;
        }

        private void MockGetAllTournamentsQuery(IEnumerable<Tournament> testData)
        {
            _getAllQueryMock.Setup(tr => tr.Execute(It.IsAny<GetAllCriteria>())).Returns(testData.ToList());
        }

        private void MockGetOldTournamentsQuery(IEnumerable<Tournament> testData)
        {
            _getOldTournamentsQuery.Setup(tr => tr.Execute(It.IsAny<OldTournamentsCriteria>())).Returns(testData.ToList());
        }

        private void MockGetByIdQuery(Tournament testData)
        {
            _getByIdQueryMock.Setup(tr => tr.Execute(It.IsAny<FindByIdCriteria>())).Returns(testData);
        }

        private void MockGetTournamentGames(int tournamentId)
        {
            _gameServiceMock.Setup(tr => tr.GetTournamentGames(tournamentId)).Returns(new List<GameResultDto>());
        }
        private void MockGetUniqueTournamentQuery(Tournament testData)
        {
            _uniqueTournamentQueryMock.Setup(tr => tr.Execute(It.IsAny<UniqueTournamentCriteria>())).Returns(testData);
        }

        private void MockGetAllTournamentTeamsQuery(List<TeamTournamentDto> testData)
        {
            _getAllTournamentTeamsQuery.Setup(tr => tr.Execute(It.IsAny<FindByTournamentIdCriteria>())).Returns(testData);
        }

        private void MockGetAllTournamentDivisionsQuery(ICollection<Division> testData)
        {
            _getAllTournamentDivisionsQuery.Setup(tr => tr.Execute(It.IsAny<TournamentDivisionsCriteria>())).Returns(testData);
        }

        private void MockGetAllTournamentGroupsQuery(List<Group> testData)
        {
            _getAllTournamentGroupsQuery.Setup(tr => tr.Execute(It.IsAny<DivisionGroupsCriteria>())).Returns(testData);
        }

        private void MockGetAllTournamentGroupTeamQuery(List<TeamTournamentAssignmentDto> groupteam)
        {
            _getAllGroupsTeamsQuery.Setup(gt => gt.Execute(It.IsAny<GetAllCriteria>())).Returns(groupteam);
        }

        private void MockGetAllTournamentTeamsQueryTwoCalls(List<TeamTournamentDto> firstCallTestData, List<TeamTournamentDto> secondCallTestData)
        {
            _getAllTournamentTeamsQuery.SetupSequence(tr => tr.Execute(It.IsAny<FindByTournamentIdCriteria>()))
                .Returns(firstCallTestData)
                .Returns(secondCallTestData);
        }

        private void MockTimeProviderUtcNow(DateTime date)
        {
            _timeMock.Setup(c => c.UtcNow).Returns(date);
        }

        private void MockAuthServiceThrowsExeption(AuthOperation operation)
        {
            _authServiceMock.Setup(tr => tr.CheckAccess(operation)).Throws<AuthorizationException>();
        }

        private void MockGetTournamentByGroupId(Tournament testData)
        {
            _getTournamentId.Setup(tr => tr.Execute(It.IsAny<TournamentByGroupCriteria>())).Returns(testData);
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
                            .AddTournament(new TournamentBuilder()
                                            .WithId(4)
                                            .WithName("Tournament 4")
                                            .WithDescription("Tournament 4 description")
                                            .WithSeason(2014)
                                            .WithScheme(TournamentSchemeEnum.PlayOff)
                                            .WithRegulationsLink("www.Volleyball.dp.ua/Regulations/Tournaments('4')")
                                            .WithApplyingPeriodStart(new DateTime(2015, 02, 20))
                                            .WithApplyingPeriodEnd(new DateTime(2015, 06, 20))
                                            .WithGamesStart(new DateTime(2015, 06, 30))
                                            .WithGamesEnd(new DateTime(2015, 11, 30))
                                            .WithTransferStart(new DateTime(2015, 08, 20))
                                            .WithTransferEnd(new DateTime(2015, 09, 10))
                                            .Build())
                            .Build();
        }

        private List<Tournament> BuildArchivedTournamentsList()
        {
            var tournaments = new TournamentServiceTestFixture()
                            .AddTournament(new TournamentBuilder()
                                            .WithId(5)
                                            .WithName("Tournament 5")
                                            .WithDescription("Tournament 5 description")
                                            .WithSeason(2014)
                                            .WithScheme(TournamentSchemeEnum.TwoAndHalf)
                                            .WithRegulationsLink("www.Volleyball.dp.ua/Regulations/Tournaments('5')")
                                            .WithApplyingPeriodStart(new DateTime(2015, 02, 20))
                                            .WithApplyingPeriodEnd(new DateTime(2015, 06, 20))
                                            .WithGamesStart(new DateTime(2015, 06, 30))
                                            .WithGamesEnd(new DateTime(2015, 11, 30))
                                            .WithTransferStart(new DateTime(2015, 08, 20))
                                            .WithTransferEnd(new DateTime(2015, 09, 10))
                                            .Build())
                            .AddTournament(new TournamentBuilder()
                                            .WithId(6)
                                            .WithName("Tournament 6")
                                            .WithDescription("Tournament 6 description")
                                            .WithSeason(2014)
                                            .WithScheme(TournamentSchemeEnum.PlayOff)
                                            .WithRegulationsLink("www.Volleyball.dp.ua/Regulations/Tournaments('6')")
                                            .WithApplyingPeriodStart(new DateTime(2015, 02, 20))
                                            .WithApplyingPeriodEnd(new DateTime(2015, 06, 20))
                                            .WithGamesStart(new DateTime(2015, 06, 30))
                                            .WithGamesEnd(new DateTime(2015, 11, 30))
                                            .WithTransferStart(new DateTime(2015, 08, 20))
                                            .WithTransferEnd(new DateTime(2015, 09, 10))
                                            .Build())
                            .Build();

            foreach (var t in tournaments)
            {
                t.IsArchived = true;
            }

            return tournaments;
        }

        private List<TeamTournamentDto> CreateTeamsInTournament()
        {
            var existingTeams = new List<TeamTournamentDto>
            {
                new TeamTournamentDto { TeamId = SPECIFIC_TEAM_ID },
                new TeamTournamentDto { TeamId = SPECIFIC_TEAM_ID + 1 },
            };
            return existingTeams;
        }

        private void VerifyCreateTournament(Tournament tournament, Times times)
        {
            _tournamentRepositoryMock.Verify(tr => tr.Add(It.Is<Tournament>(t => TournamentsAreEqual(t, tournament))), times);
            _unitOfWorkMock.Verify(uow => uow.Commit(), times);
        }

        private void VerifyDeleteTournament(int tournamentId, Times times)
        {
            _tournamentRepositoryMock.Verify(tr => tr.Remove(tournamentId), times);
            _unitOfWorkMock.Verify(uow => uow.Commit(), Times.Once());
        }

        private void VerifyTeamsAdded(Times repositoryTimes, Times uowTimes)
        {
            _tournamentRepositoryMock.Verify(tr => tr.AddTeamToTournament(It.IsAny<int>(), It.IsAny<int>()), repositoryTimes);
            _unitOfWorkMock.Verify(uow => uow.Commit(), uowTimes);
        }

        private void VerifyTeamDeleted(int teamId, int tourmanentId, Times times, Times uowTimes)
        {
            _tournamentRepositoryMock.Verify(tr => tr.RemoveTeamFromTournament(teamId, tourmanentId), times);
            _unitOfWorkMock.Verify(uow => uow.Commit(), uowTimes);
        }

        private void VerifyTeamsRemoved(int tourmanentId, Times times)
        {
            _tournamentRepositoryMock.Verify(tr => tr.RemoveTeamFromTournament(It.IsAny<int>(), tourmanentId), times);
            _unitOfWorkMock.Verify(uow => uow.Commit(), Times.Once());
        }

        private void VerifyDivisionsDeleted(Times times)
        {
            _tournamentRepositoryMock.Verify(tr => tr.RemoveDivision(It.IsAny<int>()), times);
            _unitOfWorkMock.Verify(uow => uow.Commit(), Times.Once());
        }

        private void VerifyEditTournament(Tournament tournament, Times times)
        {
            _tournamentRepositoryMock.Verify(tr => tr.Update(It.Is<Tournament>(t => TournamentsAreEqual(t, tournament))), times);
            _unitOfWorkMock.Verify(uow => uow.Commit(), times);
        }

        private void VerifyCheckAccess(AuthOperation operation, Times times)
        {
            _authServiceMock.Verify(tr => tr.CheckAccess(operation), times);
        }

        private void VerifyCreateSchedule(int tourmanentId, Times times, Times uowTimes)
        {
            _gameServiceMock.Verify(gs => gs.RemoveAllGamesInTournament(tourmanentId), times);
            _gameServiceMock.Verify(gs => gs.AddGames(It.IsAny<List<Game>>()), times);
            _unitOfWorkMock.Verify(uow => uow.Commit(), uowTimes);
        }

        private void VerifyAddTeamsToTournament(Times times)
        {
            _tournamentRepositoryMock.Verify(ts => ts.AddTeamToTournament(It.IsAny<int>(), It.IsAny<int>()), times);
        }

        /// <summary>
        /// Verify if Commit() is invoked certain amount of times
        /// </summary>
        /// <param name="times">Amount of times Commit has to be invoked</param>
        /// <param name="message">Message to show if verify failed</param>
        private void VerifyCommit(Times times, string message)
        {
            _unitOfWorkMock.Verify(uow => uow.Commit(), times, message);
        }

        /// <summary>
        /// Checks if exception was thrown and has appropriate message
        /// Checks if thrown exceptions are of the same type
        /// </summary>
        /// <param name="exception">Exception that has been thrown</param>
        /// <param name="expected">Expected exception</param>
        private void VerifyExceptionThrown(Exception exception, Exception expected)
        {
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception.GetType().Equals(expected.GetType()), "Exception is of the wrong type");
            Assert.IsTrue(exception.Message.Equals(expected.Message));
        }

        #endregion
    }
}