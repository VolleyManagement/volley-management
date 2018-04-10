namespace VolleyManagement.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
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
    using DivisionConstants = Domain.Constants.Division;
    using GroupConstants = Domain.Constants.Group;
    using TournamentConstants = Domain.Constants.Tournament;
    using TournamentResources = Domain.Properties.Resources;

#pragma warning disable S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)
    /// <summary>
    /// Defines TournamentService
    /// </summary>
    public class TournamentService : ITournamentService
#pragma warning restore S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)
    {
        #region Const & Readonly

        private const int MIN_TEAMS_TO_PLAY_ONE_ROUND = 2;
        private const int GAMES_TO_PLAY_ONE_ROUND = 2;
        private const int DONT_CREATE_SCHEDULE_TEAMS_COUNT = 1;

        private static readonly TournamentStateEnum[] _actualStates =
            {
                TournamentStateEnum.Current, TournamentStateEnum.Upcoming
            };

        private static readonly TournamentStateEnum[] _finishedStates =
            {
                TournamentStateEnum.Finished
            };

        #endregion

        #region Fields

        private readonly ITournamentRepository _tournamentRepository;
        private readonly IAuthorizationService _authService;
        private readonly IGameService _gameService;

        #endregion

        #region Query Objects

        private readonly IQuery<Tournament, UniqueTournamentCriteria> _uniqueTournamentQuery;
        private readonly IQuery<ICollection<Tournament>, GetAllCriteria> _getAllQuery;
        private readonly IQuery<Tournament, FindByIdCriteria> _getByIdQuery;
        private readonly IQuery<ICollection<Team>, GetAllCriteria> _getAllTeamsQuery;
        private readonly IQuery<ICollection<TeamTournamentDto>, FindByTournamentIdCriteria> _tournamentTeamsQuery;
        private readonly IQuery<ICollection<Division>, TournamentDivisionsCriteria> _getAllTournamentDivisionsQuery;
        private readonly IQuery<ICollection<Group>, DivisionGroupsCriteria> _getAllTournamentGroupsQuery;
        private readonly IQuery<TournamentScheduleDto, TournamentScheduleInfoCriteria> _getTournamentDtoQuery;
        private readonly IQuery<Tournament, TournamentByGroupCriteria> _getTournamenrByGroupQuery;
        private readonly IQuery<ICollection<Tournament>, OldTournamentsCriteria> _getOldTournamentsQuery;

        #endregion

        #region Constructor

#pragma warning disable S107 // Methods should not have too many parameters
        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentService"/> class
        /// </summary>
        /// <param name="tournamentRepository"> The tournament repository.</param>
        /// <param name="uniqueTournamentQuery"> First By Name object query.</param>
        /// <param name="getAllQuery"> Get All object query. </param>
        /// <param name="getByIdQuery">Get tournament by id query.</param>
        /// <param name="getAllTeamsQuery">Get All Teams query.</param>
        /// <param name="tournamentTeamsQuery">Get All Tournament Teams query.</param>
        /// <param name="getAllTournamentDivisionsQuery">Get All Tournament Divisions query.</param>
        /// <param name="getAllTournamentGroupsQuery">Get All Tournament Groups query.</param>
        /// <param name="getTournamentDtoQuery">Get tournament data transfer object query.</param>
        /// <param name="getTournamenrByGroupQuery">Get tournament by given group query.</param>
        /// <param name="getOldTournamentsQuery">Get old tournaments query.</param>
        /// <param name="authService">Authorization service</param>
        /// <param name="gameService">The game service</param>
        public TournamentService(
            ITournamentRepository tournamentRepository,
            IQuery<Tournament, UniqueTournamentCriteria> uniqueTournamentQuery,
            IQuery<ICollection<Tournament>, GetAllCriteria> getAllQuery,
            IQuery<Tournament, FindByIdCriteria> getByIdQuery,
            IQuery<ICollection<Team>, GetAllCriteria> getAllTeamsQuery,
            IQuery<ICollection<Division>, TournamentDivisionsCriteria> getAllTournamentDivisionsQuery,
            IQuery<ICollection<Group>, DivisionGroupsCriteria> getAllTournamentGroupsQuery,
            IQuery<TournamentScheduleDto, TournamentScheduleInfoCriteria> getTournamentDtoQuery,
            IQuery<Tournament, TournamentByGroupCriteria> getTournamenrByGroupQuery,
            IQuery<ICollection<Tournament>, OldTournamentsCriteria> getOldTournamentsQuery,
            IQuery<ICollection<TeamTournamentDto>, FindByTournamentIdCriteria> tournamentTeamsQuery,
            IAuthorizationService authService,
            IGameService gameService)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            _tournamentRepository = tournamentRepository;
            _uniqueTournamentQuery = uniqueTournamentQuery;
            _getAllQuery = getAllQuery;
            _getByIdQuery = getByIdQuery;
            _getAllTournamentDivisionsQuery = getAllTournamentDivisionsQuery;
            _getAllTournamentGroupsQuery = getAllTournamentGroupsQuery;
            _getTournamentDtoQuery = getTournamentDtoQuery;
            _getTournamenrByGroupQuery = getTournamenrByGroupQuery;
            _getOldTournamentsQuery = getOldTournamentsQuery;
            _authService = authService;
            _gameService = gameService;
            _getAllTeamsQuery = getAllTeamsQuery;
            _tournamentTeamsQuery = tournamentTeamsQuery;
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Get all tournaments
        /// </summary>
        /// <returns>All tournaments</returns>
        public ICollection<Tournament> Get()
        {
            ArchiveOld();

            return _getAllQuery.Execute(new GetAllCriteria());
        }

        /// <summary>
        /// Get only actual tournaments
        /// </summary>
        /// <returns>actual tournaments</returns>
        public ICollection<Tournament> GetActual()
        {
            return GetFilteredTournaments(_actualStates);
        }

        /// <summary>
        /// Returns only archived tournaments
        /// </summary>
        /// <returns>Archived tournaments</returns>
        public ICollection<Tournament> GetArchived()
        {
            _authService.CheckAccess(AuthOperations.Tournaments.ViewArchived);

            return GetArchivedTournaments();
        }

        /// <summary>
        /// Get only finished tournaments
        /// </summary>
        /// <returns>Finished tournaments</returns>
        public ICollection<Tournament> GetFinished()
        {
            return GetFilteredTournaments(_finishedStates);
        }

        /// <summary>
        /// Returns all teams for specific tournament
        /// </summary>
        /// <param name="tournamentId">Id of Tournament for getting teams</param>
        /// <returns>Tournament teams</returns>
        public ICollection<TeamTournamentDto> GetAllTournamentTeams(int tournamentId)
        {
            return _tournamentTeamsQuery.Execute(new FindByTournamentIdCriteria { TournamentId = tournamentId });
        }

        /// <summary>
        /// Returns all divisions for specific tournament
        /// </summary>
        /// <param name="tournamentId">Id of Tournament to get divisions list</param>
        /// <returns>Tournament divisions</returns>
        public ICollection<Division> GetAllTournamentDivisions(int tournamentId)
        {
            return _getAllTournamentDivisionsQuery.Execute(new TournamentDivisionsCriteria { TournamentId = tournamentId });
        }

        /// <summary>
        /// Returns all groups for specific tournament
        /// </summary>
        /// <param name="divisionId">Id of Division to get group list</param>
        /// <returns>Tournament groups</returns>
        public ICollection<Group> GetAllTournamentGroups(int divisionId)
        {
            return _getAllTournamentGroupsQuery.Execute(new DivisionGroupsCriteria { DivisionId = divisionId });
        }

        /// <summary>
        /// Returns all teams that don't take part in specific tournament
        /// </summary>
        /// <param name="tournamentId">Id of Tournament for getting teams</param>
        /// <returns>Teams that don't take part in tournament</returns>
        public IEnumerable<Team> GetAllNoTournamentTeams(int tournamentId)
        {
            var allTeamsList = _getAllTeamsQuery.Execute(new GetAllCriteria());
            var tournamentTeamsList = GetAllTournamentTeams(tournamentId);
            return allTeamsList.Where(l2 => tournamentTeamsList.All(l1 => l1.TeamId != l2.Id));
        }

        /// <summary>
        /// Finds tournament data transfer object by tournament id
        /// </summary>
        /// <param name="tournamentId">Tournament id</param>
        /// <returns>The <see cref="TournamentScheduleDto"/></returns>
        public TournamentScheduleDto GetTournamentScheduleInfo(int tournamentId)
        {
            var result = _getTournamentDtoQuery
                .Execute(new TournamentScheduleInfoCriteria { TournamentId = tournamentId });

            foreach (var item in result.Divisions)
            {
                item.NumberOfRounds = CalculateNumberOfRounds(result.Scheme, item.TeamCount);
            }

            return result;
        }

        /// <summary>
        /// Create a new tournament
        /// </summary>
        /// <param name="tournamentToCreate">A Tournament to create</param>
        public void Create(Tournament tournamentToCreate)
        {
            _authService.CheckAccess(AuthOperations.Tournaments.Create);

            if (tournamentToCreate == null)
            {
                throw new ArgumentNullException("tournamentToCreate");
            }

            ValidateTournament(tournamentToCreate);
            ValidateDivisions(tournamentToCreate.Divisions);
            ValidateGroups(tournamentToCreate.Divisions);

            tournamentToCreate.LastTimeUpdated = TimeProvider.Current.UtcNow;

            _tournamentRepository.Add(tournamentToCreate);
            _tournamentRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Finds a Tournament by id
        /// </summary>
        /// <param name="id">id for search</param>
        /// <returns>A found Tournament</returns>
        public Tournament Get(int id)
        {
            return _getByIdQuery.Execute(new FindByIdCriteria { Id = id });
        }

        /// <summary>
        /// Edit tournament
        /// </summary>
        /// <param name="tournamentToEdit">Tournament to edit</param>
        public void Edit(Tournament tournamentToEdit)
        {
            _authService.CheckAccess(AuthOperations.Tournaments.Edit);
            ValidateTournament(tournamentToEdit, isUpdate: true);

            tournamentToEdit.LastTimeUpdated = TimeProvider.Current.UtcNow;

            _tournamentRepository.Update(tournamentToEdit);
            _tournamentRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Delete tournament by id.
        /// </summary>
        /// <param name="id">The id of tournament to delete.</param>
        public void Delete(int id)
        {
            _authService.CheckAccess(AuthOperations.Tournaments.Delete);

            RemoveAllRelatedDataFromTournament(id);
            _tournamentRepository.Remove(id);
            _tournamentRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Archive tournament by id.
        /// </summary>
        /// <param name="id">The id of tournament to archive.</param>
        public void Archive(int id)
        {
            _authService.CheckAccess(AuthOperations.Tournaments.Archive);

            var getTournamentToArchive = Get(id);

            if (getTournamentToArchive == null)
            {
                throw new ArgumentException(
                    TournamentResources.TournamentWasNotFound);
            }

            Archive(getTournamentToArchive);
            _tournamentRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Activate tournament by id.
        /// </summary>
        /// <param name="id">The id of tournament to archive.</param>
        public void Activate(int id)
        {
            _authService.CheckAccess(AuthOperations.Tournaments.Activate);

            var tournamentToActivate = Get(id);

            if (tournamentToActivate == null)
            {
                throw new ArgumentException(
                    TournamentResources.TournamentWasNotFound);
            }

            tournamentToActivate.IsArchived = false;

            _tournamentRepository.Update(tournamentToActivate);
            _tournamentRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Method for autho-archiving old tournaments.
        /// Finds old tournaments to be archived.
        /// As we don't have reliable task scheduling at the moment of writing,
        /// we call this method every time user retrieves list of tournaments to make sure we do not show outdated tournaments.
        /// </summary>
        public void ArchiveOld()
        {
            var criteria = new OldTournamentsCriteria {
                CheckDate = TimeProvider.Current.UtcNow.AddYears(-TournamentConstants.YEARS_AFTER_END_TO_BE_OLD)
            };

            // Gets old tournaments that need to be archived
            var old = _getOldTournamentsQuery.Execute(criteria);

            if (Enumerable.Any(old))
            {
                foreach (var item in old)
                {
                    Archive(item);
                }

                _tournamentRepository.UnitOfWork.Commit();
            }
        }

        /// <summary>
        /// Adds selected teams to tournament
        /// </summary>
        /// <param name="groupTeam">Teams related to specific groups that will be added to tournament</param>
        public void AddTeamsToTournament(ICollection<TeamTournamentAssignmentDto> groupTeam)
        {
            _authService.CheckAccess(AuthOperations.Tournaments.ManageTeams);

            var groupTeamCount = groupTeam.Count;

            if (groupTeamCount == 0)
            {
                throw new ArgumentException(
                    TournamentResources.CollectionIsEmpty);
            }

            var tournamentId = GetTournamentByGroup(groupTeam.First().GroupId).Id;
            var allTeams = GetAllTournamentTeams(tournamentId);
            var numberOfTeamAlreadyExist = 0;

            foreach (var item in groupTeam)
            {
                var tournamentTeam = allTeams.SingleOrDefault(t => t.TeamId == item.TeamId);

                if (tournamentTeam == null)
                {
                    _tournamentRepository.AddTeamToTournament(item.TeamId, item.GroupId);
                }
                else
                {
                    numberOfTeamAlreadyExist++;
                }
            }

            if (numberOfTeamAlreadyExist != 0)
            {
                throw new ArgumentException(
                    TournamentResources.TeamNameInCurrentGroupOfTournamentNotUnique);
            }

            var totalTeamCount = allTeams.Count + groupTeamCount;
            CreateSchedule(tournamentId, totalTeamCount);

            _tournamentRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Deletes team from tournament
        /// </summary>
        /// <param name="teamId">Team to delete</param>
        /// <param name="tournamentId">Tournament to un assign team</param>
        public void DeleteTeamFromTournament(int teamId, int tournamentId)
        {
            _authService.CheckAccess(AuthOperations.Tournaments.ManageTeams);

            try
            {
                _tournamentRepository.RemoveTeamFromTournament(teamId, tournamentId);
            }
            catch (ConcurrencyException ex)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.TeamInTournamentNotFound, ex);
            }

            var allTeams = GetAllTournamentTeams(tournamentId);
            var count = allTeams.Count - 1;
            CreateSchedule(tournamentId, count);

            _tournamentRepository.UnitOfWork.Commit();
        }

        public static byte CalculateNumberOfRounds(TournamentSchemeEnum scheme, int teamCount)
        {
            byte numberOfRounds = 0;

            switch (scheme)
            {
                case TournamentSchemeEnum.One:
                    numberOfRounds = GetNumberOfRoundsByScheme1(teamCount);
                    break;
                case TournamentSchemeEnum.Two:
                    numberOfRounds = GetNumberOfRoundsByScheme2(teamCount);
                    break;
                case TournamentSchemeEnum.PlayOff:
                    numberOfRounds = GetNumberOfRoundsByPlayOffScheme(teamCount);
                    break;
                default:
                    throw new InvalidOperationException("This scheme doesn't exist");
            }

            return numberOfRounds;
        }

        /// <summary>
        /// Gets tournament by its group
        /// </summary>
        /// <param name="groupId">id of group </param>
        /// <returns>Return current tournament.</returns>
        public Tournament GetTournamentByGroup(int groupId)
        {
            return _getTournamenrByGroupQuery.Execute(new TournamentByGroupCriteria { GroupId = groupId });
        }

        #endregion

        #region Private

        private static UniqueTournamentCriteria BuildUniqueTournamentCriteria(Tournament newTournament, bool isUpdate)
        {
            var criteria = new UniqueTournamentCriteria { Name = newTournament.Name };

            if (isUpdate)
            {
                criteria.EntityId = newTournament.Id;
            }

            return criteria;
        }

        /// <summary>
        /// Calculate number of rounds in tournament by scheme 1.
        /// </summary>
        /// <param name="teamCount">Number of teams.</param>
        /// <returns>Number of rounds.</returns>
        private static byte GetNumberOfRoundsByScheme1(int teamCount)
        {
            return Convert.ToByte((teamCount % 2 == 0) && (teamCount != 0) ? teamCount - 1 : teamCount);
        }

        /// <summary>
        /// Calculate number of rounds in tournament by scheme 2.
        /// </summary>
        /// <param name="teamCount">Number of teams.</param>
        /// <returns>Number of rounds.</returns>
        private static byte GetNumberOfRoundsByScheme2(int teamCount)
        {
            return Convert.ToByte(2 * GetNumberOfRoundsByScheme1(teamCount));
        }

        /// <summary>
        /// Archive tournament.
        /// </summary>
        /// <param name="tournament">Tournament to archive.</param>
        private void Archive(Tournament tournament)
        {
            tournament.IsArchived = true;

            _tournamentRepository.Update(tournament);
        }

        private List<Tournament> GetFilteredTournaments(IEnumerable<TournamentStateEnum> statesFilter)
        {
            return Get().Where(t => statesFilter.Contains(t.State) && !t.IsArchived).ToList();
        }

        private List<Tournament> GetArchivedTournaments()
        {
            return Get().Where(t => t.IsArchived).ToList();
        }

        private void RemoveAllRelatedDataFromTournament(int tournamentId)
        {
            RemoveAllTeamsFromTournament(GetAllTournamentTeams(tournamentId), tournamentId);
            RemoveAllDivisionsFromTournament(GetAllTournamentDivisions(tournamentId));

            _gameService.RemoveAllGamesInTournament(tournamentId);
        }

        private void RemoveAllTeamsFromTournament(ICollection<TeamTournamentDto> allTeamsInTournament, int tournamentId)
        {
            if (allTeamsInTournament != null)
            {
                foreach (var team in allTeamsInTournament)
                {
                    try
                    {
                        _tournamentRepository.RemoveTeamFromTournament(team.TeamId, tournamentId);
                    }
                    catch (ConcurrencyException ex)
                    {
                        throw new MissingEntityException(ServiceResources.ExceptionMessages.TeamInTournamentNotFound, ex);
                    }
                }
            }
        }

        private void RemoveAllDivisionsFromTournament(ICollection<Division> allDivisionsInTournament)
        {
            if (allDivisionsInTournament != null)
            {
                foreach (var division in allDivisionsInTournament)
                {
                    try
                    {
                        _tournamentRepository.RemoveDivision(division.Id);
                    }
                    catch (ConcurrencyException ex)
                    {
                        throw new MissingEntityException(ServiceResources.ExceptionMessages.DivisionInTournamentNotFound, ex);
                    }
                }
            }
        }

        private void ValidateTournament(Tournament tournament, bool isUpdate = false)
        {
            ValidateUniqueTournamentName(tournament, isUpdate);
            ValidateTournamentDates(tournament);
        }

        private void ValidateUniqueTournamentName(Tournament newTournament, bool isUpdate = false)
        {
            var criteria = BuildUniqueTournamentCriteria(newTournament, isUpdate);
            var tournament = _uniqueTournamentQuery.Execute(criteria);

            if (tournament != null)
            {
                throw new TournamentValidationException(
                    TournamentResources.TournamentNameMustBeUnique,
                    TournamentConstants.UNIQUE_NAME_KEY,
                    "Name");
            }
        }

        private static void ValidateTournamentDates(Tournament tournament)
        {
            ValidateTournamentApplyingPeriod(tournament);
            ValidateTournamentGamesPeriod(tournament);
            ValidateTournamentTrasferPeriod(tournament);
        }

        private static void ValidateTournamentApplyingPeriod(Tournament tournament)
        {
            // if registration dates comes before current date
            if (TimeProvider.Current.UtcNow >= tournament.ApplyingPeriodStart)
            {
                throw new TournamentValidationException(
                    TournamentResources.LateRegistrationDates,
                    TournamentConstants.APPLYING_START_BEFORE_NOW,
                    TournamentConstants.APPLYING_START_CAPTURE);
            }

            // if registration start date comes after end date
            if (tournament.ApplyingPeriodStart >= tournament.ApplyingPeriodEnd)
            {
                throw new TournamentValidationException(
                    TournamentResources.WrongRegistrationDatesPeriod,
                    TournamentConstants.APPLYING_START_DATE_AFTER_END_DATE,
                    TournamentConstants.APPLYING_START_CAPTURE);
            }

            // if registration end date comes after games start date
            if (tournament.ApplyingPeriodEnd >= tournament.GamesStart)
            {
                throw new TournamentValidationException(
                    TournamentResources.WrongRegistrationGames,
                    TournamentConstants.APPLYING_END_DATE_AFTER_START_GAMES,
                    TournamentConstants.GAMES_START_CAPTURE);
            }
        }

        private static void ValidateTournamentGamesPeriod(Tournament tournament)
        {
            // if games start date comes after end date
            if (tournament.GamesStart >= tournament.GamesEnd)
            {
                throw new TournamentValidationException(
                    TournamentResources.WrongStartTournamentDates,
                    TournamentConstants.START_GAMES_AFTER_END_GAMES,
                    TournamentConstants.GAMES_END_CAPTURE);
            }
        }

        private static void ValidateTournamentTrasferPeriod(Tournament tournament)
        {
            // if there is no transfer period
            if (!tournament.TransferStart.HasValue && !tournament.TransferEnd.HasValue)
            {
                return;
            }

            // if transfer start is not specified
            if (!tournament.TransferStart.HasValue && tournament.TransferEnd.HasValue)
            {
                throw new TournamentValidationException(
                    TournamentResources.TransferStartMissing,
                    TournamentConstants.TRANSFER_START_MISSING,
                    TournamentConstants.TRANSFER_START_CAPTURE);
            }

            // if transfer end is not specified
            if (tournament.TransferStart.HasValue && !tournament.TransferEnd.HasValue)
            {
                throw new TournamentValidationException(
                    TournamentResources.TransferEndMissing,
                    TournamentConstants.TRANSFER_END_MISSING,
                    TournamentConstants.TRANSFER_END_CAPTURE);
            }

            // if games start date comes after transfer start date
            if (tournament.GamesStart >= tournament.TransferStart.GetValueOrDefault())
            {
                throw new TournamentValidationException(
                    TournamentResources.WrongTransferStart,
                    TournamentConstants.TRANSFER_PERIOD_BEFORE_GAMES_START,
                    TournamentConstants.TRANSFER_START_CAPTURE);
            }

            // if transfer start date comes after end date
            if (tournament.TransferStart.GetValueOrDefault() >= tournament.TransferEnd.GetValueOrDefault())
            {
                throw new TournamentValidationException(
                    TournamentResources.WrongTransferPeriod,
                    TournamentConstants.TRANSFER_END_BEFORE_TRANSFER_START,
                    TournamentConstants.TRANSFER_END_CAPTURE);
            }

            // if transfer end date comes before games end date
            if (tournament.TransferEnd.GetValueOrDefault() >= tournament.GamesEnd)
            {
                throw new TournamentValidationException(
                    TournamentResources.InvalidTransferEndpoint,
                    TournamentConstants.TRANSFER_END_AFTER_GAMES_END,
                    TournamentConstants.GAMES_END_CAPTURE);
            }
        }

        private void ValidateDivisions(ICollection<Division> divisions)
        {
            ValidateDivisionCount(divisions.Count);
            ValidateUniqueDivisionNames(divisions);
        }

        private static void ValidateDivisionCount(int count)
        {
            if (!DivisionValidation.IsDivisionCountWithinRange(count))
            {
                throw new ArgumentException(
                    string.Format(
                        TournamentResources.DivisionCountOutOfRange,
                        DivisionConstants.MIN_DIVISIONS_COUNT,
                        DivisionConstants.MAX_DIVISIONS_COUNT));
            }
        }

        private void ValidateUniqueDivisionNames(ICollection<Division> divisions)
        {
            if (divisions.Select(d => new { Name = d.Name.ToUpper(CultureInfo.InvariantCulture) }).Distinct().Count() != divisions.Count)
            {
                throw new ArgumentException(TournamentResources.DivisionNamesNotUnique);
            }
        }

        private void ValidateGroups(ICollection<Division> divisions)
        {
            foreach (var division in divisions)
            {
                ValidateGroupCount(division.Groups.Count);
                ValidateUniqueGroupNames(division.Groups);
            }
        }

        private static void ValidateGroupCount(int count)
        {
            if (!GroupValidation.IsGroupCountWithinRange(count))
            {
                throw new ArgumentException(
                    string.Format(
                    TournamentResources.GroupCountOutOfRange,
                    GroupConstants.MIN_GROUPS_COUNT,
                    GroupConstants.MAX_GROUPS_COUNT));
            }
        }

        private void ValidateUniqueGroupNames(ICollection<Group> groups)
        {
            if (groups.Select(g => new { Name = g.Name.ToUpper(CultureInfo.InvariantCulture) }).Distinct().Count() != groups.Count)
            {
                throw new ArgumentException(TournamentResources.GroupNamesNotUnique);
            }
        }

        private static byte GetNumberOfRoundsByPlayOffScheme(int teamCount)
        {
            byte rounds = 0;
            for (byte i = 0; i < teamCount; i++)
            {
                if (Math.Pow(MIN_TEAMS_TO_PLAY_ONE_ROUND, i) >= teamCount)
                {
                    rounds = i;
                    break;
                }
            }

            return rounds;
        }

        private static int GetGamesCount(int teamsCount)
        {
            return (int)Math.Pow(GAMES_TO_PLAY_ONE_ROUND, GetNumberOfRoundsByPlayOffScheme((byte)teamsCount));
        }

        private void CreateSchedule(int tournamentId, int allTeamsCount)
        {
            var tournament = Get(tournamentId);

            if (tournament.Scheme == TournamentSchemeEnum.PlayOff
                && allTeamsCount > DONT_CREATE_SCHEDULE_TEAMS_COUNT)
            {
                var gamesToAdd = GetAllGamesInPlayOffTournament(tournamentId, allTeamsCount);
                _gameService.RemoveAllGamesInTournament(tournamentId);
                _gameService.AddGames(gamesToAdd);
            }
        }

        private static List<Game> GetAllGamesInPlayOffTournament(int tournamentId, int teamsCount)
        {
            var roundsCount = GetNumberOfRoundsByPlayOffScheme((byte)teamsCount);
            var gamesCount = GetGamesCount(teamsCount);
            var games = new List<Game>();

            for (var i = 1; i <= gamesCount; i++)
            {
                var game = new Game {
                    TournamentId = tournamentId,
                    HomeTeamId = null,
                    AwayTeamId = null,
                    Result = new Result(),
                    Round = GetRoundNumber(roundsCount, gamesCount, i),
                    GameNumber = (byte)i,
                    GameDate = null
                };

                games.Add(game);
            }

            return games;
        }

        private static byte GetRoundNumber(int roundsCount, int gamesCount, int gameNumber)
        {
            byte roundNumber = 1;

            var roundStartGameNumber = 0;
            var roundEndGameNumber = gamesCount / 2;
            var numberOfGamesInRound = gamesCount / 2;

            for (var i = 1; i <= roundsCount; i++)
            {
                if (gameNumber > roundStartGameNumber && gameNumber <= roundEndGameNumber)
                {
                    roundNumber = (byte)i;
                    break;
                }

                if (i + 1 == roundsCount)
                {
                    roundStartGameNumber = roundEndGameNumber;
                    roundEndGameNumber += numberOfGamesInRound;
                }
                else
                {
                    roundStartGameNumber = roundEndGameNumber;
                    roundEndGameNumber += numberOfGamesInRound / 2;
                    numberOfGamesInRound = numberOfGamesInRound / 2;
                }
            }

            return roundNumber;
        }

        #endregion
    }
}