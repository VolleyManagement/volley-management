namespace VolleyManagement.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Crosscutting.Contracts.Providers;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Exceptions;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Data.Queries.Team;
    using VolleyManagement.Data.Queries.Tournament;
    using VolleyManagement.Domain.TeamsAggregate;
    using VolleyManagement.Domain.TournamentsAggregate;
    using DivisionConstants = VolleyManagement.Domain.Constants.Division;
    using GroupConstants = VolleyManagement.Domain.Constants.Group;
    using TournamentConstants = VolleyManagement.Domain.Constants.Tournament;
    using TournamentResources = VolleyManagement.Domain.Properties.Resources;

    /// <summary>
    /// Defines TournamentService
    /// </summary>
    public class TournamentService : ITournamentService
    {
        #region Const & Readonly

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

        #endregion

        #region Query Objects

        private readonly IQuery<Tournament, UniqueTournamentCriteria> _uniqueTournamentQuery;
        private readonly IQuery<List<Tournament>, GetAllCriteria> _getAllQuery;
        private readonly IQuery<Tournament, FindByIdCriteria> _getByIdQuery;
        private readonly IQuery<List<Team>, FindByTournamentIdCriteria> _getAllTeamsQuery;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentService"/> class
        /// </summary>
        /// <param name="tournamentRepository"> The tournament repository  </param>
        /// <param name="uniqueTournamentQuery"> First By Name object query  </param>
        /// <param name="getAllQuery"> Get All object query. </param>
        /// <param name="getByIdQuery">Get tournament by id query.</param>
        /// <param name="getAllTeamsQuery">Get All Tournament Teams query.</param>
        public TournamentService(
            ITournamentRepository tournamentRepository,
            IQuery<Tournament, UniqueTournamentCriteria> uniqueTournamentQuery,
            IQuery<List<Tournament>, GetAllCriteria> getAllQuery,
            IQuery<Tournament, FindByIdCriteria> getByIdQuery,
            IQuery<List<Team>, FindByTournamentIdCriteria> getAllTeamsQuery)
        {
            _tournamentRepository = tournamentRepository;
            _uniqueTournamentQuery = uniqueTournamentQuery;
            _getAllQuery = getAllQuery;
            _getByIdQuery = getByIdQuery;
            _getAllTeamsQuery = getAllTeamsQuery;
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
            return _getAllTeamsQuery.Execute(new FindByTournamentIdCriteria { TournamentId = tournamentId });
        }

        /// <summary>
        /// Create a new tournament
        /// </summary>
        /// <param name="tournamentToCreate">A Tournament to create</param>
        public void Create(Tournament tournamentToCreate)
        {
            if (tournamentToCreate == null)
            {
                throw new ArgumentNullException("tournamentToCreate");
            }

            ValidateTournament(tournamentToCreate);
            ValidateDivisions(tournamentToCreate.Divisions);
            ValidateGroups(tournamentToCreate.Divisions);

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
            ValidateTournament(tournamentToEdit, isUpdate: true);

            _tournamentRepository.Update(tournamentToEdit);
            _tournamentRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Delete tournament by id.
        /// </summary>
        /// <param name="id">The id of tournament to delete.</param>
        public void Delete(int id)
        {
            _tournamentRepository.Remove(id);
            _tournamentRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Adds teams to tournament
        /// </summary>
        /// <param name="teams">Teams for adding to tournament.</param>
        /// <param name="tournamentId">Tournament to assign team.</param>
        public void AddTeamsToTournament(IEnumerable<Team> teams, int tournamentId)
        {
            var allTeams = GetAllTournamentTeams(tournamentId);

            foreach (var team in teams)
            {
                var tournamentTeam = allTeams.SingleOrDefault(t => t.Id == team.Id);
                if (tournamentTeam == null)
                {
                    _tournamentRepository.AddTeamToTournament(team.Id, tournamentId);
                }
                else
                {
                    throw new ArgumentException(
                        TournamentResources.TeamNameInTournamentNotUnique, tournamentTeam.Name);
                }
            }

            _tournamentRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Deletes team from tournament
        /// </summary>
        /// <param name="teamId">Team to delete</param>
        /// <param name="tournamentId">Tournament to un assign team</param>
        public void DeleteTeamFromTournament(int teamId, int tournamentId)
        {
            try
            {
                _tournamentRepository.RemoveTeamFromTournament(teamId, tournamentId);
            }
            catch (ConcurrencyException ex)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.TeamInTournamentNotFound, ex);
            }

            _tournamentRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Counts number of rounds for specified tournament
        /// </summary>
        /// <param name="tournament">Tournament for which we count rounds</param>
        /// <param name="teamCount">Count of teams in tournament</param>
        /// <returns>Number of rounds</returns>
        public byte GetNumberOfRounds(Tournament tournament, int teamCount)
        {
            byte numberOfRounds = 0;

            switch (tournament.Scheme)
            {
                case TournamentSchemeEnum.One:
                    numberOfRounds = GetNumberOfRoundsByScheme1(teamCount);
                    break;
                case TournamentSchemeEnum.Two:
                    numberOfRounds = GetNumberOfRoundsByScheme2(teamCount);
                    break;
            }

            return numberOfRounds;
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

        #endregion
    }
}