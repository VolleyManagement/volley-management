namespace VolleyManagement.Services
{
    using System;
    using System.Collections.Generic;
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

    /// <summary>
    /// Defines TournamentService
    /// </summary>
    public class TournamentService : ITournamentService
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
        private readonly IQuery<List<Tournament>, GetAllCriteria> _getAllQuery;
        private readonly IQuery<Tournament, FindByIdCriteria> _getByIdQuery;
        private readonly IQuery<List<Team>, GetAllCriteria> _getAllTeamsQuery;
        private readonly IQuery<List<Team>, FindByTournamentIdCriteria> _getAllTournamentTeamsQuery;
        private readonly IQuery<List<Division>, TournamentDivisionsCriteria> _getAllTournamentDivisionsQuery;
        private readonly IQuery<List<Group>, DivisionGroupsCriteria> _getAllTournamentGroupsQuery;
        private readonly IQuery<TournamentScheduleDto, TournamentScheduleInfoCriteria> _getTournamentDtoQuery;
<<<<<<< HEAD
        private readonly IQuery<List<Team>, FindTeamsByGroupIdCriteria> _getTeamByGroupIdQuery;
        private readonly IQuery<Division, FindByIdCriteria> _getDivisionByIdQuery;
=======
        private readonly IQuery<int, TournamentByGroupCriteria> _getTournamenrByGroupQuery;
>>>>>>> master

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentService"/> class
        /// </summary>
        /// <param name="tournamentRepository"> The tournament repository.</param>
        /// <param name="uniqueTournamentQuery"> First By Name object query.</param>
        /// <param name="getAllQuery"> Get All object query. </param>
        /// <param name="getByIdQuery">Get tournament by id query.</param>
        /// <param name="getAllTeamsQuery">Get All Teams query.</param>
        /// <param name="getAllTournamentTeamsQuery">Get All Tournament Teams query.</param>
        /// <param name="getAllTournamentDivisionsQuery">Get All Tournament Divisions query.</param>
        /// <param name="getAllTournamentGroupsQuery">Get All Tournament Groups query.</param>
        /// <param name="getTournamentDtoQuery">Get tournament data transfer object query.</param>
<<<<<<< HEAD
        /// <param name="getTeamByGroupIdQuery">Get Teams by Group id query</param>
        /// <param name="getDivisionByIdQuery">Get Division by id query</param>
=======
        /// <param name="getTournamenrByGroupQuery">Get tournament by given group query.</param>
>>>>>>> master
        /// <param name="authService">Authorization service</param>
        /// <param name="gameService">The game service</param>
        public TournamentService(
            ITournamentRepository tournamentRepository,
            IQuery<Tournament, UniqueTournamentCriteria> uniqueTournamentQuery,
            IQuery<List<Tournament>, GetAllCriteria> getAllQuery,
            IQuery<Tournament, FindByIdCriteria> getByIdQuery,
            IQuery<List<Team>, GetAllCriteria> getAllTeamsQuery,
            IQuery<List<Team>, FindByTournamentIdCriteria> getAllTournamentTeamsQuery,
            IQuery<List<Division>, TournamentDivisionsCriteria> getAllTournamentDivisionsQuery,
            IQuery<List<Group>, DivisionGroupsCriteria> getAllTournamentGroupsQuery,
            IQuery<TournamentScheduleDto, TournamentScheduleInfoCriteria> getTournamentDtoQuery,
<<<<<<< HEAD
            IQuery<List<Team>, FindTeamsByGroupIdCriteria> getTeamByGroupIdQuery,
            IQuery<Division, FindByIdCriteria> getDivisionByIdQuery,
=======
            IQuery<int, TournamentByGroupCriteria> getTournamenrByGroupQuery,
>>>>>>> master
            IAuthorizationService authService,
            IGameService gameService)
        {
            _tournamentRepository = tournamentRepository;
            _uniqueTournamentQuery = uniqueTournamentQuery;
            _getAllQuery = getAllQuery;
            _getByIdQuery = getByIdQuery;
            _getAllTeamsQuery = getAllTeamsQuery;
            _getAllTournamentTeamsQuery = getAllTournamentTeamsQuery;
            _getAllTournamentDivisionsQuery = getAllTournamentDivisionsQuery;
            _getAllTournamentGroupsQuery = getAllTournamentGroupsQuery;
            _getTournamentDtoQuery = getTournamentDtoQuery;
            _getTournamenrByGroupQuery = getTournamenrByGroupQuery;
            _authService = authService;
            _gameService = gameService;
            _getTeamByGroupIdQuery = getTeamByGroupIdQuery;
            _getDivisionByIdQuery = getDivisionByIdQuery;
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Get all tournaments
        /// </summary>
        /// <returns>All tournaments</returns>
        public List<Tournament> Get()
        {
            return _getAllQuery.Execute(new GetAllCriteria());
        }

        /// <summary>
        /// Get only actual tournaments
        /// </summary>
        /// <returns>actual tournaments</returns>
        public List<Tournament> GetActual()
        {
            return GetFilteredTournaments(_actualStates);
        }

        /// <summary>
        /// Get only finished tournaments
        /// </summary>
        /// <returns>Finished tournaments</returns>
        public List<Tournament> GetFinished()
        {
            return GetFilteredTournaments(_finishedStates);
        }

        /// <summary>
        /// Returns all teams for specific tournament
        /// </summary>
        /// <param name="tournamentId">Id of Tournament for getting teams</param>
        /// <returns>Tournament teams</returns>
        public List<Team> GetAllTournamentTeams(int tournamentId)
        {
            return _getAllTournamentTeamsQuery.Execute(new FindByTournamentIdCriteria { TournamentId = tournamentId });
        }

        /// <summary>
        /// Returns all divisions for specific tournament
        /// </summary>
        /// <param name="tournamentId">Id of Tournament to get divisions list</param>
        /// <returns>Tournament divisions</returns>
        public List<Division> GetAllTournamentDivisions(int tournamentId)
        {
            return _getAllTournamentDivisionsQuery.Execute(new TournamentDivisionsCriteria { TournamentId = tournamentId });
        }

        /// <summary>
        /// Returns all groups for specific tournament
        /// </summary>
        /// <param name="divisionId">Id of Division to get group list</param>
        /// <returns>Tournament groups</returns>
        public List<Group> GetAllTournamentGroups(int divisionId)
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
            return allTeamsList.Where(l2 => tournamentTeamsList.All(l1 => l1.Id != l2.Id));
        }

        /// <summary>
        /// Finds tournament data transfer object by tournament id
        /// </summary>
        /// <param name="tournamentId">Tournament id</param>
        /// <returns>The <see cref="TournamentScheduleDto"/></returns>
        public TournamentScheduleDto GetTournamentScheduleInfo(int tournamentId)
        {
            return _getTournamentDtoQuery
                .Execute(new TournamentScheduleInfoCriteria { TournamentId = tournamentId });
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

            _tournamentRepository.Remove(id);
            _tournamentRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Adds selected teams to tournament
        /// </summary>
        /// <param name="groupTeam">Teams related to specific groups that will be added to tournament</param>
        public void AddTeamsToTournament(List<TeamTournamentAssignmentDto> groupTeam)
        {
            _authService.CheckAccess(AuthOperations.Tournaments.ManageTeams);

            var groupTeamCount = groupTeam.Count;

            if (groupTeamCount == 0)
            {
                throw new ArgumentException(
                    TournamentResources.CollectionIsEmpty);
            }

            var tournamentId = GetTournamentByGroup(groupTeam[0].GroupId);
            var allTeams = GetAllTournamentTeams(tournamentId);
            int numberOfTeamAlreadyExist = 0;

            foreach (var item in groupTeam)
            {
                var tournamentTeam = allTeams.SingleOrDefault(t => t.Id == item.TeamId);

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
            var count = allTeams.Count() - 1;
            CreateSchedule(tournamentId, count);

            _tournamentRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Counts number of rounds for specified tournament
        /// </summary>
        /// <param name="tournament">Tournament for which we count rounds</param>
        /// <returns>Number of rounds</returns>
        public byte GetNumberOfRounds(TournamentScheduleDto tournament)
        {
            byte numberOfRounds = 0;

            switch (tournament.Scheme)
            {
                case TournamentSchemeEnum.One:
                    numberOfRounds = GetNumberOfRoundsByScheme1(tournament.TeamCount);
                    break;
                case TournamentSchemeEnum.Two:
                    numberOfRounds = GetNumberOfRoundsByScheme2(tournament.TeamCount);
                    break;
                case TournamentSchemeEnum.PlayOff:
                    numberOfRounds = GetNumberOfRoundsByPlayOffScheme(tournament.TeamCount);
                    break;
            }

            return numberOfRounds;
        }

        /// <summary>
        /// Checks if there are teams in the group
        /// </summary>
        /// <param name="groupId">Id of Group to check</param>
        /// <returns>True if there are no teams in the group</returns>
        public bool IsGroupEmpty(int groupId)
        {
            var teams = _getTeamByGroupIdQuery.Execute(new FindTeamsByGroupIdCriteria { GroupId = groupId });
            return teams.Count == 0;
        }

        /// <summary>
        /// Checks if there are teams in the division
        /// </summary>
        /// <param name="divisionId">Id of Division to check</param>
        /// <returns>True if there are no teams in the group</returns>
        public bool IsDivisionEmpty(int divisionId)
        {
            var division = _getDivisionByIdQuery.Execute(new FindByIdCriteria { Id = divisionId });

            foreach (var group in division.Groups)
            {
                if (!IsGroupEmpty(group.Id))
                {
                    return false;
                }
            }

            return true;
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

        private int GetTournamentByGroup(int groupId)
        {
            return _getTournamenrByGroupQuery.Execute(new TournamentByGroupCriteria { GroupId = groupId });
        }

        /// <summary>
        /// Calculate number of rounds in tournament by scheme 1.
        /// </summary>
        /// <param name="teamCount">Number of teams.</param>
        /// <returns>Number of rounds.</returns>
        private byte GetNumberOfRoundsByScheme1(int teamCount)
        {
            return Convert.ToByte((teamCount % 2 == 0) && (teamCount != 0) ? teamCount - 1 : teamCount);
        }

        /// <summary>
        /// Calculate number of rounds in tournament by scheme 2.
        /// </summary>
        /// <param name="teamCount">Number of teams.</param>
        /// <returns>Number of rounds.</returns>
        private byte GetNumberOfRoundsByScheme2(int teamCount)
        {
            return Convert.ToByte(2 * GetNumberOfRoundsByScheme1(teamCount));
        }

        private List<Tournament> GetFilteredTournaments(IEnumerable<TournamentStateEnum> statesFilter)
        {
            return Get().Where(t => statesFilter.Contains(t.State)).ToList();
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

        private void ValidateTournamentDates(Tournament tournament)
        {
            ValidateTournamentApplyingPeriod(tournament);
            ValidateTournamentGamesPeriod(tournament);
            ValidateTournamentTrasferPeriod(tournament);
        }

        private void ValidateTournamentApplyingPeriod(Tournament tournament)
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

            // ToDo: Revisit this requirement
            ////double totalApplyingPeriodDays = (tournament.ApplyingPeriodEnd - tournament.ApplyingPeriodStart).TotalDays;

            ////// if registration period is little
            ////if (totalApplyingPeriodDays < ExceptionParams.DAYS_BETWEEN_START_AND_END_APPLYING_DATE)
            ////{
            ////    throw new TournamentValidationException(
            ////        MessageList.WrongThreeMonthRule,
            ////        ExceptionParams.APPLYING_PERIOD_LESS_THREE_MONTH,
            ////        ExceptionParams.APPLYING_END_CAPTURE);
            ////}
        }

        private void ValidateTournamentGamesPeriod(Tournament tournament)
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

        private void ValidateTournamentTrasferPeriod(Tournament tournament)
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

        private void ValidateDivisions(List<Division> divisions)
        {
            ValidateDivisionCount(divisions.Count);
            ValidateUniqueDivisionNames(divisions);
        }

        private void ValidateDivisionCount(int count)
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

        private void ValidateUniqueDivisionNames(List<Division> divisions)
        {
            if (divisions.Select(d => new { Name = d.Name.ToUpper() }).Distinct().Count() != divisions.Count)
            {
                throw new ArgumentException(TournamentResources.DivisionNamesNotUnique);
            }
        }

        private void ValidateGroups(List<Division> divisions)
        {
            foreach (var division in divisions)
            {
                ValidateGroupCount(division.Groups.Count);
                ValidateUniqueGroupNames(division.Groups);
            }
        }

        private void ValidateGroupCount(int count)
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

        private void ValidateUniqueGroupNames(List<Group> groups)
        {
            if (groups.Select(g => new { Name = g.Name.ToUpper() }).Distinct().Count() != groups.Count)
            {
                throw new ArgumentException(TournamentResources.GroupNamesNotUnique);
            }
        }

        private byte GetNumberOfRoundsByPlayOffScheme(byte teamCount)
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

        private int GetGamesCount(int teamsCount)
        {
            return (int)Math.Pow(GAMES_TO_PLAY_ONE_ROUND, GetNumberOfRoundsByPlayOffScheme((byte)teamsCount));
        }

        private void CreateSchedule(int tournamentId, int allTeamsCount)
        {
            var tournament = Get(tournamentId);
            if (tournament.Scheme == TournamentSchemeEnum.PlayOff)
            {
                if (allTeamsCount > DONT_CREATE_SCHEDULE_TEAMS_COUNT)
                {
                    var gamesToAdd = GetAllGamesInPlayOffTournament(tournamentId, allTeamsCount);
                    _gameService.RemoveAllGamesInTournament(tournamentId);
                    _gameService.AddGames(gamesToAdd);
                }
            }
        }

        private List<Game> GetAllGamesInPlayOffTournament(int tournamentId, int teamsCount)
        {
            var roundsCount = GetNumberOfRoundsByPlayOffScheme((byte)teamsCount);
            int gamesCount = GetGamesCount(teamsCount);
            List<Game> games = new List<Game>();

            for (int i = 1; i <= gamesCount; i++)
            {
                var game = new Game();
                game.TournamentId = tournamentId;
                game.HomeTeamId = null;
                game.AwayTeamId = null;
                game.Result = new Result();
                game.Round = GetRoundNumber(roundsCount, gamesCount, i);
                game.GameNumber = (byte)i;
                game.GameDate = null;
                games.Add(game);
            }

            return games;
        }

        private byte GetRoundNumber(int roundsCount, int gamesCount, int gameNumber)
        {
            byte roundNumber = 1;

            int roundStartGameNumber = 0;
            int roundEndGameNumber = gamesCount / 2;
            int numberOfGamesInRound = gamesCount / 2;

            for (int i = 1; i <= roundsCount; i++)
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