namespace VolleyManagement.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data.Queries.Division;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Crosscutting.Contracts.Providers;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Exceptions;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Data.Queries.Tournaments;
    using VolleyManagement.Domain.TournamentsAggregate;
    using DivisionConstants = VolleyManagement.Domain.Constants.Division;
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

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentService"/> class
        /// </summary>
        /// <param name="tournamentRepository"> The tournament repository  </param>
        /// <param name="uniqueTournamentQuery"> First By Name object query  </param>
        /// <param name="getAllQuery"> Get All object query. </param>
        /// <param name="getByIdQuery">Get tournament by id query.</param>
        public TournamentService(
            ITournamentRepository tournamentRepository,
            IQuery<Tournament, UniqueTournamentCriteria> uniqueTournamentQuery,
            IQuery<List<Tournament>, GetAllCriteria> getAllQuery,
            IQuery<Tournament, FindByIdCriteria> getByIdQuery)
        {
            _tournamentRepository = tournamentRepository;
            this._uniqueTournamentQuery = uniqueTournamentQuery;
            this._getAllQuery = getAllQuery;
            this._getByIdQuery = getByIdQuery;
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
            return this.GetFilteredTournaments(_actualStates);
        }

        /// <summary>
        /// Get only finished tournaments
        /// </summary>
        /// <returns>Finished tournaments</returns>
        public List<Tournament> GetFinished()
        {
            return this.GetFilteredTournaments(_finishedStates);
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

            IsTournamentNameUnique(tournamentToCreate);
            ValidateTournament(tournamentToCreate);

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
            var criteria = new FindByIdCriteria { Id = id };
            return _getByIdQuery.Execute(criteria);
        }

        /// <summary>
        /// Edit tournament
        /// </summary>
        /// <param name="tournamentToEdit">Tournament to edit</param>
        public void Edit(Tournament tournamentToEdit)
        {
            IsTournamentNameUnique(tournamentToEdit, isUpdate: true);
            ValidateTournament(tournamentToEdit);

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
        /// Checks whether tournament name is unique or not.
        /// </summary>
        /// <param name="newTournament">tournament to edit or create</param>
        /// <param name="isUpdate">Specifies operation</param>
        private void IsTournamentNameUnique(Tournament newTournament, bool isUpdate = false)
        {
            var criteria = BuildUniqueTournamentCriteria(newTournament, isUpdate);

            var tournament = this._uniqueTournamentQuery.Execute(criteria);

            if (tournament != null)
            {
                throw new TournamentValidationException(
                    TournamentResources.TournamentNameMustBeUnique,
                    TournamentConstants.UNIQUE_NAME_KEY,
                    "Name");
            }
        }

        /// <summary>
        /// Checks the tournament dates
        /// </summary>
        /// <param name="tournament">Tournament to check</param>
        private void AreDatesValid(Tournament tournament)
        {
            // ToDo: Re-factor it - code is hard to read

            // if registration dates before now
            if (TimeProvider.Current.UtcNow >= tournament.ApplyingPeriodStart)
            {
                throw new TournamentValidationException(
                    TournamentResources.LateRegistrationDates,
                    TournamentConstants.APPLYING_START_BEFORE_NOW,
                    TournamentConstants.APPLYING_START_CAPTURE);
            }

            // if registration start date after end date
            if (tournament.ApplyingPeriodStart >= tournament.ApplyingPeriodEnd)
            {
                throw new TournamentValidationException(
                    TournamentResources.WrongRegistrationDatesPeriod,
                    TournamentConstants.APPLYING_START_DATE_AFTER_END_DATE,
                    TournamentConstants.APPLYING_START_CAPTURE);
            }

            // if registration period is after games start
            if (tournament.ApplyingPeriodEnd >= tournament.GamesStart)
            {
                throw new TournamentValidationException(
                    TournamentResources.WrongRegistrationGames,
                    TournamentConstants.APPLYING_END_DATE_AFTER_START_GAMES,
                    TournamentConstants.GAMES_START_CAPTURE);
            }

            // if registration period is after games start
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

            // if tournament start dates goes after tournament end
            if (tournament.GamesStart >= tournament.GamesEnd)
            {
                throw new TournamentValidationException(
                    TournamentResources.WrongStartTournamentDates,
                    TournamentConstants.START_GAMES_AFTER_END_GAMES,
                    TournamentConstants.GAMES_END_CAPTURE);
            }

            IsTrasferPeriodValid(tournament);
        }

        /// <summary>
        /// Check whether transfer period is valid
        /// </summary>
        /// <param name="tournament">Tournament to validate</param>
        /// <exception cref="TournamentValidationException">Tournament Validation Exception</exception>
        private void IsTrasferPeriodValid(Tournament tournament)
        {
            // if there is no transfer period
            if (!tournament.TransferStart.HasValue && !tournament.TransferEnd.HasValue)
            {
                return;
            }

            // if transfer end missing
            if (tournament.TransferStart.HasValue && !tournament.TransferEnd.HasValue)
            {
                throw new TournamentValidationException(
                    TournamentResources.TransferEndMissing,
                    TournamentConstants.TRANSFER_END_MISSING,
                    TournamentConstants.TRANSFER_END_CAPTURE);
            }

            // if transfer start missing
            if (!tournament.TransferStart.HasValue && tournament.TransferEnd.HasValue)
            {
                throw new TournamentValidationException(
                    TournamentResources.TransferStartMissing,
                    TournamentConstants.TRANSFER_START_MISSING,
                    TournamentConstants.TRANSFER_START_CAPTURE);
            }

            // if games date go after transfer start
            if (tournament.GamesStart >= tournament.TransferStart.GetValueOrDefault())
            {
                throw new TournamentValidationException(
                    TournamentResources.WrongTransferStart,
                    TournamentConstants.TRANSFER_PERIOD_BEFORE_GAMES_START,
                    TournamentConstants.TRANSFER_START_CAPTURE);
            }

            // if transfer start goes after transfer end
            if (tournament.TransferStart.GetValueOrDefault() >= tournament.TransferEnd.GetValueOrDefault())
            {
                throw new TournamentValidationException(
                    TournamentResources.WrongTransferPeriod,
                    TournamentConstants.TRANSFER_END_BEFORE_TRANSFER_START,
                    TournamentConstants.TRANSFER_END_CAPTURE);
            }

            // if transfer end is before tournament end date
            if (tournament.TransferEnd.GetValueOrDefault() >= tournament.GamesEnd)
            {
                throw new TournamentValidationException(
                    TournamentResources.InvalidTransferEndpoint,
                    TournamentConstants.TRANSFER_END_AFTER_GAMES_END,
                    TournamentConstants.GAMES_END_CAPTURE);
            }
        }

        private List<Tournament> GetFilteredTournaments(IEnumerable<TournamentStateEnum> statesFilter)
        {
            return this.Get().Where(t => statesFilter.Contains(t.State)).ToList();
        }

        private void IsDivisionsCountValid(IList<Division> divisions)
        {
            if (!TournamentValidationSpecification.IsDivisionCountValid(divisions))
            {
                throw new ArgumentOutOfRangeException(
                    string.Format(
                        ServiceResources.ExceptionMessages.OutOfDivisionsCountRange,
                        Domain.Constants.Tournament.MIN_DIVISIONS_COUNT,
                        Domain.Constants.Tournament.MAX_DIVISIONS_COUNT));
            }
        }

        private void AreDivisionsUnique(IList<Division> divisions)
        {
            foreach (Division division in divisions)
            {
                if (divisions.Where(d => d.Name == division.Name).Count() > 1)
                {
                    throw new ArgumentException(ServiceResources.ExceptionMessages.DivisionsAreNotUniq);
                }
            }
        }

        private void IsDivisionCountWithinRange(int count)
        {
            if (!TournamentValidationSpecification.IsDivisionCountWithinRange(count))
            {
                throw new ArgumentException(
                    string.Format(
                        TournamentResources.DivisionCountOutOfRange,
                        TournamentConstants.MIN_DIVISIONS_COUNT,
                        TournamentConstants.MAX_DIVISIONS_COUNT));
            }
        }

        private void AreDivisionNamesUnique(List<Division> divisions)
        {
            if (divisions.Select(d => new { Name = d.Name.ToUpper() }).Distinct().Count() != divisions.Count)
            {
                throw new ArgumentException(TournamentResources.DivisionNamesNotUnique);
            }
        }

        private void IsGroupCountWithinRange(int count)
        {
            if (!DivisionValidation.IsGroupCountWithinRange(count))
            {
                throw new ArgumentException(
                    string.Format(
                    TournamentResources.GroupCountOutOfRange,
                    DivisionConstants.MIN_GROUPS_COUNT,
                    DivisionConstants.MAX_GROUPS_COUNT));
            }
        }

        private void AreGroupNamesUnique(List<Group> groups)
        {
            if (groups.Select(g => new { Name = g.Name.ToUpper() }).Distinct().Count() != groups.Count)
            {
                throw new ArgumentException(TournamentResources.GroupNamesNotUnique);
            }
        }

        private void ValidateTournament(Tournament tournament)
        {
            AreDatesValid(tournament);
            IsDivisionsCountValid(tournament.Divisions);
            AreDivisionsUnique(tournament.Divisions);
            SetDivisionsAndGroupsId(tournament);
        }

        private void SetDivisionsAndGroupsId(Tournament tournament)
        {
            foreach (Division division in tournament.Divisions)
            {
                division.TournamentId = tournament.Id;
                IsGroupCountWithinRange(division.Groups.Count);
                AreGroupNamesUnique(division.Groups);

                foreach (Group group in division.Groups)
                {
                    group.DivisionId = division.Id;
                }
            }
        }
}
        #endregion
    }
}
